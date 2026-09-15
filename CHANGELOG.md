# Changelog

Notable changes to ClearERP. Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/);
versions follow [Semantic Versioning](https://semver.org/).

Entries from v0.1.0 onward are generated from Conventional Commits by
`scripts/build-release-notes.sh`. Anything earlier predates the convention and
is summarised by hand.

## [Unreleased]

Nothing yet.

## [0.1.0] — 2026-09-15

First tagged release. The application has been deployed since August; this tag
gives a name to what is running.

### Added

- Multi-tenant inventory and procurement across six industry verticals, with
  tenant isolation enforced by EF Core global query filters
- Traceable stock movements with full transaction history
- Multi-step procurement workflow: draft, approve, receive
- Role-based access control for Admin, Inventory Manager and Warehouse Staff
- Operational reporting with KPIs and low-stock alerts
- A complete audit trail
- Demo-first seeding: six pre-loaded companies and quick-fill login

### Fixed

- `tokenStorage.isExpired()` now fails closed on a malformed expiry. An
  unparseable value produced `Invalid Date`, every comparison against which is
  false, so a corrupted expiry read as a live session and the client kept
  sending a stale token.

### Infrastructure

- Hosting moved from Railway to Render, Cloudflare Pages and Neon. Reasoning,
  including why Neon rather than Render's 30-day-expiry free Postgres, is in
  `DECISIONS.md`.
- `DATABASE_URL` accepted as a URI and converted to Npgsql format, handling the
  omitted-port and percent-encoded-password cases
- The frontend is now tested, not only built: 20 Vitest tests over the API
  client and token store at 100% coverage
- Conventional commits enforced on pull requests; releases generated from them
- `main` protected, CodeQL and Dependabot enabled, actions pinned to SHAs
- `.DS_Store` untracked

[Unreleased]: https://github.com/bibeshpyakurel/ClearERP/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/bibeshpyakurel/ClearERP/releases/tag/v0.1.0
