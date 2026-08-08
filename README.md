# ClearERP

A full-stack multi-tenant ERP system demonstrating inventory management, procurement workflows, and operational reporting across six industry verticals. Built as a portfolio project showcasing enterprise application architecture with .NET and React.

**Live Demo:** [https://clearerp.bibespyakurel1100.workers.dev](https://clearerp.bibespyakurel1100.workers.dev)

> The demo runs entirely on free hosting tiers, so the API sleeps after 15 minutes
> of inactivity. The first request after an idle period takes 30–60 seconds while
> the container and database wake up. Subsequent requests are fast.

---

## What is ClearERP?

ClearERP simulates the inventory and purchasing workflows found in real ERP systems used by manufacturing, warehousing, and operations-driven companies. Unlike generic CRUD demos, it implements the business logic that matters in production ERP software:

- **Traceable stock movements** with full transaction history
- **Multi-step procurement workflows** (draft → approve → receive)
- **Multi-tenant isolation** with per-company data separation
- **Role-based access control** (Admin, Inventory Manager, Warehouse Staff)
- **Operational reporting** with KPIs and low-stock alerts
- **Complete audit trail** for compliance and accountability

---

## Demo-First Design

This project is built for immediate evaluation. There's no complex setup, no account provisioning, and no database seeding required.

**Start the app and sign in** — everything works out of the box.

Each of the six demo companies comes pre-loaded with:
- Items, categories, and suppliers
- Purchase orders in various stages
- Inventory balances and transaction history
- Role-appropriate user accounts

The login page offers **quick-fill buttons** that auto-populate credentials, making it trivial to explore different companies and roles.

---

## Multi-Tenant Architecture

ClearERP demonstrates enterprise multi-tenancy: each company operates in complete isolation with its own data, users, and business context.

| Company | Industry | Focus Area |
|---------|----------|------------|
| **ClearFurniture Corp** | Furniture Manufacturing | Office furniture components and finished goods |
| **TechFlow Electronics** | Electronics | Circuit boards, components, and assemblies |
| **FreshFoods Co** | Food & Beverage | Perishable inventory with shelf-life management |
| **CloudPeak SaaS** | SaaS / Cloud | Software licenses and hardware assets |
| **NetBridge IT Services** | IT Services | Network equipment and service materials |
| **ShieldCore Cybersecurity** | Cybersecurity | Security appliances and monitoring tools |

This showcases how a single ERP platform adapts to fundamentally different business contexts — a key architectural decision in real-world enterprise software.

---

## Demo Credentials

### Furniture Manufacturing
| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@clearfurniture.local` | `Admin123!` |
| Warehouse | `warehouse@clearfurniture.local` | `Warehouse123!` |

### Electronics
| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@techflow-electronics.local` | `Admin123!` |
| Warehouse | `warehouse@techflow-electronics.local` | `Warehouse123!` |

### Food & Beverage
| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@freshfoods.local` | `Admin123!` |
| Warehouse | `warehouse@freshfoods.local` | `Warehouse123!` |

### SaaS / Cloud
| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@cloudpeak.local` | `Admin123!` |
| Warehouse | `warehouse@cloudpeak.local` | `Warehouse123!` |

### IT Services
| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@netbridge.local` | `Admin123!` |
| Warehouse | `warehouse@netbridge.local` | `Warehouse123!` |

### Cybersecurity
| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@shieldcore.local` | `Admin123!` |
| Warehouse | `warehouse@shieldcore.local` | `Warehouse123!` |

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                         Browser                                  │
│                    React SPA (Vite)                             │
├─────────────────────────────────────────────────────────────────┤
│                      REST API                                    │
│               ASP.NET Core Web API                              │
├───────────────┬───────────────┬───────────────┬─────────────────┤
│   API Layer   │  Application  │    Domain     │ Infrastructure  │
│  Controllers  │   Services    │   Entities    │   EF Core +     │
│  Contracts    │   Interfaces  │   Value Objs  │   PostgreSQL    │
│  Validation   │   DTOs        │   Enums       │   JWT Auth      │
└───────────────┴───────────────┴───────────────┴─────────────────┘
```

### Clean Architecture

The backend follows **Clean Architecture** principles with four distinct layers:

- **Domain** — Core business entities, value objects, and domain logic
- **Application** — Use cases, service interfaces, DTOs, and business rules
- **Infrastructure** — EF Core, database access, JWT token generation, external services
- **API** — Controllers, request/response contracts, validation, middleware

### Frontend Architecture

The React frontend uses a feature-based structure:

- **React 19** with TypeScript for type safety
- **React Router** for client-side navigation
- **TanStack Query** for server state management
- **React Hook Form** with Zod validation
- **Material UI** for the component library

---

## Tech Stack

### Frontend
| Technology | Version | Purpose |
|------------|---------|---------|
| React | 19 | UI library |
| TypeScript | 5 | Type safety |
| Vite | 7 | Build tool |
| React Router | 7 | Routing |
| TanStack Query | 5 | Server state |
| React Hook Form | 7 | Form management |
| Zod | 3 | Schema validation |
| Material UI | 7 | Component library |

### Backend
| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 9 | Runtime |
| ASP.NET Core | 9 | Web framework |
| Entity Framework Core | 9 | ORM |
| PostgreSQL | 16 | Database |
| FluentValidation | — | Request validation |
| Serilog | — | Structured logging |
| JWT Bearer | — | Authentication |
| Swagger/OpenAPI | — | API documentation |

### Infrastructure
| Technology | Purpose |
|------------|---------|
| Docker Compose | Local development orchestration |
| Docker | Containerized deployment (multi-stage builds) |
| Render | API hosting (Docker web service) |
| Cloudflare Workers | Frontend hosting (static assets on the edge) |
| Neon | Managed serverless PostgreSQL |
| GitHub Actions | CI/CD pipeline |
| Testcontainers | Integration test database |

---

## Repository Structure

```
ClearERP/
├── .github/
│   └── workflows/
│       └── ci.yml                  # GitHub Actions CI pipeline
├── backend/
│   ├── src/
│   │   ├── ClearErp.Api/           # Controllers, contracts, middleware, validation
│   │   ├── ClearErp.Application/   # Services, interfaces, DTOs
│   │   ├── ClearErp.Domain/        # Entities, enums, domain logic
│   │   └── ClearErp.Infrastructure/# EF Core, migrations, seeding, auth
│   ├── tests/
│   │   ├── ClearErp.UnitTests/
│   │   └── ClearErp.IntegrationTests/
│   ├── Dockerfile
│   ├── .env.example
│   └── ClearErp.sln
├── frontend/
│   ├── src/
│   │   ├── api/                    # API client and type definitions
│   │   ├── app/                    # App config, routes, navigation
│   │   ├── components/             # Shared UI components
│   │   ├── features/               # Feature modules (auth, items, POs, etc.)
│   │   ├── layouts/                # Page layouts
│   │   └── pages/                  # Route pages
│   ├── public/
│   ├── Dockerfile
│   ├── nginx.conf
│   ├── wrangler.jsonc               # Cloudflare Workers static-asset config
│   └── .env.example
├── docs/
│   ├── api/                        # API endpoint documentation
│   ├── architecture/               # Design documents
│   ├── screenshots/                # UI screenshots
│   └── demo-playbook.md
├── .env.example
├── render.yaml                     # Render Blueprint for the API
├── docker-compose.yml
└── SECURITY.md
```

---

## Business Workflows

### 1. Authentication & Authorization
- JWT-based authentication with role claims
- Tenant-scoped data isolation via global query filters
- Three role levels: Admin, Inventory Manager, Warehouse Staff

### 2. Item Master Management
- Create and maintain inventory items
- Assign categories, SKUs, units, and costs
- Set reorder thresholds for low-stock alerts
- Activate/deactivate items without deletion

### 3. Supplier Management
- Maintain supplier directory with contact info
- Map suppliers to the items they provide
- Track supplier-specific SKUs

### 4. Purchase Order Lifecycle
- **Draft** — Create and edit before submission
- **Approved** — Ready for receiving
- **Partially Received** — Some goods received
- **Completed** — Fully received
- **Cancelled** — Order cancelled

### 5. Goods Receipt
- Receive inventory against approved POs
- Select destination warehouse and location
- Automatic inventory balance updates
- Transaction audit trail

### 6. Inventory Operations
- View real-time balances by item/warehouse
- Issue stock for operational consumption
- Adjust stock for corrections, damage, or cycle counts
- Full transaction history with reasons

### 7. Reporting & Analytics
- Dashboard KPIs and trend indicators
- Low-stock alerts based on reorder levels
- Stock valuation reports
- Purchase order summaries
- Recent transaction activity

### 8. Audit Logging
- Track key actions: logins, purchases, receipts, adjustments
- Immutable audit trail for compliance
- Admin-accessible audit log viewer

---

## Setup Instructions

### Prerequisites

- **Node.js 22+**
- **.NET 9 SDK**
- **PostgreSQL 16**
- **Docker** (optional, for containerized setup)

### Quick Start with Docker

```bash
# Clone the repository
git clone https://github.com/bibeshpyakurel/ClearERP.git
cd ClearERP

# Copy environment files
cp backend/.env.example backend/.env
cp frontend/.env.example frontend/.env.local

# Start all services
docker compose up --build
```

Open:
- Frontend: http://localhost:5173
- Backend API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Local Development

**1. Start PostgreSQL**

```bash
# macOS with Homebrew
brew services start postgresql@16

# Or use Docker
docker run -d --name clearerp-db \
  -e POSTGRES_USER=clearerp \
  -e POSTGRES_PASSWORD=clearerp \
  -e POSTGRES_DB=clearerp \
  -p 5432:5432 \
  postgres:16
```

**2. Configure Environment**

```bash
cp backend/.env.example backend/.env
cp frontend/.env.example frontend/.env.local
```

**3. Run the Backend**

```bash
cd backend
dotnet restore
dotnet run --project src/ClearErp.Api
```

The backend will:
- Apply EF Core migrations automatically
- Seed all demo data on first run
- Listen on http://localhost:5000

**4. Run the Frontend**

```bash
cd frontend
npm install
npm run dev
```

Open http://localhost:5173

---

## Deployment

ClearERP deploys across three free tiers, with no credit card required:

| Component | Platform | Notes |
|-----------|----------|-------|
| API | [Render](https://render.com) | Docker web service, 750 instance-hrs/mo, 512 MB RAM |
| Frontend | [Cloudflare Workers](https://workers.cloudflare.com) | Static assets, global CDN, no cold start |
| Database | [Neon](https://neon.com) | Serverless Postgres, 0.5 GB storage, 100 CU-hrs/mo |

Render's own free PostgreSQL is deliberately **not** used: those instances expire
30 days after creation and are deleted after a 14-day grace period. Neon's free
plan has no such expiry.

### 1. Database (Neon)

1. Create a project at [neon.com](https://neon.com) — pick a region close to
   Render's `oregon` (e.g. AWS `us-west-2`) to keep query latency low.
2. Copy the **pooled** connection string from the dashboard — it looks like:

   ```
   postgresql://neondb_owner:PASSWORD@ep-xxx-pooler.us-west-2.aws.neon.tech/neondb?sslmode=require
   ```

That string is used as-is. The API accepts a `postgres://` URI via `DATABASE_URL`
and converts it to Npgsql's key/value format internally, always enforcing
`SSL Mode=Require` — Neon rejects unencrypted connections. If you'd rather supply
an Npgsql-format string directly, set `ConnectionStrings__DefaultConnection`
instead; `DATABASE_URL` wins when both are set.

No schema setup is needed. The API applies EF Core migrations and seeds demo data
automatically on first boot.

### 2. API (Render)

The repo ships a [`render.yaml`](render.yaml) Blueprint, so the service is defined
in code rather than clicked together in the dashboard.

1. In Render, choose **New → Blueprint** and connect this repository.
2. Render reads `render.yaml` and prompts for the two secrets it can't generate:
   - `DATABASE_URL` — the Neon connection string from step 1
   - `CORS_ALLOWED_ORIGIN` — leave a placeholder for now; you'll set it in step 4
3. Deploy. The first build takes a few minutes; migrations and seeding run on
   startup, so watch the logs until `/health` reports `Healthy`.
4. Note the assigned URL, e.g. `https://clearerp-api.onrender.com`.

`Jwt__Key` is generated by Render as a random 256-bit value. Rotating it
invalidates every issued token, forcing all users to sign in again.

### 3. Frontend (Cloudflare Workers)

The frontend deploys as a Workers static-asset project. There is no Worker
script — the Vite build is served straight from Cloudflare's edge, configured by
[`frontend/wrangler.jsonc`](frontend/wrangler.jsonc) rather than dashboard-only
settings.

1. In Cloudflare, choose **Workers & Pages → Create → Workers** and connect this
   repository.
2. Configure the build:

   | Setting | Value |
   |---------|-------|
   | Build command | `npm run build` |
   | Deploy command | `npx wrangler deploy` |
   | Non-production branch command | `npx wrangler versions upload` |
   | Root directory | `frontend` |

   Build and deploy are separate fields. There is no build-output setting — that
   comes from `assets.directory` in `wrangler.jsonc`.

3. Add the build-time environment variable — Vite inlines this at build time, so
   changing it later requires a rebuild, not just a restart:

   ```
   VITE_API_BASE_URL = https://clearerp-api.onrender.com/api
   ```

   The `/api` suffix matters; the client appends paths directly to this base.

4. Deploy, then note the assigned URL, e.g. `https://clearerp.workers.dev`.

SPA routing comes from `not_found_handling: "single-page-application"` in
`wrangler.jsonc`, so deep links like `/purchase-orders/42` resolve to
`index.html` instead of a 404. Do **not** add a Pages-style `_redirects` file
containing `/* /index.html 200` — the Workers asset router strips `/index` and
`.html` before matching and rejects that rule as an infinite loop, failing the
deploy.

### 4. Connect the two

Set `CORS_ALLOWED_ORIGIN` on the Render service to the Workers URL and redeploy:

```
CORS_ALLOWED_ORIGIN = https://clearerp.workers.dev
```

Use the exact origin, with no trailing slash. The value accepts a
comma-separated list, and Cloudflare gives preview versions their own
subdomains, so add any preview origins you actually use — they are distinct
origins and will otherwise fail CORS:

```
CORS_ALLOWED_ORIGIN = https://clearerp.workers.dev,https://dev-clearerp.workers.dev
```

### Free-tier tradeoffs

- **Cold starts.** Render free services spin down after 15 minutes idle and take
  30–60 seconds to wake. Neon scales its compute to zero after 5 minutes idle and
  wakes in a few hundred milliseconds.
- **Resist the urge to add a keep-alive ping.** Render's 750 monthly instance
  hours do cover one always-on service, but constant pings against `/health` also
  keep Neon's compute awake around the clock, which exceeds the free plan's
  100 CU-hours well before the month ends. `/health` runs a database check on
  every call.
- **Storage.** Neon's free plan allows 0.5 GB, comfortably above what the seeded
  demo data needs.

### Environment variable reference

| Variable | Where | Purpose |
|----------|-------|---------|
| `ASPNETCORE_ENVIRONMENT` | Render | `Production` |
| `DATABASE_URL` | Render | Neon `postgres://` URI (takes priority) |
| `ConnectionStrings__DefaultConnection` | Render | Alternative: Npgsql-format connection string |
| `CORS_ALLOWED_ORIGIN` | Render | Comma-separated allowed frontend origins |
| `Jwt__Key` | Render | JWT signing key (auto-generated) |
| `Jwt__Issuer` / `Jwt__Audience` | Render | Token issuer and audience |
| `Swagger__Enabled` | Render | Set `true` to expose `/swagger` in production |
| `VITE_API_BASE_URL` | Cloudflare Workers | API base URL including `/api` |

---

## API Documentation

With the backend running, explore the API at:

- **Swagger UI**: http://localhost:5000/swagger

### Testing the API

1. Call `POST /api/auth/login` with credentials
2. Copy the `accessToken` from the response
3. Click "Authorize" in Swagger
4. Enter: `Bearer <your-token>`
5. Explore endpoints:
   - `GET /api/items` — Item catalog
   - `GET /api/suppliers` — Supplier directory
   - `GET /api/categories` — Item categories
   - `GET /api/warehouses` — Warehouses with locations
   - `GET /api/inventory/balances` — Current stock levels
   - `GET /api/purchase-orders` — PO list
   - `GET /api/reports/stock-summary` — Stock summary
   - `GET /api/audit-logs` — Audit trail

---

## Testing

### Backend Tests

```bash
cd backend
dotnet test
```

Tests include:
- Unit tests for domain logic and services
- Integration tests with real PostgreSQL (via Testcontainers)

### Frontend Verification

```bash
cd frontend
npm run build    # Production build
npm run lint     # ESLint check
```

---

## Why ERP Across Industries?

ERP systems aren't one-size-fits-all. Different industries have fundamentally different:

| Industry | Key Challenge | ERP Focus |
|----------|--------------|-----------|
| **Manufacturing** | Bill of materials, production scheduling | Component tracking, work orders |
| **Electronics** | Rapid obsolescence, component variants | Revision control, lot tracking |
| **Food & Beverage** | Perishability, regulatory compliance | Shelf-life, batch traceability |
| **SaaS** | License management, subscription tracking | Asset management, renewals |
| **IT Services** | Project-based inventory, serial tracking | Equipment allocation, service items |
| **Cybersecurity** | Appliance deployment, firmware versions | Asset inventory, compliance tracking |

ClearERP demonstrates that **the same core ERP engine** — items, suppliers, procurement, inventory — adapts to serve any of these contexts. That's the architectural insight this project showcases.

---

## Why This Project Matters

This isn't a todo app or a generic CRUD demo. It's a **production-grade ERP simulation** that demonstrates:

### Technical Skills
- Clean Architecture with proper layer separation
- Multi-tenant data isolation at the database level
- Complex business workflows (procurement lifecycle)
- Role-based access control
- Full-stack TypeScript/C# development

### Business Domain Knowledge
- Understanding of ERP/inventory management concepts
- Appreciation for audit trails and compliance
- Workflow state machines (draft → approved → received)
- Operational reporting and KPIs

### Engineering Practices
- Feature-based code organization
- API-first development with OpenAPI
- Form validation with schema-based approaches
- Server state management patterns
- CI/CD pipeline configuration

---

## License

MIT License — see [LICENSE](LICENSE) for details.

---

## Author

Built by [Bibesh Pyakurel](https://github.com/bibeshpyakurel) as a portfolio project demonstrating full-stack enterprise application development.
