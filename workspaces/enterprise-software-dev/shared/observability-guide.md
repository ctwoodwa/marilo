# Observability Guide

## Rule

All services MUST emit OpenTelemetry traces, metrics, and structured logs to the configured backend.
The same OTEL config is used in all environments - only the exporter endpoint changes.

## Required Instrumentation Per Service

| Service | Traces | Metrics | Logs |
|---|---|---|---|
| Blazor frontend | HTTP outbound, page load | Request count, load time | Structured via ILogger |
| YARP gateway | All proxied requests | Request rate, latency, errors per route | Structured via ILogger |
| DAB API | HTTP inbound, DB queries | Request count, query duration, error rate | Structured via ILogger |
| Any background service | Span per job | Job duration, failure count | Structured via ILogger |

## Environment Exporters

| Environment | Exporter | Endpoint |
|---|---|---|
| dev | Aspire Dashboard (OTLP) | http://localhost:18888 |
| test | {{OBSERVABILITY_BACKEND}} | {{OBSERVABILITY_ENDPOINT_TEST}} |
| prod | {{OBSERVABILITY_BACKEND}} | {{OBSERVABILITY_ENDPOINT_PROD}} |

## Required Resource Attributes

All services must set: service.name, service.version, deployment.environment.

## Correlation

All services must propagate W3C TraceContext headers. YARP must forward them to upstream services.
All log entries must include TraceId and SpanId.