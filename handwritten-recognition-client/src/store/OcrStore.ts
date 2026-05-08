import { create } from "zustand";
import type { IOcrStoreModel } from "../model/OcrStoreModel";
import type { OcrJobDto } from "../model/dto/OcrJobDto";
import HttpService from "../services/HttpService";
import type OcrJobStatus from "../cross/enums/OcrJobStatus";

export const useOcrStore = create<IOcrStoreModel>((set) => ({
  imageBmp: {} as ImageBitmap,
  imgUrl:"",
  isLoading: false,
  imageHeight: 0,
  imageWidth: 0,
  jobs: [] as Array<OcrJobDto>,
  getImage: async (jobData: OcrJobDto) => {
    if (!jobData || jobData === undefined) return;
    set({ isLoading: true });
    const url = `api/Ocr/DownloadDocumentImageForJob/jobId=${jobData.jobId}`;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const httpClientInstance: HttpService<any, any> = new HttpService<
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      any,
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      any
    >();
    const imageFile = await httpClientInstance.DownloadFile(url);
    const imageUrl=URL.createObjectURL(imageFile);
    const bmp=await createImageBitmap(imageFile as ImageBitmapSource);
    set({
      imageHeight:bmp.height,
      imageWidth:bmp.width,
      imageBmp:bmp,
      imgUrl:imageUrl,
      isLoading:false
    });
  },
  setImageDimensions(width, height) {
    set({
      imageHeight: height,
      imageWidth: width,
    });
  },
  getJobs: async (
    status: OcrJobStatus,
    startDate: moment.Moment|null,
    endDate: moment.Moment|null,
  ) => {
    set({ isLoading: true });
    try {
      const httpClientInstance: HttpService<
        string,
        Array<OcrJobDto>
      > = new HttpService();
      let url: string = `api/ocr/GetOcrJobsByStatus/StatusId=${status}`;
      if (startDate && endDate) {
        url += `&StartDate=${startDate.toISOString()}&EndDate=${endDate.toISOString()}`;
      }
      const result = await httpClientInstance.DoBaseHttpRequest(url,"get");
      console.log(result);
      set({ isLoading: false, jobs: result });
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      set({ isLoading: false, errorData: error });
    }
  },
  setIsLoading:(isLoadingValue)=>{
    set({isLoading:isLoadingValue});
  }
}));
