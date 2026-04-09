# Stage 03: Domain Modeling

Design entity models, service interfaces, and data contracts for new feature areas.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../02-ia-and-shell/output/ia-plan.md` | Full file | Know what pages need what data |
| Reference | `../../shared/implementation-guardrails.md` | Full file | Architecture rules (DTO vs VM, DAB conventions) |
| Config | `../../_config/domain-expansion.md` | "Likely entity shape" sections | Domain entity sketches |

## Process

1. Read the IA plan — extract the list of pages and what data each needs.
2. Read domain expansion entity sketches — use as starting points, not final shapes.
3. For each new domain area (Assets, Inspections, Deficiencies, Conditions, Remodeling):
   a. Define the EF Core entity (C# record or class).
   b. Define the service interface (`IAssetService`, `IInspectionService`, etc.).
   c. Define the DTO (transport contract) and VM (page-facing view model).
   d. Note DAB entity config needed.
4. Design the dynamic form schema model:
   a. `FormSchema` record — fields, types, validation rules, conditional logic.
   b. `FormFieldDefinition` — per-field config.
   c. `IFormSchemaService` — load/save schemas.
5. Map entity relationships: Asset → Inspections → Deficiencies → Risks. Document FK chains.
6. **[Checkpoint]** — Present entity models and service interfaces. Get human approval.
7. Run audit. Write models to output.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 6 | Entity models, service interfaces, relationship diagram | Whether the model shape and granularity are correct |

## Audit

| Check | Pass Condition |
|-------|---------------|
| DTO/VM separation | No page binds directly to an EF entity or DTO |
| Service per domain | Each domain area has its own `I*Service` interface |
| DAB compatibility | Entities use simple types compatible with DAB GraphQL mapping |
| No orphan entities | Every entity is reachable from at least one service method |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Domain models | `output/domain-models.md` | C# entity records, service interfaces, DTO/VM pairs, relationship diagram |
| Dynamic form schema | `output/dynamic-form-schema.md` | FormSchema model, field types, rendering strategy |
