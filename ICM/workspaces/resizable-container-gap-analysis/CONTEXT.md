# ResizableContainer Gap Analysis

Structured lifecycle for resolving documented gaps in MariloResizableContainer: import, prioritize, design, plan, implement, validate.

## Task Routing

| Task Type | Go To | Description |
|-----------|-------|-------------|
| Import gap analysis | `stages/01-intake/CONTEXT.md` | Parse existing gap files or assess current state to identify gaps |
| Prioritize gaps | `stages/02-prioritize/CONTEXT.md` | Score impact, sequence dependencies, produce resolution backlog |
| Design resolution | `stages/03-resolution-design/CONTEXT.md` | Define target patterns, choose solutions, capture decisions |
| Plan remediation | `stages/04-remediation-plan/CONTEXT.md` | Break resolutions into tasks, phases, and success criteria |
| Implement changes | `stages/05-implement/CONTEXT.md` | Execute code/config/process changes per plan |
| Validate and close | `stages/06-validate/CONTEXT.md` | Verify closure via tests/reviews, add enforcement guardrails |

## Shared Resources

| Resource | Location | Contains |
|----------|----------|----------|
| Gap context | `_config/gap-context.md` | Scope, target project, source files, resolution tracking |
| Coverage summary | `_config/coverage-summary.md` | Component status, stage output index |
| Workspace status | `_status/workspace-status.md` | Pipeline snapshot with stage completion |
| Gap record format | `shared/gap-record-format.md` | Standard shape for normalized gap records (ID prefix: GAP-RESIZABLE-CONTAINER) |
| Priority framework | `shared/priority-framework.md` | Scoring criteria and sequencing rules |
| Resolution record format | `shared/resolution-record-format.md` | Standard shape for resolution decisions (ID prefix: RES-RESIZABLE-CONTAINER) |
| Validation checklist | `shared/validation-checklist.md` | Closure criteria and enforcement checks |
| Test coverage ownership | `shared/test-coverage-ownership.md` | Test ownership guide with component test path |

## Rules

1. Run stages in order; carry outputs forward via `output/` folders.
2. Use `gap-resizable-container-*.md` for output naming.
3. Every resolution traces to a documented gap. No opportunistic changes.
4. Read affected code before designing a resolution.
