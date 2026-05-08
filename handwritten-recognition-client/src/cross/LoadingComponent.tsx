import { Skeleton } from "@mui/material";

const LoadingComponent = () => {
  return (
    <>
      <Skeleton
        animation="wave"
        variant="text"
        sx={{ fontSize: "1rem", padding: "5%" }}
      />
      <Skeleton
        animation="wave"
        variant="text"
        sx={{ fontSize: "2rem", padding: "5%" }}
      />
      <Skeleton
        animation="wave"
        variant="rectangular"
        height={"25%"}
        sx={{ padding: "5%" }}
      />
      <Skeleton
        animation="wave"
        variant="rectangular"
        height={"25%"}
        sx={{ padding: "5%" }}
      />
    </>
  );
};
export default LoadingComponent;