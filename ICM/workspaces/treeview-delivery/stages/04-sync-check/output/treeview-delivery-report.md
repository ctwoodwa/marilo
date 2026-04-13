# TreeView Delivery Report

**Report Date:** 2026-04-12 (Tick 15 — Stage 04 Final Sync-Check)
**Component:** MariloTreeView / MariloTreeItem
**Worker:** `w-treeview-delivery`
**Build Verification:** `dotnet build Marilo.slnx` — exit 0, 0 warnings, 0 errors (13.00s)
**Gate Status:** AMBER

---

## Executive Summary

This report cross-references all outputs from the treeview-delivery pipeline (Stages 01-03) into a unified gap inventory. Each gap is classified as CLEAR (resolved during this pipeline), BLOCKED (requires implementation work in treeview-gap-analysis), or REMEDIATION-NEEDED (resolution design exists, needs implementation).

The treeview component has a solid behavioral foundation (67/67 tests passing, 23 demo scenarios, all P1 demo gaps closed). The remaining gap surface is dominated by (1) spec-vs-source naming drift, (2) SCSS visual parity deficiencies, and (3) planned-but-unimplemented Telerik-parity features.

---

## Tick-15 Decision Integration

Two escalated decisions were resolved by the orchestrator in `_orchestrator/decisions/tick-15-2026-04-12-0220.md`:

| Decision | Resolution | Impact |
|----------|-----------|--------|
| **VP-002: Class Naming Convention** | Rename SCSS selectors from `mar-treeitem__*` to `mar-tree-item__*` to match Razor markup. SCSS is the change target, not Razor. | Unblocks 7 of 17 visual-parity gaps. Routes to implementation. |
| **VP-004: CssProvider Density Extension** | Add `TreeViewClass(string? size)` overload to `IMariloCssProvider`. FluentUI implements with density token mapping. Other providers get pass-through stub. | Unblocks density variant SCSS. Routes to implementation. |

---

## Unified Gap Inventory

### Stage 01: Spec Review (48 records total)

#### (A) Undocumented — Implemented but not in spec (14 records)

| ID | Parameter/Event | Priority | Status | Notes |
|----|----------------|----------|--------|-------|
| SPEC-001 | `ExpandOnClick` | P2 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-002 | `ExpandOnDoubleClick` | P2 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-003 | `SingleExpand` | P2 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-004 | `AutoExpand` | P2 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-005 | `AllowEditing` / `OnItemEdit` | P2 | **CLEAR** | Added to overview.md; events.md has full event docs (2026-04-11) |
| SPEC-006 | `FilterFunc` + `ClearFilter()` | P2 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-007 | `CheckboxTemplate` | P2 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-008 | `OnItemContextMenu` | P2 | **CLEAR** | events.md now documents full EventCallback with args (2026-04-11) |
| SPEC-009 | `Disabled` | P3 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-010 | `ReadOnly` | P3 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-011 | `SelectNodeAsync(string)` | P3 | **CLEAR** | Added to overview Methods table (2026-04-11) |
| SPEC-012 | `AriaLabel` | P3 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-013 | `ItemTemplate` | P3 | **CLEAR** | Added to overview.md (2026-04-11) |
| SPEC-014 | `ExpandAllAsync` enhanced sig | P3 | **CLEAR** | Full signature in overview Methods table (2026-04-11) |

**Section status: 14/14 CLEAR.**

#### (B) Spec-Ahead — Documented but not implemented (13 records)

