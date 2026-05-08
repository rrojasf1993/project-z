import { StrictMode, useState } from "react";
import "../index.css";
import { Outlet, Link } from "react-router-dom";

import {
  AppBar,
  Badge,
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
  CircleNotificationsOutlined,
  EditDocument,
  Money,
  RateReview,
  Rule,
  Settings,
  Upload,
} from "@mui/icons-material";
import "./BaseComponent.css";
import NotificationPanel from "../cross/NotificationsPanel/NotificationPanel";
const BaseComponent = () => 
{

  const [anchorElement, setAnchorElement] = useState<HTMLElement | null>(null);
  const [notificationPanelAnchorElement, setNotificationPanelAnchorElement] = useState<HTMLElement | null>(null);
  
  const handleMenuClose = (): void => {
    setAnchorElement(null);
  };

  return (
    <StrictMode>
      <div>
        <Box sx={{ flexGrow: 1 }}>
          <AppBar position="static" className="mainAppBar">
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
                <MenuItem component={Link} to="/images-upload">
                  <Upload />
                  Cargar documentos
                </MenuItem>
                <MenuItem component={Link} to="/documents-pending-review">
                  <RateReview />
                  Documentos pendientes de revision
                </MenuItem>
                <MenuItem component={Link} to="/pending-jobs">
                  <Money />
                  Jobs Pendientes
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
              <EditDocument/>
              <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                Paperly 
              </Typography>
              <IconButton color="inherit">
                <AccountBoxOutlined />
              </IconButton>
              <IconButton color="inherit" onClick={(e)=>setNotificationPanelAnchorElement(e.currentTarget)}>
                <Badge badgeContent={"a"} color="warning">
                <CircleNotificationsOutlined/>
                </Badge>
              </IconButton>
            </Toolbar>
          </AppBar>
          <NotificationPanel anchorElement={notificationPanelAnchorElement} onClose={()=>setNotificationPanelAnchorElement(null)}/>
        </Box>
        <Outlet />
      </div>
    </StrictMode>
  );
};
export default BaseComponent;
