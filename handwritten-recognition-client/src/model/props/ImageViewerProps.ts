
import type { OcrLineDto } from "../dto/OcrLineDto";

export interface ImageViewerProps{
   
  imageUrl:string;
  lines:Array<OcrLineDto>
  onFieldSelect: (lineIdx:number)=>void,
}