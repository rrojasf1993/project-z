from typing import List
from pydantic import BaseModel
from Models import OcrLine
class OcrResult(BaseModel):
    lines:List[OcrLine]
    processingTime: float