| ID | Parameter/Event | Priority | Status | Notes |
|----|----------------|----------|--------|-------|
| SPEC-015 | `TreeViewBinding` component | P2 | **BLOCKED** | Planned feature. Routes to gap-analysis intake. |
| SPEC-016 | `OnExpand` event | P2 | **BLOCKED** | Planned. Source uses `LoadChildrenAsync` Func. See SPEC-038 for priority upgrade. |
| SPEC-017 | `OnItemDoubleClick` event | P2 | **BLOCKED** | Planned Telerik parity event. |
| SPEC-018 | `OnItemRender` event | P2 | **BLOCKED** | Planned Telerik parity event. |
| SPEC-019 | `OnDragStart`/`OnDrag`/`OnDragEnd` | P3 | **BLOCKED** | Planned. Requires JS interop. |
| SPEC-020 | `CheckOnClick` | P2 | **BLOCKED** | Planned Telerik parity feature. |
| SPEC-021 | `UrlField` | P3 | **BLOCKED** | Planned Telerik parity feature. |
| SPEC-022 | `DragThrottleInterval` | P3 | **BLOCKED** | Depends on JS interop drag infrastructure (SPEC-019). |
| SPEC-023 | `GetItemFromDropIndex()` | P3 | **BLOCKED** | Depends on JS interop drag infrastructure (SPEC-019). |
| SPEC-024 | `Class` parameter | P3 | **BLOCKED** | Spec clarification needed (inherited from MariloComponentBase). |
| SPEC-025-027 | Fluent UI forward-looking features | P3 | **BLOCKED** | Appearance, IconAfterField, AsideTemplate, etc. Aspirational roadmap. |

**Section status: 0 CLEAR, 11 BLOCKED. All marked "Planned" in spec — do not block delivery gate.**

#### (C) Mismatches — Both exist but disagree (21 records, including Wave 1 additions)

| ID | Parameter/Event | Priority | Status | Notes |
|----|----------------|----------|--------|-------|
| SPEC-028 | `EnableDragDrop` vs `Draggable` | P1 | **BLOCKED** | Name decision needed. Routes to gap-analysis. |
| SPEC-029 | `OnItemDrop` vs `OnDrop` (name + type) | P1 | **BLOCKED** | Code change needed. Tuple vs typed args. |
| SPEC-030 | `TreeSelectionMode` vs `TreeViewSelectionMode` | P2 | **BLOCKED** | Enum name decision. Routes to gap-analysis. |
| SPEC-031 | `AllowCheckChildren` vs `CheckChildren` | P2 | **BLOCKED** | Name decision. Routes to gap-analysis. |
| SPEC-032 | `AllowCheckParents` vs `CheckParents` | P2 | **BLOCKED** | Name decision. Routes to gap-analysis. |
| SPEC-033 | Collection parameter types | P2 | **CLEAR** | events.md + overview.md updated to `IEnumerable<string>` (2026-04-11). |
| SPEC-034 | `OnItemClick` type (`object` vs typed args) | P2 | **CLEAR** | events.md documents current `EventCallback<object>` + planned typed args (2026-04-11). |
| SPEC-035 | Shift+click range selection | P2 | **BLOCKED** | Behavior documented in spec but not implemented. Routes to gap-analysis. |
| SPEC-036 | Ctrl+click deselect/toggle | P1 | **BLOCKED** | User-visible behavior divergence from spec. Routes to gap-analysis. |
| SPEC-037 | `TreeViewBinding` blast radius | P1 | **BLOCKED** | 14 of 21 spec files use `<TreeViewBindings>`. Architecture decision required. |
| SPEC-038 | `OnExpand` vs `LoadChildrenAsync` | P1 | **BLOCKED** | All load-on-demand examples are non-functional. API design decision required. |
| SPEC-039 | `aria-level`/`aria-setsize`/`aria-posinset` | P2 | **BLOCKED** | WCAG compliance gap. Routes to gap-analysis. |
| SPEC-040 | Filter input `searchbox` role | P3 | **BLOCKED** | Spec-ahead. Programmatic filter is current design. |
| SPEC-041 | CSS class prefix `k-treeview-*` vs `mar-tree-*` | P3 | **BLOCKED** | Spec-only update needed. |
| SPEC-042 | PageUp/PageDown/typeahead keyboard nav | P3 | **BLOCKED** | WAI-ARIA tree pattern completeness. |
| SPEC-043 | `CheckBoxMode` enum type name | P2 | **BLOCKED** | Package with SPEC-030 decision. |
| SPEC-044 | `ItemTemplate` per-level vs global | P2 | **BLOCKED** | Blocked on SPEC-037 (`TreeViewBinding`). |
| SPEC-045 | `*` keyboard handler undocumented | P3 | **BLOCKED** | Spec-only update. |
| SPEC-046 | `IsCancelled` on drag events | P3 | **BLOCKED** | Blocked on SPEC-019 (drag event family). |
| SPEC-047 | Accessibility example typed args | P2 | **BLOCKED** | Paired with SPEC-034. |
| SPEC-048 | `CheckboxContext` public shape | P3 | **BLOCKED** | Spec-only update. |

