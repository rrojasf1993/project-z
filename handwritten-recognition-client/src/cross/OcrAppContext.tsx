import { createContext } from "react";
import type { IAppStateModel } from "../model/AppStateModel";

const OcrAppContext=createContext({} as IAppStateModel);

export default OcrAppContext;