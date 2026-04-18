import { Box, Button, Stack, Typography } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";

export function NotFoundPage() {
  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "grid",
        placeItems: "center",
        px: 2,
        background:
          "linear-gradient(180deg, #f4efe6 0%, #e3ede4 100%)",
      }}
    >
      <Stack spacing={2} alignItems="center">
        <Typography variant="overline" sx={{ letterSpacing: 2 }}>
          ClearERP
        </Typography>
        <Typography variant="h3">Page not found</Typography>
        <Typography color="text.secondary" textAlign="center" maxWidth={420}>
          The route you tried to open does not exist in the current frontend scaffold.
        </Typography>
        <Button component={RouterLink} to="/" variant="contained">
          Go to dashboard
        </Button>
      </Stack>
    </Box>
  );
}
