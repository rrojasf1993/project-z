from fastapi import APIRouter, Depends, HTTPException
import os
import gc
from Models.InputFile import InputFile
routerInstance = APIRouter()


# Usar una función que devuelva la instancia solo cuando se pida
# O mejor aún, dentro de un Singleton que cargue el modelo on-demand
def get_ocr_service():
    from Services.OcrService import OcrService  # Import local para evitar carga inicial
    return OcrService()


@routerInstance.post("/processv2")
async def processv2(inputFileData: InputFile, _ocr_service=Depends(get_ocr_service)):
    if not os.path.exists(inputFileData.path):
        raise HTTPException(status_code=400, detail="The file doesn't exist")
    try:
        # Procesar el archivo
        ocr_result=_ocr_service.extractText(inputFileData.path)
        print(ocr_result)
        # Sugerencia: Eliminar el archivo una vez procesado para liberar recursos del SO
        # os.remove(inputFileData.path)

        return ocr_result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
    finally:
        # Forzar la recolección de basura si el objeto de resultado es muy grande
        # o si el servicio dejó residuos en memoria.
        del _ocr_service
        gc.collect()