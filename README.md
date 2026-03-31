# Mini ERP Inventory Management System

Mini ERP Inventory Management System is a full-stack internal business web app built to simulate the kind of inventory and purchasing workflows used by manufacturing, warehousing, and operations-driven companies. It is designed as a portfolio project, but it follows the same core ideas you would expect in a real ERP module: traceable stock movement, purchasing workflows, reporting, and role-based access.

## Project Summary

This project demonstrates how to build a modern ERP-style inventory system using:

- `React` for the frontend
- `ASP.NET Core Web API` for backend business logic
- `PostgreSQL` for data storage
- `OpenAPI / Swagger` for API exploration and testing

The application supports:

- secure sign-in
- item master management
- supplier management
- inventory tracking
- stock issue and stock adjustment workflows
- purchase order management
- goods receipt processing
- reporting and dashboard KPIs
- audit logging

## Resume-Ready Summary

Built a full-stack mini ERP inventory management system using `React`, `.NET 9`, `PostgreSQL`, and `OpenAPI`. Implemented item, supplier, inventory, purchasing, reporting, and audit-log workflows with role-based authentication, seeded demo data, and a layered backend architecture designed to model real internal business software.

## Architecture Overview

The project uses a standard 3-tier web application architecture:

```text
Browser
  -> React frontend
  -> ASP.NET Core API
  -> PostgreSQL database
```

### Frontend

The frontend is a React single-page application that runs in the browser. It handles:

- login flow
- protected routes
- forms and validation
- tables, reports, and dashboards
- API calls to the backend

### Backend

The backend is an ASP.NET Core Web API. It acts as the main business layer and handles:

- authentication and authorization
- ERP business rules
- stock movement logic
- purchase order workflows
- reporting queries
- audit logging
- database access through EF Core

### Database

The PostgreSQL database stores:

- users and roles
- items and categories
- suppliers and supplier-item mappings
- inventory balances
- inventory transactions
- purchase orders and purchase order lines
- goods receipts
- stock adjustments
- audit logs

## Tech Stack

### Frontend

- `Node.js 22`
- `React 19`
- `TypeScript 5`
- `Vite 7`
- `React Router 7`
- `@tanstack/react-query 5`
- `React Hook Form 7`
- `Zod 3`
- `MUI 7`

### Backend

- `.NET 9`
- `ASP.NET Core Web API`
- `Entity Framework Core 9`
- `Npgsql` PostgreSQL provider
- `FluentValidation`
- `Serilog`
- `JWT Bearer Authentication`
- `Swagger / OpenAPI`

### Database and Dev Tooling

- `PostgreSQL 16`
- `Docker Compose`

## Repository Structure

```text
.
├── backend/
│   ├── src/
│   │   ├── MiniErp.Api/
│   │   ├── MiniErp.Application/
│   │   ├── MiniErp.Domain/
│   │   └── MiniErp.Infrastructure/
│   └── tests/
│       ├── MiniErp.UnitTests/
│       └── MiniErp.IntegrationTests/
├── docs/
│   ├── api/
│   ├── architecture/
│   └── screenshots/
├── frontend/
└── README.md
```

## Business Workflows

### 1. Authentication

- user signs in with email and password
- backend verifies password hash
- API returns JWT token
- frontend stores token and uses it for protected requests

### 2. Item Master

- create inventory items
- assign category, SKU, unit, reorder level, and cost
- activate and deactivate items

### 3. Supplier Management

- create and maintain supplier records
- associate suppliers with items they provide

### 4. Purchase Orders

- create purchase orders
- add line items
- track status as draft, approved, partially received, completed, or cancelled

### 5. Goods Receipt

- receive stock against purchase orders
- update received quantities
- generate inventory transactions

### 6. Inventory Operations

- view balances by item and location
- issue stock for operational usage
- adjust stock for corrections or damage
- keep an auditable transaction history

### 7. Reporting

- dashboard KPIs
- low-stock reporting
- stock valuation
- purchase order summaries
- recent transaction activity

### 8. Audit Logging

- log key actions such as login, purchasing, receipts, stock issue, and stock adjustment
- expose read-only audit history for administrators

## Demo Data

The local demo database includes seeded data so the UI is useful immediately after startup.

Current demo coverage includes:

- `6` inventory items
- `3` suppliers
- `6` purchase orders across multiple statuses
- `7` inventory transactions
- low-stock examples
- audit log activity

Demo login accounts:

- `admin@minierp.local` / `Admin123!`
- `warehouse@minierp.local` / `Warehouse123!`

## Setup Instructions

### Prerequisites

Install:

- `.NET 9 SDK`
- `Node.js`
- `PostgreSQL 16`

Optional:

- `Docker Desktop` or Docker Engine with Compose

