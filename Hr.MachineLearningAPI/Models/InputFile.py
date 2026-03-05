from pydantic import BaseModel
class InputFile(BaseModel):
    path: str
