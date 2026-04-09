# Security Architecture

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: security-architecture | docs/icm-defaults/030-architecture/security-architecture.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: TBD  
> Last updated: 2026-01-10

## Identity Provider: Entra ID

### Protocol
- **OIDC** (OpenID Connect) for user authentication
- **OAuth2** for delegated authorization
- Support for both **user identities** and **service principals**

### Token Issuance
1. Client authenticates with Entra ID
2. Entra ID issues **JWT access token** + **refresh token**
3. Client includes JWT in `Authorization: Bearer <token>` header for all API calls

---

## Authentication (AuthN)

### JWT Bearer Tokens

**Token validation occurs at:**
- **APIM (API Gateway)** - first line of defense
- **Individual services** - defense in depth

**JWT claims used:**
- `sub` - subject (user/service principal ID)
- `roles` / `groups` - authorization roles
- `aud` - audience (ensures token is for {{ PRODUCT_NAME }} APIs)
- `exp` - expiration
- `iat` - issued at
- Custom claims as needed (tenant, permissions, etc.)

### Service-to-Service Authentication

**Options:**
1. **Managed Identity** (Azure) or equivalent
2. **Service principals** with client credentials flow
3. **mTLS** (mutual TLS) for highly sensitive service communication

---

## Authorization (AuthZ)

### Role-Based Access Control (RBAC)

**Enforcement points:**
- **APIM** - coarse-grained routing and rate limiting based on roles
- **Service level** - fine-grained authorization at operation/resource level

**Role examples:**
- `Admin` - full system access
- `User` - standard user operations
- `ReadOnly` - query access only
- `ServiceAccount` - inter-service communication

### Policy-Based Authorization

For complex scenarios:
- **Attribute-Based Access Control (ABAC)** using policy engines
- **Resource-level permissions** (e.g., user can only access their own orders)

---

## AI/MCP Security

### Least Privilege Principle

**MCP (Model Control Plane):**
- Exposes curated "tools" that call {{ PRODUCT_NAME }} APIs with **restricted service principal**
- Each tool has explicit **permission grants**
- No direct database access from AI components

### Audit Trail

All AI tool invocations are:
- **Logged** with correlation IDs
- **Traced** via OTEL
- **Auditable** for compliance and security review

---

## Secrets Management

### Storage
- **Azure Key Vault** (or equivalent managed secrets service)
- **No secrets in code, config files, or environment variables** (use secret references)

### Access Pattern
- Services use **Managed Identity** to access Key Vault
- Secrets rotated regularly
- Audit logs for all secret access

---

## Network Security

### Ingress
- **APIM** is the **only public ingress point**
- All other services are internal/private

### Egress
- Services call external systems via **controlled endpoints**
- External integrations authenticated via Service Bus with dedicated credentials

### TLS
- **TLS 1.2+** for all HTTP traffic
- **mTLS** for highly sensitive internal communication (optional)

---

## Security Best Practices

1. **Defense in depth** - validate at gateway AND service level
2. **Least privilege** - grant minimum required permissions
3. **Audit everything** - all authentication/authorization events logged
4. **Secrets rotation** - automated rotation policies
5. **Threat modeling** - regular security reviews of architecture and code
