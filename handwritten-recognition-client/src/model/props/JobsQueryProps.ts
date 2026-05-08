import type moment from "moment";
import type OcrJobStatus from "../../cross/enums/OcrJobStatus";
import type { OcrJobDto } from "../dto/OcrJobDto";

export default interface JobsQueryProps{
    handleSeeImage: (jobData:OcrJobDto)=>void;
    currentJobs:Array<OcrJobDto>;
    handleSearchRequest:(statusId:OcrJobStatus, startDate:moment.Moment|null, endDate:moment.Moment|null)=>void;
    handleSeeErrorDetails:(errorDetails:string)=>void;
}