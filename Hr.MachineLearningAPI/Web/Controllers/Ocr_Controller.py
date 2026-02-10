from fastapi import APIRouter, UploadFile, File, HTTPException
from Services.OcrService import OcrService
import time
import shutil
import os
import uuid

routerInstance = APIRouter()
uploadDir_Path = "uploads"
os.makedirs(uploadDir_Path, exist_ok=True)

# Instancia del servicio
_ocrServiceInstance = OcrService()
@routerInstance.post("/api/process")
async def process_Document(file: UploadFile = File(...)):
    if not file.content_type.startswith("image/"):
        raise HTTPException(status_code=400, detail="Only image files are allowed")

    file_id = str(uuid.uuid4())
    file_path = os.path.join(uploadDir_Path, f"{file_id}_{file.filename}")

    start_time = time.time()

    try:
        with open(file_path, "wb") as buffer:
            shutil.copyfileobj(file.file, buffer)
        ocr_result = _ocrServiceInstance.extractText(file_path)

    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error procesando imagen: {str(e)}")
    finally:

        os.remove(file_path)
        pass

    processing_time = round(time.time() - start_time, 2)

    return {
        "fileId": file_id,
        "result": ocr_result,
        "processingTime": f"{processing_time}s"
    }
