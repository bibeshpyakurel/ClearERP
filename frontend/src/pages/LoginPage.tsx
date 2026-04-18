import LockRoundedIcon from "@mui/icons-material/LockRounded";
import {
  Alert,
  Box,
  Button,
  Chip,
  Container,
  Divider,
  InputAdornment,
  Paper,
  Stack,
  Typography,
} from "@mui/material";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { z } from "zod";
import { AppFormTextField } from "../components/AppFormTextField";
import { useAuth } from "../features/auth/AuthContext";
import { ApiClientError } from "../api/client";

const loginSchema = z.object({
  email: z.string().email("Enter a valid email address."),
  password: z.string().min(6, "Password must be at least 6 characters."),
});

type LoginFormValues = z.infer<typeof loginSchema>;

const demoAccounts = [
  {
    label: "Admin Demo",
    email: "admin@clearerp.local",
    password: "Admin123!",
    description: "Full ERP access including audit logs.",
  },
  {
    label: "Warehouse Demo",
    email: "warehouse@clearerp.local",
    password: "Warehouse123!",
    description: "Operational flow without admin-only screens.",
  },
] as const;

export function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { isAuthenticated, login } = useAuth();
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const { control, handleSubmit, setError, setValue, formState } = useForm<LoginFormValues>({
    defaultValues: {
      email: "admin@clearerp.local",
      password: "Admin123!",
    },
  });

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/", { replace: true });
    }
  }, [isAuthenticated, navigate]);

  const onSubmit = handleSubmit(async (values) => {
    setErrorMessage(null);

    const parsed = loginSchema.safeParse(values);
    if (!parsed.success) {
      parsed.error.issues.forEach((issue) => {
        const fieldName = issue.path[0];

        if (fieldName === "email" || fieldName === "password") {
          setError(fieldName, { message: issue.message });
        }
      });
      return;
    }

    try {
      await login(parsed.data);
      const redirectTo =
        typeof location.state === "object" &&
        location.state &&
        "from" in location.state &&
        typeof location.state.from === "string"
          ? location.state.from
          : "/";

      navigate(redirectTo, { replace: true });
    } catch (error) {
      if (error instanceof ApiClientError) {
        setErrorMessage(error.message);
        return;
      }

      setErrorMessage("Unable to sign in right now.");
    }
  });

  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "grid",
        placeItems: "center",
        background:
          "radial-gradient(circle at top left, #f6ead6 0%, #e2ece4 48%, #cad8d0 100%)",
        px: 2,
      }}
    >
      <Container maxWidth="sm">
        <Paper elevation={0} sx={{ p: { xs: 3, md: 5 }, borderRadius: 6 }}>
          <Stack spacing={3}>
            <Box>
              <Stack direction="row" spacing={1} sx={{ mb: 1.5, flexWrap: "wrap" }}>
                <Chip label="Demo Ready" color="primary" size="small" />
                <Chip label="Seeded Data" variant="outlined" size="small" />
              </Stack>
              <Typography variant="overline" sx={{ letterSpacing: 2, color: "primary.main" }}>
                ClearERP
              </Typography>
              <Typography variant="h4">Operations Sign In</Typography>
              <Typography variant="body1" color="text.secondary" sx={{ mt: 1 }}>
                Sign in with a seeded account and you can inspect items, purchasing, inventory,
                reports, and audit history immediately.
              </Typography>
            </Box>

            {errorMessage ? <Alert severity="error">{errorMessage}</Alert> : null}

            <Alert severity="info">
              This build is intentionally demo-friendly: seeded data is already present, workflows are
              connected, and no extra onboarding is required.
            </Alert>

            <Stack spacing={1.25}>
              <Typography variant="subtitle2">Try a demo persona</Typography>
              <Stack direction={{ xs: "column", sm: "row" }} spacing={1.25}>
                {demoAccounts.map((account) => (
                  <Paper
                    key={account.email}
                    variant="outlined"
                    sx={{ p: 1.5, flex: 1, borderRadius: 3 }}
                  >
                    <Stack spacing={1}>
                      <Box>
                        <Typography variant="body2" sx={{ fontWeight: 700 }}>
                          {account.label}
                        </Typography>
                        <Typography variant="caption" color="text.secondary">
                          {account.description}
                        </Typography>
                      </Box>
                      <Typography variant="caption" color="text.secondary">
                        {account.email}
                      </Typography>
                      <Button
                        size="small"
                        variant="outlined"
                        onClick={() => {
                          setValue("email", account.email);
                          setValue("password", account.password);
                        }}
                      >
                        Use credentials
                      </Button>
                    </Stack>
                  </Paper>
                ))}
              </Stack>
            </Stack>

            <Divider />

            <Stack component="form" spacing={2} onSubmit={onSubmit}>
              <AppFormTextField
                control={control}
                name="email"
                label="Email"
                fullWidth
                autoComplete="email"
              />
              <AppFormTextField
                control={control}
                name="password"
                type="password"
                label="Password"
                fullWidth
                autoComplete="current-password"
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <LockRoundedIcon fontSize="small" />
                    </InputAdornment>
                  ),
                }}
              />
              <Button
                type="submit"
                variant="contained"
                size="large"
                disabled={formState.isSubmitting}
              >
                {formState.isSubmitting ? "Signing in..." : "Sign in"}
              </Button>
            </Stack>

            <Paper
              variant="outlined"
              sx={{ p: 2, borderRadius: 3, backgroundColor: "rgba(15,82,87,0.02)" }}
            >
              <Stack spacing={0.75}>
                <Typography variant="subtitle2">Suggested 5-minute walkthrough</Typography>
                <Typography variant="body2" color="text.secondary">
                  1. Review the dashboard KPIs.
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  2. Open purchase orders and receive stock.
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  3. Inspect inventory balances and transactions.
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  4. Open reports to see low-stock and valuation outputs.
                </Typography>
              </Stack>
            </Paper>
          </Stack>
        </Paper>
      </Container>
    </Box>
  );
}
