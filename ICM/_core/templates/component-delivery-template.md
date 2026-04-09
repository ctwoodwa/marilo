<!-- Target: under 120 lines. Trim if longer. -->
<!-- Build note: this file is an archetype guide and folder scaffold. It is NOT
     a runnable workspace. Copy it, rename the folder, and fill in the placeholders.
     See _core/placeholder-syntax.md for {{VARIABLE}} conventions. -->
# Component Delivery Workspace

A workspace archetype for delivering a discrete component, feature, or library that requires
coordinated code, documentation, examples, and validation before it can be considered shipped.

## When to Use This Pattern

Use a component-delivery workspace when ALL of the following are true:

- You are delivering something with a defined API surface or interface contract.
- Delivery requires more than code -- it also needs docs, usage examples, or sign-off.
- There are explicit acceptance criteria or a stakeholder who must approve the result.
- The work spans multiple sessions or involves more than one contributor.

If you only have gaps to close with no new API surface, use `gap-analysis-resolution` instead.
If you are designing a full platform from scratch, use `enterprise-software-dev` instead.

## Recommended Stages

<!-- Build note: stages are a suggested shape, not a fixed pipeline. Skip or merge
     stages that do not apply. Stage 06 (release) is optional for internal-only work. -->

| Stage | Name | Purpose |
| ----- | ---- | ------- |
| 01 | Scope and contracts | Define what is being delivered, the API surface, and acceptance criteria |
| 02 | Implementation | Code work; may reference gap-analysis outputs for known issues |
| 03 | Documentation | API docs, usage guides, migration notes |
| 04 | Examples and demos | Working examples that prove the component in realistic scenarios |
| 05 | Validation | Integration tests, stakeholder review, sign-off checklist |
| 06 | Release coordination | Versioning, changelog, deployment notes (optional) |

## Folder Scaffold

```
component-delivery-{{NAME}}/
├── CLAUDE.md              (workspace routing)
├── CONTEXT.md             (task routing and shared resources)
├── _status/               (summary snapshots -- one file per stage)
├── setup/                 (onboarding questionnaire)
├── stages/
│   ├── 01-scope/
│   ├── 02-implement/
│   ├── 03-document/
│   ├── 04-examples/
│   ├── 05-validate/
│   └── 06-release/        (optional)
└── shared/                (cross-stage reference files)
```

## What Belongs Where

<!-- Build note: output/ folders hold finished artifacts only. Work-in-progress
     drafts and notes stay in the stage folder itself, not in output/. -->

| Artifact type | Location |
| ------------- | -------- |
| API contracts and interface decisions | `stages/01-scope/output/` |
| Implementation code references (links, not code) | `stages/02-implement/output/` |
| Usage documentation | `stages/03-document/output/` |
| Working examples | `stages/04-examples/output/` |
| Validation results and sign-off | `stages/05-validate/output/` |
| Release notes and changelog | `stages/06-release/output/` |

## Integration Points

<!-- Build note: these are the standard handoff patterns. Document any actual
     file paths when you wire up a real workspace. -->

**Consuming from another workspace**
- A `gap-analysis-resolution` workspace can feed Stage 02. Place resolution designs in
  `stages/02-implement/` as inputs before starting implementation.

**Producing for another workspace**
- Stage 05 outputs (test coverage gaps, integration results) can seed a
  `test-coverage-expansion` workspace. Link to `stages/05-validate/output/` as the source.

## Suggested Triggers

<!-- Build note: `setup` and `status` are inherited from the root CLAUDE.md.
     Add only triggers specific to this workspace type. -->

| Keyword | Action |
| ------- | ------ |
| `setup` | Run onboarding questionnaire |
| `status` | Show delivery pipeline completion across all stages |
| `deliver` | Jump to the next incomplete stage |
| `validate` | Jump to Stage 05 validation |

## Stage Handoffs

Each stage writes finished artifacts to its own `output/` folder. The next stage reads from
there. Editing an output file is picked up automatically by the downstream stage.
