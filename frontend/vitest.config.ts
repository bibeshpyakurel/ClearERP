import { defineConfig } from "vitest/config";

export default defineConfig({
  test: {
    // Plain Node: these are unit tests over pure logic. Browser globals that
    // the code under test needs (window.localStorage, fetch) are stubbed
    // explicitly in the tests, which keeps what is being faked visible.
    environment: "node",
    include: ["src/**/*.{test,spec}.ts"],
    coverage: {
      provider: "v8",
      reporter: ["text-summary"],
      include: ["src/api/client.ts", "src/features/auth/tokenStorage.ts"],
      thresholds: { statements: 90, branches: 85, functions: 90, lines: 90 },
    },
  },
});