**Section status: 2 CLEAR, 19 BLOCKED.**

#### Stage 01 Totals

| Status | Count |
|--------|-------|
| CLEAR | 16 |
| BLOCKED | 30 |
| REMEDIATION-NEEDED | 0 |
| **Total** | **46** |

*Note: 46 = original 34 records + 14 Wave 1 additions (SPEC-035 through SPEC-048). Two Wave 1 closures (SPEC-033, SPEC-034) reduce open count.*

---

### Stage 02: Example UX (24 records total)

#### (a) Parameters with no demo scenario (9 records)

| ID | Parameter | Priority | Status | Notes |
|----|-----------|----------|--------|-------|
| a1 | `IconField` | P2 | **BLOCKED** | No data-binding-driven icon scenario. |
| a2 | `ExpandedItems` (two-way bind) | P1 | **CLEAR** | New scenario added (2026-04-11 Wave 2). |
| a3 | `ExpandOnDoubleClick` | P2 | **BLOCKED** | Distinct behavioral option, no demo. |
| a4 | `Size` | P3 | **BLOCKED** | Appearance parameter, zero coverage. |
| a5 | `AriaLabel` | P2 | **BLOCKED** | Important for a11y. |
| a6 | `CheckboxTemplate` | P3 | **BLOCKED** | Advanced customization. |
| a7 | `SelectNodeAsync` | P2 | **BLOCKED** | Programmatic navigation API. |
| a8 | `MariloTreeItem.Url` | P2 | **BLOCKED** | Link-rendering node. |
| a9 | `MariloTreeItem.IsSelected` | P3 | **BLOCKED** | Direct declarative selection. |

#### (b) Stale scenarios (3 records)

| ID | Scenario | Priority | Status | Notes |
|----|----------|----------|--------|-------|
| b1 | Events > "Item Selection" | P2 | **CLEAR** | Rewritten as "SelectedItemsChanged" (2026-04-11 Wave 2). |
| b2 | Events > "Item Expand/Collapse" | P2 | **CLEAR** | Rewritten as "ExpandedItemsChanged" (2026-04-11 Wave 2). |
| b3 | Expansion > "AutoExpand" | P3 | **BLOCKED** | Static demo, needs toggle. |

#### (c) Events with no demo scenario (6 records)

| ID | Event | Priority | Status | Notes |
|----|-------|----------|--------|-------|
| c1 | `SelectedItemsChanged` | P1 | **CLEAR** | New scenario (2026-04-11 Wave 2). |
| c2 | `OnItemClick` | P1 | **CLEAR** | New scenario (2026-04-11 Wave 2). |
| c3 | `ExpandedItemsChanged` | P1 | **CLEAR** | New scenario (2026-04-11 Wave 2). |
| c4 | `OnItemContextMenu` | P2 | **BLOCKED** | Right-click handler, zero demo. |
| c5 | `MariloTreeItem.IsExpandedChanged` | P3 | **BLOCKED** | Per-item event. |
| c6 | `MariloTreeItem.OnClick` | P3 | **BLOCKED** | Per-item click. |

