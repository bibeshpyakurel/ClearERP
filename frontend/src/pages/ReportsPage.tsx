import { Alert, Box, CircularProgress, Grid, Stack, Typography } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { ApiClientError } from "../api/client";
import { AppDataTable, type TableColumn } from "../components/AppDataTable";
import { PageSection } from "../components/PageSection";
import { useAuth } from "../features/auth/AuthContext";
import { useThemeMode } from "../features/theme/ThemeContext";
import { reportsApi } from "../features/reports/api";
import type {
  LowStockReportItem,
  StockValuationReportItem,
} from "../features/reports/types";

export function ReportsPage() {
  const { accessToken } = useAuth();
  const { mode } = useThemeMode();

  const lowStockQuery = useQuery({
    queryKey: ["reports-low-stock"],
    queryFn: async () => {
      if (!accessToken) {
        throw new Error("Missing access token.");
      }

      return reportsApi.getLowStock(accessToken);
    },
    enabled: Boolean(accessToken),
  });

  const stockValuationQuery = useQuery({
    queryKey: ["reports-stock-valuation"],
    queryFn: async () => {
      if (!accessToken) {
        throw new Error("Missing access token.");
      }

      return reportsApi.getStockValuation(accessToken);
    },
    enabled: Boolean(accessToken),
  });

  const purchaseOrderSummaryQuery = useQuery({
    queryKey: ["reports-po-summary"],
    queryFn: async () => {
      if (!accessToken) {
        throw new Error("Missing access token.");
      }

      return reportsApi.getPurchaseOrderSummary(accessToken);
    },
    enabled: Boolean(accessToken),
  });

  const lowStockColumns: TableColumn<LowStockReportItem>[] = [
    { key: "itemSku", header: "SKU", render: (row) => row.itemSku },
    { key: "itemName", header: "Item", render: (row) => row.itemName },
    { key: "reorderLevel", header: "Reorder", align: "right", render: (row) => row.reorderLevel },
    { key: "quantityAvailable", header: "Available", align: "right", render: (row) => row.quantityAvailable },
    { key: "shortfall", header: "Shortfall", align: "right", render: (row) => row.shortfall },
  ];

  const valuationColumns: TableColumn<StockValuationReportItem>[] = [
    { key: "itemSku", header: "SKU", render: (row) => row.itemSku },
    { key: "itemName", header: "Item", render: (row) => row.itemName },
    { key: "quantityOnHand", header: "On Hand", align: "right", render: (row) => row.quantityOnHand },
    { key: "standardCost", header: "Unit Cost", align: "right", render: (row) => `$${row.standardCost.toFixed(2)}` },
    { key: "inventoryValue", header: "Inventory Value", align: "right", render: (row) => `$${row.inventoryValue.toFixed(2)}` },
  ];

  const poSummaryBars = purchaseOrderSummaryQuery.data
    ? [
        { label: "Draft", value: purchaseOrderSummaryQuery.data.draftCount },
        { label: "Approved", value: purchaseOrderSummaryQuery.data.approvedCount },
        { label: "Partially Received", value: purchaseOrderSummaryQuery.data.partiallyReceivedCount },
        { label: "Completed", value: purchaseOrderSummaryQuery.data.completedCount },
        { label: "Cancelled", value: purchaseOrderSummaryQuery.data.cancelledCount },
      ]
    : [];

  const maxBarValue = Math.max(...poSummaryBars.map((bar) => bar.value), 1);

  return (
    <Stack spacing={3}>
      <PageSection
        eyebrow="Reporting"
        title="Operational reports"
        description="Live reporting surface for replenishment, valuation, and purchase-order status monitoring."
      >
        <Grid container spacing={2}>
          <Grid size={{ xs: 12 }}>
            <PageSection
              title="Low-stock report"
              description="Items at or below reorder threshold."
            >
              {lowStockQuery.isError ? (
                <Alert severity="error">
                  {lowStockQuery.error instanceof ApiClientError
                    ? lowStockQuery.error.message
                    : "Unable to load the low-stock report."}
                </Alert>
              ) : lowStockQuery.isLoading ? (
                <Box sx={{ display: "grid", placeItems: "center", py: 6 }}>
                  <Stack spacing={1.5} alignItems="center">
                    <CircularProgress />
                    <Typography color="text.secondary">Loading low-stock report...</Typography>
                  </Stack>
                </Box>
              ) : (
                <AppDataTable
                  columns={lowStockColumns}
                  rows={lowStockQuery.data ?? []}
                  emptyMessage="No low-stock items right now."
                />
              )}
            </PageSection>
          </Grid>

          <Grid size={{ xs: 12, xl: 7 }}>
            <PageSection
              title="Stock valuation report"
              description={`Total inventory value: $${
                stockValuationQuery.data?.totalInventoryValue.toFixed(2) ?? "0.00"
              }`}
            >
              {stockValuationQuery.isError ? (
                <Alert severity="error">
                  {stockValuationQuery.error instanceof ApiClientError
                    ? stockValuationQuery.error.message
                    : "Unable to load the stock valuation report."}
                </Alert>
              ) : stockValuationQuery.isLoading ? (
                <Box sx={{ display: "grid", placeItems: "center", py: 6 }}>
                  <Stack spacing={1.5} alignItems="center">
                    <CircularProgress />
                    <Typography color="text.secondary">Loading stock valuation...</Typography>
                  </Stack>
                </Box>
              ) : (
                <AppDataTable
                  columns={valuationColumns}
                  rows={stockValuationQuery.data?.items ?? []}
                  emptyMessage="No valuation records available."
                />
              )}
            </PageSection>
          </Grid>

          <Grid size={{ xs: 12, xl: 5 }}>
            <PageSection
              title="Purchase order summary"
              description={`Open PO value: $${
                purchaseOrderSummaryQuery.data?.totalOpenPurchaseOrderValue.toFixed(2) ?? "0.00"
              }`}
            >
              {purchaseOrderSummaryQuery.isError ? (
                <Alert severity="error">
                  {purchaseOrderSummaryQuery.error instanceof ApiClientError
                    ? purchaseOrderSummaryQuery.error.message
                    : "Unable to load the purchase order summary."}
                </Alert>
              ) : purchaseOrderSummaryQuery.isLoading ? (
                <Box sx={{ display: "grid", placeItems: "center", py: 6 }}>
                  <Stack spacing={1.5} alignItems="center">
                    <CircularProgress />
                    <Typography color="text.secondary">Loading purchase order summary...</Typography>
                  </Stack>
                </Box>
              ) : (
                <Stack spacing={1.5}>
                  {poSummaryBars.map((bar) => (
                    <Box key={bar.label}>
                      <Stack direction="row" justifyContent="space-between" sx={{ mb: 0.5 }}>
                        <Typography variant="body2">{bar.label}</Typography>
                        <Typography variant="body2" color="text.secondary">
                          {bar.value}
                        </Typography>
                      </Stack>
                      <Box
                        sx={{
                          height: 12,
                          borderRadius: 999,
                          bgcolor: mode === "dark" ? "rgba(255,255,255,0.07)" : "rgba(15,82,87,0.08)",
                          overflow: "hidden",
                        }}
                      >
                        <Box
                          sx={{
                            width: `${(bar.value / maxBarValue) * 100}%`,
                            height: "100%",
                            borderRadius: 999,
                            background:
                              "linear-gradient(90deg, #0f5257 0%, #c97a40 100%)",
                          }}
                        />
                      </Box>
                    </Box>
                  ))}
                </Stack>
              )}
            </PageSection>
          </Grid>
        </Grid>
      </PageSection>
    </Stack>
  );
}
