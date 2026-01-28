import type React from "react";
import type { IImageUploadProps } from "../../model/props/IImageUploadProps";
import { useState, type ChangeEvent } from "react";
import type { ImageItem } from "../../model/props/ImageItem";
import imageIcon from "./ImageIcon.png";
import ImagePreviewer from "../ImagePreview/ImagePreviewer";
import {
  AppBar,
  Box,
  Button,
  Card,
  CardActionArea,
  CardActions,
  CardContent,
  CardMedia,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControlLabel,
  Grid,
  IconButton,
  Paper,
  Switch,
  Toolbar,
  Typography,
} from "@mui/material";
import VisibilityIcon from "@mui/icons-material/Visibility";
import CloseIcon from "@mui/icons-material/Close";
const ImageUpload: React.FC<IImageUploadProps> = ({ onUploadComplete }) => {
  /* const useStyles = makeStyles({

  });*/
  //const classes = useStyles();
  const [previewData, setPreviewData] = useState<ImageItem[]>([]);
  const [showPreviewModal, setShowPreviewModal] = useState<boolean>(false);
  const [selectedImageItemForPreview, setShowSelectedImageItemForPreview] =
    useState<ImageItem>({} as ImageItem);

  const handleFileChange = (evt: ChangeEvent<HTMLInputElement>): void => {
    const uploadedFiles = Array.from(evt.target.files || []);
    const newImages = uploadedFiles.map((file) => ({
      file,
      previewUrl: URL.createObjectURL(file),
      selected: false,
    }));
    const images = [...previewData, ...newImages];
    setPreviewData(images);
  };

  const handleUpload = (): void => {
    const fileItemsToUploadToServer = previewData.filter((i) => {
      return i.selected;
    });
    console.log("Uploading ", fileItemsToUploadToServer);
    //onUploadComplete()
  };

  const handlePreview = (item: ImageItem): void => {
    setShowSelectedImageItemForPreview(item);
    setShowPreviewModal(!showPreviewModal);
  };

  const splitArray = (array: ImageItem[], chunkSize: number) => {
    const result = [];
    for (let i = 0; i < array.length; i += chunkSize) {
      result.push(array.slice(i, i + chunkSize));
    }
    return result;
  };

  const handleClosePreviewModal = (): void => {
    setShowPreviewModal(false);
    setShowSelectedImageItemForPreview({} as ImageItem);
  };

  const handleSelectAll = (): void => {
    const updatedPreviewData = previewData.map((item) => ({
      ...item,
      selected: true,
    }));
    setPreviewData(updatedPreviewData);
  };

  const areAllSelected = previewData.length > 0 && previewData.every((item) => item.selected);

  const handleSelectItem = (selectedItem: ImageItem): void => {
    const index = previewData.findIndex((pd) => pd === selectedItem);
    if (index !== -1) {
      const updatedPreviewData = [...previewData];
      updatedPreviewData[index] = {
        ...updatedPreviewData[index],
        selected: !updatedPreviewData[index].selected,
      };
      setPreviewData(updatedPreviewData);
    }
  };

  const renderImgsDiv = () => {
    let element = null;
    const arrays = splitArray(previewData, 3);
    element = (
      <>
        {arrays.map((element, index) => {
          return (
            <Grid key={index} container spacing={3}>
              {element.map((subElement, index) => {
                return (
                  <Grid key={index} size={element.length}>
                    <Card variant="outlined">
                      <CardContent>
                        <Typography variant="h6">
                          <img src={imageIcon} alt="icon" />
                          <b>{subElement.file.name}</b>
                        </Typography>
                      </CardContent>
                      <CardMedia
                        component="img"
                        height="140"
                        width="140"
                        src={subElement.previewUrl}
                        alt={"Vista previa de ;" + subElement.file.name}
                      />

                      <CardActionArea>
                        <CardActions>
                          <IconButton onClick={() => handlePreview(subElement)}>
                            <VisibilityIcon />
                          </IconButton>
                          <FormControlLabel
                            control={
                              <Switch
                                onClick={() => handleSelectItem(subElement)}
                                checked={subElement.selected}
                              />
                            }
                            label="Seleccionar "
                          />
                        </CardActions>
                      </CardActionArea>
                    </Card>
                  </Grid>
                );
              })}
            </Grid>
          );
        })}
      </>
    );
    return element;
  };

  return (
    <>
      <Grid container spacing={2}>
        <Grid size={12}>
          <Paper>
            <Typography variant="h3" gutterBottom>
              Suba la(s) imagen(es) de la(s) multa(s)
            </Typography>
          </Paper>
          <Paper>
            <label htmlFor="imageUpload">Adjunte las imagenes: </label>
            <input
              type="file"
              accept="image/*"
              multiple={true}
              placeholder="Archivos a cargar"
              id="imageUpload"
              onChange={handleFileChange}
            />
          </Paper>
        </Grid>
        <Grid size={12}>
          <Paper>
            <FormControlLabel
              control={<Switch color="warning" onClick={() => handleSelectAll()} checked={areAllSelected} />}
              label="Seleccionar todos"
            />
          </Paper>
        </Grid>
        <hr></hr>
        <div>{renderImgsDiv()}</div>
        <Dialog fullScreen maxWidth="lg" open={showPreviewModal}>
          <DialogTitle>
            <AppBar>
              <Toolbar>
                <Typography
                  variant="h6"
                  component="div"
                  sx={{ flexGrow: 1 }}
                ></Typography>
                Vista Previa de {selectedImageItemForPreview?.file?.name}
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
                  imgSrc={selectedImageItemForPreview?.previewUrl}
                  imgDescription={selectedImageItemForPreview?.file?.name}
                />
              </Paper>
            </Box>
          </DialogContent>
        </Dialog>
        <hr></hr>
        <Grid container>
          <Grid size={12} spacing={2}>
            <Paper>
              <Button color="primary" variant="contained">
                Cargar seleccionadas
              </Button>
              <Button variant="outlined" color="warning">
                Reiniciar
              </Button>
            </Paper>
          </Grid>
        </Grid>
      </Grid>
    </>
  );
};
export default ImageUpload;
