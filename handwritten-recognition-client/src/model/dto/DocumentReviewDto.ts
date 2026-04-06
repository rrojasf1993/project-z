import type { OcrDocumentDto } from "./OcrDocumentDto";
import type { OcrJobDto } from "./OcrJobDto";

export type DocumentReviewDto = {
  ocrReviewDocumentData: OcrDocumentDto;
  ocrReviewJobData: OcrJobDto;
};
