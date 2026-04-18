const getEnvValue = (key: string, fallback: string) => {
  const value = import.meta.env[key as keyof ImportMetaEnv];
  return typeof value === "string" && value.length > 0 ? value : fallback;
};

export const env = {
  appName: getEnvValue("VITE_APP_NAME", "ClearERP"),
  apiBaseUrl: getEnvValue("VITE_API_BASE_URL", "http://localhost:5000/api"),
  authTokenStorageKey: getEnvValue(
    "VITE_AUTH_TOKEN_STORAGE_KEY",
    "clear-erp.access-token",
  ),
};
