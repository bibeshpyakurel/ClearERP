# Migrations

This folder contains Entity Framework Core migrations for the ClearERP database schema.

## Apply migrations

```bash
dotnet ef database update --project backend/src/ClearErp.Infrastructure --startup-project backend/src/ClearErp.Api
```

## Add a new migration (after changing the domain model)

```bash
dotnet ef migrations add <MigrationName> --project backend/src/ClearErp.Infrastructure --startup-project backend/src/ClearErp.Api --output-dir Persistence/Migrations
```

## Remove the last migration (if not yet applied)

```bash
dotnet ef migrations remove --project backend/src/ClearErp.Infrastructure --startup-project backend/src/ClearErp.Api
```

> **Note:** Migrations are applied automatically on startup via `MigrateAsync()` in `Program.cs`.
> You only need to run `database update` manually when working outside of the running application (e.g., in CI/CD pipelines or for production deployments).
