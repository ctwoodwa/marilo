# AI Architecture (OpenAI + MCP)

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: ai-architecture | docs/icm-defaults/030-architecture/ai-architecture.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: TBD  
> Last updated: 2026-01-10

## AI Integration Layer

### Components

- **OpenAI**: Large Language Model (LLM) provider
- **MCP (Model Control Plane)**: Mediation/control plane between LLM and internal tools/data

### Architecture

```
┌─────────────────────────────────────┐
│  Client (user interacts with AI)   │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│         APIM (API Gateway)          │
│  (routes AI requests if exposed)    │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│      MCP (Model Control Plane)      │
│  - Tool registry                    │
│  - Access policies                  │
│  - Prompt constraints               │
│  - Audit/observability              │
└─────────────────────────────────────┘
       ↓                    ↓
┌────────────┐    ┌──────────────────┐
│  OpenAI    │    │  {{ PRODUCT_NAME }} APIs     │
│  (LLM)     │    │  (via tools)     │
└────────────┘    └──────────────────┘
```

---

## MCP (Model Control Plane) Responsibilities

### 1. Tool Registry

MCP exposes curated "tools" that the LLM can invoke:

**Tool examples:**
- `GetCustomerOrders(customerId)` - retrieve customer orders
- `UpdateOrderStatus(orderId, newStatus)` - update order status
- `SearchCatalog(query)` - search product catalog
- `CreateBasket(items)` - create shopping basket

**Key principle:** Tools call **{{ PRODUCT_NAME }} APIs** (not direct database access).

### 2. Access Control (Least Privilege)

Each tool has **explicit permission grants**:

- **Service principal** used by MCP has minimal permissions
- **Tool-level authorization** - each tool authorized independently
- **User context propagation** - if AI acts on behalf of a user, user's permissions apply

**Security boundary:**
- MCP runs with a **restricted service principal**
- LLM cannot directly access {{ PRODUCT_NAME }} infrastructure
- All actions are mediated through controlled tools

### 3. Prompt Constraints and Output Filtering

**Input constraints:**
- Prompt injection detection
- Input validation (e.g., max length, allowed characters)

**Output constraints:**
- PII filtering - prevent accidental disclosure of sensitive data
- Output validation - ensure responses meet safety/compliance requirements

### 4. Audit and Observability

All AI interactions are fully observable:

| Signal | Captured Data |
|--------|---------------|
| **Traces** | Full request flow (client → MCP → OpenAI → tool → API) |
| **Logs** | Prompts, tool invocations, responses, errors |
| **Metrics** | Latency, token usage, error rates, tool invocation counts |
| **Audit** | User, timestamp, tool used, data accessed |

**Correlation IDs** link AI requests through entire system for end-to-end tracing.

---

## OpenAI Integration

### API Usage

**MCP calls OpenAI APIs:**
- **Completion API** - for conversational AI
- **Embeddings API** - for semantic search, RAG (Retrieval-Augmented Generation)
- **Fine-tuning** - domain-specific model customization (optional)

### Token Management

- **Rate limiting** - prevent runaway token consumption
- **Cost tracking** - monitor and budget token usage
- **Caching** - cache embeddings and frequent queries

---

## Data Access Patterns

### Preferred: API/Tool-Based Access

**Why:**
- **Authorization** - RBAC enforced at API level
- **Auditability** - all access logged via API telemetry
- **Consistency** - business logic enforced (no raw DB manipulation)
- **Versioning** - API contracts provide stability

### Anti-Pattern: Direct Database Access

**Avoid:**
- AI/MCP directly querying operational databases
- Bypasses business logic and authorization
- Creates tight coupling to data schema

**Exception:** Read-only access to **Data Lake Gold layer** for analytics use cases (with appropriate access controls).

---

## AI Use Cases in {{ PRODUCT_NAME }}

### 1. Conversational Commerce

**Scenario:** Customer asks "What's the status of my order?"

**Flow:**
1. Client sends query to MCP (via APIM)
2. MCP calls OpenAI with prompt + available tools
3. OpenAI determines tool needed: `GetCustomerOrders(customerId)`
4. MCP invokes tool → calls **Ordering API**
5. API returns order data
6. MCP provides data to OpenAI
7. OpenAI generates natural language response
8. Response returned to client

### 2. Intelligent Search

**Scenario:** Semantic product search

**Flow:**
1. Client query: "comfortable running shoes for marathon"
2. MCP generates **embedding** via OpenAI
3. MCP calls `SearchCatalog` tool with embedding
4. Catalog API performs vector search
5. Results returned and formatted by LLM

### 3. Content Generation

**Scenario:** Generate product descriptions

**Flow:**
1. MCP receives product metadata
2. OpenAI generates description based on template + metadata
3. Output validated and filtered by MCP
4. Published via Catalog API

---

## Privacy and Compliance

### Data Sent to OpenAI

**Principles:**
- **Minimize PII** - only send data necessary for request
- **Anonymize** - use customer IDs instead of names/emails where possible
- **Compliance** - ensure OpenAI usage complies with data residency/GDPR requirements

### Data Retention

- **OpenAI retention policies** - understand and document what OpenAI retains
- **Audit logs** - retain full audit trail of all AI interactions in {{ PRODUCT_NAME }}

---

## Future Enhancements

### Retrieval-Augmented Generation (RAG)

**Pattern:**
1. User query triggers semantic search against **Data Lake** or **vector database**
2. Relevant documents retrieved and provided as context to LLM
3. LLM generates response grounded in retrieved data

**Benefits:**
- Reduces hallucination
- Grounds responses in {{ PRODUCT_NAME }} data
- Enables domain-specific knowledge without fine-tuning

### Fine-Tuning

- Train custom models on {{ PRODUCT_NAME }}-specific data (product catalogs, customer interactions)
- Improves accuracy and reduces token costs for common queries
