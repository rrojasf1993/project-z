import type OcrJobStatus from "../../cross/enums/OcrJobStatus"


export type OcrJobDto={
    jobId:object,
    error:string,
    status:OcrJobStatus,
    fileName:string,
    preprocessedFileName:string,
    createdAt:Date,
    updatedAt:Date
}