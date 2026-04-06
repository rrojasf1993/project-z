import { createRoot } from "react-dom/client";
import "./index.css";
import { RouterProvider } from "react-router";
import routerInstance from "./cross/routeConfig.ts";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";

createRoot(document.getElementById("root")!).render(
  <LocalizationProvider dateAdapter={AdapterMoment}>
    <RouterProvider router={routerInstance} />
  </LocalizationProvider>
);