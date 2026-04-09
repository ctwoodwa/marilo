# Closure Report: Editor Batch 2a — Import/Export with Markdig

> Date: 2026-04-09
> Components: `MariloEditor`
> Scope: single

---

## Summary

| Gap | Title | Status |
|-----|-------|--------|
| GAP-EDITOR-005 | MariloEditor: Missing import/export functionality | **Resolved** |

## GAP-EDITOR-005: Import/Export

- **Status:** Resolved
- **Changed:**
  - `src/Marilo.Components/Editors/EditorFormatConverter.cs` (new) — `IEditorFormatConverter` interface, `MarkdownFormatConverter` (Markdig), `PlainTextFormatConverter`, `BasicHtmlToMarkdown` helper, DI extension methods
  - `src/Marilo.Components/Editors/MariloEditor.razor` — `@inject`, `ImportAsync`, `ExportAsync`, `ResolveConverter`
  - `Directory.Packages.props` — Markdig 0.39.1 (MIT)
  - `src/Marilo.Components/Marilo.Components.csproj` — `<PackageReference Include="Markdig" />`
- **Tests:** 8 new bUnit tests in `EditorTests.cs`. **675/675 total suite passing** (runtime validated).
- **Human decision honored:** Markdig is a bounded adapter for import/export, not the editor's core model. The editor's internal `Value` remains an HTML string; Markdig only participates at the conversion boundary.

### Remaining Editor gaps (Batch 2 residual)

| Gap | Severity | Description | Blocker |
|-----|----------|-------------|---------|
| GAP-EDITOR-002 | High | Adaptive toolbar (overflow popup) | JS interop (ResizeObserver) |
| GAP-EDITOR-004 | High | Table/image resize | JS interop (drag handles) |
| GAP-EDITOR-003 | Critical | ProseMirror integration | Architecture decision (deferred) |
| GAP-EDITOR-006 | Critical | Demo pages | Editor delivery CDW |
| GAP-EDITOR-012 | Medium | Large content handling | Performance optimization (deferred) |
