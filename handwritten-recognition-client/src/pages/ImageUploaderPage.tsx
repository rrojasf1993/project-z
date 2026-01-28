import { Grid, Paper, Typography } from "@mui/material";
import ImageUpload from "../components/ImageUploader/ImageUpload";

const ImageUploaderPage: React.FC = () => {
  const handleUploadComplete = (imageUrl: string, file: File): void => {
    throw new Error("Function not implemented.");
  };

  return (
    <Grid container spacing={2}>
      <Paper elevation={3}>
        <Grid size={12}>
          <Typography variant="h3">OCr Multas Manuales</Typography>
        </Grid>
        <Grid size={12}>
          <ImageUpload onUploadComplete={handleUploadComplete} />
        </Grid>
      </Paper>
    </Grid>
  );
};

export default ImageUploaderPage;
