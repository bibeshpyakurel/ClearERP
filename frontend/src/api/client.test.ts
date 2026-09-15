import { afterEach, describe, expect, it, vi } from "vitest";
import { ApiClientError, apiClient, setUnauthorizedHandler } from "./client";

/** Await a request that is expected to reject, and hand back the typed error. */
const expectRejection = async (promise: Promise<unknown>): Promise<ApiClientError> => {
  try {
    await promise;
    throw new Error("expected the request to reject, but it resolved");
  } catch (caught) {
    if (caught instanceof ApiClientError) return caught;
    throw caught;
  }
};

/**
 * The API client is the single place every request passes through, so the
 * behaviour worth pinning is not "it can fetch" but the things every caller
 * silently depends on: that the bearer token is attached, that a 401 triggers
 * the session-expiry path exactly once, that a 204 does not get parsed as
 * JSON, and that a non-JSON error body still produces a usable error rather
 * than a parse exception.
 */

const jsonResponse = (body: unknown, status = 200) =>
  new Response(JSON.stringify(body), {
    status,
    headers: { "Content-Type": "application/json" },
  });

afterEach(() => {
  vi.unstubAllGlobals();
  setUnauthorizedHandler(null);
});

describe("auth header", () => {
  it("sends a bearer token when one is supplied", async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ ok: true }));
    vi.stubGlobal("fetch", fetchMock);

    await apiClient.request("/items", { token: "abc123" });

    const headers = fetchMock.mock.calls[0][1].headers as Headers;
    expect(headers.get("Authorization")).toBe("Bearer abc123");
    expect(headers.get("Content-Type")).toBe("application/json");
  });

  it("omits the Authorization header entirely when there is no token", async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ ok: true }));
    vi.stubGlobal("fetch", fetchMock);

    await apiClient.request("/health", { token: null });

    const headers = fetchMock.mock.calls[0][1].headers as Headers;
    expect(headers.has("Authorization")).toBe(false);
  });

  it("does not send an Authorization header for an empty-string token", async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ ok: true }));
    vi.stubGlobal("fetch", fetchMock);

    await apiClient.request("/health", { token: "" });

    const headers = fetchMock.mock.calls[0][1].headers as Headers;
    expect(headers.has("Authorization")).toBe(false);
  });
});

describe("request shape", () => {
  it("defaults to GET and sends no body", async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({}));
    vi.stubGlobal("fetch", fetchMock);

    await apiClient.request("/items");

    expect(fetchMock.mock.calls[0][1].method).toBe("GET");
    expect(fetchMock.mock.calls[0][1].body).toBeUndefined();
  });

  it("serialises the body as JSON for a write", async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ id: 1 }));
    vi.stubGlobal("fetch", fetchMock);

    await apiClient.request("/items", { method: "POST", body: { name: "Bolt" } });

    expect(fetchMock.mock.calls[0][1].method).toBe("POST");
    expect(fetchMock.mock.calls[0][1].body).toBe('{"name":"Bolt"}');
  });
});

describe("responses", () => {
  it("returns undefined for 204 rather than trying to parse a body", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(new Response(null, { status: 204 })),
    );

    await expect(apiClient.request("/items/1", { method: "DELETE" })).resolves.toBeUndefined();
  });

  it("throws ApiClientError carrying status, traceId and field errors", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        jsonResponse(
          {
            status: 422,
            title: "Validation failed",
            detail: "Quantity must be positive.",
            traceId: "trace-9",
            errors: { quantity: ["must be positive"] },
          },
          422,
        ),
      ),
    );

    const error = await expectRejection(apiClient.request("/items"));

    expect(error.status).toBe(422);
    expect(error.message).toBe("Quantity must be positive.");
    expect(error.traceId).toBe("trace-9");
    expect(error.errors).toEqual({ quantity: ["must be positive"] });
  });

  it("still produces a usable error when the failure body is not JSON", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        new Response("<html>502 Bad Gateway</html>", { status: 502 }),
      ),
    );

    const error = await expectRejection(apiClient.request("/items"));

    expect(error.status).toBe(502);
    expect(error.message).toBe("The request could not be completed.");
    expect(error.traceId).toBe("unavailable");
  });
});

describe("401 handling", () => {
  it("invokes the unauthorized handler exactly once on a 401", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        jsonResponse(
          { status: 401, title: "Unauthorized", detail: "Token expired.", traceId: "t" },
          401,
        ),
      ),
    );
    const onUnauthorized = vi.fn();
    setUnauthorizedHandler(onUnauthorized);

    await apiClient.request("/items").catch(() => undefined);

    expect(onUnauthorized).toHaveBeenCalledTimes(1);
  });

  it("does not invoke the unauthorized handler for a 403", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        jsonResponse(
          { status: 403, title: "Forbidden", detail: "Wrong role.", traceId: "t" },
          403,
        ),
      ),
    );
    const onUnauthorized = vi.fn();
    setUnauthorizedHandler(onUnauthorized);

    await apiClient.request("/items").catch(() => undefined);

    // A 403 means "signed in, not allowed" — signing the user out would be wrong.
    expect(onUnauthorized).not.toHaveBeenCalled();
  });

  it("clearing the handler stops it firing", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        jsonResponse(
          { status: 401, title: "Unauthorized", detail: "Token expired.", traceId: "t" },
          401,
        ),
      ),
    );
    const onUnauthorized = vi.fn();
    setUnauthorizedHandler(onUnauthorized);
    setUnauthorizedHandler(null);

    await apiClient.request("/items").catch(() => undefined);

    expect(onUnauthorized).not.toHaveBeenCalled();
  });
});
