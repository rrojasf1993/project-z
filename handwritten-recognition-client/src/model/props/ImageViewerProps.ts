
import type { OcrLineDto } from "../dto/OcrLineDto";

export interface ImageViewerProps{
   
  imageUrl:string;
  lines:Array<OcrLineDto>
  selectedBoxId:number,
  onFieldSelect: (lineIdx:number)=>void,
  onFieldFocus: React.MouseEventHandler<HTMLDivElement> | undefined;
}