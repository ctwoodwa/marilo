# Component Gap-Analysis Workspace -- Template

Use when a new component needs structured gap tracking and resolution.
Target time to instantiate: under 5 minutes.

## When to Use

Every component built through `component-builder` gets a gap-analysis workspace. It provides structured intake, prioritization, resolution design, remediation planning, implementation, and validation for gaps discovered during delivery or ongoing development.

## How to Instantiate

1. Create a new directory: `workspaces/{{component-slug}}-gap-analysis/`
2. Copy the folder structure below
3. Fill `_config/gap-context.md` with component-specific values
4. Fill `_config/coverage-summary.md` with initial empty state
5. Clear any template comments

## Folder Structure

```
{{component-slug}}-gap-analysis/
├── CLAUDE.md
├── CONTEXT.md
├── _config/
│   ├── gap-context.md
│   └── coverage-summary.md
├── _status/
│   └── workspace-status.md
├── setup/
│   └── questionnaire.md
├── stages/
│   ├── 01-intake/
│   │   ├── CONTEXT.md
│   │   └── output/
│   │       └── .gitkeep
│   ├── 02-prioritize/
│   │   ├── CONTEXT.md
│   │   └── output/
│   │       └── .gitkeep
│   ├── 03-resolution-design/
│   │   ├── CONTEXT.md
│   │   └── output/
│   │       └── .gitkeep
│   ├── 04-remediation-plan/
│   │   ├── CONTEXT.md
│   │   └── output/
│   │       └── .gitkeep
│   ├── 05-implement/
│   │   ├── CONTEXT.md
│   │   └── output/
│   │       └── .gitkeep
│   └── 06-validate/
│       ├── CONTEXT.md
│       └── output/
│           └── .gitkeep
└── shared/
    ├── gap-record-format.md
    ├── priority-framework.md
    ├── resolution-record-format.md
    ├── validation-checklist.md
    └── test-coverage-ownership.md
```

## Placeholder Table

| Placeholder | Fill in | Example |
|-------------|---------|---------|
| `{{component-name}}` | PascalCase display name | `DataGrid` |
| `{{component-slug}}` | lowercase directory slug | `datagrid` |
| `{{source-subfolder}}` | subfolder under Marilo.Components/ | `Data` |
| `{{test-path}}` | subfolder under Marilo.Tests.Unit/ | `P1Core` |
| `{{active-phase}}` | current dev phase | `Phase 1 (initial build)` |
| `{{category}}` | component category | `DataDisplay` |

---

## CLAUDE.md Template

