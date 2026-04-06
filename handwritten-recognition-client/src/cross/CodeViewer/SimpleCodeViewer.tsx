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
            disabled={props.disabled}
            variant="outlined"
            sx={{
              width: "100%",
              "& .MuiInputBase-input": {
                whiteSpace: "pre",
                fontFamily: "Consolas, 'Courier New', monospace",
              },
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
