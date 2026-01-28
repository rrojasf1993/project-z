
import { StrictMode } from "react";
import "../index.css";
import ImageUploaderPage from "../pages/ImageUploaderPage.tsx";
import { AppBar, Box, Button, IconButton, Toolbar, Typography } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';

const BaseComponent=()=>
{

    return( <StrictMode>
 
      <div >
  <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Mondá
          </Typography>
          <Button color="inherit">Login</Button>
        </Toolbar>
      </AppBar>
    </Box>
        <ImageUploaderPage />
      </div>
  </StrictMode>)
}
export default BaseComponent;