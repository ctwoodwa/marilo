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
| Total gaps identified | 24 (6 original + 16 from source review + 2 post-reconstruction) |
| Original (implemented) | 6 |
| Phase 1 — Core | 5 (tri-state, checked binding, multi-select, lazy load, keyboard) |
| Phase 2 — Enhanced | 6 (expand-on-click, single-expand, auto-expand, batch expand, filter, disabled) |
| Phase 2.5 — Fix | 2 (GAP-readonly-guards: ReadOnly missing from guards; GAP-expandall-lazyload: ExpandAll skips lazy nodes) |
| Phase 3 — Advanced | 5 (virtualization, programmatic nav, context menu, checkbox template, inline edit) |
| Stage routing | All stages complete for Phases 1-3 and 2.5; Gap 18 deferred |

## Resolution Tracking

| Stage | Status | Output |
|-------|--------|--------|
| 01-intake | complete | `Navigation/GAP_ANALYSIS_PART2.md` (original + source review gaps) |
| 02-prioritize | complete | Phased in `Navigation/resolution/RESOLUTION_STATUS.md` |
| 03-resolution-design | complete | `Navigation/resolution/IMPLEMENTATION_NOTES.md` (patterns from source review) |
| 04-remediation-plan | skipped | (batch scope) |
| 05-implement | complete | Refactor to partial files + 21/22 gaps implemented (Gap 18 virtualization deferred) |
| 06-validate | complete | `stages/06-validate/output/gap-treeview-closure-report.md` |

## Test Coverage Rollup

| Batch | Tests written | Tests passing | Coverage notes |
| ----- | ------------- | ------------- | -------------- |
| treeview | 45 bUnit (17 Ph1 + 28 Ph2) | 45/45 | Gap 18 (virtualization) deferred; no test coverage for deferred gap |
| form | 20 | 20/20 ✅ | Stage 06 closed (2026-04-02); 35+ resolved (RES-FORM-001/002/003), 11 deferred to Phase 2+; runtime re-validated 2026-04-10 (FormTests 20/20) |
| t4-pickers-batch1 | 17 | 17 | Stage 06 closed (2026-04-03); 7 resolved, 3 partially resolved |
| t4-pickers-batch2 | 9 | pending | Stage 06 closed (2026-04-04); 4/4 resolved; runtime test pending |
| t4-pickers-batch3 | 17 | 17 | Stage 06 closed (2026-04-05); 12 resolved, 1 won't fix; 547/547 full suite |
| t4-pickers-batch4 | 12 | 12/12 ✅ | Stage 06 closed (2026-04-08); 2/2 resolved (MSEL-003 GroupField + DTP-002 tumbler steps); runtime validated 2026-04-09 (667/667 full suite) |
| t4-pickers-batch5 | 12 | 12/12 ✅ | Stage 06 closed (2026-04-08); 2/2 resolved (MSEL-006 OnRead/Rebind/ValueMapper + DTP-003 typed input); also closes OnRead portion of GAP-MSEL-001; runtime validated 2026-04-09 |
| t4-pickers-batch6 | 11 | 11/11 ✅ | Stage 06 closed (2026-04-08); 2/2 resolved (MSEL-001 final OnChange/OnItemRender + MSEL-007 ItemHeight/PageSize); MSEL-007 ScrollMode deferred; runtime validated 2026-04-09 (1 test fixed: OnItemRender cache double-rebuild) |
| t4-pickers-batch7 | 7 | 7/7 ✅ | Stage 06 closed (2026-04-08); 1/1 resolved (MSEL-005 MultiSelectSettings + MultiSelectPopupSettings child API); subagent-driven dev mode; also fixed Batch 6 SetParametersAndRender→Render bUnit v2 build break; runtime validated 2026-04-09 |
| editor-batch2a | 8 | 8/8 ✅ | Stage 06 closed (2026-04-09); GAP-EDITOR-005 resolved (import/export with Markdig + plaintext); 675/675 full suite runtime validated |
| readonly-guards | 6 | 6 | Stage 06 closed (2026-04-03) |
| expandall-lazyload | 6 | 6 | Stage 06 closed (2026-04-03) |

