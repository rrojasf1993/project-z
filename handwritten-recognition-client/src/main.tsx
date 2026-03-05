import { createRoot } from "react-dom/client";
import "./index.css";
import BaseComponent from "./components/BaseComponent.tsx";
import { RouterProvider } from "react-router";
import routerInstance from "./cross/routeConfig.ts";

createRoot(document.getElementById("root")!).render(
  <>
    <BaseComponent>
    <RouterProvider router={routerInstance} />
    </BaseComponent>
  </>,
);
