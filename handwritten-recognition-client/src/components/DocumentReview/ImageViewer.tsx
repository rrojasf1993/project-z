import { useEffect, useRef } from "react";
import type { ImageViewerProps } from "../../model/props/ImageViewerProps";
import "./ImageViewer.css";
import { useOcrStore } from "../../store/OcrStore";
import type { IOcrStoreModel } from "../../model/OcrStoreModel";
import { useCanvasStore } from "../../store/CanvasStore";
import type { ICanvasStoreModel } from "../../model/CanvasStoreModel";
import { IconButton, Paper, Toolbar } from "@mui/material";
import { Restore, ZoomIn, ZoomOut } from "@mui/icons-material";
const ImageViewer = (props: ImageViewerProps) => {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const imageData = useOcrStore((s: IOcrStoreModel) => s.imageBmp);
  const imageUrl = useOcrStore((s: IOcrStoreModel) => s.imgUrl);
  const setImageDims = useOcrStore((s: IOcrStoreModel) => s.setImageDimensions);
  const currImgWidth = useOcrStore((s: IOcrStoreModel) => s.imageWidth);
  const currImgHeight = useOcrStore((s: IOcrStoreModel) => s.imageHeight);
  const currOffset = useCanvasStore((s: ICanvasStoreModel) => s.offset);
  const currZoomLevel = useCanvasStore((s) => s.zoom);
  const setZoomLevel = useCanvasStore((s) => s.setZoom);

  const configureBoundingBoxes = useCanvasStore(
    (s: ICanvasStoreModel) => s.configureBoundingBoxes,
  );
  const boundingBoxes = useCanvasStore((s: ICanvasStoreModel) => s.boxes);
  const configureImageSize = useCanvasStore(
    (s: ICanvasStoreModel) => s.setImageSize,
  );
  const configureImageViewer = () => {
    if (!canvasRef.current || !imageData) return;
    const canvasInstance = canvasRef.current;
    const ctx = canvasInstance.getContext("2d");
    const imageInstance = new Image();
    imageInstance.src = imageUrl;
    imageInstance.onload = () => {
      setImageDims(imageInstance.width, imageInstance.height);
      configureImageSize(imageInstance.width, imageInstance.height);
      canvasInstance.height = imageInstance.naturalHeight;
      canvasInstance.width = imageInstance.naturalWidth;
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
  }); // [
  //   imageUrl,
  //   currZoomLevel,
  //   currOffset,
  //   boundingBoxes,
  //   currImgHeight,
  //   currImgWidth,
  // ]);

  const handleCanvasClick = ({
    event,
  }: {
    event: MouseEvent<HTMLCanvasElement, MouseEvent>;
  }): void => {
    const rectangle = canvasRef.current?.getBoundingClientRect();
    const x = event.clientX - rectangle?.left;
    const y = event.clientY - rectangle?.top;
    const { boxes, imageSize, zoom, offset } = useCanvasStore.getState();
    const clickedBoundignBox = boxes.find((box) => {
      const bx = box.x * imageSize.width * zoom + offset.x;
      const by = box.y * imageSize.height * zoom + offset.y;
      const bw = box.w * imageSize.width * zoom;
      const bh = box.h * imageSize.height * zoom;
      return x >= bx && x <= bx + bw && y >= by && y <= by + bh;
    });
    if (clickedBoundignBox) {
      console.log(JSON.stringify(clickedBoundignBox));
      props.onFieldSelect(boxes.indexOf(clickedBoundignBox));
    }
  };

  return (
    <>
      <Paper>
        <Toolbar variant="regular" className="imgToolbar">
          <IconButton  onClick={()=>{
              const curr=currZoomLevel;
              setZoomLevel(curr+1);
          }}>
            <ZoomIn />
          </IconButton>
          <IconButton disabled={currZoomLevel<=1} onClick={()=>{
            const curr=currZoomLevel;
              setZoomLevel(curr-1);
          }}>
            <ZoomOut />
          </IconButton>
          <IconButton disabled={currZoomLevel===1} onClick={
            ()=>{
              setZoomLevel(1);
            }
          }>
            <Restore />
          </IconButton>
        </Toolbar>
      </Paper>
      <div className="imageReviewContainerDiv">
        <canvas
          ref={canvasRef}
          onClick={(e) => handleCanvasClick({ event: e })}
        ></canvas>
      </div>
    </>
  );
};
export default ImageViewer;
