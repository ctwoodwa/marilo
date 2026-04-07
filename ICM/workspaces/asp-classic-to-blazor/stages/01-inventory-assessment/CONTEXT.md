# Inventory and Assessment

Catalog all ASP Classic pages, map dependencies, and rate complexity to scope the migration.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Source project | `{{ASP_SOURCE_PATH}}` | All .asp, .inc, Global.asa files | The pages and includes to catalog |
| Pattern reference | `../../shared/asp-to-blazor-patterns.md` | "ASP Classic Pattern" column | Recognize patterns during inventory |

## Process

1. Scan the source project for all `.asp` files and record each page with its path and approximate line count.
2. Scan for all `#include` files (`.inc`, `.asp` includes) and record each with its path.
3. For each page, extract: inline SQL statements, COM object references (`Server.CreateObject`), Session/Application usage, and Response.Redirect targets.
4. Build a dependency map: which pages include which files, which pages share database calls, which pages share COM objects.
5. Classify each page by complexity:
   - **Simple**: static display, no database, no COM (estimate: 1-2 hours)
   - **Standard**: CRUD forms, basic queries, session reads (estimate: 4-8 hours)
   - **Complex**: multi-table joins, COM objects, business logic, report generation (estimate: 8-16 hours)
   - **Critical**: authentication flows, payment processing, external integrations (estimate: 16+ hours)
6. Identify VBScript patterns needing special handling (On Error Resume Next blocks, custom string functions, date formatting, CDO email).
7. Produce the inventory artifact with totals per complexity tier and a recommended conversion order (simple pages first, critical pages last).

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Page inventory | `output/{{PROJECT_SLUG}}-page-inventory.md` | Table with columns: Page, Path, Complexity, Line Count, Dependencies, Notes |
| Dependency map | `output/{{PROJECT_SLUG}}-dependency-map.md` | Adjacency list: each page and what it includes or calls |

<!-- Target: keep this file under 80 lines. -->
