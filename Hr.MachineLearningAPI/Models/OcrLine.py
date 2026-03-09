from typing import List
from pydantic import BaseModel
class OcrLine(BaseModel):
    text: str
    confidenceStatus: str
    boundingBox: List[List[int]]
    lineId: int
    confidenceScore: float