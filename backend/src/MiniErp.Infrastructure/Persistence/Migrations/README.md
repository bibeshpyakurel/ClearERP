# Migrations

This folder is reserved for Entity Framework Core migrations.

Once the `.NET` SDK is installed, generate and apply the first migration with:

```bash
dotnet ef migrations add InitialCreate --project backend/src/MiniErp.Infrastructure --startup-project backend/src/MiniErp.Api --output-dir Persistence/Migrations
dotnet ef database update --project backend/src/MiniErp.Infrastructure --startup-project backend/src/MiniErp.Api
```

These commands were not run in the current environment because `dotnet` is not installed.
