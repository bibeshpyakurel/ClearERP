# Runbook

What to do when ClearERP misbehaves.

## At a glance

| Thing | Where |
| ----- | ----- |
| API | Render, Docker service `clearerp-api`, region oregon |
| Frontend | Cloudflare (`clearerp.bibespyakurel1100.workers.dev`) |
| Database | Neon Postgres, free tier |
| Health endpoint | `/health` — Render's own `healthCheckPath` |
| Blueprint | `render.yaml` |

## Symptom: the first request takes 30–60 seconds

**This is expected, not an incident.** Render's free tier sleeps the container
after 15 minutes of inactivity, and Neon suspends idle compute. The first
request wakes both. Subsequent requests are fast.

Only investigate if a *warm* request is slow, or if the wake takes longer than
about 90 seconds.

## Symptom: the API will not start after a deploy

Check the Render deploy log before anything else. The two failures seen so far:

1. **Docker path errors naming a directory nobody wrote.** `rootDir` changes
   what `dockerfilePath` and `dockerContext` resolve against — they are relative
   to `rootDir`, not the repository root. See `DECISIONS.md`.
2. **Database connection failures.** See below.

## Symptom: "password authentication failed" against Neon

Almost always the connection string rather than the credentials.

- `DATABASE_URL` takes priority over `ConnectionStrings__DefaultConnection` when
  both are set. If someone added the second to "fix" the first, the second is
  being ignored.
- Paste Neon's **pooled** URI verbatim. The app handles percent-decoding of the
  password and defaults the port to 5432 when the URI omits it — pre-converting
  it by hand reintroduces exactly the bugs that conversion exists to avoid.
- TLS is forced on regardless of what the URI requests.

Verify the connection independently before changing anything:

```bash
psql "$DATABASE_URL" -c 'select 1'
```

## Symptom: the frontend loads but every request fails with CORS

`CORS_ALLOWED_ORIGIN` on the Render service is a comma-separated allowlist of
frontend origins. A new Cloudflare preview domain is a new origin and will be
refused until it is added.

## Symptom: a user sees data from another tenant

**Stop and treat this as a security incident.** Tenant isolation is enforced by
EF Core global query filters. A query that has called `IgnoreQueryFilters()`, or
a raw SQL path that bypasses the DbContext, is the first thing to look for.

Do not fix it by filtering in the controller. The isolation belongs in one place,
and adding a second place to get it right is how the first place stops being
trusted.

## Symptom: CI is red on main

- **`build-test`** — backend tests run against a real Postgres 16 service
  container, so a failure here is usually a genuine migration or query problem
  rather than a mocking artefact.
- **Frontend tests** — coverage thresholds are high (90/85/90/90) because they
  cover a deliberately small surface: the API client and token store. If new
  code drops coverage, extend the surface or the tests, not the threshold.

## Escalation

There is no second on-call. Roll back to the previous Render deploy first; it
is one click and it buys time to think.
