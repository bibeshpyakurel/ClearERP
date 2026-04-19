import LockRoundedIcon from "@mui/icons-material/LockRounded";
import DarkModeRoundedIcon from "@mui/icons-material/DarkModeRounded";
import LightModeRoundedIcon from "@mui/icons-material/LightModeRounded";
import {
  Alert,
  Box,
  Button,
  Chip,
  Container,
  Divider,
  IconButton,
  InputAdornment,
  Paper,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import { useThemeMode } from "../features/theme/ThemeContext";
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
    label: "Admin",
    email: "admin@clearerp.local",
    password: "Admin123!",
  },
  {
    label: "Warehouse",
    email: "warehouse@clearerp.local",
    password: "Warehouse123!",
  },
] as const;

export function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { isAuthenticated, login } = useAuth();
  const { mode, toggleMode } = useThemeMode();
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
        position: "relative",
        overflow: "hidden",
        background: mode === "dark"
          ? "linear-gradient(135deg, #0d2420 0%, #0f1a18 25%, #0b0b0d 50%, #1a0f07 75%, #1f1108 100%)"
          : "linear-gradient(135deg, #b8ddd4 0%, #eef7f2 28%, #fdf6ec 55%, #fde3c4 80%, #d4ede6 100%)",
        px: 2,
        py: 3,
      }}
    >
      <style>{`
        @keyframes orb1 {
          0%, 100% { transform: translate(0px,   0px)  scale(1);    }
          30%       { transform: translate(90px, -70px) scale(1.14); }
          65%       { transform: translate(-50px, 80px) scale(0.9);  }
        }
        @keyframes orb2 {
          0%, 100% { transform: translate(0px,    0px)  scale(1);    }
          35%       { transform: translate(-80px,  90px) scale(1.1);  }
          70%       { transform: translate(70px,  -55px) scale(1.18); }
        }
        @keyframes orb3 {
          0%, 100% { transform: translate(0px,  0px)  scale(1);   }
          50%       { transform: translate(55px, 65px) scale(0.88); }
        }
        @keyframes orb4 {
          0%, 100% { transform: translate(0px,   0px)  scale(1);   }
          40%       { transform: translate(-60px,-50px) scale(1.1); }
          75%       { transform: translate(40px,  70px) scale(0.92);}
        }
      `}</style>

      {/* dot-grid texture */}
      <Box sx={{
        position: "absolute", inset: 0, zIndex: 0, pointerEvents: "none",
        backgroundImage: mode === "dark"
          ? "radial-gradient(circle, rgba(255,255,255,0.035) 1px, transparent 1px)"
          : "radial-gradient(circle, rgba(15,82,87,0.1) 1px, transparent 1px)",
        backgroundSize: "26px 26px",
      }} />

      {/* edge vignette */}
      <Box sx={{
        position: "absolute", inset: 0, zIndex: 0, pointerEvents: "none",
        background: mode === "dark"
          ? "radial-gradient(ellipse at 50% 50%, transparent 35%, rgba(6,6,7,0.85) 100%)"
          : "radial-gradient(ellipse at 50% 50%, transparent 40%, rgba(20,60,55,0.12) 100%)",
      }} />

      {/* orb 1 — deep teal, top-left */}
      <Box sx={{
        position: "absolute", top: "-20%", left: "-15%",
        width: 680, height: 680, borderRadius: "50%",
        background: mode === "dark"
          ? "radial-gradient(circle, rgba(15,90,80,0.35) 0%, transparent 65%)"
          : "radial-gradient(circle, rgba(15,82,87,0.2) 0%, transparent 65%)",
        filter: "blur(80px)",
        animation: "orb1 17s ease-in-out infinite",
        pointerEvents: "none", zIndex: 0,
      }} />

      {/* orb 2 — muted warm, bottom-right */}
      <Box sx={{
        position: "absolute", bottom: "-25%", right: "-15%",
        width: 720, height: 720, borderRadius: "50%",
        background: mode === "dark"
          ? "radial-gradient(circle, rgba(160,85,25,0.28) 0%, transparent 65%)"
          : "radial-gradient(circle, rgba(201,122,64,0.22) 0%, transparent 65%)",
        filter: "blur(90px)",
        animation: "orb2 20s ease-in-out infinite",
        pointerEvents: "none", zIndex: 0,
      }} />

      {/* orb 3 — top-right, barely there */}
      <Box sx={{
        position: "absolute", top: "-8%", right: "-8%",
        width: 400, height: 400, borderRadius: "50%",
        background: mode === "dark"
          ? "radial-gradient(circle, rgba(15,82,87,0.1) 0%, transparent 68%)"
          : "radial-gradient(circle, rgba(74,179,188,0.18) 0%, transparent 68%)",
        filter: "blur(60px)",
        animation: "orb3 13s ease-in-out infinite",
        pointerEvents: "none", zIndex: 0,
      }} />

      {/* orb 4 — warm bottom-left */}
      <Box sx={{
        position: "absolute", bottom: "4%", left: "2%",
        width: 340, height: 340, borderRadius: "50%",
        background: mode === "dark"
          ? "radial-gradient(circle, rgba(100,60,20,0.1) 0%, transparent 68%)"
          : "radial-gradient(circle, rgba(249,210,140,0.45) 0%, transparent 68%)",
        filter: "blur(60px)",
        animation: "orb4 22s ease-in-out infinite",
        pointerEvents: "none", zIndex: 0,
      }} />

      <Container maxWidth="xs" sx={{ position: "relative", zIndex: 1 }}>
        <Paper elevation={0} sx={{
          p: 3, borderRadius: 1,
          backgroundColor: mode === "dark" ? "rgba(18,18,20,0.82)" : "rgba(255,253,250,0.78)",
          backdropFilter: "blur(24px) saturate(1.2)",
          border: mode === "dark"
            ? "1px solid rgba(255,255,255,0.08)"
            : "1px solid rgba(255,255,255,0.75)",
          boxShadow: mode === "dark"
            ? "0 8px 48px rgba(0,0,0,0.6), inset 0 1px 0 rgba(255,255,255,0.05)"
            : "0 8px 40px rgba(15,82,87,0.1), inset 0 1px 0 rgba(255,255,255,0.9)",
        }}>
          <Stack spacing={2}>
            <Box>
              <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mb: 1 }}>
                <Stack direction="row" spacing={1} sx={{ flexWrap: "wrap" }}>
                  <Chip label="Demo Ready" color="primary" size="small" />
                  <Chip label="Seeded Data" variant="outlined" size="small" />
                </Stack>
                <Tooltip title={mode === "light" ? "Dark mode" : "Light mode"}>
                  <IconButton size="small" onClick={toggleMode}>
                    {mode === "light" ? <DarkModeRoundedIcon fontSize="small" /> : <LightModeRoundedIcon fontSize="small" />}
                  </IconButton>
                </Tooltip>
              </Stack>
              <Typography variant="overline" sx={{ letterSpacing: 2, color: "primary.main" }}>
                ClearERP
              </Typography>
              <Typography variant="h5">Sign In</Typography>
            </Box>

            {errorMessage ? <Alert severity="error">{errorMessage}</Alert> : null}

            <Box>
              <Typography variant="caption" color="text.secondary" sx={{ mb: 0.75, display: "block" }}>
                Quick sign in as
              </Typography>
              <Stack direction="row" spacing={1}>
                {demoAccounts.map((account) => (
                  <Button
                    key={account.email}
                    size="small"
                    variant="outlined"
                    sx={{ flex: 1 }}
                    onClick={() => {
                      setValue("email", account.email);
                      setValue("password", account.password);
                    }}
                  >
                    {account.label}
                  </Button>
                ))}
              </Stack>
            </Box>

            <Divider />

            <Stack component="form" spacing={1.5} onSubmit={onSubmit}>
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
                fullWidth
                disabled={formState.isSubmitting}
              >
                {formState.isSubmitting ? "Signing in..." : "Sign in"}
              </Button>
            </Stack>
          </Stack>
        </Paper>
      </Container>
    </Box>
  );
}
