import { useState } from "react";
import type { PendingDocumentListProps } from "../../model/props/PendingDocumentsProps";
import type { OcrDocumentDto } from "../../model/dto/OcrDocumentDto";
import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { Grid, IconButton, Paper } from "@mui/material";
import { RateReviewTwoTone } from "@mui/icons-material";
import moment from "moment";
import { DatePicker } from "@mui/x-date-pickers";

const PendingDocumentsLoad = (props: PendingDocumentListProps) => 
{
  const columns: GridColDef<OcrDocumentDto>[] = [
    {
      field: "id",
      headerName: "ID Documento",
      sortable: false,
      description: "Id unico del documento",
    },
    {
      field: "createdAt",
      headerName: "Fecha Creacion",
      description:
        "Fecha de creacion del documento por el motor de extraccion Ocr",
    },
    {
      field: "confidenceAvg",
      headerName: "Confianza Promedio %",
      description: "Confianza promedio del proceso de extraccion Ocr",
      valueGetter: (value, row) => `${row.confidenceAvg * 100}`,
    },
    {
      field: "lines",
      headerName: "Lineas Reconocidas",
      description: "Cantidad de lineas reconocidas por el motor Ocr",
      sortable: true,
      valueGetter: (value, row) => `${row.lines.length}`,
    },
    {
      field: "Accion",
      headerName: "Acciones",
      description: "Acciones",
      sortable: false,
      filterable:false,
      disableColumnMenu: true,
      renderCell: (params) => {
        return (
          <div>
            <IconButton onClick={() => handleReviewRequest(params.row)}>
              <RateReviewTwoTone />
            </IconButton>
          </div>
        );
      },
    },
  ];
  const handleReviewRequest = (ocrDocument: OcrDocumentDto) => {
    console.log(ocrDocument);
  };

  const [startDate, setStartDate] = useState<moment.Moment | null>(moment());
  const [endDate, setEndDate] = useState<moment.Moment | null>(moment());

  return (
    <>
      <Grid container spacing={2}>
        <Grid size={12}>
          <Paper sx={{ width: "100%", overflow: "hidden" }}>
            <Paper>
              <p>Rango de fechas para consulta</p>
              <Grid container>
                <Grid size={6}>
                  <DatePicker defaultValue={startDate} label="Fecha Inicio"  onChange={setStartDate}/>
                </Grid>
                <Grid size={6}>
                  <DatePicker defaultValue={endDate} label="Fecha Fin" onChange={setEndDate} />
                </Grid>
              </Grid>
            </Paper>
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
              autosizeOnMount={true}
              autosizeOptions={{
                expand: true,
                includeHeaderFilters: true,
                includeHeaders: true,
              }}
              disableRowSelectionOnClick
              density="comfortable"
            />
          </Paper>
        </Grid>
      </Grid>
    </>
  );
};
export default PendingDocumentsLoad;
