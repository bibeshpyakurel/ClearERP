# Decisions

Records of choices that were not obvious, so they are not relitigated from
scratch later. Newest first. A decision stays here once made; if it is reversed,
the reversal is appended rather than the entry deleted.

Several of these describe work done in August 2026 and written down in
September. They were reconstructed from `render.yaml`, the diff of
`28fb349`, and the code they produced — the reasoning existed, it just lived in
inline comments and commit subjects like `deployable` instead of anywhere a
reader would look.

---

## 2026-08-07 — Move hosting from Railway to Render + Cloudflare Pages + Neon

**Status:** accepted, in production.

ClearERP ran as a single Railway deployment. It now runs as three pieces: the
.NET API on Render as a Docker service, the React frontend on Cloudflare Pages,
and Postgres on Neon.

**Why split it up.** The parts have genuinely different shapes. The frontend is
static output that wants a CDN and costs nothing to serve. The API is a
long-running container that needs a runtime. Keeping them in one platform meant
paying container prices to serve static assets and accepting one platform's
opinion about both.

**Why Neon rather than Render's Postgres.** Render's free Postgres instances
expire 30 days after creation and are then deleted. For a portfolio project
whose entire purpose is that a reviewer can open the live demo at any time, a
database with a 30-day fuse is not a database. Neon's free tier persists. This
is recorded at the top of `render.yaml` too, because that is where someone
provisioning the blueprint will be standing when the question occurs to them.

**What it cost.** Render's free tier sleeps after 15 minutes of inactivity, so
the first request after an idle period takes 30–60 seconds while the container
and database wake. That trade is stated plainly in the README rather than hidden,
because a reviewer hitting a 45-second cold start with no explanation concludes
the app is broken.

**Follow-up that was needed:** see the two entries below. Neither was
anticipated.

---

## 2026-08-07 — Accept `DATABASE_URL` as a URI and convert it to Npgsql format

**Status:** accepted.

Managed Postgres providers hand out a connection **URI**
(`postgresql://user:pass@host/db?sslmode=require`). Npgsql wants a
semicolon-delimited keyword string. Something has to translate, and the options
were to make the operator paste a hand-converted string or to do it in code.

Doing it in code won, because hand conversion is exactly the kind of step that
is performed correctly the first time and wrongly at 2am during an incident.
`DATABASE_URL` is accepted verbatim and takes priority over
`ConnectionStrings__DefaultConnection` when both are set.

Two details in the conversion were not obvious and are the reason this entry
exists:

- **`Uri.Port` is `-1` when the URI omits the port**, which managed providers
  routinely do. Passing `-1` through produces a connection string that fails
  with an error naming neither the port nor the URI. The code defaults to 5432.
- **Userinfo is percent-encoded per RFC 3986.** Generated passwords regularly
  contain `@`, `/` and `%`, which must be decoded before Npgsql sees them.
  Without `Uri.UnescapeDataString`, roughly one generated password in ten fails
  authentication with a plain "password authentication failed" and no hint that
  the password was mangled in transit.

TLS is forced on (`SSL Mode=Require`) regardless of what the URI asked for.

---

## 2026-08-07 — `rootDir` changes what Docker paths resolve against

**Status:** accepted. Recorded because it cost a broken deploy.

The first Render blueprint set `rootDir: backend` alongside `dockerfilePath` and
`dockerContext` written relative to the repository root. Render resolves those
two paths relative to `rootDir`, not the repository, so the build looked for the
Dockerfile in `backend/backend/` and failed.

Fixed in `e48dbce` by making both paths relative to `rootDir`. The Dockerfile's
`COPY src/...` instructions expect a context of
`/opt/render/project/src/backend`, which is what the corrected paths produce.

The general lesson, which applies well beyond Render: when a platform offers a
"root directory" setting, establish what every *other* path in that file
resolves against before assuming it is the repository root. The failure mode is
a path error that names a directory nobody wrote down.

---

## 2026-09-15 — The frontend is tested, not only built

**Status:** accepted.

CI built the frontend and ran no tests against it, while the .NET backend had 36
test files and a real Postgres 16 service container. The asymmetry was not a
judgement that the frontend mattered less; there was simply no test tooling in
`frontend/`.

Vitest now runs over the two modules every request passes through: the API
client and the token store. Both are at 100% coverage, with thresholds set high
because this is a small, deliberately chosen surface rather than a whole-app
average.

Writing them immediately found a bug. `tokenStorage.isExpired()` returned `true`
when no expiry was stored — failing closed, correctly — but a **malformed**
expiry produced `Invalid Date`, and every comparison against `Invalid Date` is
`false`, so a corrupted value read as a live session and the app kept sending a
stale token. It now checks for `NaN` explicitly and fails closed there too.
