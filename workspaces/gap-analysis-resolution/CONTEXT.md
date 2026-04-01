# Gap Analysis Resolution

Structured lifecycle for resolving documented gaps: import, prioritize, design, plan, implement, validate.

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
| Gap record format | `shared/gap-record-format.md` | Standard shape for normalized gap records |
| Priority framework | `shared/priority-framework.md` | Scoring criteria and sequencing rules |
| Resolution record format | `shared/resolution-record-format.md` | Standard shape for resolution decisions |
| Validation checklist | `shared/validation-checklist.md` | Closure criteria and enforcement checks |
| Canonical defaults | `references/layer-3-defaults-index.md` | Pointers to enterprise defaults (read-only) |

## Rules

1. Treat referenced defaults as canonical read-only sources.
2. Run stages in order; carry outputs forward via `output/` folders.
3. Use `gap-[slug]-*.md` for output naming (slug = component or area name).
4. Every resolution traces to a documented gap. No opportunistic changes.
