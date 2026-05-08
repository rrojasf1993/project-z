import { Grid, Paper, Typography } from "@mui/material";
import PendingDocumentsLoad from "../components/PendingDocuments/PendingDocumentsLoad";
import type { OcrDocumentDto } from "../model/dto/OcrDocumentDto";
import {  useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useDocumentsStore } from "../store/DocumentsStore";
import type { IDocumentsStoreModel } from "../model/DocumentsStoreModel";
import LoadingComponent from "../cross/LoadingComponent";

const PendingDocumentsPage = () => {
  const isLoading=useDocumentsStore((s:IDocumentsStoreModel)=>s.isLoading);
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const errorData=useDocumentsStore((s:IDocumentsStoreModel)=>s.errorData);
  const pendingDocs=useDocumentsStore((s:IDocumentsStoreModel)=>s.pendingDocuments);
  const getPendingDocs=useDocumentsStore((s:IDocumentsStoreModel)=>s.getPendingDocuments);
  const getJobDataForCurrDoc=useDocumentsStore((s:IDocumentsStoreModel)=>s.getJobInfoForDocument);
  const setCurrentDocInfo=useDocumentsStore((s:IDocumentsStoreModel)=>s.setCurrentDocumentData);
  const navigate = useNavigate();

  useEffect(() => {
    getDocumentsPendingReview(null,null)
  },[]);

 const getDocumentsPendingReview = (startDate: moment.Moment | null,endDate: moment.Moment | null) => {
    getPendingDocs(startDate, endDate);
  };
 const getJobInfoForDocument = async (documentId: string) => {
  await getJobDataForCurrDoc(documentId);
};

  const handleReview = async (docInfo: OcrDocumentDto) => {
    await getJobInfoForDocument(docInfo.id);
    setCurrentDocInfo(docInfo);
    navigate({ pathname: "/document-review" });
  };

  return (
    <Grid container spacing={2} sx={{ padding: "30px" }}>
      <Paper elevation={2} sx={{ width: "100%" }}>
        <Grid size={12}>
          <Grid size={12}>
            <Typography variant="h2">
              Documentos pendientes de revision
            </Typography>
            <p>
              Aqui podra ver los documentos pendientes de revision, gestione el
              filtro para poder consultar y revisarlos
            </p>
          </Grid>
          <>
            {isLoading ? (
              <LoadingComponent/>
            ) : (
              <PendingDocumentsLoad
                documentsList={pendingDocs}
                onReviewAction={handleReview}
                handleSearch={getDocumentsPendingReview}
              />
            )}
          </>
        </Grid>
      </Paper>
    </Grid>
  );
};
export default PendingDocumentsPage;
