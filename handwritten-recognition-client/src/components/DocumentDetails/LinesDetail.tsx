import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import type { OcrLineDto } from "../../model/dto/OcrLineDto";
import type { ILinesDetails } from "../../model/props/ILinesDetailsProps";
import { Grid, Paper, Typography } from "@mui/material";

const LinesDetails = (props: ILinesDetails) => 
{

  const getBgColor=(confidence:number):string=>{
    if(confidence < 0.5) {
      return 'error.main';
    } else if(confidence >=0.6 && confidence<=0.8) {
      return 'warning.main';
    } else {
      return 'success.main';
    }
  }
  const columns: GridColDef<OcrLineDto>[] = [
    {
      field: "lineIndex",
      headerName: "Numero Linea",
      description: "Numero de la linea",
      width:120
    },
    {
      field: "text",
      headerName: "Texto",
      description: "Texto de la linea",
      width:300
    },
    {
      field: "confidenceScore",
      headerName: "Confianza  %",
      width:120,
      description: "Confianza  del proceso de extraccion Ocr",
      valueGetter: (value) => `${(value)}`,
      renderCell: (params) => {
        return (
          <>
          <Typography variant="body1" gutterBottom sx={{ color: getBgColor(params.value) }}>
            <b>{(params.value*100).toFixed(2)}</b>
          </Typography>
          </>
        );
      },
    },
    {
      field: "status",
      headerName: "Estado",
      width:120,
      description: "Estado del proceso de extraccion Ocr",
        renderCell: (params) => {
        return (
          <>
          <Typography variant="body1" gutterBottom sx={{ color: getBgColor(params.row.confidenceScore) }}>
            <b>{params.row.confidenceStatus}</b>
          </Typography>
          </>
        );
      },
    },
  ];

  return (
    <Grid container>
      <Grid size={12}>
        <Paper elevation={3}>
          <DataGrid
            columns={columns}
            rows={props.linesData}
            getRowId={(row) => row.lineId}
            initialState={{
              pagination: {
                paginationModel: {
                  pageSize: 30,
                },
              },
            }}
            pageSizeOptions={[30]}
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
  );
};
export default LinesDetails;