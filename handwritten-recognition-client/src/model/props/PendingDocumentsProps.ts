import type { OcrDocumentDto } from "../dto/OcrDocumentDto"

export type PendingDocumentListProps=
{
    documentsList:Array<OcrDocumentDto>;
    onReviewAction:(documentData:OcrDocumentDto)=>void;
    handleSearch:(startDate: moment.Moment | null, endDate: moment.Moment | null) => void;
}