import MenuRoundedIcon from "@mui/icons-material/MenuRounded";
import LogoutRoundedIcon from "@mui/icons-material/LogoutRounded";
import {
  AppBar,
  Box,
  Drawer,
  IconButton,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Stack,
  Toolbar,
  Typography,
  useMediaQuery,
  useTheme,
  Button,
  Chip,
} from "@mui/material";
import { useState } from "react";
import { NavLink, Outlet, useLocation } from "react-router-dom";
import { env } from "../app/env";
import { navigationItems } from "../app/navigation";
import { useAuth } from "../features/auth/AuthContext";

const drawerWidth = 280;

function SidebarContent({
  primaryRole,
  onNavigate,
}: {
  primaryRole: string | null;
  onNavigate?: () => void;
}) {
  const location = useLocation();
  const visibleNavigationItems = navigationItems.filter(
    (item) => !item.allowedRoles || (primaryRole ? item.allowedRoles.includes(primaryRole) : false),
  );

  return (
    <Box
      sx={{
        height: "100%",
        px: 2,
        py: 3,
        background:
          "linear-gradient(180deg, #103a41 0%, #0f5257 50%, #1f6a61 100%)",
        color: "common.white",
      }}
    >
      <Stack spacing={3}>
        <Box>
          <Typography variant="overline" sx={{ letterSpacing: 2.4, opacity: 0.8 }}>
            ClearERP
          </Typography>
          <Typography variant="h5">Operations Console</Typography>
          <Typography variant="body2" sx={{ opacity: 0.8, mt: 1 }}>
            Inventory, procurement, and reporting in one workspace.
          </Typography>
        </Box>

        <List disablePadding sx={{ display: "grid", gap: 1 }}>
          {visibleNavigationItems.map((item) => {
            const isActive = location.pathname === item.path;
            const Icon = item.icon;

            return (
              <ListItemButton
                key={item.path}
                component={NavLink}
                to={item.path}
                onClick={onNavigate}
                sx={{
                  borderRadius: 3,
                  color: "inherit",
                  bgcolor: isActive ? "rgba(255,255,255,0.14)" : "transparent",
                  "&:hover": {
                    bgcolor: "rgba(255,255,255,0.1)",
                  },
                }}
              >
                <ListItemIcon sx={{ color: "inherit", minWidth: 40 }}>
                  <Icon />
                </ListItemIcon>
                <ListItemText primary={item.label} />
              </ListItemButton>
            );
          })}
        </List>
      </Stack>
    </Box>
  );
}

export function AppShell() {
  const theme = useTheme();
  const isDesktop = useMediaQuery(theme.breakpoints.up("lg"));
  const [mobileOpen, setMobileOpen] = useState(false);
  const { currentUser, primaryRole, logout } = useAuth();

  return (
    <Box
      sx={{
        minHeight: "100vh",
        background:
          "linear-gradient(180deg, #f4efe6 0%, #e4eee5 45%, #d8e3da 100%)",
      }}
    >
      <AppBar
        position="fixed"
        color="transparent"
        elevation={0}
        sx={{
          width: { lg: `calc(100% - ${drawerWidth}px)` },
          ml: { lg: `${drawerWidth}px` },
          backdropFilter: "blur(14px)",
          borderBottom: "1px solid rgba(15, 82, 87, 0.12)",
        }}
      >
        <Toolbar sx={{ minHeight: 80 }}>
          {!isDesktop && (
            <IconButton edge="start" onClick={() => setMobileOpen(true)} sx={{ mr: 2 }}>
              <MenuRoundedIcon />
            </IconButton>
          )}

          <Stack direction="row" spacing={2} alignItems="center" sx={{ flexGrow: 1, minWidth: 0 }}>
            <Box>
              <Typography variant="h6" sx={{ fontWeight: 700 }}>
                {env.appName}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Demo-friendly operations workspace
              </Typography>
            </Box>
            <Chip
              label="Seeded demo data"
              size="small"
              sx={{ display: { xs: "none", md: "inline-flex" } }}
            />
          </Stack>

          <Stack
            direction={{ xs: "column-reverse", sm: "row" }}
            spacing={1.25}
            alignItems={{ xs: "flex-end", sm: "center" }}
          >
            <Chip
              label={primaryRole ?? "User"}
              color="primary"
              variant="outlined"
              size="small"
            />
            <Typography
              variant="body2"
              color="text.secondary"
              sx={{ display: { xs: "none", md: "block" }, maxWidth: 240 }}
              noWrap
            >
              {currentUser?.email ?? "Signed in"}
            </Typography>
            <Button
              color="inherit"
              startIcon={<LogoutRoundedIcon />}
              onClick={logout}
            >
              Sign out
            </Button>
          </Stack>
        </Toolbar>
      </AppBar>

      <Box component="nav" sx={{ width: { lg: drawerWidth }, flexShrink: { lg: 0 } }}>
        <Drawer
          variant={isDesktop ? "permanent" : "temporary"}
          open={isDesktop ? true : mobileOpen}
          onClose={() => setMobileOpen(false)}
          ModalProps={{ keepMounted: true }}
          sx={{
            "& .MuiDrawer-paper": {
              width: drawerWidth,
              border: "none",
            },
          }}
        >
          <SidebarContent primaryRole={primaryRole} onNavigate={() => setMobileOpen(false)} />
        </Drawer>
      </Box>

      <Box
        component="main"
        sx={{
          ml: { lg: `${drawerWidth}px` },
          px: { xs: 2, md: 4 },
          pt: { xs: 12, md: 14 },
          pb: 4,
        }}
      >
        <Outlet />
      </Box>
    </Box>
  );
}
