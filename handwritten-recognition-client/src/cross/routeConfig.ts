import { createBrowserRouter } from "react-router";
import ImageUploaderPage from "../pages/ImageUploaderPage";
import DocumentReviewPage from "../pages/DocumentReviewPage";
import StartPage from "../pages/StartPage";
import PendingDocumentsPage from "../pages/PendingDocumentsPage";

const routerInstance=createBrowserRouter([
    {
        "path":"/",
        "Component":StartPage
    },
    {
        "path":"/images-upload",
         "Component": ImageUploaderPage
    },
    {
        "path":"/documents-pending-review",
         "Component": PendingDocumentsPage
    },
    {
        "path":"/document-review",
        "Component":DocumentReviewPage
    }
]);
export default routerInstance;