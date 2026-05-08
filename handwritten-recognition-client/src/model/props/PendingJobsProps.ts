import type moment from "moment";
import type OcrJobStatus from "../../cross/enums/OcrJobStatus";
import type { OcrJobDto } from "../dto/OcrJobDto";

export interface PendingJobsProps{
    onSearchRequest:(status:OcrJobStatus, startDate?:moment.Moment, endDate?:moment.Moment)=>Promise<void>;
    jobs:Array<OcrJobDto>
}