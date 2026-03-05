import {
  Apps,
  Calculate,
  CalendarToday,
  EditDocument,
  FileOpen,
  RateReviewRounded,
  TableRows,
} from "@mui/icons-material";
import Folder from "@mui/icons-material/Folder";
import {
  Grid,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableFooter,
  TableHead,
  TableRow,
} from "@mui/material";
import type { PendingDocumentListProps } from "../../model/props/PendingDocumentsProps";

const PendingDocumentsLoad = (props:PendingDocumentListProps) => {
  return (
    <>
      <Grid container spacing={2}>
        <Grid size={12}>
          <Paper sx={{ width: "100%", overflow: "hidden" }}>

          </Paper>
        </Grid>
      </Grid>
    </>
  );
};
export default PendingDocumentsLoad;