```markdown
<!-- Target: ~800 tokens. Trim if longer. -->
# Component Gap-Analysis Workspace -- {{component-name}}

Structured gap tracking and resolution for Marilo{{component-name}}.

## Folder Map

\`\`\`
{{component-slug}}-gap-analysis/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── _config/
│   ├── gap-context.md                 (component paths, resolution tracking)
│   └── coverage-summary.md            (test coverage rollup)
├── _status/
│   └── workspace-status.md            (Layer 0 orientation snapshot)
├── setup/
│   └── questionnaire.md               (onboarding)
├── stages/
│   ├── 01-intake/                     (import or assess gaps)
│   │   └── output/
│   ├── 02-prioritize/                 (score and sequence gaps)
│   │   └── output/
│   ├── 03-resolution-design/          (design fix for each gap)
│   │   └── output/
│   ├── 04-remediation-plan/           (break into atomic tasks)
│   │   └── output/
│   ├── 05-implement/                  (execute changes)
│   │   └── output/
│   └── 06-validate/                   (verify and close gaps)
│       └── output/
└── shared/
    ├── gap-record-format.md
    ├── priority-framework.md
    ├── resolution-record-format.md
    ├── validation-checklist.md
    └── test-coverage-ownership.md
\`\`\`

## Entry Paths

| Starting from... | Begin at |
|-------------------|----------|
| Existing gap list (from delivery spec review) | stages/01-intake/CONTEXT.md (import mode) |
| Fresh component (no prior gap work) | stages/01-intake/CONTEXT.md (assess mode) |

## Cold Start

Load `_status/workspace-status.md` first for pipeline orientation.
Then load `_config/gap-context.md` for full component paths and resolution state.

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Enter stages/01-intake/CONTEXT.md |
| `prioritize` | Enter stages/02-prioritize/CONTEXT.md |
| `resolve` | Enter stages/03-resolution-design/CONTEXT.md |
| `plan` | Enter stages/04-remediation-plan/CONTEXT.md |
| `implement` | Enter stages/05-implement/CONTEXT.md |
| `close` | Enter stages/06-validate/CONTEXT.md |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

\`\`\`
Pipeline Status: {{component-slug}}-gap-analysis

  [01-intake] --> [02-prioritize] --> [03-resolution-design] --> [04-remediation-plan] --> [05-implement] --> [06-validate]
    STATUS           STATUS                STATUS                     STATUS                  STATUS           STATUS
\`\`\`

## Routing

| You want to... | Go to |
|----------------|-------|
| Import or discover gaps | stages/01-intake/CONTEXT.md |
| Score and sequence gaps | stages/02-prioritize/CONTEXT.md |
| Design resolution for each gap | stages/03-resolution-design/CONTEXT.md |
| Break resolutions into tasks | stages/04-remediation-plan/CONTEXT.md |
| Execute implementation | stages/05-implement/CONTEXT.md |
| Verify and close gaps | stages/06-validate/CONTEXT.md |
| Read gap configuration | _config/gap-context.md |
| Read delivery workspace | /workspaces/Marilo/workspaces/{{component-slug}}-delivery/CLAUDE.md |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Intake | _config/gap-context.md, stages/01-intake/CONTEXT.md | Stages 02-06 |
| Prioritize | _config/gap-context.md, stages/01-intake/output/, stages/02-prioritize/CONTEXT.md | Stages 03-06 |
| Resolution design | stages/02-prioritize/output/, stages/03-resolution-design/CONTEXT.md | Stages 01, 04-06 |
| Remediation plan | stages/03-resolution-design/output/, stages/04-remediation-plan/CONTEXT.md | Stages 01-02, 05-06 |
| Implement | stages/04-remediation-plan/output/, stages/05-implement/CONTEXT.md | Stages 01-03, 06 |
| Validate | stages/05-implement/output/, stages/06-validate/CONTEXT.md, shared/validation-checklist.md | Stages 01-04 |
| Any task | _config/gap-context.md always | Other component workspaces |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there.

## Global Constraints

1. **Traceability:** Every resolution must trace back to a gap ID from Stage 01.
2. **Code-first:** Read actual source before designing resolutions; never resolve from spec alone.
3. **Append-only records:** Gap records are never deleted, only status-updated.
4. **One-gap-one-decision:** Each gap gets exactly one resolution decision (even if batched for implementation).

<!-- Gap workspace owns: gap discovery, resolution design, implementation.
     Delivery workspace owns: spec accuracy, Example UX completeness.
     Never modify spec or demo files from this workspace. -->
```

---

## CONTEXT.md Template

```markdown
# Component Gap-Analysis Workspace -- {{component-name}}

Structured gap tracking and resolution for {{component-name}}.

## Task Routing

| Task Type | Entry Point | Trigger Keyword |
|-----------|-------------|-----------------|
| Import/discover gaps | stages/01-intake/CONTEXT.md | ingest |
| Score and sequence | stages/02-prioritize/CONTEXT.md | prioritize |
| Design resolutions | stages/03-resolution-design/CONTEXT.md | resolve |
| Plan remediation | stages/04-remediation-plan/CONTEXT.md | plan |
| Execute changes | stages/05-implement/CONTEXT.md | implement |
| Verify and close | stages/06-validate/CONTEXT.md | close |

## Shared Resources

| Resource | Location | Section to Load |
|----------|----------|-----------------|
| Gap context | _config/gap-context.md | Full file |
| Coverage summary | _config/coverage-summary.md | Full file |
| Gap record format | shared/gap-record-format.md | Full file |
| Priority framework | shared/priority-framework.md | Full file |
| Resolution record format | shared/resolution-record-format.md | Full file |
| Validation checklist | shared/validation-checklist.md | Full file |
| Test coverage ownership | shared/test-coverage-ownership.md | Full file |
| Delivery workspace | /workspaces/Marilo/workspaces/{{component-slug}}-delivery/CLAUDE.md | Routing table only |
```

---

## gap-context.md Template

