import type { BoundingBox } from "./BoundingBoxDto";

export type OcrLineDto={
    lineId:number,
    originalText:string,
    correctedText:string|null,
    confidenceScore:number,
    confidenceStatus:string,
    boundingBox:Array<Array<number>>
    lineIndex:number;
    frontendFriendlyBoxes:Array<BoundingBox>
}