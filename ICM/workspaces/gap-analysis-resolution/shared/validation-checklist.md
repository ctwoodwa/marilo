# Validation Checklist

Framework for verifying that a gap is truly closed and will not re-open.

## Closure Criteria

For each gap, verify all applicable checks:

### Implementation Verification

| Check | How to Verify |
|-------|---------------|
| Target pattern adopted | Compare current code against the resolution record's target pattern. The implementation matches. |
| Original gap behavior gone | The specific deficiency described in the gap record is no longer present. |
| No regression | Existing tests pass. No previously working behavior is broken. |
| Tests cover the change | New or updated tests exercise the resolved behavior specifically. |

### Integration Verification

| Check | How to Verify |
|-------|---------------|
| Consumers unaffected (or migrated) | If the change affects a public API, all callers have been updated. |
| Cross-cutting consistency | If this gap was part of a theme, all related gaps follow the same pattern. |
| Build succeeds | The project compiles without warnings related to the change. |

### Documentation Verification

| Check | How to Verify |
|-------|---------------|
| API docs updated | Component spec or API reference reflects the new behavior. |
| Examples updated | Code samples and reference implementations show the new pattern. |
| Gap analysis updated | The gap record status is set to Resolved with evidence. |

### Enforcement Verification

| Check | How to Verify |
|-------|---------------|
| Review guidance exists | Code review checklist or PR template mentions the new pattern. |
| Automated check (if applicable) | Analyzer rule, lint rule, or CI check prevents regression. |
| Template updated (if applicable) | Component template or scaffold generates the correct pattern for new components. |

## Closure Statuses

| Status | Meaning | Required Evidence |
|--------|---------|-------------------|
| **Resolved** | Gap fully closed | Code change + test + at least one enforcement mechanism |
| **Partially resolved** | Core issue fixed, edge cases remain | Code change + description of what remains + follow-up gap ID |
| **Deferred** | Intentionally postponed | Rationale for deferral + target date or condition for revisit |
| **Won't fix** | Accepted as-is | Rationale explaining why the gap is acceptable |

## Evidence Format

For each resolved gap, provide:

```markdown
**GAP-[AREA]-[NNN]: [Title]**
- Status: Resolved
- Changed: [file paths modified]
- Tests: [test file paths or "manual verification: [steps]"]
- Enforcement: [what prevents regression]
- Notes: [any deviations from the resolution record]
```
