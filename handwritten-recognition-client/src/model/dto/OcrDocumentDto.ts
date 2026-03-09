import type { OcrLineDto } from "./OcrLineDto";

export type OcrDocumentDto={
  id:string,
  createdAt:Date,
  confidenceAvg:number,
  lines:Array<OcrLineDto>
}
