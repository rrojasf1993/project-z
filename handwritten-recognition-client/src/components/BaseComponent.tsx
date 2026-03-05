import { StrictMode, useState } from "react";
import "../index.css";
import ImageUploaderPage from "../pages/ImageUploaderPage.tsx";
import {
  AppBar,
  Box,
  IconButton,
  Menu,
  MenuItem,
  Toolbar,
  Typography,
} from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import {
  AccountBoxOutlined,
  HeatPump,
  RateReview,
  Rule,
  Settings,
  Upload,
} from "@mui/icons-material";



const BaseComponent = (props: any) => {
  const [anchorElement, setAnchorElement] = useState<HTMLElement | null>(null);
//  const navigate = useNavigate();

  const handleRedirect=async (e: React.MouseEvent<HTMLAnchorElement>)=>{
    console.log(e);
    //const s={"pathname":"/"};
    //await navigate({path: e.currentTarget.attributes.getNamedItem("href")?.value} as To)
  }

  const handleMenuClose = (): void => {
    setAnchorElement(null);
  };
  return (
    <StrictMode>
      <div>
        <Box sx={{ flexGrow: 1 }}>
          <AppBar position="static">
            <Toolbar>
              <IconButton
                size="large"
                edge="start"
                color="inherit"
                aria-label="menu"
                sx={{ mr: 2 }}
                onClick={(e: React.MouseEvent<HTMLElement>) =>
                  setAnchorElement(e.currentTarget)
                }
              >
                <MenuIcon />
              </IconButton>
              <Menu
                anchorEl={anchorElement}
                anchorOrigin={{
                  vertical: "top",
                  horizontal: "left",
                }}
                keepMounted
                transformOrigin={{
                  vertical: "top",
                  horizontal: "left",
                }}
                open={Boolean(anchorElement)}
                onClose={handleMenuClose}
              >
                <MenuItem href="/images-upload" onClick={handleRedirect} >
                  <Upload />
                  Cargar documentos
                </MenuItem>
                <MenuItem href="/document-review" onClick={handleRedirect}>
                  <RateReview />
                  Revisar documentos
                </MenuItem>
                <MenuItem>
                  <HeatPump />
                  Revisar estado cargas masivas
                </MenuItem>
                <MenuItem>
                  <Rule />
                  Configuracion de reglas
                </MenuItem>
                <MenuItem>
                  <Settings />
                  Configuracion general
                </MenuItem>
              </Menu>
              <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                Mondá
              </Typography>
              <IconButton color="inherit">
                <AccountBoxOutlined />
              </IconButton>
            </Toolbar>
          </AppBar>
        </Box>
        {props.children}
      </div>
    </StrictMode>
  );
};
export default BaseComponent;
