# PM Demo — Domain Expansion Scope

New feature areas entering the PM demo beyond the current project-management baseline.

## Asset Management

**What it is:** The PM demo expands from pure project management into facility/infrastructure asset management. Assets are physical or logical items (buildings, floors, rooms, equipment, systems) that projects, inspections, and work orders operate against.

**Design questions to resolve:**
- Entity hierarchy: Asset → Sub-asset → Component? Or flat with type/category?
- Navigation: top-level sidebar group ("Assets") or nested under existing groups?
- Detail views: dedicated asset detail page or slide-over from a list?
- Relationship to projects: one-to-many (asset has many projects) or many-to-many?
- Asset register list: filterable grid or card-based browser?
- Likely Marilo components: MariloDataGrid, MariloDrawer, MariloTreeView, MariloCard

**Likely pages:**
- `/assets` — asset register (grid with filters)
- `/assets/{id}` — asset detail (tabs: overview, history, inspections, conditions, documents)
- `/assets/map` — floor-plan or site-map view (stretch)

---

## Dynamic Forms (Cross-Cutting Capability)

**What it is:** Schema-driven form rendering that allows different inspection types, deficiency reports, condition assessments, and potentially settings/configuration UIs to use different field sets without hard-coding a page per form type. This is **cross-cutting infrastructure**, not a standalone feature or a single page — it is consumed by every asset-related workflow and may eventually replace hardcoded settings forms too.

**Design questions to resolve:**
- Schema format: JSON schema, custom config records, or database-driven?
- Where schemas live: seeded in DB? Config files? Admin-created at runtime?
- Rendering: single `DynamicForm.razor` component that interprets a schema, or code-gen per form type?
- Validation: schema-defined rules (required, min/max, regex) mapped to DataAnnotations or FluentValidation?
- Field types supported: text, number, date, select, multi-select, checkbox, photo upload, signature, GPS coordinates, rating scale?
- Conditional logic: show/hide fields based on other field values?
- Offline/progressive: any offline-first requirement for field inspections?

**Likely components:**
- `DynamicForm.razor` — interprets a form schema and renders Marilo inputs
- `DynamicFormField.razor` — renders one field based on its schema definition
- `FormSchemaEditor.razor` — admin tool for building/editing form schemas (stretch)
- Service: `IFormSchemaService` — load schemas by type/category

**Relationship to other features:** Inspections, deficiencies, condition assessments, and future admin-configurable workflows all consume dynamic forms. The form infrastructure must exist before those features can be built with any flexibility. Dynamic forms should be designed as a reusable capability in the Marilo component system, not scoped narrowly to one domain.

**Cross-cutting design implications:**
- The `DynamicForm` component should be domain-agnostic — it interprets a schema and renders Marilo inputs. It knows nothing about inspections, deficiencies, or assets.
- Schema storage, versioning, and admin editing are separate concerns from form rendering.
- Validation rules defined in schemas must map cleanly to Blazor's `EditContext` / `DataAnnotations` pipeline.
- Dynamic forms are a reusable building block that multiple pages consume — treat as infrastructure, not a feature.

---

## Asset Inspections

**What it is:** Scheduled or ad-hoc inspections of assets. An inspection uses a dynamic form template, is assigned to an inspector, has a due date, produces findings (deficiencies), and generates a report.

**Likely entity shape:**
- Inspection: id, asset_id, form_schema_id, inspector_id, scheduled_date, completed_date, status, form_data (JSON), notes
- InspectionTemplate: id, name, form_schema_id, frequency, asset_type_filter

**Likely pages:**
- `/inspections` — inspection schedule/list
- `/inspections/{id}` — inspection detail with embedded dynamic form
- `/inspections/new` — create inspection (pick asset, pick template, assign)

---

## Deficiencies

**What it is:** Problems found during inspections or reported independently. A deficiency is linked to an asset, optionally to an inspection finding, has a severity, status, and resolution workflow.

