import type { BaseStateModel } from "./BaseStateModel";
import type { OcrDocumentDto } from "./dto/OcrDocumentDto";
import type { OcrJobDto } from "./dto/OcrJobDto";

export interface IDocumentsStoreModel extends BaseStateModel{
  pendingDocuments: Array<OcrDocumentDto>;
  currentDocumentJobInfo:OcrJobDto;
  getPendingDocuments:( startDate: moment.Moment | null,endDate: moment.Moment | null)=>void;
  getJobInfoForDocument:(documentId:string)=>Promise<void>;
  currentDocumentData: OcrDocumentDto,
  setCurrentDocumentData:(item:OcrDocumentDto)=>void;
  modifiedDocumentData:OcrDocumentDto,
  setModifiedDocumentData:(item:OcrDocumentDto)=>void;
  lineModifyMessage:string;
  setLineModifyMessage:(msg:string)=>void;
  updateResult:OcrDocumentDto;
  updateDocumentWithCorrections:(updateDocument:OcrDocumentDto)=>Promise<void>;
}