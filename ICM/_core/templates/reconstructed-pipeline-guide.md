# Reconstructed Pipeline Guide

Use this guide when building a workspace around code that already exists. The pipeline runs backward: artifacts are inferred from the codebase rather than used to drive it.

---

## When to Use Reconstructed Mode

Use reconstructed mode when:

- The codebase predates the workspace
- The team wants ICM governance, audit, or extension coverage without redoing completed work
- A code review or compliance audit needs structured artifacts to validate against

Use standard mode when you are starting from requirements and building forward.

---

## What Artifacts Are Reconstructed vs. Created

| Stage | Artifact | Treatment |
| ----- | -------- | --------- |
| 01 Discovery | System inventory, dependency map | Reconstructed -- read from code, config, and repo history |
| 02 Architecture | Component diagram, data flow | Reconstructed -- inferred from actual structure; note deviations from standards |
| 03 Build plan | Implementation record | Reconstructed -- documents what was built, not what to build |
| 04 Integration | Integration map | Reconstructed -- traced from real wiring (DI, config, middleware) |
| 05 Standards check | Gaps and deviations list | Created fresh -- compare actual code against Layer 3 defaults |
| 06 Validation | Test results, coverage report | Validated -- run against real code, not specs |

**Reconstructed** = inferred from existing code and config.
**Created** = produced fresh with no prior equivalent in the repo.
**Validated** = run against the actual codebase; results reflect reality.

---

## How Plan Tracking Differs

In standard mode, the plan tracks what to build. In reconstructed mode, it tracks what has been documented and what gaps remain.

Replace standard status terms with these:

| Standard status | Reconstructed equivalent |
| --------------- | ------------------------ |
| PENDING | NOT DOCUMENTED |
| IN PROGRESS | DOCUMENTING |
| COMPLETE | DOCUMENTED |
| VALIDATED | VALIDATED |

Add a `GAPS` column to the plan. Each gap is a place where the real code does not meet the target standard. Gaps are action items, not failures.

---

## Guardrails: Do Not Falsify Evidence

This is the most important rule in reconstructed mode.

- Reconstructed artifacts must reflect actual code. If the code does not meet a standard, write that down. Do not write what the code should do.
- If a pattern is partially implemented, say "partial." Do not say "complete."
- Validation stages must run against the real codebase. Validating a reconstructed spec instead of the code defeats the purpose.
- If a gap would be embarrassing to document, document it anyway. The point of reconstruction is accurate coverage, not a clean report.

---

## How to Declare Reconstructed Mode

Add a `Mode` section near the top of the workspace CLAUDE.md:

```markdown
## Mode

reconstructed -- this workspace documents an existing codebase; artifacts describe what exists, not what to build
```

Alternatively, create `_config/mode.md` with the same content if you prefer to keep CLAUDE.md minimal.

---

## How Stage CONTEXT.md Files Adapt

Change the Process section verbs to reflect analysis rather than creation.

Standard phrasing:

```
1. Write the component inventory based on requirements.
2. Define the data model.
```

Reconstructed phrasing:

```
1. Analyze existing code to produce the component inventory.
2. Trace the data model from migrations, entity classes, and config.
```

Also update the Inputs table to point at source code folders and config files rather than prior-stage specs.

If a stage has an Audit section, add a check: "All artifacts reflect actual code state, not aspirational state."
