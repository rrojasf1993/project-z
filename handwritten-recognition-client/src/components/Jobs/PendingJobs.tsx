import { Grid, IconButton, MenuItem, Paper, Select } from "@mui/material";
import OcrJobStatus from "../../cross/enums/OcrJobStatus";
import { useState } from "react";
import moment from "moment";
import { DatePicker } from "@mui/x-date-pickers";
import {
  CancelOutlined,
  ErrorOutlineRounded,
  Image,
  Search,
} from "@mui/icons-material";
import { DataGrid, type GridColDef, type GridRowId } from "@mui/x-data-grid";
import type JobsQueryProps from "../../model/props/JobsQueryProps";
import type { OcrJobDto } from "../../model/dto/OcrJobDto";

const PendingJobs = (props: JobsQueryProps) => {
  const MenuItems = [
    {
      Title: "Pendientes",
      Id: OcrJobStatus.Pending,
    },
    {
      Title: "En Proceso",
      Id: OcrJobStatus.Processing,
    },
    {
      Title: "Fallidos",
      Id: OcrJobStatus.Failed,
    },
    {
      Title: "Completados",
      Id: OcrJobStatus.Completed,
    },
    {
      Title: "Cancelados",
      Id: OcrJobStatus.Canceled,
    },
  ];
  const [selectedMenuItem, setSelectedMenuItem] = useState<OcrJobStatus>(
    OcrJobStatus.Pending,
  );

  const [startDate, setStartDate] = useState<moment.Moment | null>(null);
  const [endDate, setEndDate] = useState<moment.Moment | null>(null);

  const columns: GridColDef<OcrJobDto>[] = [
    {
      field: "jobId",
      headerName: "ID Job",
      sortable: false,
      description: "Id unico del Job",
      width: 320,
    },
    {
      field: "createdAt",
      headerName: "Fecha Creacion",
      description: "Fecha de creacion del job ",
      valueGetter: (value, row): string =>
        `${moment(row.createdAt).format("YYYY-MM-DD:hh:mm")}`,
      width: 150,
    },
    {
      field: "fileName",
      headerName: "Ruta del archivo",
      description: "Ruta original del archivo cargado",
      valueGetter: (value, row) => `${row.fileName}`,
      width: 450,
    },
    {
      field: "updatedAt",
      headerName: "Actualizado en",
      description: "Fecha de actualizacion",
      width: 150,
      valueGetter: (value, row): string =>
        `${moment(row.updatedAt).format("YYYY-MM-DD:hh:mm")}`,
    },
    {
      field: "error",
      headerName: "Error",
      description: "Error (Si hubo)",
      valueGetter: (value, row) => {
        if (value === null) {
          return "No se presento error durante el procesamiento";
        } else {
          return row.error;
        }
      },
      width: 450,
    },
    {
      field: "Accion",
      headerName: "Acciones",
      description: "Acciones",
      sortable: false,
      filterable: false,
      disableColumnMenu: true,
      width: 120,
      renderCell: (params) => {
        return (
          <>
            <div>
              <IconButton
                onClick={() => props.handleSeeErrorDetails(params.row.error)}
                disabled={params.row.error === null}
                sx={{ color: "red" }}
              >
                <ErrorOutlineRounded />
              </IconButton>
              <IconButton onClick={() => props.handleSeeImage(params.row)}>
                <Image />
              </IconButton>
              {selectedMenuItem === OcrJobStatus.Pending ||
              selectedMenuItem === OcrJobStatus.Processing ? (
                <IconButton>
                  <CancelOutlined />
                </IconButton>
              ) : null}
            </div>
          </>
        );
      },
    },
  ];

  const handleStatusSelect = (
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    event: any,
  ): void => {
    event.preventDefault();
    setSelectedMenuItem(event.target.value);
  };

  return (
    <>
      <Grid container spacing={2}>
        <Grid container sx={{ padding: "3%" }}>
          <Grid size={3}>
            <b>Estado</b>
            <Select
              label="Estado"
              variant="outlined"
              value={selectedMenuItem}
              onChange={(e) => handleStatusSelect(e)}
              defaultValue={OcrJobStatus.Pending}
            >
              {MenuItems.map((menuItem) => {
                return (
                  <MenuItem key={menuItem.Id} value={menuItem.Id}>
                    {menuItem.Title}
                  </MenuItem>
                );
              })}
            </Select>
          </Grid>
          <Grid size={3}>
            <DatePicker
              defaultValue={startDate}
              value={startDate}
              onChange={(value) => setStartDate(value)}
              label="Fecha Inicio"
              autoFocus={true}
              disableFuture
              minDate={moment().subtract("3", "years")}
              minDate={moment().subtract("years", "5")}
            />
          </Grid>
          <Grid size={3}>
            <DatePicker
              defaultValue={endDate}
              value={endDate}
              onChange={(value) => setEndDate(value)}
              label="Fecha Final"
              autoFocus={true}
              disableFuture
              minDate={moment().subtract("3", "years")}
            />
          </Grid>
          <Grid size={3}>
            <IconButton
              onClick={() =>
                props.handleSearchRequest(selectedMenuItem, startDate, endDate)
              }
            >
              <Search />
            </IconButton>
          </Grid>
        </Grid>
      </Grid>
      <Grid container>
        <Grid size={12} sx={{ padding: "3%" }}>
          <Paper elevation={2}>
            <DataGrid
              rows={props.currentJobs}
              columns={columns}
              initialState={{
                pagination: {
                  paginationModel: {
                    pageSize: 20,
                  },
                },
              }}
              getRowId={(row) => row.jobId as unknown as GridRowId}
              pageSizeOptions={[20]}
              //autosizeOnMount={true}
              autosizeOptions={{
                expand: false,
                includeHeaderFilters: true,
                includeHeaders: true,
              }}
              disableRowSelectionOnClick
              disableColumnResize={true}
              density="comfortable"
            />
          </Paper>
        </Grid>
      </Grid>
    </>
  );
};
export default PendingJobs;
