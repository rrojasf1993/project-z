import { Grid, Paper, TextField } from "@mui/material";
import type { SimpleCodeViewerProps } from "../../model/props/SimpleCodeViewerProps";
import "./SimpleCodeViewer.css";
const SimpleCodeViewer = (props: SimpleCodeViewerProps) => {
  return (
    <>
      <Grid size={12}>
        <Paper elevation={2} className="codeEditorTextAreaContainerPaper">
          <TextField
            multiline
            fullWidth
            placeholder={props.text}
            disabled={false}
            variant="filled"
            sx={{
              width: "620px",
              "& .MuiInputBase-input": {
                whiteSpace: "pre",
                fontFamily: "Consolas, 'Courier New', monospace",
              },
              overflowX:"scroll",
              maxWidth:"620px",
              overflowY:"scroll"
            }}
          >
            {props.text}
          </TextField>
        </Paper>
      </Grid>
    </>
  );
};
export default SimpleCodeViewer;
