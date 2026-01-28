import cv2
import numpy as np
import os
import tempfile
from typing import List,Any,Dict
from paddleocr import PaddleOCR
import logging

from Models import OcrLine
from Util.MatplotLibUtil import MatplotlibUtil


class OcrService:
    def __init__(self, useGpu:bool,language:str="es" ):
        self._useGpu = useGpu
        self._language = language
        self._OcrInstance=PaddleOCR()
        self._DebugUtil=MatplotlibUtil()
        os.environ["FLAGS_new_executor"] = "0"
        os.environ["FLAGS_use_mkldnn"] = "0"
        logging.basicConfig(format='%(asctime)s - %(levelname)s - %(message)s', datefmt='%Y-%m-%d %H:%M:%S',filename="./OcrService.log",filemode="a+")

    def deskewImage(self, image:np.ndarray)->np.ndarray:
        self._DebugUtil.renderStepWithImage(image,"Original")
        grayScaleImg = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        self._DebugUtil.renderStepWithImage(grayScaleImg, "Grayscale")
        nonEmptyPixels=np.column_stack(np.where(grayScaleImg>0))
        if(nonEmptyPixels.size == 0):
            return image
        imageAngle=cv2.minAreaRect(nonEmptyPixels)[-1]
        if(imageAngle < 45):
            imageAngle=-(90+imageAngle)
        else:
          imageAngle=-imageAngle
        (height, width) = image.shape[:2]
        (centerX, centerY) = (width // 2, height // 2)
        tempMatrix =  cv2.getRotationMatrix2D((centerX, centerY), imageAngle, 1.0)
        cos = np.abs(tempMatrix[0, 0])
        sin = np.abs(tempMatrix[0, 1])
        newWidth = int((height * sin) + (width * cos))
        newHeight = int((height * cos) + (width * sin))
        tempMatrix[0, 2] += (newWidth / 2) - centerX
        tempMatrix[1, 2] += (newHeight / 2) - centerY
        rotatedImage = cv2.warpAffine(image,tempMatrix,(newWidth,newHeight))
        self._DebugUtil.renderStepWithImage(rotatedImage, "Deskewed")
        return rotatedImage


    def preProcessImg(self, path:str)->str:
        img = cv2.imread(path)
        tmpPath=""
        try:
            if(img is None):
                raise FileNotFoundError("No se puede leer el archivo")
            #deskewedImage=self.deskewImage(img)
            deskewedImage=img
            grayScaleDeskewedImg=cv2.cvtColor(deskewedImage, cv2.COLOR_BGR2GRAY)
            self._DebugUtil.renderStepWithImage(grayScaleDeskewedImg, "Grayscale and deskewed")
            #binarizacion otsu
            _,binarized=cv2.threshold(grayScaleDeskewedImg,0,255,cv2.THRESH_BINARY | cv2.THRESH_OTSU)
            self._DebugUtil.renderStepWithImage(binarized, "Otsu Binarized")
            tmpPath=os.path.join(os.path.dirname(path),os.path.splitext(os.path.basename(path))[0]+"_preProcessed.png")
            cv2.imwrite(tmpPath,binarized)
        except RuntimeError as errData:
            logging.error(errData)
        return tmpPath

    def parseRawOcrData(self, rawOcrData)->List[Dict[str,OcrLine]]:
        lines=[]
        for line in rawOcrData:
            boundingBox=[int (x) for x in np.array(line[0]).reshape(-1).tolist()]
            text,score=line[1][0],line[1][1]
            lines.append({"boundingBox":boundingBox,"text":text,"confidence":score})
        return lines

    def extractText(self,path:str)->str:
      preProcessedImgData=self.preProcessImg(path)
      rawOcrData=self._OcrInstance.ocr(preProcessedImgData)
      parsedOcrData=self.parseRawOcrData(rawOcrData)
      try:
        os.remove(path)
      except Exception as exc:
        logging.error(exc)
      return parsedOcrData

