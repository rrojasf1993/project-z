import { createBrowserRouter } from "react-router";
import ImageUploaderPage from "../pages/ImageUploaderPage";
import DocumentReviewPage from "../pages/DocumentReviewPage";
import StartPage from "../pages/StartPage";
import PendingDocumentsPage from "../pages/PendingDocumentsPage";
import BaseComponent from "../components/BaseComponent";
import PendingJobsPage from "../pages/PendingJobsPage";

const routerInstance = createBrowserRouter([
  {
    path: "/",
    Component: BaseComponent,
    children: [
      { index: true, Component: StartPage },
      {
        path: "/images-upload",
        Component: ImageUploaderPage,
      },
      {
        path: "/pending-jobs",
        Component: PendingJobsPage,
      },
      {
        path: "/documents-pending-review",
        Component: PendingDocumentsPage,
      },
      {
        path: "/document-review",
        Component: DocumentReviewPage,
      },
    ],
  },
]);
export default routerInstance;
