from fastapi import FastAPI
import uvicorn
from Web.Controllers.Ocr_Controller import routerInstance

app = FastAPI(
    title="Handwritten OCR API",
    version="0.1.0",
    root_path="/api"
)

app.include_router(routerInstance)

if __name__ == "__main__":
    # Esto permite correrlo con: python main.py
    uvicorn.run("Main:app", host="127.0.0.1", port=5030, reload=True)