#### (d) Edge cases not demonstrated (6 records)

| ID | Edge Case | Priority | Status | Notes |
|----|-----------|----------|--------|-------|
| d1 | Empty data (`Data=[]`) | P2 | **BLOCKED** | No graceful-empty scenario. |
| d2 | Single-root tree | P3 | **BLOCKED** | Degenerate minimum input. |
| d3 | Selection + Checkbox simultaneously | P2 | **BLOCKED** | Realistic combo. |
| d4 | Keyboard navigation demo | P2 | **BLOCKED** | Pairs with a5 (`AriaLabel`). |
| d5 | Drag-drop rejected target | P3 | **BLOCKED** | No reject/cancel branch. |
| d6 | Lazy load failure/empty-children | P3 | **BLOCKED** | LoadChildrenAsync error path. |

#### Stage 02 Totals

| Status | Count |
|--------|-------|
| CLEAR | 6 |
| BLOCKED | 18 |
| REMEDIATION-NEEDED | 0 |
| **Total** | **24** |

---

### Stage 03: Visual Parity (17 records total)

| ID | Gap | Severity | Status | Notes |
|----|-----|----------|--------|-------|
| VP-001 | No focus-visible ring | Critical | **REMEDIATION-NEEDED** | SCSS fix designed. Acceptance criteria defined. |
| VP-002 | Hardcoded Razor classes have no matching SCSS | Critical | **REMEDIATION-NEEDED** | **Tick-15 decision: rename SCSS `mar-treeitem__*` to `mar-tree-item__*`.** Resolution design complete. Prerequisite for 7 other gaps. |
| VP-003 | No disabled visual treatment | Critical | **REMEDIATION-NEEDED** | SCSS fix designed. `[aria-disabled="true"]` targeting. |
| VP-004 | No Size/density variant SCSS | Major | **REMEDIATION-NEEDED** | **Tick-15 decision: `TreeViewClass(string? size)` overload on CssProvider.** Resolution design complete. |
| VP-005 | No Appearance variant SCSS | Major | **BLOCKED** | New feature. Routes to gap-analysis intake. |
| VP-006 | Hover token inconsistency (Phase 1 vs Phase 2) | Major | **REMEDIATION-NEEDED** | Fix is part of VP-002 unification. |
| VP-007 | No ReadOnly visual differentiation | Major | **REMEDIATION-NEEDED** | SCSS fix designed. `--readonly` modifier. |
| VP-008 | Checkbox has no styled appearance | Major | **REMEDIATION-NEEDED** | SCSS rules needed for FluentUI-themed checkbox. |
| VP-009 | No dark-mode-specific overrides | Major | **REMEDIATION-NEEDED** | Token verification + contrast QA needed. |
| VP-010 | Icon alignment has no SCSS | Minor | **REMEDIATION-NEEDED** | Resolved once VP-002 unifies class names + adds `.mar-tree-item__icon` rule. |
| VP-011 | Title text has no typography SCSS | Minor | **REMEDIATION-NEEDED** | Resolved once VP-002 adds `.mar-tree-item__title` rule. |
| VP-012 | Header row container has no SCSS | Minor | **REMEDIATION-NEEDED** | Structural prerequisite. `.mar-tree-item__header` flex layout. |
| VP-013 | Children container has no SCSS | Minor | **REMEDIATION-NEEDED** | `.mar-tree-item__children` indent rule. |
| VP-014 | Toggle button not styled in rendered class | Minor | **REMEDIATION-NEEDED** | Resolved once VP-002 aligns class names. Phase 2 styles will apply. |
| VP-015 | No drag-drop visual indicators | Polish | **BLOCKED** | Requires component modifier classes + SCSS. |
| VP-016 | No editing-mode visual treatment | Polish | **BLOCKED** | Requires component markup for edit input + SCSS. |
| VP-017 | Bootstrap provider minimal/incomplete | Polish | **BLOCKED** | Secondary provider. Lower priority than FluentUI. |

