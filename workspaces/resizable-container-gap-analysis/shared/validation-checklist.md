# Validation Checklist -- ResizableContainer

Framework for verifying that a gap is truly closed and will not re-open.

## Closure Criteria

For each gap, verify all applicable checks:

### Code Verification

| Check | How to Verify |
|-------|---------------|
| Code compiles | `dotnet build` succeeds with no errors or warnings related to ResizableContainer |
| Target pattern adopted | Compare current code against the resolution record's target pattern |
| Original gap behavior gone | The specific deficiency described in the gap record is no longer present |
| No regressions | All existing tests pass; no previously working behavior is broken |

### Test Verification

| Check | How to Verify |
|-------|---------------|
| Tests pass | `dotnet test` passes for `tests/Marilo.Tests.Unit/Layout/MariloResizableContainerTests.cs` |
| Tests cover the change | New or updated tests exercise the resolved behavior specifically |
| Test coverage adequate | Critical paths and edge cases have test assertions |

### Spec and Documentation Verification

| Check | How to Verify |
|-------|---------------|
| Spec updated | Component specification reflects the new behavior |
| API docs current | Parameters, events, and methods are documented accurately |
| Gap analysis updated | The gap record status is set to Resolved with evidence |

### Demo Verification

| Check | How to Verify |
|-------|---------------|
| Demo updated | Demo page in `samples/Marilo.Demo/` reflects the resolved behavior |
| Demo renders correctly | Visual inspection confirms the component works as expected |

### Provider Verification

| Check | How to Verify |
|-------|---------------|
| IMariloCssProvider updated | If CSS classes changed, interface method exists |
| BootstrapCssProvider updated | Bootstrap provider returns correct classes |
| FluentUICssProvider updated | FluentUI provider returns correct classes |
| ProviderSwitcher updated | ProviderSwitcher delegates the new interface method |
| SCSS rebuilt | `npm run scss:build` ran after any style changes |

## Closure Statuses

| Status | Meaning | Required Evidence |
|--------|---------|-------------------|
| **Resolved** | Gap fully closed | Code change + test + at least one enforcement mechanism |
| **Partially resolved** | Core issue fixed, edge cases remain | Code change + description of what remains + follow-up gap ID |
| **Deferred** | Intentionally postponed | Rationale for deferral + target date or condition for revisit |
| **Won't fix** | Accepted as-is | Rationale explaining why the gap is acceptable |

## Evidence Format

```markdown
**GAP-RESIZABLE-CONTAINER-[NNN]: [Title]**
- Status: Resolved
- Changed: [file paths modified]
- Tests: [test file paths or "manual verification: [steps]"]
- Enforcement: [what prevents regression]
- Notes: [any deviations from the resolution record]
```
