from typing import List

from pydantic import BaseModel

from Models.TableCell import TableCell


class TableRow(BaseModel):
    cells: List[TableCell]