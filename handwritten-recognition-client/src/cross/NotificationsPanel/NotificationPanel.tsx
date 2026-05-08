import { List, Paper, Popover, Typography } from "@mui/material";
import type NotificationsPanelProps from "../../model/props/NotificationPanelProps";
import "./NotificationPanel.css";
const NotificationPanel = (props: NotificationsPanelProps) => {
  return (
    <Popover
      open={Boolean(props.anchorElement)}
      anchorEl={props.anchorElement}
      onClose={props.onClose}
      anchorOrigin={{
        horizontal: "right",
        vertical: "top",
      }}
    >
      <Paper className="notificationPanelTitleContainer">
        <Typography variant="h6">Notificaciones</Typography>
      </Paper>
      <List
        sx={{
          width: 450,
          maxHeight: 600,
          overflow: "auto",
        }}
      >
        <Paper elevation={1}>
          <h2>No hay notificaciones</h2>
        </Paper>
      </List>
    </Popover>
  );
};
export default NotificationPanel;
