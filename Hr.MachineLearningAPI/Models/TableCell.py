from pydantic import BaseModel


class TableCell(BaseModel):
    text: str
    row: int
    col: int