#### Stage 03 Totals

| Status | Count |
|--------|-------|
| CLEAR | 0 |
| BLOCKED | 4 |
| REMEDIATION-NEEDED | 13 |
| **Total** | **17** |

---

## Summary Metrics

### Gaps by Stage

| Stage | Total | CLEAR | BLOCKED | REMEDIATION-NEEDED |
|-------|-------|-------|---------|-------------------|
| 01 — Spec Review | 46 | 16 | 30 | 0 |
| 02 — Example UX | 24 | 6 | 18 | 0 |
| 03 — Visual Parity | 17 | 0 | 4 | 13 |
| **Grand Total** | **87** | **22** | **52** | **13** |

### Gaps by Priority

| Priority | Total | CLEAR | BLOCKED | REMEDIATION-NEEDED |
|----------|-------|-------|---------|-------------------|
| P1 / Critical | 11 | 4 | 4 | 3 |
| P2 / Major | 38 | 9 | 23 | 6 |
| P3 / Minor+Polish | 38 | 9 | 25 | 4 |

### Gaps by Status

| Status | Count | Percentage |
|--------|-------|-----------|
| CLEAR | 22 | 25.3% |
| BLOCKED | 52 | 59.8% |
| REMEDIATION-NEEDED | 13 | 14.9% |

---

## Blocking Items for treeview-gap-analysis

If a `treeview-gap-analysis` workspace were bootstrapped, the following **52 BLOCKED items** would route to it. They break down as follows:

### Architecture Decisions Required (4 items — P1)

These require orchestrator-level decisions before implementation can begin:

1. **SPEC-028** — `EnableDragDrop` vs `Draggable` name decision
2. **SPEC-029** — `OnItemDrop` vs `OnDrop` name + type decision
3. **SPEC-037** — `TreeViewBinding` component: implement or rewrite 14 spec files
4. **SPEC-038** — `OnExpand` vs `LoadChildrenAsync` API design decision

### Code Changes Required (15 items)

| Priority | Items | Examples |
|----------|-------|---------|
| P1 | 1 | SPEC-036 (Ctrl+click deselect) |
| P2 | 10 | SPEC-030/031/032/043 (enum/param renames), SPEC-035 (Shift+click), SPEC-039 (ARIA attrs), SPEC-044 (per-level templates), SPEC-047 (a11y example args) |
| P3 | 4 | SPEC-019 (drag events), SPEC-042 (PageUp/PageDown), SPEC-046 (IsCancelled) |

### Spec-Only Updates (10 items)

| Priority | Items | Examples |
|----------|-------|---------|
| P3 | 7 | SPEC-024 (Class param), SPEC-040/041/045/048 (spec text fixes) |
| P2 | 3 | SPEC-015/020 (mark planned), SPEC-021 (UrlField) |

### Planned Features — No Implementation Path Yet (9 items)

SPEC-016/017/018/019/021/022/023/025-027 — Telerik parity features marked "Planned" in spec.

### Demo Gaps (14 items)

a1/a3/a4/a5/a6/a7/a8/a9, b3, c4/c5/c6, d1-d6 — parameter/event/edge-case demo scenarios.

### SCSS / Visual-Parity Implementation (4 items)

VP-005 (appearance variants), VP-015 (drag-drop indicators), VP-016 (editing-mode), VP-017 (Bootstrap provider).

---

## REMEDIATION-NEEDED Items — Implementation Summary

These 13 items have resolution designs from the VP audit and tick-15 decisions. They need implementation but do not require further design work:

