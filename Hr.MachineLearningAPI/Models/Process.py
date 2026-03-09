from pydantic import BaseModel

class ImagePreProcess(BaseModel):
    imageProfile:str=""
    processingTime:float=0