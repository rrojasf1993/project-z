import { createRoot } from "react-dom/client";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import BaseComponent from "./components/BaseComponent.tsx";

createRoot(document.getElementById("root")!).render(<BaseComponent />);
