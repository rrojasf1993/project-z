import uuid
from typing import List
from pydantic import BaseModel
class OcrLine(BaseModel):
    text: str
    confidenceStatus: str
    boundingBox: List[List[int]]
    lineId: uuid.UUID=uuid.uuid4()
    lineIndex:int
    confidenceScore: float