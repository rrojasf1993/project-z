from pydantic import BaseModel


class ImageInfo(BaseModel):
    originalWidth: int=0
    originalHeight: int=0
    actualWidth: int=0
    actualHeight: int=0
