import type OcrJobStatus from "../cross/enums/OcrJobStatus";
import type { BaseStateModel } from "./BaseStateModel";
import type { OcrJobDto } from "./dto/OcrJobDto";

export interface IOcrStoreModel extends BaseStateModel{
     
    imageUrl:string,
    imageWidth:number,
    imageHeight:number,
    jobs:Array<OcrJobDto>
    getImage:(jobData:OcrJobDto)=>Promise<void>;
    setImageDimensions:(width:number, height:number)=>void;
    getJobs:(status:OcrJobStatus, startDate:moment.Moment|null, endDate:moment.Moment|null) =>Promise<void>
}