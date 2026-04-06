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
import { useState, type ChangeEvent } from "react";
import type { OcrDocumentDto } from "../../model/dto/OcrDocumentDto";
import type { OcrLineDto } from "../../model/dto/OcrLineDto";

const LineViewer = (props: ILineViewProps) => {
  const [modifiedDocument, setModifiedDocument] = useState<OcrDocumentDto>(
    props.documentData,
  );
  const [modifiedLine, setModifiedLine] = useState<OcrLineDto | undefined>();
  const [showPopup, setShowPopup] = useState<boolean>();
  const [disabledSaveButtonsIndexes, setDisableButtonIndexes] = useState<
    Array<number>
  >([]);
  const [defaultPopupMsg, setPopupMsg] = useState<string>("");

   
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

  const handleSaveCorrectedTextForLine = () => {
    const modifiableDocument = modifiedDocument;
    const currDisabledButtons = disabledSaveButtonsIndexes;
    if (modifiedLine) {
      const updatableLineIndex = modifiableDocument.lines.findIndex(
        (ul) => ul.lineIndex == modifiedLine?.lineIndex,
      );
      modifiableDocument.lines[updatableLineIndex] = modifiedLine;
      currDisabledButtons.push(modifiedLine.lineIndex);
      setPopupMsg(
        "Se han guardado exitosamente los cambios en la linea " +
          (modifiedLine?.lineIndex + 1) +
          " del documento",
      );
      setShowPopup(true);
    } else {
      setPopupMsg("No se ha hecho ningun cambio en las lineas reconocidas");
      setShowPopup(true);
    }
    setModifiedDocument(modifiableDocument);
    setDisableButtonIndexes(currDisabledButtons);
  };

  const handleChangeLineText = (
    e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    index: number,
  ): void => {
    const currLine = props.documentData.lines.find(
      (l) => l.lineIndex === index,
    );
    if (currLine) currLine.correctedText = e.currentTarget.value;
    setModifiedLine(currLine);
  };

  const handleUndoChangesForLine = (index: number): void => {
    const modifiableDocument = modifiedDocument;
    const disabledSaveButtons = disabledSaveButtonsIndexes;
    const updatableLineIndex = modifiableDocument.lines.findIndex(
      (ul) => ul.lineIndex == index,
    );
    if (updatableLineIndex != -1) {
      modifiableDocument.lines[updatableLineIndex].correctedText = null;
      disabledSaveButtons.splice(index, 1);
      setPopupMsg(
        "Se han deshecho exitosamente los cambios en la linea " +
          (index + 1) +
          "del documento",
      );
      setShowPopup(true);
    }
    setModifiedDocument(modifiableDocument);
    setDisableButtonIndexes(disabledSaveButtons);
  };

  const getDefaultTextForLine = (lineIndex: number) => {
    const currentLine = modifiedDocument.lines.at(lineIndex);
    if (currentLine) {
      if (currentLine.correctedText !== null) return currentLine.correctedText;
      return currentLine.originalText;
    }
  };

  const getComponentToRender = () => {
    let component = <h2>No hay lineas detectadas</h2>;
    if (props.documentData && props.documentData.lines) {
      component = (
        <>
          {props.documentData.lines.map((line) => {
            return (
              <Grid
                className="lineViewerLineContainerGrid"
                key={line.lineIndex}
              >
                <Grid size={12}>
                  <TextField
                    key={line.lineId}
                    focused={props.selectedLineIndex === line.lineIndex}
                    defaultValue={getDefaultTextForLine(line.lineIndex)}
                    onClick={() =>
                      handleLineViewerTextFieldClick(line.lineIndex)
                    }
                    onChange={(e) => handleChangeLineText(e, line.lineIndex)}
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
                    <IconButton
                      onClick={handleSaveCorrectedTextForLine}
                      disabled={disabledSaveButtonsIndexes.includes(
                        line.lineIndex,
                      )}
                    >
                      <CheckCircleOutlined />
                    </IconButton>
                    <IconButton
                      onClick={() => handleUndoChangesForLine(line.lineIndex)}
                      disabled={
                        !disabledSaveButtonsIndexes.includes(line.lineIndex)
                      }
                    >
                      <UndoRounded />
                    </IconButton>
                  </Grid>
                </Grid>
                <Snackbar
                  autoHideDuration={4000}
                  anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
                  open={modifiedLine && showPopup}
                  onClose={() => setShowPopup(false)}
                >
                  <SnackbarContent message={defaultPopupMsg} />
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
      <Paper className="paperContainer">
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
