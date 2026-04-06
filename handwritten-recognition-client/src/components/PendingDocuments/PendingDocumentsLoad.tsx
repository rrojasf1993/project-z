import { useCallback, useState } from "react";
import type { PendingDocumentListProps } from "../../model/props/PendingDocumentsProps";
import type { OcrDocumentDto } from "../../model/dto/OcrDocumentDto";
import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import {
  Box,
  Dialog,
  DialogContent,
  DialogTitle,
  Grid,
  IconButton,
  Paper,
  Typography,
} from "@mui/material";
import {
  DetailsRounded,
  NewspaperTwoTone,
  RateReviewTwoTone,
  SearchOutlined,
} from "@mui/icons-material";
import moment from "moment";
import { DatePicker } from "@mui/x-date-pickers";
import LinesDetails from "../DocumentDetails/LinesDetail";
import CloseIcon from "@mui/icons-material/Close";
const PendingDocumentsLoad = (props: PendingDocumentListProps) => {
  const columns: GridColDef<OcrDocumentDto>[] = [
    {
      field: "id",
      headerName: "ID Documento",
      sortable: false,
      description: "Id unico del documento",
      width:360
    },
    {
      field: "createdAt",
      headerName: "Fecha Creacion",
      description:
        "Fecha de creacion del documento por el motor de extraccion Ocr",
      valueGetter: (value, row): string =>
        `${moment(row.createdAt).format("YYYY-MM-DD:hh:mm")}`,
      width:220
    },
    {
      field: "confidenceAvg",
      headerName: "Confianza Promedio %",
      description: "Confianza promedio del proceso de extraccion Ocr",
      valueGetter: (value, row) => `${(row.confidenceAvg * 100).toFixed(2)}`,
      width:220
    },
    {
      field: "lines",
      headerName: "Lineas Reconocidas",
      description: "Cantidad de lineas reconocidas por el motor Ocr",
      sortable: true,
      valueGetter: (value, row) => `${row.lines.length}`,
      width:220
    },
    {
      field: "Accion",
      headerName: "Acciones",
      description: "Acciones",
      sortable: false,
      filterable: false,
      disableColumnMenu: true,
      width:120,
      renderCell: (params) => {
        return (
          <>
            <div>
              <IconButton onClick={() => handleReviewRequest(params.row)}>
                <RateReviewTwoTone sx={{color:"green"}} />
              </IconButton>
              <IconButton onClick={() => handleShowDetails(params.row)}>
                <NewspaperTwoTone  sx={{color:"darkorange"}}/>
              </IconButton>
            </div>
          </>
        );
      },
    },
  ];

  const handleReviewRequest = useCallback((ocrDocument: OcrDocumentDto) => {
    console.log("ocrDocument", ocrDocument);
    props.onReviewAction(ocrDocument);
  }, [props]);

  const handleShowDetails = useCallback((ocrDocument: OcrDocumentDto) => {
    console.log("ocrDocument", ocrDocument);
    setCurrentDocument(ocrDocument);
    setIsDetailsOpen(true);
  }, []);

  const [startDate, setStartDate] = useState<moment.Moment | null>(null);
  const [endDate, setEndDate] = useState<moment.Moment | null>(null);
  const [currentDocument, setCurrentDocument] = useState<OcrDocumentDto | null>(
    null,
  );
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);
  return (
    <>
      <Grid container spacing={2}>
        <Grid size={12}>
          <Paper sx={{ width: "100%", overflow: "hidden" }}>
            <Paper>
              <Grid container sx={{ padding: "3%" }}>
                <Grid size={3}>
                  <p>Rango de fechas para consulta </p>
                </Grid>
                <Grid size={3}>
                  <DatePicker
                    defaultValue={startDate}
                    label="Fecha Inicio"
                    autoFocus={true}
                    disableFuture
                    minDate={moment().subtract("years", "5")}
                    onChange={setStartDate}
                  />
                </Grid>
                <Grid size={3}>
                  <DatePicker
                    defaultValue={endDate}
                    label="Fecha Fin"
                    disablePast
                    minDate={moment()}
                    maxDate={moment().add("years", 5)}
                    onChange={setEndDate}
                  />
                </Grid>
                <Grid size={3}>
                  <IconButton
                    onClick={() => props.handleSearch(startDate, endDate)}
                  >
                    <SearchOutlined />
                  </IconButton>
                </Grid>
              </Grid>
            </Paper>
            <Grid container>
              <Grid size={12} sx={{ padding: "3%" }}>
                <Paper elevation={2}>
                  <DataGrid
                    rows={props.documentsList}
                    columns={columns}
                    initialState={{
                      pagination: {
                        paginationModel: {
                          pageSize: 20,
                        },
                      },
                    }}
                    pageSizeOptions={[20]}
                    // autosizeOnMount={true}
                    autosizeOptions={{
                      expand: true,
                      includeHeaderFilters: true,
                      includeHeaders: true,
                      outliersFactor:2.5
                    }}
                    disableRowSelectionOnClick
                    disableColumnResize={true}
                    density="comfortable"
                  />
                </Paper>
              </Grid>
            </Grid>
          </Paper>
        </Grid>
      </Grid>
      <Dialog
        open={isDetailsOpen}
        maxWidth="xl"
        onClose={() => setIsDetailsOpen(false)}
      >
        <DialogTitle>
          <Typography
            variant="h6"
            component="div"
            sx={{ flexGrow: 1 }}
          ></Typography>
          Detalles de reconocimento ocr para el documento: {currentDocument?.id}
          <IconButton
            size="small"
            color="inherit"
            onClick={() => setIsDetailsOpen(false)}
          >
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        <DialogContent>
          <Box>
            <Paper sx={{ padding: "2%" }}>
              <LinesDetails linesData={currentDocument?.lines || []} />
            </Paper>
          </Box>
        </DialogContent>
      </Dialog>
    </>
  );
};
export default PendingDocumentsLoad;
