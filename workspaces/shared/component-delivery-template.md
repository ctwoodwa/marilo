# Component Delivery Workspace -- Template

Use when a component graduates to CDW-warranted status (see `workspaces/shared/workspace-routing.md`).
Target time to instantiate: under 5 minutes.

## When to Use

A CDW is warranted when a component is **Complex** tier or has active multi-phase gap work requiring
coordinated spec, Example UX, and source/test alignment. Every component built through `component-builder`
gets a delivery workspace automatically via Stage 07.

## How to Instantiate

1. Create directory: `workspaces/{{component-slug}}-delivery/`
2. Copy the folder structure below
3. Fill all `{{placeholder}}` fields in every file
4. Clear stage outputs: leave only `.gitkeep` files in `stages/*/output/`

## Folder Structure

```
{{component-slug}}-delivery/
├── CLAUDE.md
├── CONTEXT.md
├── _config/
│   └── delivery-context.md
├── _status/
│   └── workspace-status.md
├── shared/
│   └── spec-coverage-format.md
└── stages/
    ├── 01-spec-review/
    │   ├── CONTEXT.md
    │   └── output/
    │       └── .gitkeep
    ├── 02-example-ux/
    │   ├── CONTEXT.md
    │   ├── shared/
    │   │   └── demo-scenario-format.md
    │   └── output/
    │       └── .gitkeep
    └── 03-sync-check/
        ├── CONTEXT.md
        ├── shared/
        │   └── delivery-checklist.md
        └── output/
            └── .gitkeep
```

## Placeholder Table

| Placeholder | Fill in | Example |
|-------------|---------|---------|
| `{{component-name}}` | PascalCase display name | `DataGrid` |
| `{{component-slug}}` | lowercase directory slug | `datagrid` |
| `{{complexity-tier}}` | from routing.md | `Complex (CDW warranted)` |
| `{{active-phase}}` | current dev phase | `Phase 1 (initial build)` |
| `{{source-subfolder}}` | subfolder under Marilo.Components/ | `Data` |
| `{{test-path}}` | subfolder under Marilo.Tests.Unit/ | `P1Core` |
| `{{category}}` | component category | `DataDisplay` |
| `{{date}}` | creation date ISO format | `2026-04-05` |

---

## CLAUDE.md Template

```markdown
<!-- Target: ~800 tokens. Trim if longer. -->
# Component Delivery Workspace -- {{component-name}}

Coordinates spec accuracy, Example UX completeness, and source+tests alignment for a single complex Blazor component.

## Folder Map

\`\`\`
{{component-slug}}-delivery/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── _config/
│   └── delivery-context.md            (component paths, state tracking, gate status)
├── _status/
│   └── workspace-status.md            (Layer 0 orientation snapshot)
├── stages/
│   ├── 01-spec-review/                (audit API spec vs. implementation)
│   │   └── output/                    (spec gap list)
│   ├── 02-example-ux/                 (audit and update demo page scenarios)
│   │   ├── shared/                    (demo scenario format)
│   │   └── output/                    (demo gap list, updated demo page)
│   └── 03-sync-check/                 (confirm all three artifacts are in sync)
│       ├── shared/                    (delivery checklist)
│       └── output/                    (delivery report)
└── shared/
    └── spec-coverage-format.md        (gap record format for spec audits)
\`\`\`

## Cold Start

Load `_status/workspace-status.md` first for pipeline orientation (Layer 0 snapshot -- not authoritative).
Then load `_config/delivery-context.md` for full component paths and gate status.

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `spec` | Enter stages/01-spec-review/CONTEXT.md |
| `demo` | Enter stages/02-example-ux/CONTEXT.md |
| `sync` | Enter stages/03-sync-check/CONTEXT.md |
| `deliver` | Run all three stages in sequence |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

\`\`\`
Pipeline Status: {{component-slug}}-delivery

  [01-spec-review]  ------>  [02-example-ux]  ------>  [03-sync-check]
      STATUS                     STATUS                    STATUS
\`\`\`

## Routing

| You want to... | Go to |
|----------------|-------|
| Audit the API spec vs. implementation | stages/01-spec-review/CONTEXT.md |
| Audit and update the Example UX | stages/02-example-ux/CONTEXT.md |
| Confirm all three artifacts are in sync | stages/03-sync-check/CONTEXT.md |
| Read delivery configuration | _config/delivery-context.md |
| Read gap-analysis workspace for this component | /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis/CLAUDE.md |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Spec review | _config/delivery-context.md, stages/01-spec-review/CONTEXT.md | Stages 02-03 files |
| Example UX work | _config/delivery-context.md, stages/02-example-ux/CONTEXT.md | Stages 01, 03 files |
| Sync check | All three stage outputs, stages/03-sync-check/shared/delivery-checklist.md | Reference files |
| Any task | _config/delivery-context.md always | Other component CDWs |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there.

<!-- CDW owns: spec accuracy, Example UX completeness.
     CDW delegates: source changes and test writing to
     /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis.
     Never modify component source files directly from this workspace. -->
```

