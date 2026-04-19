import { createContext, useContext, useState, useMemo, useCallback } from "react";
import { createTheme, ThemeProvider, CssBaseline, alpha } from "@mui/material";

type Mode = "light" | "dark";

interface ThemeContextValue {
  mode: Mode;
  toggleMode: () => void;
}

const ThemeContext = createContext<ThemeContextValue>({
  mode: "light",
  toggleMode: () => {},
});

export function useThemeMode() {
  return useContext(ThemeContext);
}

// ─── Dark token palette ────────────────────────────────────────────────────
// Level 0  bg.default   #0b0b0d   page canvas
// Level 1  bg.paper     #18181b   cards / sections
// Level 2  bg.elevated  #242428   dialogs / menus / popovers
// Level 3  bg.raised    #2e2e33   hover / pressed surfaces
//
// Primary accent in dark: #2a9ba3  — teal, lightened enough to read on dark
// Text:  primary #f2f2f0 | secondary #8c8c96 | disabled #46464e
// Border base: rgba(255,255,255,0.08)
// ──────────────────────────────────────────────────────────────────────────

function buildTheme(mode: Mode) {
  const isDark = mode === "dark";

  // Light mode keeps the original brand teal.
  // Dark mode uses a lightened readable teal so it shows on near-black.
  const primaryMain = isDark ? "#2a9ba3" : "#0f5257";

  return createTheme({
    palette: {
      mode,
      primary:   { main: primaryMain },
      secondary: { main: "#c97a40" },
      background: isDark
        ? { default: "#0b0b0d", paper: "#18181b" }
        : { default: "#f4efe6", paper: "#fffdf8" },
      divider: isDark ? "rgba(255,255,255,0.08)" : "rgba(15,82,87,0.12)",
      text: isDark
        ? { primary: "#f2f2f0", secondary: "#8c8c96", disabled: "#46464e" } as never
        : { primary: "#0d2426", secondary: "#4a6e71" },
      action: isDark
        ? { hover: "rgba(255,255,255,0.05)", selected: "rgba(255,255,255,0.08)" }
        : {},
    },
    shape: { borderRadius: 4 },
    typography: {
      fontFamily: '"Segoe UI", "Helvetica Neue", sans-serif',
      h4: { fontWeight: 700, letterSpacing: -0.5 },
      h5: { fontWeight: 700 },
    },
    components: {
      // ── Paper (cards, sections) ──────────────────────────────────────────
      MuiPaper: {
        styleOverrides: {
          root: {
            backgroundImage: "none",
            ...(isDark && {
              backgroundColor: "#18181b",
              border: "1px solid rgba(255,255,255,0.08)",
            }),
          },
        },
      },

      // ── Table ────────────────────────────────────────────────────────────
      MuiTableHead: {
        styleOverrides: {
          root: {
            "& .MuiTableCell-root": {
              backgroundColor: isDark ? "#111114" : alpha("#0f5257", 0.04),
              color: isDark ? "#8c8c96" : "#4a6e71",
              fontWeight: 600,
              fontSize: "0.72rem",
              letterSpacing: "0.06em",
              textTransform: "uppercase",
              borderBottom: isDark
                ? "1px solid rgba(255,255,255,0.08)"
                : "1px solid rgba(15,82,87,0.12)",
            },
          },
        },
      },
      MuiTableRow: {
        styleOverrides: {
          root: {
            "&.MuiTableRow-hover:hover": {
              backgroundColor: isDark
                ? "rgba(255,255,255,0.04)"
                : "rgba(15,82,87,0.04)",
            },
            "& .MuiTableCell-root": {
              borderBottom: isDark
                ? "1px solid rgba(255,255,255,0.06)"
                : "1px solid rgba(15,82,87,0.07)",
              color: isDark ? "#f2f2f0" : undefined,
            },
          },
        },
      },

      // ── Chip ─────────────────────────────────────────────────────────────
      MuiChip: {
        styleOverrides: {
          root: {
            ...(isDark && {
              backgroundColor: "rgba(255,255,255,0.06)",
              borderColor: "rgba(255,255,255,0.1)",
              color: "#c4c4cc",
            }),
          },
          outlinedPrimary: {
            ...(isDark && {
              backgroundColor: "rgba(42,155,163,0.1)",
              borderColor: "rgba(42,155,163,0.35)",
              color: "#2a9ba3",
            }),
          },
          colorPrimary: {
            ...(isDark && {
              backgroundColor: "rgba(42,155,163,0.15)",
              color: "#2a9ba3",
            }),
          },
        },
      },

      // ── Inputs ───────────────────────────────────────────────────────────
      MuiInputBase: {
        styleOverrides: {
          root: {
            ...(isDark && {
              backgroundColor: "rgba(255,255,255,0.04)",
              color: "#f2f2f0",
            }),
          },
        },
      },
      MuiOutlinedInput: {
        styleOverrides: {
          notchedOutline: {
            ...(isDark && { borderColor: "rgba(255,255,255,0.12)" }),
          },
          root: {
            ...(isDark && {
              "&:hover .MuiOutlinedInput-notchedOutline": {
                borderColor: "rgba(255,255,255,0.22)",
              },
              "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
                borderColor: "#2a9ba3",
              },
            }),
          },
        },
      },
      MuiInputLabel: {
        styleOverrides: {
          root: {
            ...(isDark && {
              color: "#8c8c96",
              "&.Mui-focused": { color: "#2a9ba3" },
            }),
          },
        },
      },

      // ── Buttons ──────────────────────────────────────────────────────────
      MuiButton: {
        styleOverrides: {
          containedPrimary: {
            ...(isDark && {
              backgroundColor: "#2a9ba3",
              color: "#0b0b0d",
              fontWeight: 600,
              "&:hover": { backgroundColor: "#33b0b9" },
            }),
          },
          outlinedPrimary: {
            ...(isDark && {
              borderColor: "rgba(255,255,255,0.14)",
              color: "#c4c4cc",
              "&:hover": {
                borderColor: "rgba(255,255,255,0.26)",
                backgroundColor: "rgba(255,255,255,0.05)",
              },
            }),
          },
          text: {
            ...(isDark && {
              color: "#c4c4cc",
              "&:hover": { backgroundColor: "rgba(255,255,255,0.05)" },
            }),
          },
          textPrimary: {
            ...(isDark && {
              color: "#2a9ba3",
              "&:hover": { backgroundColor: "rgba(42,155,163,0.08)" },
            }),
          },
        },
      },
      MuiIconButton: {
        styleOverrides: {
          root: {
            ...(isDark && {
              color: "#8c8c96",
              "&:hover": {
                backgroundColor: "rgba(255,255,255,0.07)",
                color: "#f2f2f0",
              },
            }),
          },
        },
      },

      // ── Alerts ───────────────────────────────────────────────────────────
      MuiAlert: {
        styleOverrides: {
          root: {
            ...(isDark && {
              backgroundColor: "rgba(255,255,255,0.05)",
              border: "1px solid rgba(255,255,255,0.08)",
              color: "#f2f2f0",
            }),
          },
          icon: {
            ...(isDark && { color: "#8c8c96 !important" }),
          },
        },
      },

      // ── Divider ──────────────────────────────────────────────────────────
      MuiDivider: {
        styleOverrides: {
          root: {
            borderColor: isDark
              ? "rgba(255,255,255,0.08)"
              : "rgba(15,82,87,0.12)",
          },
        },
      },

      // ── AppBar ───────────────────────────────────────────────────────────
      MuiAppBar: {
        styleOverrides: {
          root: {
            ...(isDark && {
              backgroundColor: "rgba(11,11,13,0.88)",
              backdropFilter: "blur(14px)",
              borderBottom: "1px solid rgba(255,255,255,0.08)",
              boxShadow: "none",
            }),
          },
        },
      },

      // ── Menu / Dropdown ──────────────────────────────────────────────────
      MuiMenu: {
        styleOverrides: {
          paper: {
            ...(isDark && {
              backgroundColor: "#242428",
              border: "1px solid rgba(255,255,255,0.1)",
              boxShadow: "0 8px 32px rgba(0,0,0,0.6)",
            }),
          },
        },
      },
      MuiMenuItem: {
        styleOverrides: {
          root: {
            ...(isDark && {
              color: "#c4c4cc",
              "&:hover": { backgroundColor: "rgba(255,255,255,0.06)" },
              "&.Mui-selected": {
                backgroundColor: "rgba(42,155,163,0.12)",
                color: "#2a9ba3",
              },
            }),
          },
        },
      },

      // ── Dialog ───────────────────────────────────────────────────────────
      MuiDialog: {
        styleOverrides: {
          paper: {
            ...(isDark && {
              backgroundColor: "#242428",
              border: "1px solid rgba(255,255,255,0.1)",
              boxShadow: "0 16px 64px rgba(0,0,0,0.7)",
            }),
          },
        },
      },
      MuiDialogTitle: {
        styleOverrides: {
          root: {
            ...(isDark && { color: "#f2f2f0" }),
          },
        },
      },

      // ── Select ───────────────────────────────────────────────────────────
      MuiSelect: {
        styleOverrides: {
          icon: {
            ...(isDark && { color: "#8c8c96" }),
          },
        },
      },

      // ── Tooltip ──────────────────────────────────────────────────────────
      MuiTooltip: {
        styleOverrides: {
          tooltip: {
            ...(isDark && {
              backgroundColor: "#2e2e33",
              color: "#f2f2f0",
              border: "1px solid rgba(255,255,255,0.1)",
              fontSize: "0.75rem",
            }),
          },
        },
      },
    },
  });
}

export function AppThemeProvider({ children }: { children: React.ReactNode }) {
  const [mode, setMode] = useState<Mode>(
    () => (localStorage.getItem("clear-erp.theme") as Mode | null) ?? "light",
  );

  const toggleMode = useCallback(() => {
    setMode((prev) => {
      const next: Mode = prev === "light" ? "dark" : "light";
      localStorage.setItem("clear-erp.theme", next);
      return next;
    });
  }, []);

  const theme = useMemo(() => buildTheme(mode), [mode]);

  return (
    <ThemeContext.Provider value={{ mode, toggleMode }}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </ThemeContext.Provider>
  );
}
