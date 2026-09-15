import { env } from "../../app/env";

const storageKey = env.authTokenStorageKey;
const expiryKey = `${env.authTokenStorageKey}:expiry`;

export const tokenStorage = {
  get() {
    return window.localStorage.getItem(storageKey);
  },
  getExpiry() {
    return window.localStorage.getItem(expiryKey);
  },
  isExpired() {
    const expiry = window.localStorage.getItem(expiryKey);
    if (!expiry) return true;

    // Fail closed. An unparseable expiry yields Invalid Date, and every
    // comparison against Invalid Date is false — so a corrupted value used to
    // read as "not expired" and the app would keep sending a stale token.
    const expiresAt = new Date(expiry);
    if (Number.isNaN(expiresAt.getTime())) return true;

    return expiresAt <= new Date();
  },
  set(token: string, expiresAtUtc: string) {
    window.localStorage.setItem(storageKey, token);
    window.localStorage.setItem(expiryKey, expiresAtUtc);
  },
  clear() {
    window.localStorage.removeItem(storageKey);
    window.localStorage.removeItem(expiryKey);
  },
};
