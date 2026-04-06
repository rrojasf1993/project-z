import {
  Grid,
  Paper,
  Snackbar,
  SnackbarContent,
  Stack,
  Typography,
} from "@mui/material";
import ImageUpload from "../components/ImageUploader/ImageUpload";
import type { ImageItem } from "../model/props/ImageItem";
import HttpService from "../services/HttpService";
import { useState } from "react";
import type { OcrJobDto } from "../model/dto/OcrJobDto";

const ImageUploaderPage: React.FC = () => {
  const [openProcessPopover, setOpenProcessPopover] = useState(false);
  const [currentJobsArray, setCurrentJobsArray] = useState<Array<OcrJobDto>>(
    [] as Array<OcrJobDto>,
  );

  const handleUploadComplete = async (
    fileItemsToUploadToServer: ImageItem[],
  ): Promise<void> => {
    const httpClientInstance: HttpService<
      Array<File>,
      Array<OcrJobDto>
    > = new HttpService();
    const url = "api/Ocr/ProcessOcrRequest";
    let result: Array<OcrJobDto> = [] as Array<OcrJobDto>;
    const dataToPost: File[] = fileItemsToUploadToServer.map((f) => {
      return f.file;
    });
    result = await httpClientInstance.DoPostMultipartFormData(url, dataToPost);
    if (result === null) {
      alert("Algo fallo");
    } else {
      setOpenProcessPopover(true);
      setCurrentJobsArray(result);
      console.log(result);
    }
  };

  return (
    <Grid container spacing={2} sx={{ padding: "30px" }}>
      <Paper elevation={2} sx={{ width: "100%", p: 2 }}>
        <Grid size={12}>
          <Typography variant="h4" gutterBottom>
            OCR documentos manuales
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Arrastre imágenes al área indicada o elija archivos. Use el lápiz
            para recortar o voltear antes de enviar al servidor.
          </Typography>
        </Grid>
        <Grid size={12}>
          <ImageUpload onUploadComplete={handleUploadComplete} />
        </Grid>
      </Paper>
      <Snackbar
        autoHideDuration={6000}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        open={openProcessPopover}
        onClose={() => setOpenProcessPopover(false)}
      >
        <Stack>
          {currentJobsArray.map((job, index) => {
            return (
              <SnackbarContent
                key={index}
                message={
                  "Se ha creado un job de reconocimiento Ocr Para el archivo: " +
                  job.fileName +
                  " Id Job " +
                  job.jobId
                }
              />
            );
          })}
        </Stack>
      </Snackbar>
    </Grid>
  );
};

export default ImageUploaderPage;
