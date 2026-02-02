from fastapi import FastAPI
from Web.Controllers.Ocr_Controller import router as ocr_router

app = FastAPI(
    title="Handwritten OCR API",
    version="0.1.0"
)

app.include_router(ocr_router, prefix="/ocr")