```markdown
# Gap Context -- {{component-name}}

## Target Project

| Field | Value |
|-------|-------|
| Solution | Marilo.Components |
| Framework | .NET 10 / Blazor |
| Component | Marilo{{component-name}} |
| Category | {{category}} |
| Source path | src/Marilo.Components/{{source-subfolder}}/ |
| Test path | tests/Marilo.Tests.Unit/{{test-path}}/{{component-name}}Tests.cs |

## Gap Source

| Field | Value |
|-------|-------|
| Source type | component-builder Stage 07 output |
| Source file | Not yet populated |

## Resolution Scope

| Field | Value |
|-------|-------|
| Total gaps | 0 |
| Active phase | {{active-phase}} |

## Resolution Tracking

| Stage | Status | Last run |
|-------|--------|----------|
| 01-intake | Not started | -- |
| 02-prioritize | Not started | -- |
| 03-resolution-design | Not started | -- |
| 04-remediation-plan | Not started | -- |
| 05-implement | Not started | -- |
| 06-validate | Not started | -- |

## Test Coverage Rollup

| Batch | Tests | Passing |
|-------|-------|---------|
| (none yet) | 0 | -- |

## Constraints

- No Telerik dependencies
- License: MIT / Apache-2.0 / BSD only
- Must inherit from MariloComponentBase
- Must use CssProvider pattern (no hardcoded CSS classes)
```

---

## coverage-summary.md Template

```markdown
# Coverage Summary -- {{component-name}}

## Component Status

| Component | Phase | Gaps Total | Resolved | Open | Tests |
|-----------|-------|-----------|----------|------|-------|
| {{component-name}} | {{active-phase}} | 0 | 0 | 0 | 0 |

## Stage Output Index

| Stage | Latest Output | Date |
|-------|--------------|------|
| 01-intake | -- | -- |
| 02-prioritize | -- | -- |
| 03-resolution-design | -- | -- |
| 04-remediation-plan | -- | -- |
| 05-implement | -- | -- |
| 06-validate | -- | -- |

## Recent Movement

(No activity yet)

## Active Blockers

(None)
```

---

## workspace-status.md Template

```markdown
# Workspace Status

<!-- SUMMARY SNAPSHOT -- read at session start for fast orientation only.
     NOT authoritative. If this file contradicts a stage output, the stage output wins.
     Update this file after completing a stage or a significant batch of work.
     Do not append -- replace the content and update the date.
     Keep this file under 50 lines. See Pattern 16. -->

## Header

| Field | Value |
|-------|-------|
| Workspace | {{component-slug}}-gap-analysis |
| Last updated | {{date}} |
| Current phase | Pre-run (no stages executed yet) |

## Pipeline Status

\`\`\`
Stage 01 -- [ ] intake
Stage 02 -- [ ] prioritize
Stage 03 -- [ ] resolution-design
Stage 04 -- [ ] remediation-plan
Stage 05 -- [ ] implement
Stage 06 -- [ ] validate
\`\`\`

Key outputs so far:

- None. Workspace scaffolded but no stages run.

## Next Actions

1. Run Stage 01 (intake) to import gaps from delivery spec review or assess component source.

## Upstream Dependencies

- Component built via component-builder.
- Delivery workspace: {{component-slug}}-delivery (scaffolded, pre-run).
```

---

## Stage CONTEXT.md Templates

The six stage CONTEXT.md files should be copied from `gap-analysis-resolution/stages/*/CONTEXT.md` with the following replacements:
- All hardcoded paths replaced with `{{component-slug}}` and `{{component-name}}` placeholders
- All references to `gap-analysis-resolution` replaced with `{{component-slug}}-gap-analysis`
- Output file naming: `gap-{{component-slug}}-*.md`

## Shared Files

Copy these files verbatim from `gap-analysis-resolution/shared/`:
- `gap-record-format.md` -- replace area prefix with `GAP-{{COMPONENT-SLUG}}-[NNN]`
- `priority-framework.md` -- copy verbatim (no component-specific content)
- `resolution-record-format.md` -- replace prefix with `RES-{{COMPONENT-SLUG}}-[NNN]`
- `validation-checklist.md` -- copy verbatim
- `test-coverage-ownership.md` -- copy verbatim

## Questionnaire Template

```markdown
# Gap-Analysis Onboarding -- {{component-name}}

Answer these questions to configure the gap-analysis workspace.

**Q1.** What is the component name? (PascalCase)
> Default: {{component-name}}

**Q2.** What is the component source path?
> Default: src/Marilo.Components/{{source-subfolder}}/

**Q3.** Entry path: Do you have an existing gap list to import, or should we assess the source fresh?
> Options: existing | fresh

**Q4.** (If existing) Where is the gap source file?
> Example: stages/01-spec-review/output/{{component-slug}}-spec-gaps.md

**Q5.** What is the target state? Describe what "fully resolved" looks like for this component.

**Q6.** Any constraints beyond the defaults (no Telerik, MIT/Apache/BSD, MariloComponentBase, CssProvider)?

After onboarding, proceed to `stages/01-intake/CONTEXT.md`.
```