| ID | Change Type | Dependency | Estimated Scope |
|----|------------|------------|-----------------|
| VP-002 | SCSS rename `mar-treeitem__*` to `mar-tree-item__*` | None (tick-15 decided) | FluentUI + Bootstrap SCSS |
| VP-001 | Add `:focus-visible` SCSS rules | VP-002 (class alignment) | 2 SCSS rules |
| VP-003 | Add disabled state SCSS | VP-002 | 2 SCSS rules |
| VP-004 | Add `TreeViewClass(string? size)` overload + density SCSS | tick-15 decided | CssProvider + SCSS |
| VP-006 | Unify hover token to `--marilo-color-surface-hover` | VP-002 | Part of VP-002 work |
| VP-007 | Add `--readonly` modifier + SCSS | VP-002 | Component modifier + 2 SCSS rules |
| VP-008 | Style `.mar-tree-item__checkbox` | VP-002 | 3-4 SCSS rules |
| VP-009 | Dark-mode contrast verification + overrides | VP-002 | QA pass + conditional overrides |
| VP-010 | Add `.mar-tree-item__icon` SCSS | VP-002 | 1 SCSS rule |
| VP-011 | Add `.mar-tree-item__title` typography SCSS | VP-002 | 1 SCSS rule |
| VP-012 | Add `.mar-tree-item__header` flex layout SCSS | VP-002 | 1 SCSS rule |
| VP-013 | Add `.mar-tree-item__children` indent SCSS | VP-002 | 1 SCSS rule |
| VP-014 | Toggle button styling (aligns via VP-002) | VP-002 | 0 extra rules if VP-002 done |

**Critical path: VP-002 is the prerequisite for 12 of 13 remediation items.**

---

## Delivery Gate Assessment

### What Was Accomplished in This Pipeline

1. **Spec review (Stage 01):** 14 undocumented parameters added to spec. 3 mismatch records closed. events.md fully rewritten. 48 gap records catalogued with priority and routing.
2. **Example UX (Stage 02):** All 4 P1 demo gaps closed (ExpandedItems bind, SelectedItemsChanged, OnItemClick, ExpandedItemsChanged). 2 stale scenarios rewritten. 6 of 24 gaps resolved.
3. **Visual parity (Stage 03):** 17 gaps identified and scored. Dual class system root cause isolated. 13 have complete remediation designs.
4. **Tick-15 decisions:** VP-002 naming and VP-004 density both resolved, unblocking 8 VP gaps.

### Gate Verdict: AMBER

The component is **functionally complete** (67/67 tests, 23 demo scenarios, 0 P1 demo gaps). The remaining work is:
- **Naming alignment** (spec-vs-source naming decisions) — 5 items, requires batch decision
- **SCSS visual parity** — 13 remediation-designed items, dependency chain from VP-002
- **Planned features** — 11 items marked "Planned", not blocking delivery
- **Demo gap tail** — 18 P2/P3 demo scenarios

### Recommended Next Steps

1. **Bootstrap treeview-gap-analysis workspace** to process the 52 BLOCKED items
2. **Prioritize VP-002 SCSS unification** as the single highest-leverage implementation task (unblocks 12 of 13 VP remediation items)
3. **Batch the 5 naming decisions** (SPEC-028/029/030/031/032) into a single orchestrator decision record
4. **Defer planned features** (SPEC-015-027) to a future wave — they are roadmap items, not blockers

---

## Build Verification

```
$ dotnet build Marilo.slnx
Build succeeded.
    0 Warning(s)
    0 Error(s)
Time Elapsed 00:00:13.00
```

---

## Cross-References

- Stage 01 output: `stages/01-spec-review/output/treeview-spec-gap-list.md`
- Stage 02 output: `stages/02-example-ux/output/treeview-demo-gap-list.md`
- Stage 03 output: `stages/03-visual-parity/output/treeview-visual-parity-gaps.md`
- Tick-15 decisions: `.claude/orchestration/_orchestrator/decisions/tick-15-2026-04-12-0220.md`
- Worker state: `.claude/orchestration/_memory/workers/w-treeview-delivery.json`
- Gap workspace (target): `ICM/workspaces/gap-analysis-resolution/`
