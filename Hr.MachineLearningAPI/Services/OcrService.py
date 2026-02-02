import logging
import os
from typing import List, Dict
import time

import cv2
import numpy as np
from paddleocr import PaddleOCR

from Models.OcrLine import OcrLine
from Models.OcrResult import OcrResult
from Util.MatplotLibUtil import MatplotlibUtil
from pprint import pprint

class OcrService:
    def __init__(self, language: str = "es"):

        self._language = language
        self._OcrInstance = PaddleOCR()
        self._DebugUtil = MatplotlibUtil()
        os.environ["FLAGS_new_executor"] = "0"
        os.environ["FLAGS_use_mkldnn"] = "0"
        logging.basicConfig(format='%(asctime)s - %(levelname)s - %(message)s', datefmt='%Y-%m-%d %H:%M:%S',
                            filename="./OcrService.log", filemode="a+")

    def deskewImage(self, image: np.ndarray) -> np.ndarray:
        self._DebugUtil.renderStepWithImage(image, "Original")
        grayScaleImg = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        self._DebugUtil.renderStepWithImage(grayScaleImg, "Grayscale")
        nonEmptyPixels = np.column_stack(np.where(grayScaleImg > 0))
        if (nonEmptyPixels.size == 0):
            return image
        imageAngle = cv2.minAreaRect(nonEmptyPixels)[-1]
        if (imageAngle < 45):
            imageAngle = -(90 + imageAngle)
        else:
            imageAngle = -imageAngle
        (height, width) = image.shape[:2]
        (centerX, centerY) = (width // 2, height // 2)
        tempMatrix = cv2.getRotationMatrix2D((centerX, centerY), imageAngle, 1.0)
        cos = np.abs(tempMatrix[0, 0])
        sin = np.abs(tempMatrix[0, 1])
        newWidth = int((height * sin) + (width * cos))
        newHeight = int((height * cos) + (width * sin))
        tempMatrix[0, 2] += (newWidth / 2) - centerX
        tempMatrix[1, 2] += (newHeight / 2) - centerY
        rotatedImage = cv2.warpAffine(image, tempMatrix, (newWidth, newHeight))
        self._DebugUtil.renderStepWithImage(rotatedImage, "Deskewed")
        return rotatedImage

    def isBlurry(self, img, threshold=100):
        if img is None:
            return True, 0.0

        if len(img.shape) == 3:
            img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

        img = img.astype(np.uint8)

        try:
            laplacian_var = cv2.Laplacian(img, cv2.CV_64F).var()
            return laplacian_var < threshold, float(laplacian_var)
        except Exception as e:
            logging.error("Error en isBlurry:", e)
            return True, 0.0

    def lowContrast(self, img, threshold=40):
        contrast = img.max() - img.min()
        return contrast < threshold, contrast

    def exposureIssues(self, img, white_thresh=245, black_thresh=10):
        total = img.size
        white_pixels = np.sum(img > white_thresh)
        black_pixels = np.sum(img < black_thresh)

        white_ratio = white_pixels / total
        black_ratio = black_pixels / total

        return white_ratio > 0.6 or black_ratio > 0.6, white_ratio, black_ratio

    def is_Bad_Image(self, grayscaleImg):
        blurry, blur_score=self.isBlurry(grayscaleImg)
        lowc, contrast = self.lowContrast(grayscaleImg)
        exposure_bad, white_ratio, black_ratio = self.exposureIssues(grayscaleImg)
        problems = sum([
            blurry,
            lowc,
            exposure_bad
        ])
        return problems >= 2, {
            "blur": blur_score,
            "contrast": contrast,
            "white_ratio": white_ratio,
            "black_ratio": black_ratio
        }



    def aggresivePreprocessing(self, grayScaleImgInstance):

        # adaptive threshold SOLO como fallback
        thresh = cv2.adaptiveThreshold(
            grayScaleImgInstance, 255,
            cv2.ADAPTIVE_THRESH_GAUSSIAN_C,
            cv2.THRESH_BINARY,
            31, 10
        )
        return thresh

    def preProcessImg(self, path: str) -> str:
        img = cv2.imread(path)
        tmpPath = ""
        try:
            if (img is None):
                raise FileNotFoundError("No se puede leer el archivo")
            grayScaleImg = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
            self._DebugUtil.renderStepWithImage(grayScaleImg, "Grayscale")
            bad, metrics = self.is_Bad_Image(grayScaleImg)
            print("Imagen mala:", bad)
            print("Metricas de imagen:", metrics)
            if bad:
                grayScaleImg = self.aggresivePreprocessing(grayScaleImg)
                self._DebugUtil.renderStepWithImage(grayScaleImg, "Grayscale with aggresive fixes")

            denoisedImg = cv2.GaussianBlur(grayScaleImg, (3, 3), 0)
            alpha = 1.2  # contraste
            beta = 10  # brillo
            adjusted = cv2.convertScaleAbs(denoisedImg, alpha=alpha, beta=beta)
            self._DebugUtil.renderStepWithImage(adjusted, "Adjusted")
            tmpPath = os.path.join(os.path.dirname(path),
                                   os.path.splitext(os.path.basename(path))[0] + "_preProcessed.png")
            cv2.imwrite(tmpPath, adjusted)
        except RuntimeError as errData:
            logging.error(errData)
        return tmpPath

    def get_confidence_status(self, confidence: float) -> str:
        if confidence < 0.6:
            return "Error"
        elif confidence < 0.8:
            return "Warning"
        return "Ok"

    def parseRawOcrData(self,  rawOcrData, processTime):
        lines = []
        confidences = []
        dt_polys=None
        rec_texts=None
        rec_scores=None
        count=0
        if(len(rawOcrData) > 0):
            for rawOcrDataItem in rawOcrData:
                dt_polys=rawOcrDataItem.get("dt_polys", [])
                rec_texts=rawOcrDataItem.get("rec_texts", [])
                rec_scores=rawOcrDataItem.get("rec_scores", [])
        else:
            print("Error procesando retorno ocr")
        count = min(len(dt_polys), len(rec_texts), len(rec_scores))
        for i in range(count):
            confidence = float(rec_scores[i])

            lines.append({
                "lineId": i,
                "text": rec_texts[i],
                "confidence": confidence,
                "status": self.get_confidence_status(confidence),
                "boundingBox": dt_polys[i]
            })

            confidences.append(confidence)

        return {
            "lines": lines,
            "confidenceAvg": sum(confidences) / len(confidences) if confidences else 0.0,
            "processingTime": processTime
        }

    def extractText(self, path: str) -> str:
        preProcessedImgData = self.preProcessImg(path)
        start = time.time()
        rawOcrData = self._OcrInstance.ocr(preProcessedImgData)
        if not rawOcrData or not rawOcrData[0]:
            logging.info("El OCR No pùdo detectar texto")
            return {
                "confidenceAvg": 0.0,
                "lines": []
            }
        parsedOcrData = self.parseRawOcrData(rawOcrData,(time.time() - start))
        try:
            os.remove(path)
        except Exception as exc:
            logging.error(exc)
        return parsedOcrData
