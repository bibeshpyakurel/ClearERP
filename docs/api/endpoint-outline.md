# API Endpoint Outline

This is the initial endpoint list for the ClearERP. It is intentionally simple and designed to guide implementation before coding begins.

## Authentication

- `POST /api/auth/login`
- `GET /api/auth/me`

## Users

- `GET /api/users`
- `POST /api/users`
- `GET /api/users/{id}`
- `PUT /api/users/{id}`
- `PATCH /api/users/{id}/roles`
- `PATCH /api/users/{id}/status`

## Categories

- `GET /api/categories`
- `POST /api/categories`

## Items

- `GET /api/items`
- `POST /api/items`
- `GET /api/items/{id}`
- `PUT /api/items/{id}`
- `PATCH /api/items/{id}/status`

## Suppliers

- `GET /api/suppliers`
- `POST /api/suppliers`
- `GET /api/suppliers/{id}`
- `PUT /api/suppliers/{id}`
- `PATCH /api/suppliers/{id}/status`

## Warehouses and Locations

- `GET /api/warehouses`
- `POST /api/warehouses`
- `GET /api/locations`
- `POST /api/locations`

## Purchase Orders

- `GET /api/purchase-orders`
- `POST /api/purchase-orders`
- `GET /api/purchase-orders/{id}`
- `PUT /api/purchase-orders/{id}`
- `PATCH /api/purchase-orders/{id}/status`

## Goods Receipts

- `GET /api/goods-receipts`
- `POST /api/goods-receipts`
- `GET /api/goods-receipts/{id}`

## Inventory

- `GET /api/inventory/balances`
- `GET /api/inventory/transactions`
- `POST /api/inventory/issues`
- `POST /api/inventory/adjustments`
- `GET /api/inventory/low-stock`

## Reports

- `GET /api/reports/stock-summary`
- `GET /api/reports/stock-valuation`
- `GET /api/reports/purchase-order-summary`
- `GET /api/reports/recent-transactions`

## Audit Logs

- `GET /api/audit-logs`
