from typing import List

from pydantic import BaseModel
class OcrLine(BaseModel):
    text: str
    confidence: float
    boundingBox: List[int]