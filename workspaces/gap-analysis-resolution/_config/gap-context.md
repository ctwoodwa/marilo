# Gap Resolution Context

Single source of truth for this resolution run. Populated during Stage 01. Every downstream stage reads from here.

## Target Project

| Field | Value |
|-------|-------|
| Project name | Marilo.Components |
| Project path | /workspaces/Marilo/src/Marilo.Components |
| Technology stack | .NET 10 / Blazor / C# / Razor Class Library |
| Repository URL | https://github.com/ctwoodwa/Marilo |

## Gap Analysis Source

| Field | Value |
|-------|-------|
| Entry path | existing |
| Source files | `Navigation/GAP_ANALYSIS_PART2.md` |
| Index file | `GAP_ANALYSIS_INDEX.md` |
| Analysis date | 2026-04-01 |
| Scope | batch (related gaps in one area: Navigation/TreeView) |

## Target State

`MariloTreeView` and `MariloTreeItem` fully implement their documented API specifications with functional and behavioral parity to the Telerik UI for Blazor equivalents, enhanced by patterns identified from open-source source review (Radzen Tree, MudBlazor TreeView, BlazorVirtualTreeView, Fancytree, excubo-ag, jsTree). This includes:

- Partial file architecture (`.razor` markup + `.razor.cs` code-behind) following `MariloDataGrid` pattern.
- Tri-state checkbox propagation with `AllowCheckChildren`/`AllowCheckParents` (Radzen pattern).
- `CheckedItems` two-way bindable collection exposed to consumers.
- `TreeSelectionMode` enum (`None`, `Single`, `Multiple`) for configurable selection behavior.
- `LoadChildrenAsync` callback for on-demand lazy loading with load-once semantics.
- Full keyboard navigation following WAI-ARIA TreeView pattern (Fancytree model).
- Phase 2 enhancements: ExpandOnClick, SingleExpand, AutoExpand, ExpandAll/CollapseAll, FilterFunc, Disabled/ReadOnly.
- Phase 3 enhancements: Virtualization, programmatic navigation, context menu event, checkbox template, inline editing.

All implementations are independent (no Telerik dependency), use MIT/Apache-2.0-compatible code only.

## Resolution Scope

| Field | Value |
|-------|-------|
| Area/module | Navigation/TreeView |
| Total gaps identified | 22 (6 original + 16 from source review) |
| Original (implemented) | 6 |
| Phase 1 — Core | 5 (tri-state, checked binding, multi-select, lazy load, keyboard) |
| Phase 2 — Enhanced | 6 (expand-on-click, single-expand, auto-expand, batch expand, filter, disabled) |
| Phase 3 — Advanced | 5 (virtualization, programmatic nav, context menu, checkbox template, inline edit) |
| Stage routing | 03 > 05 > 06 (batch — all stages complete) |

## Resolution Tracking

| Stage | Status | Output |
|-------|--------|--------|
| 01-intake | complete | `Navigation/GAP_ANALYSIS_PART2.md` (original + source review gaps) |
| 02-prioritize | complete | Phased in `Navigation/resolution/RESOLUTION_STATUS.md` |
| 03-resolution-design | complete | `Navigation/resolution/IMPLEMENTATION_NOTES.md` (patterns from source review) |
| 04-remediation-plan | skipped | (batch scope) |
| 05-implement | complete | Refactor to partial files + 21/22 gaps implemented (Gap 18 virtualization deferred) |
| 06-validate | complete | `stages/06-validate/output/gap-treeview-closure-report.md` |

## Constraints and Notes

- This is an open-source Blazor component library. All implementations must be independent — no Telerik UI for Blazor code or dependencies.
- External OSS code/packages permitted only with MIT, Apache-2.0, BSD-2-Clause, or BSD-3-Clause compatible licenses.
- Existing `MariloComponentBase` base class provides `Class`, `Style`, `AdditionalAttributes`, `CssProvider`, `IconProvider`, `ThemeService`.
- The `IMariloCssProvider` interface defines `TreeViewClass()` and `TreeItemClass(isExpanded, isSelected)`.
- A `CheckBoxMode` enum already exists with `None`, `Single`, `Multiple` values.
- The `TreeDragDropEventArgs` model exists in `Marilo.Core.Models`.
- Source review patterns are documented in `Navigation/resolution/IMPLEMENTATION_NOTES.md` — all implementations should follow those design decisions.
