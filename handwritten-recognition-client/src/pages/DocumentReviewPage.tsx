import { Grid, Paper, Typography } from "@mui/material";
import DocumentReviewer from "../components/DocumentReview/DocumentReviewer";
import { useDocumentsStore } from "../store/DocumentsStore";
import type { IDocumentsStoreModel } from "../model/DocumentsStoreModel";


const DocumentReviewPage = () => {
  const currentDocumentJobInfo=useDocumentsStore((s:IDocumentsStoreModel)=>s.currentDocumentJobInfo);
  const currentDocument=useDocumentsStore((s)=>s.currentDocumentData);
  return (
    <Grid container spacing={2} sx={{ padding: "30px" }}>
      <Paper elevation={2} sx={{ width: "100%" }}>
        <Grid size={12}>
          <Typography variant="h4">Revisar documentos procesados</Typography>
        </Grid>
        <Grid size={12}>
          <DocumentReviewer
            jobData={currentDocumentJobInfo}
            documentData={currentDocument}
          />
        </Grid>
      </Paper>
    </Grid>
  );
};
export default DocumentReviewPage;
