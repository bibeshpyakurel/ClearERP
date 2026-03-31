import {
  Box,
  Card,
  CardContent,
  Chip,
  Container,
  Divider,
  Stack,
  Typography,
} from "@mui/material";

const modules = [
  "Authentication",
  "Items",
  "Suppliers",
  "Purchase Orders",
  "Goods Receipt",
  "Inventory",
  "Stock Adjustments",
  "Reports",
  "Audit Logs",
];

export function App() {
  return (
    <Box
      sx={{
        minHeight: "100vh",
        background:
          "linear-gradient(180deg, #f5f3ee 0%, #e8efe9 45%, #d8e2dc 100%)",
        py: 6,
      }}
    >
      <Container maxWidth="lg">
        <Stack spacing={3}>
          <Typography variant="overline" sx={{ letterSpacing: 2 }}>
            Mini ERP
          </Typography>
          <Typography variant="h3" sx={{ fontWeight: 700, maxWidth: 800 }}>
            Inventory Management System
          </Typography>
          <Typography variant="body1" sx={{ maxWidth: 760 }}>
            This frontend scaffold is ready for the ERP modules that will power
            inventory tracking, purchasing workflows, and operational reporting.
          </Typography>

          <Card elevation={0} sx={{ borderRadius: 4 }}>
            <CardContent sx={{ p: 4 }}>
              <Stack spacing={2}>
                <Typography variant="h5">Planned Modules</Typography>
                <Divider />
                <Stack direction="row" spacing={1} useFlexGap flexWrap="wrap">
                  {modules.map((module) => (
                    <Chip key={module} label={module} color="primary" variant="outlined" />
                  ))}
                </Stack>
              </Stack>
            </CardContent>
          </Card>
        </Stack>
      </Container>
    </Box>
  );
}
