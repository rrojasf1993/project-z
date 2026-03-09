import React from "react";
import type { IImageUploadProps } from "../../model/props/IImageUploadProps";
import { useState, type ChangeEvent } from "react";
import type { ImageItem } from "../../model/props/ImageItem";
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
import { Image } from "@mui/icons-material";
const ImageUpload: React.FC<IImageUploadProps> = ({ onUploadComplete }) => {
  const [previewData, setPreviewData] = useState<ImageItem[]>([]);
  const [showPreviewModal, setShowPreviewModal] = useState<boolean>(false);
  const [selectedImageItemForPreview, setShowSelectedImageItemForPreview] =
    useState<ImageItem>({} as ImageItem);
  const [selectedFileName, setSelectedFileName] = useState<string>("");

  const handleFileChange = (evt: ChangeEvent<HTMLInputElement>): void => {
    const uploadedFiles = Array.from(evt.target.files || []);
    const newImages = uploadedFiles.map((file) => ({
      file,
      previewUrl: URL.createObjectURL(file),
      selected: false,
    }));
    const images = [...previewData, ...newImages];
    setSelectedFileName(evt.target.value);
    setPreviewData(images);
  };

  const handleUpload = (): void => {
    const fileItemsToUploadToServer = previewData.filter((i) => {
      return i.selected;
    });
    console.log("Uploading ", fileItemsToUploadToServer);
    onUploadComplete(fileItemsToUploadToServer);
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

  const areAllSelected =
    previewData.length > 0 && previewData.every((item) => item.selected);

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

  const getGridSizeForCardContainer = (arraySize: number): number => {
    const maxSize: number = 12; //Col-md-12
    const definedSize = maxSize / arraySize;
    return definedSize;
  };

  const renderImgsDiv = () => {
    let element = null;
    const arrays = splitArray(previewData, 4);
    element = (
      <>
        {arrays.map((element, index) => {
          return (
            <Grid key={index} container size={12} spacing={3}>
              {element.map((subElement, index) => {
                return (
                  <Grid
                    key={index}
                    size={getGridSizeForCardContainer(element.length)}
                  >
                    <Card variant="outlined">
                      <CardContent>
                        <Image />
                        <Typography variant="h6">
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
          <p>Suba la(s) imagen(es) de lo(s) documento(s)</p>
          <label htmlFor="imageUpload">Adjunte las imagenes: </label>
          <input
            type="file"
            accept="image/*"
            multiple={true}
            placeholder="Archivos a cargar"
            id="imageUpload"
            value={selectedFileName}
            onChange={handleFileChange}
          />
        </Grid>
        <Grid size={12}>
          <FormControlLabel
            control={
              <Switch
                color="warning"
                onClick={() => handleSelectAll()}
                checked={areAllSelected}
              />
            }
            label="Seleccionar todos"
          />
        </Grid>
        <Paper elevation={2}>
          <div>{renderImgsDiv()}</div>
        </Paper>
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
      </Grid>
      <Grid container>
        <Grid size={6}>
          <Button
            color="primary"
            variant="contained"
            onClick={() => handleUpload()}
            content="Cargar Seleccionados"
          >
            Cargar seleccionadas
          </Button>
        </Grid>
        <Grid size={6}>
          <Button
            variant="outlined"
            color="warning"
            onClick={() => {
              setPreviewData([] as ImageItem[]);
              setSelectedFileName("");
            }}
          >
            Reiniciar
          </Button>
        </Grid>
      </Grid>
    </>
  );
};
export default ImageUpload;