---

## CONTEXT.md Template

```markdown
# Component Delivery Workspace -- {{component-name}}

Coordinates spec, Example UX, and source+tests for {{component-name}}.

## Task Routing

| Task Type | Entry Point | Trigger Keyword |
|-----------|-------------|-----------------|
| Spec audit | stages/01-spec-review/CONTEXT.md | spec |
| Example UX audit | stages/02-example-ux/CONTEXT.md | demo |
| Sync check | stages/03-sync-check/CONTEXT.md | sync |
| Full delivery | stages/01-spec-review/CONTEXT.md | deliver |

## Shared Resources

| Resource | Location | Section to Load |
|----------|----------|-----------------|
| Delivery configuration | _config/delivery-context.md | Full file |
| Spec coverage format | shared/spec-coverage-format.md | Full file |
| Demo scenario format | stages/02-example-ux/shared/demo-scenario-format.md | Full file |
| Delivery checklist | stages/03-sync-check/shared/delivery-checklist.md | Full file |
| Gap workspace | /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis/CLAUDE.md | Routing table only |
```

---

## delivery-context.md Template

```markdown
# Delivery Context -- {{component-name}}

## Component Identity

| Field | Value |
|-------|-------|
| Component name | {{component-name}} |
| Component slug | {{component-slug}} |
| Complexity tier | {{complexity-tier}} |
| Active phase | {{active-phase}} |

## Artifact Paths

| Field | Value |
|-------|-------|
| API spec | /workspaces/Marilo/docs/component-specs/{{component-slug}}/ |
| Example UX | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/{{component-name}}/ |
| Component source | /workspaces/Marilo/src/Marilo.Components/{{source-subfolder}}/ |
| Test files | /workspaces/Marilo/tests/Marilo.Tests.Unit/{{test-path}}/{{component-name}}Tests.cs |
| Gap workspace | /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis/ |

## Spec State

| Field | Value |
|-------|-------|
| Spec version | unversioned |
| Last spec audit | not yet run |
| Open spec gaps | not yet run |

## Example UX State

| Field | Value |
|-------|-------|
| Demo page(s) | {{component-name}}/Overview.razor |
| Last demo audit | not yet run |
| Open demo gaps | not yet run |

## Delivery Gate

| Field | Value |
|-------|-------|
| Last sync check | not yet run |
| Gate status | PENDING |
| Blocking items | not yet run |

## Gap Workspace Link

| Field | Value |
|-------|-------|
| Latest closure reports | /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis/stages/06-validate/output/ |
| Coverage summary | /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis/_config/coverage-summary.md |
```

---

## workspace-status.md Template

```markdown
# Workspace Status

<!-- SUMMARY SNAPSHOT -- read at session start for fast orientation only.
     NOT authoritative. If this file contradicts a stage output, the stage output wins.
     Update this file after completing a stage or a significant batch of work.
     Do not append -- replace the content and update the date.
     Keep this file under 50 lines. -->

## Header

| Field | Value |
|-------|-------|
| Workspace | {{component-slug}}-delivery |
| Last updated | {{date}} |
| Current phase | Pre-run (no stages executed yet) |

## Pipeline Status

\`\`\`
Stage 01 -- [ ] spec-review
Stage 02 -- [ ] example-ux
Stage 03 -- [ ] sync-check
\`\`\`

Key outputs so far:

- None. Workspace scaffolded but no stages run.

## Next Actions

1. Run Stage 01 (spec-review) to audit API spec vs. current implementation.
2. Run Stage 02 (example-ux) to audit and update demo page scenarios.

## Upstream Dependencies

- Component built via component-builder.
- Gap-analysis workspace: {{component-slug}}-gap-analysis (scaffolded, pre-run).
```

---

## Stage 01: spec-review/CONTEXT.md Template

