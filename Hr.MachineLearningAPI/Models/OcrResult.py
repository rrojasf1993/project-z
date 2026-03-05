import uuid
from typing import List
from pydantic import BaseModel

from Models.ImageInfo import ImageInfo
from Models.OcrLine import OcrLine
from Models.Process import ImagePreProcess
from Models.QualityInfo import QualityInfo


class OcrResult(BaseModel):
    lines:List[OcrLine]=[]
    imageInfo: List[ImageInfo]=[]
    qualityInfo: List[QualityInfo]=[]
    documentId: uuid.UUID=uuid.uuid4()
    processData:List[ImagePreProcess]=[]
