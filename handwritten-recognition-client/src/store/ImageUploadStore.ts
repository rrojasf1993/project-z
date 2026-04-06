import { create } from "zustand";
import type { IImageUploadStoreModel } from "../model/ImageUploadStoreModel";
import type { ImageItem } from "../model/props/ImageItem";

export const useImageUploadStore = create<IImageUploadStoreModel>((set, get) => ({
  previewData: [] as Array<ImageItem>,
  showPreviewModal: false,
  selectedImageItemForPreview: null,
  addFiles: (files: Array<File>) => {
    const newImages = files.map((file) => ({
      file,
      previewUrl: URL.createObjectURL(file),
      selected: false,
    }));
    set((s) => ({ previewData: [...s.previewData, ...newImages] }));
  },
  replaceImageItem: (previewUrl: string, newFile: File) => {
    set((s) => {
      const idx = s.previewData.findIndex((i) => i.previewUrl === previewUrl);
      if (idx === -1) return s;
      const old = s.previewData[idx];
      URL.revokeObjectURL(old.previewUrl);
      const nextUrl = URL.createObjectURL(newFile);
      const next = [...s.previewData];
      next[idx] = {
        ...old,
        file: newFile,
        previewUrl: nextUrl,
      };
      let nextPreview = s.selectedImageItemForPreview;
      if (nextPreview?.previewUrl === previewUrl) {
        nextPreview = next[idx];
      }
      return { previewData: next, selectedImageItemForPreview: nextPreview };
    });
  },
  openPreviewModal: (item: ImageItem) => {
    set({
      selectedImageItemForPreview: item,
      showPreviewModal: true,
    });
  },
  closePreviewModal: () => {
    set({
      selectedImageItemForPreview: null,
      showPreviewModal: false,
    });
  },
  selectAll: () => {
    set((s) => ({
      previewData: s.previewData.map((item) => ({
        ...item,
        selected: true,
      })),
    }));
  },
  toggleSelectItem: (selectedItem: ImageItem) => {
    set((s) => ({
      previewData: s.previewData.map((item) =>
        item.previewUrl === selectedItem.previewUrl
          ? { ...item, selected: !item.selected }
          : item,
      ),
    }));
  },
  reset: () => {
    const { previewData } = get();
    previewData.forEach((item) => URL.revokeObjectURL(item.previewUrl));
    set({
      previewData: [],
      selectedImageItemForPreview: null,
      showPreviewModal: false,
    });
  },
}));
