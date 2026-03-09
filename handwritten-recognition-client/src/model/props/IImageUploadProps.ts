import type { ImageItem } from "./ImageItem";

export interface IImageUploadProps {
  onUploadComplete: (fileItemsToUploadToServer: ImageItem[]) => void;
}