```markdown
# Spec Review

Audit the API spec against the component source to identify undocumented, spec-ahead, and mismatched parameters.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Paths to spec and source |
| API spec | /workspaces/Marilo/docs/component-specs/{{component-slug}}/ | Full directory | What is documented |
| Component source | /workspaces/Marilo/src/Marilo.Components/{{source-subfolder}}/ | Parameter and event declarations only | What is implemented |
| Spec coverage format | shared/spec-coverage-format.md | Full file | Gap record format |
| Gap workspace closure reports | /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis/stages/06-validate/output/ | List + read relevant reports | Current resolution state |

## Process

1. List all parameters in the component source (public API surface only).
2. List all parameters documented in the spec.
3. Produce three lists:
   a. Implemented but not documented (undocumented parameters).
   b. Documented but not implemented (spec ahead of code).
   c. Documented and implemented but mismatched (type, name, or behaviour description does not match the source).
4. For each item in list (a) and (c): create a spec gap record using shared/spec-coverage-format.md.
5. For each item in list (b): note whether it is a known planned gap (check gap-analysis closure reports) or an unknown gap.
6. Produce a priority-ordered spec gap list.
7. Run the Audit checklist before writing to output/.
8. Write output/{{component-slug}}-spec-gap-list.md.
9. Update _config/delivery-context.md: last spec audit date and open spec gap count.

## Audit

| Check | Pass Condition |
|-------|----------------|
| All source parameters inventoried | Count matches source file scan |
| All spec parameters inventoried | Count matches spec file scan |
| No gap record missing a type classification (a/b/c) | Every gap has a type |
| Priority order justified | Each priority rank has a brief rationale |
| No spec content duplicated in this output | Output references spec, not copies it |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Spec gap list | output/{{component-slug}}-spec-gap-list.md | spec-coverage-format.md |
```

---

## Stage 02: example-ux/CONTEXT.md Template

```markdown
# Example UX

Audit the demo page for completeness against the API spec, then create or update scenarios to fill gaps.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Demo page paths |
| Stage 01 output | stages/01-spec-review/output/{{component-slug}}-spec-gap-list.md | Full file | Which parameters need demo coverage |
| API spec | /workspaces/Marilo/docs/component-specs/{{component-slug}}/ | Full directory | Parameter definitions and use cases |
| Demo scenario format | shared/demo-scenario-format.md | Full file | What a complete scenario looks like |
| Existing demo page(s) | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/{{component-name}}/ | Full file | Current state |
| Component source | /workspaces/Marilo/src/Marilo.Components/{{source-subfolder}}/ | Parameter and event declarations | Accurate API surface |

## Process

1. Read the existing demo page(s) and inventory all current scenarios.
2. Read the API spec. For every parameter and event, check demo coverage.
3. Produce a demo gap list:
   a. Parameters with no demo scenario.
   b. Parameters with a scenario but a stale code snippet.
   c. Events with no demo scenario.
   d. Edge cases (disabled, readonly, empty, error states) not demonstrated.
4. [CHECKPOINT]
5. For each item in the approved demo gap list:
   a. Write a new Blazor scenario section following demo-scenario-format.md.
   b. Scenario must use the actual Marilo component (not pseudocode).
   c. Include: title, live interactive example, code snippet panel, parameter table, link to spec section.
6. Update stale code snippets for scenarios already in the demo page.
7. Run the Audit checklist before writing to output/.
8. Write output/{{component-slug}}-demo-gap-list.md.
9. Write the updated demo page file(s).
10. Update _config/delivery-context.md: last demo audit date and open demo gap count.

## Checkpoint

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 3 | Demo gap list grouped by type (a/b/c/d) with counts | Approve gap list, deprioritize or defer any items |

## Audit

| Check | Pass Condition |
|-------|----------------|
| Every API parameter has at least one scenario | Count matches |
| Every API event has at least one scenario | Count matches |
| All code snippets use current parameter names and types | No deprecated references |
| Every new scenario is interactive | Each has user-controllable input |
| Edge cases demonstrated | Disabled, readonly, empty, error states covered |
| No Telerik component references in demo page | Zero Telerik imports |
| Scenario titles match use-case language | No "Test 1" or "Example A" |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Demo gap list | output/{{component-slug}}-demo-gap-list.md | demo-scenario-format.md |
| Updated demo page | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/{{component-name}}/ | Blazor .razor file(s) |
```

---

## Stage 03: sync-check/CONTEXT.md Template

