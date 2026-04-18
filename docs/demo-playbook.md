# Demo Playbook

This project is meant to be easy to evaluate without losing the feel of a real ERP workflow.

## What To Show First

Start with the admin account:

- email: `admin@clearerp.local`
- password: `Admin123!`

That account exposes the full workflow surface, including audit logs.

## Recommended 5-Minute Walkthrough

1. Open the dashboard and explain the KPI cards.
2. Visit items and suppliers to show master data.
3. Open purchase orders and inspect different statuses.
4. Receive a purchase order to demonstrate stock movement.
5. Open inventory to confirm balance and transaction updates.
6. Open reports and audit logs to show business visibility and traceability.

## Recommended 10-Minute Walkthrough

1. Sign in as admin.
2. Show the dashboard summary.
3. Create or inspect a purchase order.
4. Post a goods receipt.
5. Show that inventory balances and transactions change.
6. Perform a stock issue or adjustment.
7. Show low-stock and stock valuation reports.
8. Finish in audit logs to reinforce accountability and event history.

## Why The App Stays Lightweight

The system intentionally avoids heavy enterprise concerns for demo environments, such as:

- tenant provisioning
- complicated role administration flows
- advanced onboarding sequences
- multi-stage infrastructure dependencies

Instead, the project emphasizes:

- realistic business entities
- connected ERP workflows
- seeded demo data
- clear navigation
- fast local startup

## Deployment Guidance

For a simple public or portfolio deployment:

- keep one frontend deployment and one backend deployment
- use PostgreSQL with the seeded migration state
- surface the demo credentials clearly
- keep demo data resettable so the app remains predictable

## Evaluator Notes

The easiest way to understand the value of the project is to treat it like an internal operations console rather than a generic CRUD app.
The most persuasive parts are the flow between purchasing, receipts, inventory movement, reporting, and auditability.