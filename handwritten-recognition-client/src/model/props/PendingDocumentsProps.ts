import type { OcrDocumentDto } from "../dto/OcrDocumentDto"

export type PendingDocumentListProps=
{
    documentsList:Array<OcrDocumentDto>;
    onReviewAction:VoidFunction;
}