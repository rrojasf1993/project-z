import { CheckCircleOutlined, UndoRounded } from "@mui/icons-material";
import {
  Grid,
  IconButton,
  Paper,
  Snackbar,
  SnackbarContent,
  TextField,
} from "@mui/material";
import "./LineViewer.css";
import type { ILineViewProps } from "../../model/props/LineViewProps";
import type { OcrLineDto } from "../../model/dto/OcrLineDto";
import { useEffect, useState } from "react";
import type { IDocumentsStoreModel } from "../../model/DocumentsStoreModel";
import { useDocumentsStore } from "../../store/DocumentsStore";
import { useOcrStore } from "../../store/OcrStore";
import type { IOcrStoreModel } from "../../model/OcrStoreModel";

const LineViewer = (props: ILineViewProps) => {
  const modifiableDocument = useDocumentsStore(
    (s: IDocumentsStoreModel) => s.modifiedDocumentData,
  );
  const setModifiedDocumentValue = useDocumentsStore(
    (s: IDocumentsStoreModel) => s.setModifiedDocumentData,
  );
  const setPopoverMessage=useDocumentsStore((s:IDocumentsStoreModel)=>s.setLineModifyMessage);
  const linePopoverMsg=useDocumentsStore((s:IDocumentsStoreModel)=>s.lineModifyMessage);
  const currImgHeight = useOcrStore((s: IOcrStoreModel) => s.imageHeight);
  const [modified,setModified]=useState<boolean|null>(null);
  const [showPopover, setShowPopover]=useState<boolean>(false);
  useEffect(() => {
    console.log("LineViewer-lines",JSON.stringify(props.documentData.lines));
    if(modified ===null)
      setModifiedDocumentValue(props.documentData);
  },[modified, props.documentData, setModifiedDocumentValue] );

  console.log("Modified document", modifiableDocument);

  const getClassForConfidenceLevel = (confidence: number): string => {
    if (confidence <= 0.6) {
      return "lineLabelError";
    } else if (confidence >= 0.6 && confidence <= 0.8) {
      return "lineLabelWarning";
    } else {
      return "lineLabelOk";
    }
  };

  const handleLineViewerTextFieldClick = (lineIndex: number): void => {
    console.log(lineIndex);
    props.handleTextBoxClick(lineIndex);
  };

  const handleSaveCorrectedTextForLine = (line:OcrLineDto) => {
    const saveDocumentInstance = modifiableDocument;
    const modifiedLine = saveDocumentInstance.lines.find(
      (l) => l.lineId === line.lineId,
    );
    if (modifiedLine && line.correctedText!== modifiedLine.originalText) {
      modifiedLine.changesSaved = true;
      setPopoverMessage("Se han guardado los cambios para la linea "+ (line.lineIndex+1));
      setShowPopover(true);
    }
    setModifiedDocumentValue(saveDocumentInstance);
    setModified(true);
  };

  const handleChangeLineText = (
    lineInstance: OcrLineDto,
    text: string,
  ): void => {
    const changes = modifiableDocument;
    const modifiedLine = changes.lines.find(
      (l) => l.lineId === lineInstance.lineId,
    );
    if (modifiedLine && text !== modifiedLine.originalText) {
      modifiedLine.touched = true;
      modifiedLine.correctedText = text;
    }
    setModifiedDocumentValue(changes);
  };

  const handleUndoChangesForLine = (line: OcrLineDto): void => {
    const changes = modifiableDocument;
    const modifiedLine = changes.lines.find(
      (l) => l.lineId === line.lineId,
    );
    if (modifiedLine && modifiedLine.changesSaved) {
      modifiedLine.touched = false;
      modifiedLine.correctedText = "";
      modifiedLine.changesSaved=false;
    }
    setModifiedDocumentValue(changes);
    setModified(false);
  };

  const getDefaultTextForLine = (line: OcrLineDto) => {
    if ((line.touched || line.changesSaved) && line.correctedText!=="") return line.correctedText;
    return line.originalText;
  };

  const getComponentToRender = () => {
    let component = <h2>No hay lineas detectadas</h2>;
    if (modifiableDocument && modifiableDocument.lines) {
      component = (
        <>
          {modifiableDocument.lines.map((line) => {
            return (
              <Grid
                className="lineViewerLineContainerGrid"
                key={line.lineIndex}
              >
                <Grid size={12}>
                  <TextField
                    key={line.lineId}
                    focused={props.selectedLineIndex === line.lineIndex}
                    disabled={line.changesSaved}
                    defaultValue={getDefaultTextForLine(line)}
                    onClick={() =>
                      handleLineViewerTextFieldClick(line.lineIndex)
                    }
                    onChange={(e) =>
                      handleChangeLineText(line, e.currentTarget.value)
                    }
                    sx={{ width: "100%" }}
                    className="textField"
                  ></TextField>
                </Grid>
                <Grid container>
                  <Grid size={4}>
                    <label>
                      <b>
                        <i>Porcentaje de Confianza %:</i>
                      </b>{" "}
                    </label>
                    <b
                      className={getClassForConfidenceLevel(
                        line.confidenceScore,
                      )}
                    >
                      {(line.confidenceScore * 100).toFixed(2)}
                    </b>
                  </Grid>
                  <Grid size={4}>
                    <label>
                      <b>
                        <i>Estado: </i>
                      </b>
                    </label>
                    <b
                      className={getClassForConfidenceLevel(
                        line.confidenceScore,
                      )}
                    >
                      {line.confidenceStatus}
                    </b>
                  </Grid>
                  <Grid size={4}>
                    <IconButton onClick={()=>handleSaveCorrectedTextForLine(line)} disabled={line.changesSaved}>
                      <CheckCircleOutlined />
                    </IconButton>
                    <IconButton
                      onClick={() => handleUndoChangesForLine(line)}
                      disabled={!line.changesSaved}
                    >
                      <UndoRounded />
                    </IconButton>
                  </Grid>
                </Grid>
                <Snackbar
                  autoHideDuration={4000}
                  open={showPopover}
                  onClose={()=>{
                    setPopoverMessage("");
                    setShowPopover(false);
                  }}
                  anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
                >
                  <SnackbarContent message={linePopoverMsg} />
                </Snackbar>
              </Grid>
            );
          })}
        </>
      );
    }
    return component;
  };

  return (
    <>
      <Paper className="paperContainer" sx={{height:currImgHeight}}>
        <Grid container>
          <Grid size={12}>
            <>{getComponentToRender()}</>
          </Grid>
        </Grid>
      </Paper>
    </>
  );
};
export default LineViewer;
