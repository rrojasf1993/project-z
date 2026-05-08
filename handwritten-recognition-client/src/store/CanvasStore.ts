import { create } from "zustand";
import type { ICanvasStoreModel } from "../model/CanvasStoreModel";
import type { BoundingBox } from "../model/dto/BoundingBoxDto";
import type { OcrLineDto } from "../model/dto/OcrLineDto";

export const useCanvasStore = create<ICanvasStoreModel>((set, get) => ({
  imageSize: { width: 0, height: 0 },
  zoom: 1,
  offset: { x: 0, y: 0 },
  boxes: [] as Array<BoundingBox>,
  setImageSize: (newWidth: number, newHeight:number) => set({ imageSize: {width:newWidth, height:newHeight}}),
  setZoom: (zoomLevel: number) => set({ zoom: zoomLevel }),
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  setOffset: (offsetLevel: any) => set({ offset: offsetLevel }),
  setBoxes: (boxesArr: Array<BoundingBox>) => set({ boxes: boxesArr }),
  addBox: (box: BoundingBox) => {
    set((s) => ({ boxes: [...s.boxes, box] }));
  },
  updateBox: (boxId: string, updateBox: BoundingBox) =>
    set((s) => ({
      boxes: s.boxes.map((box) =>
        box.id === boxId ? { ...box, ...updateBox } : box,
      ),
    })),
  toScreenCoordinates: (box: BoundingBox) => {
    const { imageSize, zoom, offset } = get();
    return {
      x: box.x * imageSize.width + offset.x,
      y: box.y * imageSize.height + offset.y,
      w: box.w * imageSize.width * zoom,
      h: box.h * imageSize.height * zoom,
    } as BoundingBox;
  },
  toRelativeCoordinates: (box: BoundingBox) => {
    const { imageSize, zoom, offset } = get();
    return {
      x: (box.x - offset.x) / (imageSize.width * zoom),
      y: (box.y - offset.y) / (imageSize.height * zoom),
      w: box.w / (imageSize.width * zoom),
      h: box.h / (imageSize.height * zoom),
    };
  },
  configureBoundingBoxes: (lines: Array<OcrLineDto>) => {
    const { imageSize } = get();
    const rawBoundingBoxes = lines.flatMap((l) => l.frontendFriendlyBoxes);
    const friendlyBoxes = rawBoundingBoxes.map((b) => ({
      id: crypto.randomUUID(),
      x: b.x / imageSize.width,
      y: b.y / imageSize.height,
      w: b.w / imageSize.width,
      h: b.h / imageSize.height,
    })) as Array<BoundingBox>;
    set({ boxes: friendlyBoxes });
  },
}));
