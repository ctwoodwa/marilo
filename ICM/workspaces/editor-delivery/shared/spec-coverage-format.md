# Spec Coverage Format

Each spec gap record uses the following structure:

## Record Format

```
### SPEC-editor-[sequence]

| Field | Value |
|-------|-------|
| ID | SPEC-editor-[sequence] |
| Feature area | [feature area name] |
| Parameter/event | [name] |
| Gap type | undocumented / spec-ahead / mismatch |
| Source location | [file:line] |
| Spec location | [file:section] or "missing" |
| Description | [what is wrong] |
| Priority | P1 / P2 / P3 |
| Priority rationale | [why this priority] |
| Suggested resolution | [brief action] |
```

## Gap Types

| Type | Code | Meaning |
|------|------|---------|
| Undocumented | undocumented | Implemented in source but not in spec |
| Spec ahead | spec-ahead | Documented in spec but not in source |
| Mismatch | mismatch | Both exist but type, name, or description differs |

## Priority Levels

| Level | Criteria |
|-------|----------|
| P1 | Public API surface visible to developers; blocks demo scenarios |
| P2 | Documented feature with incorrect details; confusing but not blocking |
| P3 | Internal or rarely used parameter; cosmetic spec issue |
