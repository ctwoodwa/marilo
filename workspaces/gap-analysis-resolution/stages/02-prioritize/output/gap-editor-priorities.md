# Editor Gap Prioritization

> Date: 2026-04-04
> Source: gap-editor-inventory.md (12 gaps)
> Stage: 02-prioritize

## Pre-Prioritization Audit Notes

- Editor uses contenteditable + execCommand approach (not ProseMirror). This is a valid architecture for an MIT component.
- GAP-EDITOR-003 (ProseMirror) is an architecture evaluation, not a gap in existing functionality.
- GAP-EDITOR-008 (sanitization docs) — SanitizeHtml() exists; this is documentation, not code.
- GAP-EDITOR-010 (edit mode) — likely already complete (Edit/Preview/Source cover the spec).

## Priority Batches

### Batch 1: Public API + Validation + Testing (Critical + High) — 6 gaps

| Gap | Severity | Description | Effort | Notes |
|-----|----------|-------------|--------|-------|
| GAP-EDITOR-001 | Critical | Missing ExecuteAsync() public method | M | Expose internal tool execution as public API |
| GAP-EDITOR-007 | High | Missing validation integration | M | EditContext integration via FieldChanged notification |
| GAP-EDITOR-011 | Medium | Low test coverage (7 tests / 670 lines) | L | Expand to 15+ tests |
| GAP-EDITOR-008 | High | Security/sanitization documentation | S | Document existing SanitizeHtml() behavior |
| GAP-EDITOR-010 | Medium | Edit mode Div verification | S | Verify enum completeness; close if already covered |
| GAP-EDITOR-009 | Medium | Custom tool creation API | M | EditorCustomTool or extend ToolbarTemplate |

### Batch 2: Advanced Features (High, larger scope) — 3 gaps

| Gap | Severity | Description | Effort | Notes |
|-----|----------|-------------|--------|-------|
| GAP-EDITOR-002 | High | Adaptive toolbar (overflow popup) | L | ResizeObserver JS interop + overflow detection |
| GAP-EDITOR-004 | High | Table/image resize | L | JS interop drag handles |
| GAP-EDITOR-005 | High | Import/export | L | Format converters (Markdown → HTML at minimum) |

### Deferred

| Gap | Severity | Description | Reason |
|-----|----------|-------------|--------|
| GAP-EDITOR-003 | Critical | ProseMirror integration | Architecture decision; current contenteditable approach is valid for v1 |
| GAP-EDITOR-006 | Critical | Demo pages | Defer to Editor delivery CDW |
| GAP-EDITOR-012 | Medium | Large content handling | Performance optimization; assess after functional gaps closed |

## Recommended Sequence

1. **Batch 1** — ExecuteAsync, validation, test expansion, documentation gaps (highest ROI)
2. **Batch 2** — Adaptive toolbar, table/image resize, import/export (feature completeness)
3. **Deferred** — ProseMirror evaluation, demos, performance optimization

## Dependencies

- ExecuteAsync: needs understanding of current JS interop tool execution pipeline
- Validation: depends on EditContext pattern used in MariloForm ecosystem
- Custom tools: depends on understanding ToolbarTemplate and EditorTool enum
