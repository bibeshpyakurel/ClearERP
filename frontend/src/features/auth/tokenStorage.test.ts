import { beforeEach, describe, expect, it, vi, afterEach } from "vitest";
import { tokenStorage } from "./tokenStorage";

/**
 * tokenStorage decides whether the app believes it still has a valid session.
 * The property that matters most is that it fails *closed*: anything other
 * than a stored expiry in the future must read as expired, because treating an
 * unknown state as "still signed in" is how a stale token reaches the API.
 */

const createLocalStorage = () => {
  const store = new Map<string, string>();
  return {
    getItem: (k: string) => (store.has(k) ? store.get(k)! : null),
    setItem: (k: string, v: string) => void store.set(k, String(v)),
    removeItem: (k: string) => void store.delete(k),
    clear: () => store.clear(),
    get size() {
      return store.size;
    },
  };
};

let localStorageStub: ReturnType<typeof createLocalStorage>;

beforeEach(() => {
  localStorageStub = createLocalStorage();
  vi.stubGlobal("window", { localStorage: localStorageStub });
});

afterEach(() => {
  vi.unstubAllGlobals();
  vi.useRealTimers();
});

describe("round trip", () => {
  it("stores and returns a token with its expiry", () => {
    tokenStorage.set("token-abc", "2099-01-01T00:00:00.000Z");

    expect(tokenStorage.get()).toBe("token-abc");
    expect(tokenStorage.getExpiry()).toBe("2099-01-01T00:00:00.000Z");
  });

  it("returns null when nothing has been stored", () => {
    expect(tokenStorage.get()).toBeNull();
    expect(tokenStorage.getExpiry()).toBeNull();
  });

  it("clear removes both the token and the expiry", () => {
    tokenStorage.set("token-abc", "2099-01-01T00:00:00.000Z");

    tokenStorage.clear();

    expect(tokenStorage.get()).toBeNull();
    expect(tokenStorage.getExpiry()).toBeNull();
    expect(localStorageStub.size).toBe(0);
  });
});

describe("expiry fails closed", () => {
  it("reports expired when no expiry has ever been stored", () => {
    expect(tokenStorage.isExpired()).toBe(true);
  });

  it("reports expired when a token exists but its expiry does not", () => {
    // A half-written session is not a valid one.
    localStorageStub.setItem("clear-erp.access-token", "orphan-token");

    expect(tokenStorage.isExpired()).toBe(true);
  });

  it("reports expired for an unparseable expiry", () => {
    tokenStorage.set("token-abc", "not-a-date");

    // new Date("not-a-date") is Invalid Date, and every comparison against it
    // is false — so without an explicit NaN check this reads as a live session
    // and the app keeps sending a stale token.
    expect(tokenStorage.isExpired()).toBe(true);
  });
});

describe("expiry boundary", () => {
  it("is not expired while the expiry is in the future", () => {
    vi.useFakeTimers();
    vi.setSystemTime(new Date("2026-01-01T00:00:00.000Z"));
    tokenStorage.set("token-abc", "2026-01-01T00:00:01.000Z");

    expect(tokenStorage.isExpired()).toBe(false);
  });

  it("is expired once the expiry is in the past", () => {
    vi.useFakeTimers();
    vi.setSystemTime(new Date("2026-01-01T00:00:00.000Z"));
    tokenStorage.set("token-abc", "2025-12-31T23:59:59.000Z");

    expect(tokenStorage.isExpired()).toBe(true);
  });

  it("is expired exactly at the expiry instant, not one tick later", () => {
    vi.useFakeTimers();
    vi.setSystemTime(new Date("2026-01-01T00:00:00.000Z"));
    tokenStorage.set("token-abc", "2026-01-01T00:00:00.000Z");

    // The check is `expiry <= now`, so the boundary counts as expired.
    expect(tokenStorage.isExpired()).toBe(true);
  });
});
