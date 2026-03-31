# Mini ERP Inventory Management System

Mini ERP Inventory Management System is a portfolio project designed to simulate the kind of internal inventory and ERP workflows used by manufacturing and operations-driven companies. The goal is to demonstrate practical business application design using a modern stack while modeling the same kinds of workflows commonly found in legacy ERP environments.

## Project Goal

Build a full-stack internal inventory management system that supports core ERP-style operations such as item management, supplier management, purchase order receiving, stock movement tracking, and low-stock reporting.

This project is intended to show:

- understanding of inventory and ERP business workflows
- ability to design and build a layered business application
- experience with API design, database modeling, and frontend integration
- readiness to work on operational software that values stability, traceability, and maintainability

## MVP Scope

The initial MVP will include:

- user authentication with role-based access
- item master management
- supplier management
- inventory balance tracking
- inventory transaction history
- purchase order creation and viewing
- goods receipt against purchase orders
- stock issue and stock adjustment flows
- low-stock dashboard and summary reporting
- OpenAPI documentation for the backend API

## Core Business Workflows

### 1. Item Master Management

- create and update inventory items
- assign SKU, category, unit, reorder level, and storage location
- activate or deactivate items

### 2. Supplier Management

- create and maintain supplier records
- associate suppliers with procurement workflows

### 3. Purchase Order Workflow

- create purchase orders for suppliers
- add purchase order line items
- track purchase order status

### 4. Goods Receipt Workflow

- receive stock against an existing purchase order
- increase inventory on hand
- update received quantities on the purchase order
- record transaction history for auditability

### 5. Inventory Issue Workflow

- issue stock for usage, sale, or internal allocation
- decrease inventory on hand
- prevent invalid negative stock movements

### 6. Stock Adjustment Workflow

- manually adjust stock for damage, loss, or corrections
- require reason tracking for every adjustment
- keep an auditable transaction history

### 7. Reporting Workflow

- view low-stock items
- review recent inventory transactions
- display stock summary metrics

## Proposed Architecture

The project will use a standard three-tier architecture:

- `React` frontend for user interaction
- `ASP.NET Core Web API` backend for business logic and API endpoints
- `PostgreSQL` database for transactional data storage

Supporting components:

- `OpenAPI / Swagger` for API documentation
- `JWT` for authentication
- role-based authorization
- structured logging
- audit trail for critical inventory actions

## Exact Tech Stack Versions

These versions are selected to keep the stack modern, stable, and interview-friendly.

### Frontend

- `Node.js 22 LTS`
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
- `Npgsql Entity Framework Core Provider 9`
- `FluentValidation 11`
- `Swashbuckle.AspNetCore 7`
- `Serilog.AspNetCore 9`

### Database and Tooling

- `PostgreSQL 16`
- `Docker Compose v2`

## Repository Structure

```text
.
├── backend/
│   ├── src/
│   └── tests/
├── docs/
│   ├── api/
│   └── architecture/
├── frontend/
└── README.md
```

Planned backend structure:

```text
backend/
├── src/
│   ├── MiniErp.Api/
│   ├── MiniErp.Application/
│   ├── MiniErp.Domain/
│   └── MiniErp.Infrastructure/
└── tests/
    ├── MiniErp.UnitTests/
    └── MiniErp.IntegrationTests/
```

## Naming Conventions

### Projects and Folders

- Use `PascalCase` for .NET project names: `MiniErp.Api`
- Use feature-oriented folders where practical
- Use `camelCase` only for frontend local variables and functions
- Use `PascalCase` for React components and TypeScript types

### C# Code

- Use `PascalCase` for classes, records, enums, DTOs, and public members
- Use `camelCase` for local variables and parameters
- Suffix request DTOs with `Request`
- Suffix response DTOs with `Response`
- Suffix services with `Service`
- Suffix repository interfaces with `Repository`

Examples:

- `CreateItemRequest`
- `ItemResponse`
- `InventoryService`
- `IPurchaseOrderRepository`

### API Conventions

- Use plural resource names in routes
- Use `/api` as the route prefix
- Use nouns for resources and HTTP verbs for actions

Examples:

- `GET /api/items`
- `POST /api/items`
- `GET /api/purchase-orders/{id}`
- `POST /api/goods-receipts`
- `POST /api/inventory/issues`
- `POST /api/inventory/adjustments`

### Database Conventions

- Use `snake_case` for table names and columns
- Use plural table names
- Use `_id` suffix for foreign keys
- Use `created_at` and `updated_at` timestamps where applicable
- Use explicit enum-like status columns as text or constrained values

Examples:

- `items`
- `purchase_orders`
- `inventory_transactions`
- `supplier_id`
- `reorder_level`

## Initial Delivery Plan

### Phase 1

- finalize project scope
- create repository structure
- document architecture and conventions

### Phase 2

- scaffold backend solution and frontend app
- configure PostgreSQL, Swagger, and authentication

### Phase 3

- implement inventory, supplier, and purchase order workflows
- add reporting and auditability

## Setup Plan

Implementation setup will follow these steps:

1. Scaffold the backend `.NET` solution and projects.
2. Initialize the React frontend with TypeScript and Vite.
3. Configure PostgreSQL and Entity Framework Core.
4. Add authentication and role-based authorization.
5. Build the MVP workflows end-to-end.
6. Add tests, Docker support, and deployment configuration.

## Why This Project Matters

This project is intentionally designed to resemble a real internal business application rather than a generic CRUD demo. It emphasizes inventory accuracy, transaction traceability, purchasing workflows, and practical reporting, which makes it a strong portfolio piece for ERP, operations, and enterprise software roles.
