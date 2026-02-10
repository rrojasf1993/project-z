from fastapi import FastAPI
import uvicorn
# Importamos el router desde donde sea que lo definas (ver paso 2)
from Web.Controllers.Ocr_Controller import routerInstance

app = FastAPI(
    title="Handwritten OCR API",
    version="0.1.0"
)

# Incluimos las rutas del controlador
app.include_router(routerInstance)

if __name__ == "__main__":
    # Esto permite correrlo con: python main.py
    uvicorn.run("Main:app", host="0.0.0.0", port=8000, reload=True)
