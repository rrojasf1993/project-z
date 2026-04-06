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
    currentDocumentData:{} as OcrDocumentDto,
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
                url += `&StartDate=${startDate}&EndDate=${endDate}`;
            }
            const result = await httpClientInstance.DoGet(url);
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
            const result = await httpClientInstance.DoGet(url);
            set({ isLoading: false, currentDocumentJobInfo: result });
        } catch (error) {
            set({ isLoading: false, errorData: error });
        }
    },
    setCurrentDocumentData(item:OcrDocumentDto) {
        set({currentDocumentData:item});
    },
}));
