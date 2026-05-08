import type { OcrDocumentDto } from "../dto/OcrDocumentDto";

export interface ILineViewProps{
    documentData:OcrDocumentDto;
    selectedLineIndex:number;
    handleTextBoxClick:(lineId:number)=>void;
}