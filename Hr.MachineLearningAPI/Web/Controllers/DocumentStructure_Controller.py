from fastapi import APIRouter, Depends, HTTPException
from Models.ExtractTableRequest import ExtractTableRequest
from Web.Controllers.Ocr_Controller import routerInstance


def get_document_structure_Service():
    from Services.DocumentStructureService import DocumentStructureService
    return DocumentStructureService()
@routerInstance.post("/feature-extraction/extract-table")
def extract_table(request:ExtractTableRequest,  _document_svc=Depends(get_document_structure_Service)):
    print("tengo miedo")
    rawResponse = _document_svc.extract_table_data(request)