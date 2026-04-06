import type { OcrDocumentDto } from "./dto/OcrDocumentDto";
import type { OcrJobDto } from "./dto/OcrJobDto";

export interface IAppStateModel{
    ocrReviewDocumentData:OcrDocumentDto
    ocrReviewJobData:OcrJobDto
}