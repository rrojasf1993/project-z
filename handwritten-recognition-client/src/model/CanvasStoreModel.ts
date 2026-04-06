import type { BoundingBox } from "./dto/BoundingBoxDto"
import type { OcrLineDto } from "./dto/OcrLineDto";

export interface ICanvasStoreModel{
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    imageSize:any;
    zoom:number;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    offset:any;
    boxes: Array<BoundingBox>;
    setImageSize:(newWidth: number, newHeight:number)=>void;
    setZoom:(zoomLevel:number)=>void;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    setOffset:(offset:any)=>void;
    setBoxes:(boxes:Array<BoundingBox>)=>void;
    addBox:(boundingBox:BoundingBox)=>void;
    updateBox:(boxId:string, box:BoundingBox)=>void;
     
    toScreenCoordinates:(box:BoundingBox)=>BoundingBox;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    toRelativeCoordinates:(screenBox:BoundingBox)=>any;
    configureBoundingBoxes: (lines: Array<OcrLineDto>) =>void;
}