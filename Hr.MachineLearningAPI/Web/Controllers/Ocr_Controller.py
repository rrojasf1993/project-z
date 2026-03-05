<<<<<<< HEAD
from fastapi import APIRouter, UploadFile, File, HTTPException, Depends, FastAPI
from Models.OcrResult import OcrResult
=======
from fastapi import APIRouter, UploadFile, File, HTTPException
>>>>>>> 0fdab17697fc8c270d7acb38f3e90a7aa88b71af
from Services.OcrService import OcrService
from Models.InputFile import InputFile
import shutil
import os
import uuid
_ocrServiceInstance = OcrService()
routerInstance = APIRouter()
uploadDir_Path = "uploads"
os.makedirs(uploadDir_Path, exist_ok=True)

def get_OcrService():
    return _ocrServiceInstance
@routerInstance.post("/process")
async def process_Document(file: UploadFile = File(...) , _ocrService=Depends(get_OcrService))->OcrResult:
    if not (file.filename.endswith("jpg") or file.filename.endswith("jpeg") or file.filename.endswith("png")):
        raise HTTPException(status_code=400, detail="Only image files are allowed")

    file_id = str(uuid.uuid4())
    file_path = os.path.join(uploadDir_Path, f"{file_id}_{file.filename}")

    with open(file_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)
    try:
        with open(file_path, "wb") as buffer:
            shutil.copyfileobj(file.file, buffer)
        ocr_result = _ocrServiceInstance.extractText(file_path)

    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error procesando imagen: {str(e)}")
    finally:

        os.remove(file_path)
        pass

    return ocr_result

@routerInstance.post("/processv2")
async def processv2(inputFileData:InputFile, _ocrService=Depends(get_OcrService))->OcrResult:
    if not os.path.exists(inputFileData.path):
        raise HTTPException(status_code=400, detail="The file doesn't exist")
    try:
        ocr_result = _ocrService.extractText(inputFileData.path)
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
    return ocr_result

