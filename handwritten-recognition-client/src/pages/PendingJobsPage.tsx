import {
  AppBar,
  Box,
  Dialog,
  DialogContent,
  DialogTitle,
  Grid,
  IconButton,
  Paper,
  Toolbar,
  Typography,
} from "@mui/material";
import PendingJobs from "../components/Jobs/PendingJobs";
import OcrJobStatus from "../cross/enums/OcrJobStatus";
import type moment from "moment";
import { useCallback, useEffect, useState } from "react";
import type { OcrJobDto } from "../model/dto/OcrJobDto";
import LoadingComponent from "../cross/LoadingComponent";
import ImagePreviewer from "../components/ImagePreview/ImagePreviewer";
import { useOcrStore } from "../store/OcrStore";
import type { IOcrStoreModel } from "../model/OcrStoreModel";
import CloseIcon from "@mui/icons-material/Close";
import SimpleCodeViewer from "../cross/CodeViewer/SimpleCodeViewer";

const PendingJobsPage = () => {
  const getPendingJobsFromApi = useOcrStore((s: IOcrStoreModel) => s.getJobs);
  const isLoading = useOcrStore((s: IOcrStoreModel) => s.isLoading);
  const errorData = useOcrStore((s: IOcrStoreModel) => s.errorData);
  const currJobs = useOcrStore((s: IOcrStoreModel) => s.jobs);
  const getImageFromApi = useOcrStore((s: IOcrStoreModel) => s.getImage);
  const currImgUrl = useOcrStore((s: IOcrStoreModel) => s.imageUrl);

  const [showPreviewModal, setShowPreviewModal] = useState<boolean>(false);
  const [showErrorDetailsModal, setShowErrorDetailsModal] =
    useState<boolean>(false);
  const [errorDataFromChild, setShowErrorDataFromChild] = useState<string>("");

  const getPendingJobs = useCallback(
    (
      statusId: OcrJobStatus,
      startDate?: moment.Moment | null,
      endDate?: moment.Moment | null,
    ) => {
      getPendingJobsFromApi(statusId, startDate, endDate);
    },
    [],
  );

  const handleImagePreview = async (jobInfo: OcrJobDto) => {
    await getImageFromApi(jobInfo);
    setShowPreviewModal(true);
  };

  const handleClosePreviewModal = (): void => {
    setShowPreviewModal(false);
  };

  const handleCloseErrorDetailsModal = (): void => {
    setShowErrorDetailsModal(false);
  };

  useEffect(() => {
    getPendingJobs(OcrJobStatus.Pending);
  }, []);

  const showErrorDetails = (errorDetails: string): void => {
    setShowErrorDataFromChild(errorDetails);
    setShowErrorDetailsModal(true);
  };

  return (
    <Grid container spacing={2} sx={{ padding: "30px" }}>
      <Paper elevation={2} sx={{ width: "100%" }}>
        <Grid size={12}>
          <Grid size={12}>
            <Typography variant="h2">Jobs</Typography>
            <p>
              Aqui podra consultar los jobs por estado, gestione los filtros
              para consultar
            </p>
          </Grid>
          <>
            {isLoading ? (
              <LoadingComponent />
            ) : (
              <PendingJobs
                currentJobs={currJobs}
                handleSeeImage={handleImagePreview}
                handleSearchRequest={(
                  queryStatusId,
                  queryStartDate,
                  queryEndDate,
                ) =>
                  getPendingJobsFromApi(
                    queryStatusId,
                    queryStartDate,
                    queryEndDate,
                  )
                }
                handleSeeErrorDetails={(text) => showErrorDetails(text)}
              />
            )}
          </>
        </Grid>
        <Dialog maxWidth="lg" open={showPreviewModal} key={0}>
          <DialogTitle>
            <AppBar>
              <Toolbar>
                <Typography
                  variant="h6"
                  component="div"
                  sx={{ flexGrow: 1 }}
                ></Typography>
                Vista Previa de la imagen para el job Ocr
                <IconButton
                  size="small"
                  color="inherit"
                  onClick={handleClosePreviewModal}
                >
                  <CloseIcon />
                </IconButton>
              </Toolbar>
            </AppBar>
          </DialogTitle>
          <DialogContent>
            <Box>
              <Paper>
                <ImagePreviewer
                  imgSrc={currImgUrl}
                  imgDescription={"Vista previa de la imagen para el job Ocr "}
                />
              </Paper>
            </Box>
          </DialogContent>
        </Dialog>
        <Dialog
          maxWidth="lg"
          fullScreen={false}
          open={showErrorDetailsModal}
          key={1}
        >
          <DialogTitle>
            <Toolbar>
              <Typography
                variant="h6"
                component="div"
                sx={{ flexGrow: 1 }}
              ></Typography>
              Detalle del error
              <IconButton
                size="small"
                color="inherit"
                onClick={handleCloseErrorDetailsModal}
              >
                <CloseIcon />
              </IconButton>
            </Toolbar>
          </DialogTitle>
          <DialogContent>
            <Grid container>
              <Grid size={12}>
                <SimpleCodeViewer disabled={true} text={errorDataFromChild} />
              </Grid>
            </Grid>
          </DialogContent>
        </Dialog>
      </Paper>
    </Grid>
  );
};
export default PendingJobsPage;
