import { Button, Grid, Snackbar, SnackbarContent } from "@mui/material";
import type { IDocumentReviewProps } from "../../model/props/DocumentReviewProps";
import ImageViewer from "./ImageViewer";
import { useEffect, useState, type JSX } from "react";
import LineViewer from "./LineViewer";
import { useOcrStore } from "../../store/OcrStore";
import type { IOcrStoreModel } from "../../model/OcrStoreModel";
import "./DocumentReviewer.css";
import LoadingComponent from "../../cross/LoadingComponent";
import { useNavigate } from "react-router-dom";
import type { IDocumentsStoreModel } from "../../model/DocumentsStoreModel";
import { useDocumentsStore } from "../../store/DocumentsStore";
const DocumentReviewer = (props: IDocumentReviewProps) => {
  const [selectedBoxIndex, setSelectedBoxIndex] = useState<number>(0);
  const getImageForRender = useOcrStore((s: IOcrStoreModel) => s.getImage);
  const currentImgUrl = useOcrStore((s: IOcrStoreModel) => s.imgUrl);
  const isLoading = useOcrStore((s) => s.isLoading);
  const setIsLoading = useOcrStore((s) => s.setIsLoading);

  const [snackbarMsg, setSnackbarMsg] = useState<string>("");
  const [showSnackbar, setShowSnackbar] = useState<boolean>(false);

  const modifiableDocument = useDocumentsStore(
    (s: IDocumentsStoreModel) => s.modifiedDocumentData,
  );
  const updateCorrections = useDocumentsStore(
    (s: IDocumentsStoreModel) => s.updateDocumentWithCorrections,
  );
  const navigateInstance = useNavigate();
  useEffect(() => {
    getImageToRender();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.jobData, props.documentData]);

  const getImageToRender = async () => {
    await getImageForRender(props.jobData);
    console.log("url" + currentImgUrl);
    setSelectedBoxIndex(0);
  };

  const handleNavigateToPendingDocumentsQuery = async (): Promise<void> => {
    setIsLoading(true);
    await navigateInstance({ pathname: "/documents-pending-review" });
  };

  const handleSaveDocumentModification = async () => {
    await updateCorrections(modifiableDocument);
    setSnackbarMsg(
      `Se han guardado exitosamente las correcciones hechas al documento con id: ${modifiableDocument.id}`,
    );
    setShowSnackbar(true);
  };

  const getComponentToRender = (): JSX.Element => {
    let component = <LoadingComponent />;
    if (props.jobData && props.documentData && !isLoading)
      component = (
        <>
          <Grid container>
            <Grid size={6}>
              <ImageViewer
                imageUrl={currentImgUrl}
                lines={props.documentData.lines}
                onFieldSelect={handleFieldSelect}
              ></ImageViewer>
            </Grid>
            <Grid size={6}>
              <LineViewer
                documentData={props.documentData}
                selectedLineIndex={selectedBoxIndex}
                handleTextBoxClick={handleFieldSelect}
              />
            </Grid>
          </Grid>
          <Grid container className="actionsContainer">
            <Grid size={6}>
              <Button
                variant="outlined"
                color="warning"
                onClick={async () => {
                  await handleNavigateToPendingDocumentsQuery();
                }}
              >
                Regresar a la busqueda de documentos pendientes
              </Button>
            </Grid>
            <Grid size={6}>
              <Button
                variant="contained"
                color="success"
                onClick={async () => {
                  await handleSaveDocumentModification();
                }}
              >
                Extraer informacion!
              </Button>
            </Grid>
          </Grid>
        </>
      );
    return component;
  };

  const handleFieldSelect = (lineIndex: number) => {
    setSelectedBoxIndex(lineIndex);
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
      <Snackbar
        autoHideDuration={6000}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        open={showSnackbar}
        onClose={async () => {
          setShowSnackbar(false);
          setIsLoading(true);
          await navigateInstance({ pathname: "/documents-pending-review" });
        }}
      >
        <SnackbarContent message={snackbarMsg} />
      </Snackbar>
    </Grid>
  );
};
export default DocumentReviewer;
