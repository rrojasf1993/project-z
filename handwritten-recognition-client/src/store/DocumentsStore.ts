import { create } from "zustand";
import type { OcrDocumentDto } from "../model/dto/OcrDocumentDto";
import HttpService from "../services/HttpService";
import type { OcrJobDto } from "../model/dto/OcrJobDto";
import type { IDocumentsStoreModel } from "../model/DocumentsStoreModel";
export const useDocumentsStore = create<IDocumentsStoreModel>((set) => ({
  pendingDocuments: [] as Array<OcrDocumentDto>,
  isLoading: false,
  errorData: new Error(),
  currentDocumentJobInfo: {} as OcrJobDto,
  currentDocumentData: {} as OcrDocumentDto,
  modifiedDocumentData: {} as OcrDocumentDto,
  lineModifyMessage: "",
  updateResult: {} as OcrDocumentDto,
  getPendingDocuments: async (
    startDate: moment.Moment | null,
    endDate: moment.Moment | null,
  ) => {
    set({ isLoading: true });
    try {
      const httpClientInstance: HttpService<
        string,
        Array<OcrDocumentDto>
      > = new HttpService();
      const pendingStatusId = 0;
      let url: string = `api/documents/GetDocumentsByStatus/StatusId=${pendingStatusId}`;
      if (startDate && endDate) {
        url += `&StartDate=${startDate.toISOString()}&EndDate=${endDate.toISOString()}`;
      }
      const result = await httpClientInstance.DoBaseHttpRequest(url, "get");
      set({ isLoading: false, pendingDocuments: result });
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      set({ isLoading: false, errorData: error });
    }
  },
  getJobInfoForDocument: async (documentId: string) => {
    try {
      const httpClientInstance: HttpService<string, OcrJobDto> =
        new HttpService();
      set({ isLoading: true });
      const url = `api/Documents/GetDocumentDetailsById/Document=${documentId}`;
      const result = await httpClientInstance.DoBaseHttpRequest(url, "get");
      set({ isLoading: false, currentDocumentJobInfo: result });
    } catch (error) {
      set({ isLoading: false, errorData: error });
    }
  },
  updateDocumentWithCorrections: async (updatedDocument: OcrDocumentDto) => {
    try {
      const httpClientInstance: HttpService<OcrDocumentDto, OcrDocumentDto> =
        new HttpService();
      set({ isLoading: true });
      const url = `api/Documents/UpdateDocumentLines`;
      const result = await httpClientInstance.DoBaseHttpRequest(
        url,
        "patch",
        updatedDocument,
      );
      set({ isLoading: false, updateResult: result });
    } catch (error) {
      set({ isLoading: false, errorData: error });
    }
  },
  setCurrentDocumentData(item: OcrDocumentDto) {
    set({ currentDocumentData: item });
  },
  setIsLoading: (value) => {
    set({ isLoading: value });
  },
  setModifiedDocumentData: (modified: OcrDocumentDto) => {
    set({ modifiedDocumentData: modified });
  },
  setLineModifyMessage(msg) {
    set({ lineModifyMessage: msg });
  },
}));
