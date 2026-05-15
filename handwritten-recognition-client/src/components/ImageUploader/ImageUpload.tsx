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
import EditIcon from "@mui/icons-material/Edit";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import { Image } from "@mui/icons-material";
import { useDropzone } from "react-dropzone";
import "@pqina/pintura/pintura.css";
import { getCropperDefaults } from "@pqina/pintura";
import type { PinturaDefaultImageWriterResult } from "@pqina/pintura";
import { PinturaEditor } from "@pqina/react-pintura";
import { useImageUploadStore } from "../../store/ImageUploadStore";

const ImageUpload: React.FC<IImageUploadProps> = ({ onUploadComplete }) => {
  const previewData = useImageUploadStore((s) => s.previewData);
  const showPreviewModal = useImageUploadStore((s) => s.showPreviewModal);
  const selectedImageItemForPreview = useImageUploadStore(
    (s) => s.selectedImageItemForPreview,
  );
  const addFiles = useImageUploadStore((s) => s.addFiles);
  const replaceImageItem = useImageUploadStore((s) => s.replaceImageItem);
  const openPreviewModal = useImageUploadStore((s) => s.openPreviewModal);
  const closePreviewModal = useImageUploadStore((s) => s.closePreviewModal);
  const selectAll = useImageUploadStore((s) => s.selectAll);
  const toggleSelectItem = useImageUploadStore((s) => s.toggleSelectItem);
  const reset = useImageUploadStore((s) => s.reset);

  const [pinturaItem, setPinturaItem] = useState<ImageItem | null>(null);

  const editorDefaults = useMemo(() => { 
    const pinturaConfig=getCropperDefaults()
    //pinturaConfig.locale="es-ES";
    return pinturaConfig;
  }, []);

  const onDropAccepted = useCallback(
    (acceptedFiles: File[]) => {
      if (acceptedFiles.length > 0) addFiles(acceptedFiles);
    },
    [addFiles],
  );

  const { getRootProps, getInputProps, isDragActive, isDragReject, fileRejections } =
    useDropzone({
      onDropAccepted,
      accept: { "image/*": [] },
      multiple: true,
    });

  const handleUpload = (): void => {
    const fileItemsToUploadToServer = previewData.filter((i) => i.selected);
    console.log("Uploading ", fileItemsToUploadToServer);
    onUploadComplete(fileItemsToUploadToServer);
  };

  const handlePreview = (item: ImageItem): void => {
    openPreviewModal(item);
  };

  const splitArray = (array: ImageItem[], chunkSize: number) => {
    const result = [];
    for (let i = 0; i < array.length; i += chunkSize) {
      result.push(array.slice(i, i + chunkSize));
    }
    return result;
  };

  const handleClosePreviewModal = (): void => {
    closePreviewModal();
  };

  const handleSelectAll = (): void => {
    selectAll();
  };

  const areAllSelected =
    previewData.length > 0 && previewData.every((item) => item.selected);

  const handleSelectItem = (selectedItem: ImageItem): void => {
    toggleSelectItem(selectedItem);
  };

  const getGridSizeForCardContainer = (arraySize: number): number => {
    const maxSize: number = 12;
    const definedSize = maxSize / arraySize;
    return definedSize;
  };

  const handlePinturaProcess = (detail: PinturaDefaultImageWriterResult) => {
    if (!pinturaItem) return;
    replaceImageItem(pinturaItem.previewUrl, detail.dest);
    setPinturaItem(null);
  };

  const renderImgsDiv = () => {
    let element = null;
    const arrays = splitArray(previewData, 4);
    element = (
      <>
        {arrays.map((element, index) => {
          return (
            <Grid key={index} container size={12} spacing={3}>
              {element.map((subElement) => {
                return (
                  <Grid
                    key={subElement.previewUrl}
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
                          <IconButton
                            onClick={() => setPinturaItem(subElement)}
                            aria-label="Recortar o voltear imagen"
                          >
                            <EditIcon />
                          </IconButton>
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
          <Typography variant="body1" sx={{ mb: 1 }}>
            Suba la(s) imagen(es) de lo(s) documento(s). Puede arrastrarlas o
            elegirlas desde su equipo.
          </Typography>
          <Box
            {...getRootProps()}
            sx={{
              width: "100%",
              minHeight: 160,
              border: "2px dashed",
              borderColor: isDragReject
                ? "error.main"
                : isDragActive
                  ? "primary.main"
                  : "divider",
              borderRadius: 1,
              bgcolor: isDragActive ? "action.hover" : "background.paper",
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              justifyContent: "center",
              cursor: "pointer",
              px: 2,
              py: 3,
              transition:
                "border-color 0.15s ease, background-color 0.15s ease",
            }}
          >
            <input {...getInputProps()} />
            <CloudUploadIcon
              sx={{ fontSize: 40, color: "primary.main", mb: 1 }}
              aria-hidden
            />
            <Typography align="center" color="text.secondary">
              {isDragActive
                ? "Suelte las imágenes aquí…"
                : "Arrastre imágenes aquí o haga clic para elegir"}
            </Typography>
            <Typography
              variant="caption"
              align="center"
              color="text.secondary"
              sx={{ mt: 0.5 }}
            >
              Formatos de imagen habituales (PNG, JPEG, …)
            </Typography>
            {fileRejections.length > 0 ? (
              <Typography
                variant="caption"
                color="error"
                sx={{ mt: 1, textAlign: "center" }}
              >
                Algunos archivos no son imágenes válidas o no se aceptaron.
              </Typography>
            ) : null}
          </Box>
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
        <Paper elevation={2} sx={{ width: "100%" }}>
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
                  imgSrc={selectedImageItemForPreview?.previewUrl ?? ""}
                  imgDescription={selectedImageItemForPreview?.file?.name ?? ""}
                />
              </Paper>
            </Box>
          </DialogContent>
        </Dialog>

        <Dialog
          fullScreen
          open={Boolean(pinturaItem)}
          onClose={() => setPinturaItem(null)}
        >
          <DialogTitle sx={{ p: 0 }}>
            <AppBar position="static" color="default" elevation={1}>
              <Toolbar>
                <Typography variant="h6" sx={{ flexGrow: 1 }}>
                  Editar imagen
                  {pinturaItem?.file?.name ? ` — ${pinturaItem.file.name}` : ""}
                </Typography>
                <IconButton
                  edge="end"
                  color="inherit"
                  onClick={() => setPinturaItem(null)}
                  aria-label="Cerrar editor"
                >
                  <CloseIcon />
                </IconButton>
              </Toolbar>
            </AppBar>
          </DialogTitle>
          <DialogContent sx={{ p: 0, height: "calc(100vh - 64px)" }}>
            {pinturaItem ? (
              <Box sx={{ height: "100%", minHeight: 480 }}>
                <PinturaEditor
                  {...editorDefaults}
                  key={pinturaItem.previewUrl}
                  src={pinturaItem.previewUrl}
                  onProcess={handlePinturaProcess}
                  onClose={() => setPinturaItem(null)}
                />
              </Box>
            ) : null}
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
              reset();
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
