import type { ImageItem } from "./props/ImageItem";

export interface IImageUploadStoreModel {
  previewData: Array<ImageItem>;
  showPreviewModal: boolean;
  selectedImageItemForPreview: ImageItem | null;
  addFiles: (files: Array<File>) => void;
  replaceImageItem: (previewUrl: string, newFile: File) => void;
  openPreviewModal: (item: ImageItem) => void;
  closePreviewModal: () => void;
  selectAll: () => void;
  toggleSelectItem: (selectedItem: ImageItem) => void;
  reset: () => void;
}
