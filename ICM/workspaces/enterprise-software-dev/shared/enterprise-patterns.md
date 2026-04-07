# Enterprise Patterns

## When to Use Each Pattern

Use these patterns only when there is a clear present need. Do not apply speculatively.

## Retry

- Use for transient failures on external HTTP calls and database connections.
- Max 3 retries with exponential backoff (200ms, 400ms, 800ms).
- Do not retry on 400-level client errors.
- Use Polly or .NET Resilience extensions via Aspire service defaults.

## Circuit Breaker

- Use on any external dependency that could degrade the entire service.
- Open after 5 consecutive failures, half-open after 30 seconds.
- Combine with retry: circuit breaker wraps the retry policy.

## Health Checks

- Every service must expose /health/live (is the process alive) and /health/ready (is it ready to serve).
- YARP gateway uses readiness checks to route traffic.
- Aspire dashboard displays health check status automatically.

## Outbox Pattern

- Use when a service must write to a database AND publish an event atomically.
- Write to outbox table in the same transaction, publish via background worker.
- Required for any workflow crossing two service boundaries.

## CQRS

- Use only when read and write models have meaningfully different shape or scale requirements.
- Do not use CQRS just because MediatR is available.
- Simple CRUD services use a single repository pattern.