| splitter | 17 | pending | Stage 06 closed (2026-04-04); 8 resolved, 1 demo deferred; runtime test pending |
| wizard | 27 | pending | Stage 06 closed (2026-04-04); 18/18 resolved; runtime test pending |
| chart-batch1 | 16 | pending | Stage 06 closed (2026-04-04); 8/8 resolved (3 pre-existing); runtime test pending |
| chart-batch2 | 11 | pending | Stage 06 closed (2026-04-04); 5/5 resolved (1 pre-existing); runtime test pending |
| editor-batch1 | 14 | pending | Stage 06 closed (2026-04-04); 6/6 resolved (2 pre-existing); runtime test pending |
| datagrid-phase1 | 18 | 18 | Stage 06 closed (2026-04-04); 9 resolved, 1 deferred; runtime test pending |
| datagrid-phase2 | 15 | 15 | Stage 06 closed (2026-04-04); 6 resolved (validation, composite filters, auto-gen attrs, aggregates, export, CancellationToken); runtime test pending |
| datagrid-phase3 | 10 | 10 | Stage 06 closed (2026-04-05); 2 resolved (CheckBoxList filter, cell selection); 557/557 full suite |
| gantt-full | 31 | 31/31 ✅ | Stage 06 closed (2026-04-09); 20/20 gaps resolved (full generic rewrite); subagent-driven dev; runtime pending |
| t4-pickers-batch8a | 23 | 23/23 ✅ | Stage 06 closed (2026-04-09); 6/6 resolved (DRP PopupClass/ShowWeekNumbers/Size/Rounded/FillMode/DebounceDelay/Title/HeaderTemplate + DTP ValidateOn); 726/726 full suite |
| t4-pickers-batch8b | 13 | 13/13 ✅ | Stage 06 closed (2026-04-09); 4/4 resolved (TP InputMode/ValidateOn/OnChange-on-blur/CSS provider); 726/726 full suite |
| t4-pickers-batch8c | 12 | 12/12 ✅ | Stage 06 closed (2026-04-09); 3/3 resolved (FU template context/CSS provider + UPL UploadChunkSettings); 726/726 full suite |
| colorpicker-standalone | 18 | 18/18 ✅ | Stage 06 closed (2026-04-09); 5/5 CPICK gaps resolved (Gradient, Palette, FlatPicker, Views API, CSS provider); subagent-driven dev |
| drp-multiview | 5 | 5/5 ✅ | Stage 06 closed (2026-04-09); 2/2 DRP gaps resolved (Year/Decade calendar views, FocusAsync methods) |
| filemanager | 151 | 151/151 ✅ | Stage 06 closed (2026-04-09); 36/36 gaps resolved (full generic rewrite, Phases A-F); 877/877 full suite runtime validated |
| js-interop-batch3 | 14 | 14/14 ✅ | Stage 06 closed (2026-04-10); 1/1 resolved (Editor table/image resize); 1097/1097 full suite runtime validated |
| datagrid-cdw | 0 | 0 | Per-feature checklist ready for CDW handoff (2026-04-03) |

Canonical test evidence: `stages/06-validate/output/gap-*-closure-report.md`
Ownership model: `shared/test-coverage-ownership.md`

## Constraints and Notes

- This is an open-source Blazor component library. All implementations must be independent — no Telerik UI for Blazor code or dependencies.
- External OSS code/packages permitted only with MIT, Apache-2.0, BSD-2-Clause, or BSD-3-Clause compatible licenses.
- Existing `MariloComponentBase` base class provides `Class`, `Style`, `AdditionalAttributes`, `CssProvider`, `IconProvider`, `ThemeService`.
- The `IMariloCssProvider` interface defines `TreeViewClass()` and `TreeItemClass(isExpanded, isSelected)`.
- A `CheckBoxMode` enum already exists with `None`, `Single`, `Multiple` values.
- The `TreeDragDropEventArgs` model exists in `Marilo.Core.Models`.
- Source review patterns are documented in `Navigation/resolution/IMPLEMENTATION_NOTES.md` — all implementations should follow those design decisions.