### Quick Start

1. Start PostgreSQL
2. Run the backend API
3. Run the frontend dev server
4. Open `http://localhost:5173`
5. Sign in with `admin@minierp.local` / `Admin123!`

### Local Development

Start PostgreSQL if it is not already running:

```bash
brew services start postgresql@16
```

Restore the backend dependencies:

```bash
cd "/Users/bibeshpyakurel/Documents/GitHub/Mini ERP Inventory Management System"
export DOTNET_ROOT=/opt/homebrew/opt/dotnet@9/libexec
export PATH="/opt/homebrew/opt/dotnet@9/bin:$PATH"
dotnet restore backend/MiniErp.sln
```

Run the backend:

```bash
cd "/Users/bibeshpyakurel/Documents/GitHub/Mini ERP Inventory Management System"
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ROOT=/opt/homebrew/opt/dotnet@9/libexec
export PATH="/opt/homebrew/opt/dotnet@9/bin:$PATH"
dotnet run --no-build --project backend/src/MiniErp.Api/MiniErp.Api.csproj --urls http://127.0.0.1:5000
```

Run the frontend:

```bash
cd "/Users/bibeshpyakurel/Documents/GitHub/Mini ERP Inventory Management System/frontend"
npm install
npm run dev -- --host 0.0.0.0 --port 5173
```

Open the app:

- frontend: `http://localhost:5173`
- backend health: `http://127.0.0.1:5000/health`
- Swagger: `http://127.0.0.1:5000/swagger`

### Docker Setup

The repo includes:

- `docker-compose.yml`
- `backend/Dockerfile`
- `frontend/Dockerfile`

To start with Docker:

```bash
docker compose up --build
```

Expected URLs:

- frontend: `http://localhost:5173`
- backend: `http://127.0.0.1:5000`
- Swagger: `http://127.0.0.1:5000/swagger`

## Database and Migration Notes

The project uses PostgreSQL with Entity Framework Core.

Current development behavior:

- in `Development`, the backend uses `EnsureCreated()` on startup to create the schema for the local demo database
- seed data is applied through EF Core model seeding

Important note:

- a full production-style migration history is not finalized yet
- the migrations folder currently needs proper generated migration files if you want a formal migration workflow

Recommended future migration workflow:

```bash
dotnet ef migrations add InitialCreate --project backend/src/MiniErp.Infrastructure --startup-project backend/src/MiniErp.Api
dotnet ef database update --project backend/src/MiniErp.Infrastructure --startup-project backend/src/MiniErp.Api
```

Database design reference:

- [requirements-and-domain-design.md](docs/architecture/requirements-and-domain-design.md)

API outline reference:

- [endpoint-outline.md](docs/api/endpoint-outline.md)

Screenshot reference:

- [docs/screenshots/README.md](docs/screenshots/README.md)

## API Documentation

Once the backend is running:

- Swagger UI: `http://127.0.0.1:5000/swagger`

Suggested demo flow in Swagger:

1. call `POST /api/auth/login`
2. copy the access token
3. click `Authorize`
4. paste `Bearer <token>`
5. test endpoints like:
   - `GET /api/items`
   - `GET /api/suppliers`
   - `GET /api/inventory/balances`
   - `GET /api/purchase-orders`
   - `GET /api/reports/stock-summary`
   - `GET /api/audit-logs`

## Screenshots

Suggested screenshots for this project should live in `docs/screenshots/`.

Recommended captures:

- login page
- dashboard
- items page
- suppliers page
- inventory page
- purchase orders page
- reports page
- audit logs page

This repo currently includes the screenshot directory and naming guide, but screenshot image files still need to be captured and added.

## Testing

The repository contains:

- backend unit tests for services and domain logic
- backend integration tests for API behaviors

Examples covered:

- auth flow
- item service behavior
- purchase order logic
- inventory issue and adjustment rules
- goods receipt workflow
- reporting behavior

## Future Improvements

- add formal EF Core migrations and migration history
- add self-service account creation or admin user management
- add multiple warehouses and location transfers
- add pagination and server-side filtering for large datasets
- improve frontend code splitting to reduce bundle size
- add screenshot assets to the repository
- add end-to-end browser tests
- add deployment instructions for cloud hosting
- support email notifications for low stock and approvals

## Why This Project Matters

This project is intentionally built as an internal operations tool rather than a generic CRUD demo. It focuses on the kinds of workflows that matter in real ERP-style software:

- traceable stock changes
- approval and receiving workflows
- operational reporting
- auditability
- role-based access

That makes it a strong portfolio piece for:

- ERP and business systems roles
- internal tools engineering
- backend and full-stack .NET roles
- inventory, operations, and manufacturing software teams
