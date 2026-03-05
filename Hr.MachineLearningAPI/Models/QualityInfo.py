from typing import List
from pydantic import BaseModel
class QualityInfo(BaseModel):
    level:str
    blurLevel:float
    notes:List[str]=[]
    avgConfidence:float