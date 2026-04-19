# ClearERP

ClearERP is a full-stack internal business web app built to simulate the kind of inventory and purchasing workflows used by manufacturing, warehousing, and operations-driven companies. It is designed as a portfolio project, but it follows the same core ideas you would expect in a real ERP module: traceable stock movement, purchasing workflows, reporting, and role-based access.

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

## Demo-First Product Goal

This repository is intentionally designed to feel like a realistic ERP application without becoming heavy to evaluate.

The target experience is:

- technically credible for engineers and hiring teams
- simple enough to explore in one short session
- easy to boot locally or in Docker
- immediately usable with seeded data and demo personas

That means the project favors complete workflows, seeded scenarios, and guided exploration over complex tenant setup, user provisioning, or enterprise onboarding steps.

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
│   │   ├── ClearErp.Api/
│   │   ├── ClearErp.Application/
│   │   ├── ClearErp.Domain/
│   │   └── ClearErp.Infrastructure/
│   └── tests/
│       ├── ClearErp.UnitTests/
│       └── ClearErp.IntegrationTests/
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

- `admin@clearerp.local` / `Admin123!`
- `warehouse@clearerp.local` / `Warehouse123!`

Suggested evaluator flow:

- sign in as `admin@clearerp.local`
- review dashboard KPIs
- inspect items and suppliers
- open purchase orders and receive stock
- confirm the resulting inventory and report changes
- check audit logs for traceability

## Setup Instructions

### Prerequisites

Install:

- `.NET 10 SDK` or the SDK pinned in `backend/global.json`
- `Node.js`
- `PostgreSQL 16`

Optional:

- `Docker Desktop` or Docker Engine with Compose

### Quick Start

1. Copy the example environment files
2. Start PostgreSQL
3. Run the backend API
4. Run the frontend dev server
5. Open `http://localhost:5173`
6. Sign in with `admin@clearerp.local` / `Admin123!`

Create local environment files:

```bash
cp backend/.env.example backend/.env
cp frontend/.env.example frontend/.env.local
```

The committed example values are intentionally for local development only. Do not reuse them outside a local machine or CI environment you control.

### Local Development

Start PostgreSQL if it is not already running:

```bash
brew services start postgresql@16
```

Restore the backend dependencies:

```bash
cd "/Users/bibeshpyakurel/Documents/GitHub/ClearERP"
export DOTNET_ROOT=/opt/homebrew/opt/dotnet@9/libexec
export PATH="/opt/homebrew/opt/dotnet@9/bin:$PATH"
dotnet restore backend/ClearErp.sln
```

Run the backend:

```bash
cd "/Users/bibeshpyakurel/Documents/GitHub/ClearERP"
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ROOT=/opt/homebrew/opt/dotnet@9/libexec
export PATH="/opt/homebrew/opt/dotnet@9/bin:$PATH"
dotnet run --no-build --project backend/src/ClearErp.Api/ClearErp.Api.csproj --urls http://127.0.0.1:5000
```

Run the frontend:

```bash
cd "/Users/bibeshpyakurel/Documents/GitHub/ClearERP/frontend"
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
cp backend/.env.example backend/.env
docker compose up --build
```

Expected URLs:

- frontend: `http://localhost:5173`
- backend: `http://127.0.0.1:5000`
- Swagger: `http://127.0.0.1:5000/swagger`

## Database and Migration Notes

The project uses PostgreSQL with Entity Framework Core.

Current development behavior:

- the backend applies EF Core migrations on startup with `MigrateAsync()`
- seed data is applied through EF Core model seeding

Important note:

- the initial migration is committed in the repository under `backend/src/ClearErp.Infrastructure/Persistence/Migrations`
- integration tests require Docker because they use PostgreSQL Testcontainers

Migration workflow:

```bash
dotnet ef migrations add InitialCreate --project backend/src/ClearErp.Infrastructure --startup-project backend/src/ClearErp.Api
dotnet ef database update --project backend/src/ClearErp.Infrastructure --startup-project backend/src/ClearErp.Api
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

Run the full backend test suite:

```bash
dotnet test backend/ClearErp.sln --nologo
```

Run frontend verification:

```bash
cd frontend
npm ci
npm run build
```

Note: the integration test project starts a real PostgreSQL container. If Docker is not running, those tests will fail fast.

## Demo Deployment Notes

For a recruiter-friendly deployment, keep the operating model simple:

- deploy a single backend API and single frontend app
- use the seeded demo dataset
- expose the two demo accounts from this README
- keep environment configuration to connection string, JWT settings, and frontend API base URL
- prefer Docker Compose or two small containers over a complex orchestrated setup

If you want a public demo, treat the environment as read-mostly and refresh the database regularly so the walkthrough stays predictable.

## CI

GitHub Actions is configured to:

- restore and build the backend
- run unit and integration tests
- install frontend dependencies and build the Vite app

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
