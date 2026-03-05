import { Grid, Paper, Skeleton, Typography } from "@mui/material";
import PendingDocumentsLoad from "../components/PendingDocuments/PendingDocumentsLoad";
import HttpService from "../services/HttpService";
import type { OcrDocumentDto } from "../model/dto/OcrDocumentDto";
import { useEffect, useState } from "react";

const PendingDocumentsPage = () => {
  const [documents, setDocuments] = useState<Array<OcrDocumentDto>>(
    [] as Array<OcrDocumentDto>,
  );
  const [showWaitingUi, setShowWaitingUi] = useState<boolean>(false);

  useEffect(()=>{
    geDocumentsPendingReview()
  }, []);

  const geDocumentsPendingReview = async () => {
    const httpClientInstance: HttpService<
      string,
      Array<OcrDocumentDto>
    > = new HttpService();
    setShowWaitingUi(true);
    const pendingStatusId=0;
    const url: string = `api/documents/GetDocumentsByStatus/${pendingStatusId}`;
    const result = await httpClientInstance.DoGet(url);
    if (result === null) {
      alert("Algo fallo obteniendo los documentos ");
    } else {
      setShowWaitingUi(false);
      setDocuments(result);
    }
  };

  const handleReview=()=>{

  }

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
          <>{showWaitingUi ? <Skeleton animation={"pulse"}  /> : <PendingDocumentsLoad documentsList={documents} onReviewAction={handleReview} />}</>
        </Grid>
      </Paper>
    </Grid>
  );
};
export default PendingDocumentsPage;
