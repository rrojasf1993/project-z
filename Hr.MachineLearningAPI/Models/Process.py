from pydantic import BaseModel

class ImagePreProcess(BaseModel):
    imageProfile:str=""
    processingTime:int=0