**Likely entity shape:**
- Deficiency: id, asset_id, inspection_id (nullable), title, description, severity, status, reported_by, reported_date, resolved_date, resolution_notes, form_data (JSON for custom fields)

**Relationship to other features:**
- Created from inspections (auto-generated from findings) or standalone
- May trigger risk register entries if severity is high
- Resolution may generate a remodeling project

**Likely pages:**
- `/deficiencies` — deficiency register (grid)
- `/deficiencies/{id}` — deficiency detail with timeline/activity

---

## Equipment Conditions

**What it is:** Ongoing condition tracking for equipment assets. Different from inspections (which are point-in-time events) — conditions are a running assessment log that shows trend over time.

**Likely entity shape:**
- ConditionAssessment: id, asset_id, assessor_id, date, condition_rating (1-5 or A-F), form_data (JSON), notes, photos

**Likely pages:**
- Embedded in asset detail view as a "Conditions" tab
- `/conditions/new?assetId=...` — new assessment entry (dynamic form)

---

## Risks (expanded from existing)

**What it is:** The PM demo already has a Risk Register page. This expansion connects risks to assets, inspections, and deficiencies — not just projects.

**Design questions:**
- Should risks become a cross-cutting entity that can be linked to projects, assets, or deficiencies?
- Risk → deficiency linkage: automatic escalation when a deficiency severity exceeds threshold?
- Risk heat map visualization tied to assets?

---

## Remodeling Projects

**What it is:** Capital improvement or renovation projects triggered by deficiency resolution, condition degradation, or proactive planning. These are projects (like existing PM projects) but specifically scoped to physical assets.

**Likely entity shape:**
- Extends or specializes the existing project model with asset linkage, budget allocation, contractor assignment, permit tracking

**Likely pages:**
- `/projects/remodeling` — filtered project list (remodeling type)
- `/projects/remodeling/{id}` — remodeling project detail (timeline, budget, deficiency linkage)

---

## Feature Dependency Graph

```
Dynamic Forms (infrastructure — build first)
    │
    ├── Asset Inspections (consumes dynamic forms)
    │       │
    │       ├── Deficiencies (created from inspections or standalone)
    │       │       │
    │       │       ├── Risks (expanded — linked to deficiencies)
    │       │       └── Remodeling Projects (triggered by deficiency resolution)
    │       │
    │       └── Equipment Conditions (ongoing assessment, consumes dynamic forms)
    │
    └── Asset Management (entity foundation — can be built in parallel with dynamic forms)
```

## Shared Patterns Across Expansion Features (Reusable Domain System)

Asset-related workflows (inspections, deficiencies, conditions, remodeling) should be designed as a **reusable domain system**, not isolated screens. Common infrastructure:

1. **Entity detail shell**: All entities need a detail page with tabs (overview, activity, documents, related items). Build a reusable `EntityDetailShell` component that accepts tab definitions and renders consistently.
2. **Register/list views**: All entities need a filterable grid. Build a shared `EntityRegister` pattern using `MariloDataGrid` with per-entity column configs passed as parameters.
3. **Activity timeline**: Inspections, deficiencies, and conditions all have chronological activity. Build a reusable `ActivityTimeline` component consumed by any entity detail view.
4. **Photo/document attachment**: Inspections and deficiencies need photo/document upload. Build a reusable `AttachmentPanel` component.
5. **Status workflow**: Deficiencies and inspections have status transitions. Build a shared `StatusBadge` component + `IStatusTransitionService` that can be parameterized per entity type.
6. **Dynamic form embedding**: Any entity detail view that collects structured data (inspection findings, condition assessments, deficiency reports) embeds the cross-cutting `DynamicForm` component with a schema reference.
7. **Entity linking**: Assets, inspections, deficiencies, risks, and remodeling projects link to each other. Build a reusable `RelatedEntities` component that shows linked items from any entity detail view.

**Design principle:** Each new domain page should be a thin shell that composes reusable components (EntityDetailShell + tabs of ActivityTimeline, AttachmentPanel, RelatedEntities, DynamicForm). The domain-specific logic lives in services, not in page markup.
