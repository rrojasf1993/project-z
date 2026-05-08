import uuid
from typing import Optional

from pydantic import BaseModel


class ExtractTableRequest(BaseModel):
    imagePath: str
    documentId: Optional[uuid.UUID]
