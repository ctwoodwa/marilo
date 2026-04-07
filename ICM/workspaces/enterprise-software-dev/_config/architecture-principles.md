# Architecture Principles

Filled during workspace setup. Run the setup trigger to populate.

## Preferred Architecture Style

{{ARCHITECTURE_STYLE}} (e.g. layered, ports-and-adapters, CQRS)

## Core Principles

- Separation of concerns: each project has one clear responsibility.
- Dependency rule: outer layers depend on inner layers, never the reverse.
- Async-first: all I/O operations use async/await.
- Fail fast: validate inputs at boundaries, throw early, log with context.
- Configure over code: environment behavior driven by config, not conditionals.

## What to Avoid

- Over-engineering: no pattern unless there is a clear present need.
- Shared mutable state across service boundaries.
- Direct database access from the UI or gateway layer.