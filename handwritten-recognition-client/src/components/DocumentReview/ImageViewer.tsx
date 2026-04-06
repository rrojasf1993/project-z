import { useEffect, useRef } from "react";
import type { ImageViewerProps } from "../../model/props/ImageViewerProps";
import "./ImageViewer.css";
import { useOcrStore } from "../../store/OcrStore";
import type { IOcrStoreModel } from "../../model/OcrStoreModel";
import { useCanvasStore } from "../../store/CanvasStore";
import type { ICanvasStoreModel } from "../../model/CanvasStoreModel";
const ImageViewer = (props: ImageViewerProps) => {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const imageUrl = useOcrStore((s: IOcrStoreModel) => s.imageUrl);
  const setImageDims = useOcrStore((s: IOcrStoreModel) => s.setImageDimensions);
  const currImgWidth = useOcrStore((s: IOcrStoreModel) => s.imageWidth);
  const currImgHeight = useOcrStore((s: IOcrStoreModel) => s.imageHeight);
  const currOffset = useCanvasStore((s: ICanvasStoreModel) => s.offset);
  const currZoomLevel = useCanvasStore((s) => s.zoom);
  const configureBoundingBoxes = useCanvasStore(
    (s: ICanvasStoreModel) => s.configureBoundingBoxes,
  );
  const boundingBoxes = useCanvasStore((s: ICanvasStoreModel) => s.boxes);
  const configureImageSize = useCanvasStore(
    (s: ICanvasStoreModel) => s.setImageSize,
  );
  const configureImageViewer = () => {
    if (!canvasRef.current || !imageUrl) return;
    const canvasInstance = canvasRef.current;
    const ctx = canvasInstance.getContext("2d");
    const imageInstance = new Image();
    imageInstance.src = imageUrl;
    imageInstance.onload = () => {
      setImageDims(imageInstance.width, imageInstance.height);
      configureImageSize(imageInstance.width, imageInstance.height);
      canvasInstance.height = currImgHeight;
      canvasInstance.width = currImgWidth;
      ctx?.clearRect(0, 0, canvasInstance.width, canvasInstance.height);
      configureBoundingBoxes(props.lines);
      ctx?.drawImage(
        imageInstance,
        currOffset.x,
        currOffset.y,
        currImgWidth * currZoomLevel,
        currImgHeight * currZoomLevel,
      );
      boundingBoxes.forEach((bBox, index) => {
        const x = bBox.x * currImgWidth * currZoomLevel + currOffset.x;
        const y = bBox.y * currImgHeight * currZoomLevel + currOffset.y;
        const w = bBox.w * currImgWidth * currZoomLevel;
        const h = bBox.h * currImgHeight * currZoomLevel;
        ctx.strokeStyle = getBorderColorForBoundingBox(index);
        // eslint-disable-next-line @typescript-eslint/no-unused-expressions
        ((ctx.lineWidth = 2), ctx.strokeRect(x, y, w, h));
      });
    };
  };

  // const handleLineViewerTextFieldClick=(lineIndex: number): void =>{
  //       console.log(lineIndex);
  //       //props.handleTextBoxClick(lineIndex);
  // }

  const getClassForConfidenceLevel = (confidence: number): string => {
    if (confidence <= 0.6) {
      return "red";
    } else if (confidence >= 0.6 && confidence <= 0.8) {
      return "orange";
    } else {
      return "green";
    }
  };

  const getBorderColorForBoundingBox = (index: number): string => {
    const line = props.lines.find((s) => s.lineIndex === index);
    let defaultColor = "gray";
    if (line) {
      defaultColor = getClassForConfidenceLevel(line.confidenceScore);
    }
    return defaultColor;
  };

  useEffect(() => {
    configureImageViewer();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    imageUrl,
    currZoomLevel,
    currOffset,
    boundingBoxes,
    currImgHeight,
    currImgWidth,
  ]);

  return (
    <div className="imageReviewContainerDiv">
      <canvas ref={canvasRef}></canvas>
    </div>
  );
};
export default ImageViewer;
