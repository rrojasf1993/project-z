import type { ImageInfoDto } from "./ImageInfoDto";
import type { OcrLineDto } from "./OcrLineDto"
import type { ProcessDto } from "./processDto";

export type OcrResultDto={
    lines:Array<OcrLineDto>;
    documentId:string,
    imageInfo:ImageInfoDto,
    processInfo:ProcessDto
}