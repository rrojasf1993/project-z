export interface IImageUploadProps {
  onUploadComplete: (imageUrl: string, file: File) => void;
}
