import { Button, Grid } from "@mui/material";
import type { IDocumentReviewProps } from "../../model/props/DocumentReviewProps";
import ImageViewer from "./ImageViewer";
import { useEffect, useState, type JSX } from "react";
import LineViewer from "./LineViewer";
import { useOcrStore } from "../../store/OcrStore";
import type { IOcrStoreModel } from "../../model/OcrStoreModel";
import "./DocumentReviewer.css";
import LoadingComponent from "../../cross/LoadingComponent";
const DocumentReviewer = (props: IDocumentReviewProps) => {
  const [selectedBox, setSelectedBox] = useState<number>(0);

  const getImageForRender = useOcrStore((s: IOcrStoreModel) => s.getImage);
  const currentImgUrl = useOcrStore((s: IOcrStoreModel) => s.imageUrl);
  const isLoading = useOcrStore((s) => s.isLoading);
  useEffect(() => {
    getImageToRender();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.jobData, props.documentData]);

  const getImageToRender = async () => {
    await getImageForRender(props.jobData);
    console.log("url" + currentImgUrl);
    setSelectedBox(0);
  };

  const getComponentToRender = (): JSX.Element => {
    let component = <LoadingComponent/>;
    if (props.jobData && props.documentData && !isLoading)
      component = (
        <>
          <Grid container>
            <Grid size={6}>
              <ImageViewer
                imageUrl={currentImgUrl}
                lines={props.documentData.lines}
                selectedBoxId={selectedBox}
                onFieldSelect={handleFieldSelect}
                onFieldFocus={() => {
                  console.log("aaaa");
                }}
              ></ImageViewer>
            </Grid>
            <Grid size={6}>
              <LineViewer
                documentData={props.documentData}
                selectedLineIndex={selectedBox}
                handleTextBoxClick={handleFieldSelect}
              />
            </Grid>
          </Grid>
          <Grid container className="actionsContainer">
            <Grid size={6}>
              <Button variant="outlined" color="warning">
                Regresar a la busqueda de documentos pendientes
              </Button>
            </Grid>
            <Grid size={6}>
              <Button variant="contained" color="success">
                Extraer informacion!
              </Button>
            </Grid>
          </Grid>
        </>
      );
    return component;
  };

  const handleFieldSelect = (lineIndex: number) => {
    setSelectedBox(lineIndex);
  };

  return (
    <Grid container>
      <Grid size={12}>
        <div>
          <p>
            Revise la informacion reconocida por el Ocr y corrija de acuerdo a
            su necesidad
          </p>
          {getComponentToRender()}
        </div>
      </Grid>
    </Grid>
  );
};
export default DocumentReviewer;
