import type { OcrDocumentDto } from "../dto/OcrDocumentDto";
import type { OcrJobDto } from "../dto/OcrJobDto";

export interface IDocumentReviewProps{
    jobData:OcrJobDto,
    documentData:OcrDocumentDto
}