```markdown
# Sync Check

Confirm all three artifacts (spec, Example UX, source+tests) are in sync and evaluate the delivery gate.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | All artifact paths |
| Stage 01 output | stages/01-spec-review/output/ | Full file | Spec gap list |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo gap list |
| Gap workspace coverage summary | /workspaces/Marilo/workspaces/{{component-slug}}-gap-analysis/_config/coverage-summary.md | Full file | Test and closure state |
| Delivery checklist | shared/delivery-checklist.md | Full file | Gate criteria |

## Process

1. Read all three stage outputs and the coverage summary.
2. For each item in the delivery checklist, evaluate pass/fail.
3. Assign overall gate status:
   - CLEAR: all checklist items pass; component is in sync.
   - AMBER: minor gaps remain; documented with follow-up tasks.
   - BLOCKED: one or more blocking items prevent delivery gate passing.
4. Write output/{{component-slug}}-delivery-report.md.
5. Update _config/delivery-context.md: last sync check date, gate status, and blocking item count.

## Audit

| Check | Pass Condition |
|-------|----------------|
| All checklist items evaluated | No item left as "unknown" |
| Every BLOCKED item has a follow-up task | No open BLOCKED items without remediation path |
| Gate status matches checklist results | CLEAR only if zero failures; AMBER if all failures non-blocking |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Delivery report | output/{{component-slug}}-delivery-report.md | delivery-checklist.md |
```

---

## spec-coverage-format.md Template

```markdown
# Spec Coverage Format

Each spec gap record follows this shape.

## Gap Record

**ID:** SPEC-{{component-slug}}-[sequence]
**Type:** undocumented | spec-ahead | mismatch
**Parameter/Event:** [exact name from source or spec]
**Priority:** P1 (blocking) | P2 (this phase) | P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | [spec name or "missing"] | [source name or "missing"] |
| Type | [spec type or "missing"] | [source type or "missing"] |
| Default | [spec default or "missing"] | [source default or "N/A"] |
| Description | [one line or "missing"] | [inferred from code] |

**Recommended action:** [update spec | implement parameter | rename to match]
**Delegated to:** [gap-analysis intake | spec update only]
```

---

## demo-scenario-format.md Template

```markdown
# Demo Scenario Format

Each demo section in the Blazor demo page must contain:

1. A scenario title that describes a real use case (not "Test" or "Example 1").
2. A brief description (1-2 sentences) explaining when a developer would use this configuration.
3. A live interactive Blazor component using actual Marilo API.
4. At least one user-controllable input (toggle, slider, input, etc.) that changes the component's behaviour in real time.
5. A code snippet panel showing the minimal Razor markup for this scenario (collapsible preferred; matches current API exactly).
6. A parameter table listing which parameters are active in this scenario:
   | Parameter | Value in this scenario | Notes |
7. A link or anchor reference to the corresponding spec section.

A demo page section is COMPLETE when:
- Every parameter in the spec has at least one scenario where it is the primary focus.
- Every event has at least one scenario that triggers it visibly.
- Disabled state is demonstrated.
- Readonly state is demonstrated (if the component supports it).
- Empty/no-data state is demonstrated.
- Error state is demonstrated (if the component supports it).

A code snippet is STALE when:
- It references a parameter name that no longer exists in the source.
- It uses a type that has changed (e.g., string where enum is now required).
- It is missing a required parameter added after the snippet was written.
```

---

## delivery-checklist.md Template

```markdown
# Delivery Checklist

## API Spec
- [ ] All implemented parameters documented in spec
- [ ] All documented parameters implemented in source
- [ ] Parameter types match between spec and source
- [ ] Parameter defaults match between spec and source
- [ ] All events documented and implemented
- [ ] Spec version reflects current implementation phase

## Example UX
- [ ] Every spec parameter has at least one demo scenario
- [ ] Every spec event has at least one demo scenario
- [ ] Disabled state demonstrated
- [ ] Readonly state demonstrated (if supported)
- [ ] Empty/no-data state demonstrated
- [ ] Error state demonstrated (if supported)
- [ ] All code snippets use current parameter names and types
- [ ] No Telerik component references in demo pages

## Source and Tests
- [ ] All spec parameters covered by bUnit tests
- [ ] No undocumented parameters in component source
- [ ] Stage 06 closure reports exist for all active gap phases
- [ ] Pre-existing test failures documented in regression triage log
- [ ] All active gap phases show Tests Passing = YES in coverage summary

## Alignment
- [ ] Spec version consistent with gap workspace active phase
- [ ] Demo page parameter names match current source parameter names
- [ ] No parameter renamed without spec and demo page update
- [ ] delivery-context.md reflects current state of all three artifacts
```
