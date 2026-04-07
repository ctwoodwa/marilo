# Endpoint Map

Filled during Stage 01. Records the classification decision for every endpoint in the API.

## Classification Key

| Decision | Meaning |
|---|---|
| `dab` | DAB auto-generates this endpoint from the schema — remove from the API service |
| `ad-hoc` | Kept in the API service — requires business logic, solver execution, or multi-step operations |
| `dab+ad-hoc` | DAB handles the data contract; the API adds a wrapper for pre/post-processing |
| `remove` | Endpoint is deprecated or duplicated — do not migrate |

## Endpoint Classification Table

Filled during Stage 01. One row per endpoint.

| Endpoint | Method | Current Location | Decision | Rationale |
|---|---|---|---|---|
| _fill during Stage 01_ | | | | |

## DAB Entity Summary

Auto-populated from the `dab` rows above after Stage 01. Lists the entities DAB will expose.

| Entity Name | Source Table | REST Enabled | GraphQL Enabled | Notes |
|---|---|---|---|---|
| _fill during Stage 01_ | | | | |

## Ad-hoc Endpoint Summary

Auto-populated from the `ad-hoc` rows above after Stage 01.

| Endpoint | Method | Business Logic Summary | EF Core / Dapper |
|---|---|---|---|
| _fill during Stage 01_ | | | |
