import { Grid, IconButton, MenuItem, Paper, Select } from "@mui/material";
import OcrJobStatus from "../../cross/enums/OcrJobStatus";
import { useState, type ChangeEvent, type ReactNode } from "react";
import moment from "moment";
import { DatePicker } from "@mui/x-date-pickers";
import { ErrorOutlineRounded, Image, Search } from "@mui/icons-material";
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
                onClick={()=>props.handleSeeErrorDetails(params.row.error)}
                disabled={params.row.error === null}
                sx={{ color: "red" }}
              >
                <ErrorOutlineRounded />
              </IconButton>
              <IconButton onClick={() => props.handleSeeImage(params.row)}>
                <Image />
              </IconButton>
            </div>
          </>
        );
      },
    },
  ];

  const handleStatusSelect = (
    event:
      | ChangeEvent<
          Omit<HTMLInputElement, "value"> & { value: OcrJobStatus.Pending }
        >
      | (Event & { target: { value: OcrJobStatus.Pending; name: string } })
      | ChangeEvent<
          Omit<HTMLInputElement, "value"> & { value: OcrJobStatus.Processing }
        >
      | (Event & { target: { value: OcrJobStatus.Processing; name: string } })
      | ChangeEvent<
          Omit<HTMLInputElement, "value"> & { value: OcrJobStatus.Completed }
        >
      | (Event & { target: { value: OcrJobStatus.Completed; name: string } })
      | ChangeEvent<
          Omit<HTMLInputElement, "value"> & { value: OcrJobStatus.Failed }
        >
      | (Event & { target: { value: OcrJobStatus.Failed; name: string } }),
    child: ReactNode,
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
              onChange={handleStatusSelect}
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
              label="Fecha Inicio"
              autoFocus={true}
              disableFuture
              minDate={moment().subtract("years", "5")}
            />
          </Grid>
          <Grid size={3}>
            <DatePicker
              defaultValue={endDate}
              label="Fecha Final"
              autoFocus={true}
              disableFuture
              minDate={moment().subtract("years", "5")}
            />
          </Grid>
          <Grid size={3}>
            <IconButton
              onClick={() =>
                props.handleSearchRequest(
                  selectedMenuItem,
                  undefined,
                  undefined,
                )
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
