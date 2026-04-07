# Stage 03 - Standards Alignment

Confirm or derive the documentation standards for this specific project. Produces the authoritative rules that Stage 04 applies.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Doc audit report | `../02-doc-audit/output/[slug]-doc-audit.md` | Style violations section | Reveals which rules conflict with existing patterns |
| Current doc standards | `../../_config/doc-standards.md` | All | Starting point for confirmation |
| Project context | `../../_config/project-context.md` | Audience, tone | Validates that standards match the intended readers |
| Naming reference | `../../shared/naming-conventions.md` | All | Cross-checks naming rules against the canonical reference |

## Process

1. Review style violations from the audit. Flag any rule that conflicts with the existing docs pattern.
2. Present proposed standards to the human — show only rules that differ from defaults or have conflicts.
3. Record any agreed exceptions in the `Exceptions` section of `_config/doc-standards.md`.
4. Produce a confirmed-standards snapshot for traceability.
5. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Confirmed standards snapshot | `output/[slug]-confirmed-standards.md` | Full copy of agreed rules |
| Exceptions log | `_config/doc-standards.md` (Exceptions section) | Inline update |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Proposed rule changes and conflicts | Approve, override, or add exceptions |
| 4 | Final confirmed-standards snapshot | Sign off before Stage 04 begins |

## Audit

| Check | Pass Condition |
|---|---|
| No unresolved conflicts | Every flagged rule has an approved resolution |
| Exceptions documented | Any deviation from defaults is recorded in `_config/doc-standards.md` |
| Snapshot produced | `output/[slug]-confirmed-standards.md` exists and covers all rule categories |
