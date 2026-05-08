from typing import List

from pydantic import BaseModel

from Models.TableRow import TableRow


class TableResult(BaseModel):
    rows: List[TableRow]
