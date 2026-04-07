# Spec Coverage Format

Each spec gap record follows this shape.

## Gap Record

**ID:** SPEC-wizard-[sequence]
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
**Delegated to:** [gap-analysis-resolution intake | spec update only]
