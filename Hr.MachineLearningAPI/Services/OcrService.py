import logging
import os
import uuid
import time
import cv2
import numpy as np
import gc
from typing import List, Dict
from paddleocr import PaddleOCR

from Models.ImageInfo import ImageInfo
from Models.OcrLine import OcrLine
from Models.OcrResult import OcrResult
from Models.Process import ImagePreProcess
from Models.QualityInfo import QualityInfo


class OcrService:
    def __init__(self, language: str = "es"):
        self._language = language
        #Optimize for intel processor
        os.environ["FLAGS_use_mkldnn"] = "1"
        os.environ["FLAGS_cpu_math_library_num_threads"] = "6"  # Ajusta según tu carga de RAM
        os.environ["OMP_NUM_THREADS"] = "4"
        # Configuración optimizada para CPU
        self._OcrInstance = PaddleOCR(
            use_angle_cls=True,
            lang=self._language,
            cpu_threads=4,  # Aprovecha más núcleos sin duplicar RAM de modelos
            enable_mkldnn=True  # Aceleración crucial para arquitecturas Intel

        )

        logging.basicConfig(
            format='%(asctime)s - %(levelname)s - %(message)s',
            datefmt='%Y-%m-%d %H:%M:%S',
            filename="./OcrService.log",
            filemode="a+"
        )

    def resizeImage(self, img: np.ndarray, maxSize=1200) -> np.ndarray:
        """Redimensiona la imagen para reducir el consumo de RAM en el motor OCR."""
        height, width = img.shape[:2]
        scale = max(height, width) / maxSize
        if scale > 1:
            # INTER_AREA es el mejor método para reducir tamaño manteniendo nitidez de texto
            return cv2.resize(img, (int(width / scale), int(height / scale)), interpolation=cv2.INTER_AREA)
        return img

    def preProcessImg(self, path: str) -> (np.ndarray, ImageInfo, str):
        # 1. Carga inicial
        img = cv2.imread(path)
        if img is None:
            raise FileNotFoundError(f"No se pudo leer el archivo en: {path}")

        imageInfo = ImageInfo()
        imageInfo.originalHeight, imageInfo.originalWidth = img.shape[:2]

        # 2. Conversión a Gris in-place (ahorra una copia de la imagen original)
        img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

        # 3. Resize temprano: Menos píxeles = Menos RAM en los siguientes pasos
        img = self.resizeImage(img)
        imageInfo.actualHeight, imageInfo.actualWidth = img.shape[:2]

        # 4. Análisis de calidad sobre la imagen ya reducida
        bad, metrics = self.is_Bad_Image(img)
        imageProfile = "default"

        if bad:
            imageProfile = "aggresive"
            # Sobrescribimos 'img' con el resultado del umbral adaptativo
            img = cv2.adaptiveThreshold(
                img, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C,
                cv2.THRESH_BINARY, 31, 10
            )

        # 5. Mejora de contraste (CLAHE)
        clahe = cv2.createCLAHE(clipLimit=2.0, tileGridSize=(8, 8))
        img = clahe.apply(img)

        # 6. Suavizado y Ajuste final
        img = cv2.GaussianBlur(img, (3, 3), 0)
        img = cv2.convertScaleAbs(img, alpha=1.2, beta=10)

        #guardar la imagen preprocesada para usarla en el frontend
        preprocessImgPath=self.savePreProcessedImg(img,path)
        # Retornamos el array de numpy directamente
        return img, imageInfo, imageProfile, preprocessImgPath

    def savePreProcessedImg(self, img: np.ndarray, originalPath: str) -> str:
        output_dir = os.path.join(os.sep,os.path.dirname(originalPath),"preprocessedImages");
        os.makedirs(output_dir, exist_ok=True)
        fileName = os.path.splitext(os.path.basename(originalPath))[0] + "_preProcessed.png"
        fullpath = os.path.join(output_dir, fileName)
        cv2.imwrite(fullpath, img)
        return fullpath

    def extractText(self, path: str) -> OcrResult:
        processedImg = None
        try:
            # 1. Preprocesamos (sale en escala de grises de 1 canal)
            processedImg, imgPreData, profile, preprocessImgPath = self.preProcessImg(path)

            # 2. COMPATIBILIDAD: PaddleOCR necesita 3 canales (BGR)
            # aunque la imagen sea en blanco y negro.
            if len(processedImg.shape) == 2:  # Si es Gris (H, W)
                processedImg = cv2.cvtColor(processedImg, cv2.COLOR_GRAY2BGR)  # Pasa a (H, W, 3)

            start = time.time()

            # 3. Inferencia
            # Nota: quitamos 'cls=True' si te dio problemas antes,
            # ya que lo definimos en el __init__ con use_angle_cls=True
            rawOcrData = self._OcrInstance.ocr(processedImg)

            if not rawOcrData or not rawOcrData[0]:
                return None

            processTime = time.time() - start
            return self.parseRawOcrData(rawOcrData, processTime, imgPreData, profile, preprocessImgPath)

        except Exception as e:
            logging.error(f"Error en extractText: {str(e)}")
            print(f"Error detectado: {e}")  # Para debug rápido en consola
            return None
        finally:
            if processedImg is not None:
                del processedImg
            gc.collect()

    def parseRawOcrData(self,
                        rawOcrData,
                        processTime,
                        imgProcessInfo: ImageInfo,
                        imageProcessProfile,preprocessImgFileName) -> OcrResult:

        result = OcrResult()
        result.documentId=uuid.uuid4()
        result.preProcessImgPath=preprocessImgFileName
        confidences = []

        data_source = rawOcrData[0] if isinstance(rawOcrData, list) else rawOcrData

        # Extraemos las listas directamente por su nombre de llave
        dt_polys = data_source.get("dt_polys", [])
        rec_texts = data_source.get("rec_texts", [])
        rec_scores = data_source.get("rec_scores", [])

        # El conteo se basa en la cantidad de textos detectados
        count = len(rec_texts)

        for i in range(count):
            # Evitamos errores de índice si alguna lista es más corta por alguna razón
            text = rec_texts[i]
            score = float(rec_scores[i])
            poly = dt_polys[i] if i < len(dt_polys) else []

            result.lines.append(OcrLine(
                originalText=text,
                confidenceScore=score,
                boundingBox=poly,
                confidenceStatus=self.get_confidence_status(score),
                lineIndex=i,
            ))
            confidences.append(score)

        # Metadatos finales
        avgConfidence = sum(confidences) / len(confidences) if confidences else 0.0
        result.qualityInfo.append(QualityInfo(
            level=self.get_confidence_status(avgConfidence),
            blurLevel=0,
            avgConfidence=avgConfidence
        ))
        result.imageInfo.append(imgProcessInfo)
        result.processData.append(ImagePreProcess(
            imageProfile=imageProcessProfile,
            processingTime=processTime
        ))
        return result

    def get_confidence_status(self, confidence: float) -> str:
        if confidence < 0.6: return "Error"
        if confidence < 0.8: return "Warning"
        return "Ok"

    # Los métodos auxiliares (is_Bad_Image, isBlurry, etc.) deben mantenerse,
    # pero asegúrate de que reciban la imagen ya en escala de grises.
    def is_Bad_Image(self, grayImg):
        blurry, b_score = self.isBlurry(grayImg)
        lowc, contrast = self.lowContrast(grayImg)
        exp_bad, w_r, b_r = self.exposureIssues(grayImg)

        problems = sum([blurry, lowc, exp_bad])
        return problems >= 2, {"blur": b_score, "contrast": contrast}

    def isBlurry(self, img, threshold=100):
        # Ya no necesitamos convertir a gris aquí si preProcessImg ya lo hizo
        laplacian_var = cv2.Laplacian(img, cv2.CV_64F).var()
        return laplacian_var < threshold, float(laplacian_var)

    def lowContrast(self, img, threshold=40):
        contrast = img.max() - img.min()
        return contrast < threshold, float(contrast)

    def exposureIssues(self, img, white_thresh=245, black_thresh=10):
        total = img.size
        white_ratio = np.sum(img > white_thresh) / total
        black_ratio = np.sum(img < black_thresh) / total
        return white_ratio > 0.6 or black_ratio > 0.6, white_ratio, black_ratio