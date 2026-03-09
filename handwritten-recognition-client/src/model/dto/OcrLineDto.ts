export type OcrLineDto={
    lineId:number,
    text:string,
    confidence:number,
    status:string,
    boundingBox:Array<Array<number>>
}