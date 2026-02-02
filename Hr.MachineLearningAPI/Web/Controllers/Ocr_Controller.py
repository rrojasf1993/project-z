from fastapi import APIRouter, UploadFile, File, HTTPException, Depends
from modelscope.server.api.routers import router
from Services.OcrService import OcrService
import time
import shutil
import os
import uuid

routerInstance = APIRouter()
uploadDir_Path="uploads"
os.makedirs(uploadDir_Path, exist_ok=True)

@router.post("/process")
async def process_Document(file: UploadFile = File(...), _ocrService:OcrService = Depends()):
    if not file.content_type.startswith("image/"):
        raise HTTPException(status_code=400, detail="Only image files are allowed")

    file_id = str(uuid.uuid4())
    file_path = os.path.join(uploadDir_Path, f"{file_id}_{file.filename}")

    start_time = time.time()

    with open(file_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)

    try:
        ocr_result = _ocrService.extractText(file_path)
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

    processing_time = round(time.time() - start_time, 2)

    return {
        "fileId": file_id,
        "result": ocr_result,
        "processingTime": processing_time
    }
