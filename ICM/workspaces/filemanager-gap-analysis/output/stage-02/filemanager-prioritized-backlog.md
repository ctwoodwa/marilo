# MariloFileManager — Stage 02 Prioritized Backlog

**Date:** 2026-04-09
**Source:** `filemanager-delivery/stages/01-spec-review/output/filemanager-spec-gap-list.md`
**Total gaps:** 36 (5 P1, 26 P2, 5 P3)
**Scope:** systematic → full pipeline (01 > 02 > 03 > 04 > 05 > 06)
**Architecture:** Full rewrite (170-line scaffold → generic `MariloFileManager<TItem>`)

---

## Phase Summary

| Phase | Focus | Gaps | Dependencies | Effort |
|-------|-------|:----:|-------------|--------|
| **A — Foundation** | Generic TItem, field params, naming, OnRead, partial files | 8 | None | Large (architectural rewrite) |
| **B — Events & Data** | Typed event args, selection, view binding, OnModelInit, Rebind | 10 | Phase A | Medium |
| **C — Toolbar & Navigation** | FileManagerToolBar, breadcrumb, search, toolbar tools | 4 | Phase A | Medium |
| **D — Context Menu & Editing** | Context menu, inline rename, delete confirmation, download | 5 | Phase B events |  Medium |
| **E — Upload & Preview** | FileManagerSettings, upload integration, preview pane | 3 | Phase C toolbar | Medium |
| **F — Polish** | Sort UI, Width/Class, EnableLoaderContainer, ARIA | 5 | Phases C-E | Small |
| **Spec-only** | Document undocumented source params (no code change) | 1 | None | Trivial |

---

## Phase A — Foundation (8 gaps)

**Goal:** Rewrite to generic `MariloFileManager<TItem>` with field-binding strings, rename parameters to match spec, implement OnRead, establish partial file architecture.

**Prerequisite:** None. This phase unblocks everything.

| # | Gap ID | Description | Type | Action |
|---|--------|-------------|------|--------|
| 1 | SPEC-FM-001/005 | Make component generic `<TItem>`, rename `Items` → `Data` | undocumented + spec-ahead | Rewrite component signature |
| 2 | SPEC-FM-010 | Implement 14 field-binding string parameters (`NameField`, `SizeField`, etc.) | spec-ahead | Add string params + reflection resolver |
| 3 | SPEC-FM-034 | Rename `CurrentPath` → `Path`, `CurrentPathChanged` → `PathChanged` | mismatch | Rename params |
| 4 | SPEC-FM-035 | Rename `ViewMode` → `View`, `FileManagerViewMode` → `FileManagerViewType` | mismatch | Rename param + enum |
| 5 | SPEC-FM-011 | Implement `OnRead` with `FileManagerReadEventArgs` | spec-ahead | New event + dual-mode data loading |
| 6 | SPEC-FM-007 | Add `Height` parameter | spec-ahead | Wire to inline style |
| 7 | — | Extract to partial files (.razor + .razor.cs) | architectural | Follow MariloDataGrid pattern |
| 8 | — | Create `FileManagerModels.cs` with all EventArgs types | architectural | New file in Forms/Inputs/ |

**Output:** Generic `MariloFileManager<TItem>` with field-binding, spec-aligned naming, OnRead, partial files. Folder tree and grid/list views preserved but using field resolution instead of hardcoded `FileManagerEntry` properties.

---

## Phase B — Events & Data (10 gaps)

**Goal:** Implement all spec-required events with typed EventArgs, selection model, view two-way binding.

**Prerequisite:** Phase A (generic TItem, field-binding)

| # | Gap ID | Description | Type | Action |
|---|--------|-------------|------|--------|
| 1 | SPEC-FM-033 | Rename `OnCreateFolder` → `OnCreate`, change arg to `FileManagerCreateEventArgs` | mismatch | Rename + retype |
| 2 | SPEC-FM-036 | Change `OnDelete` arg from `FileManagerEntry` to `FileManagerDeleteEventArgs` | mismatch | Retype arg |
| 3 | SPEC-FM-013 | Implement `OnEdit` (start rename) with `FileManagerEditEventArgs` | spec-ahead | New event |
| 4 | SPEC-FM-014 | Implement `OnUpdate` (complete rename) with `FileManagerUpdateEventArgs` | spec-ahead | New event |
| 5 | SPEC-FM-015 | Implement `OnDownload` with `FileManagerDownloadEventArgs` (cancellable) | spec-ahead | New event |
| 6 | SPEC-FM-016 | Implement `OnModelInit` callback (`Func<TItem>`) | spec-ahead | New callback |
| 7 | SPEC-FM-017 | Replace private `SelectedItemPath` with public `SelectedItems` (`IEnumerable<TItem>`) | spec-ahead | Param + state rewrite |
| 8 | SPEC-FM-018 | Implement `SelectedItemsChanged` | spec-ahead | EventCallback for two-way binding |
| 9 | SPEC-FM-019 | Implement `ViewChanged` event | spec-ahead | EventCallback |
| 10 | SPEC-FM-021 | Implement `Rebind()` public method | spec-ahead | Triggers OnRead or re-render |

**Output:** Complete event model matching spec; selection two-way binding; all EventArgs types defined.

---

## Phase C — Toolbar & Navigation (4 gaps)

**Goal:** Replace hardcoded toolbar with composable `<FileManagerToolBar>` child component; replace path span with breadcrumb; add search.

**Prerequisite:** Phase A (generic component)

| # | Gap ID | Description | Type | Action |
|---|--------|-------------|------|--------|
| 1 | SPEC-FM-022 | Implement `<FileManagerToolBar>` child component pattern | spec-ahead | New component with CascadingValue |
| 2 | SPEC-FM-023 | Implement 7 built-in toolbar tools | spec-ahead | 7 tool tag components |
| 3 | SPEC-FM-024 | Replace plain path span with embedded `MariloBreadcrumb` | spec-ahead | Integrate existing component |
| 4 | SPEC-FM-030 | Implement search textbox filtering current folder items | spec-ahead | Filter logic + search tool |

**Output:** Configurable toolbar with all 7 spec tools, breadcrumb navigation, search filter.

---

## Phase D — Context Menu & Editing (5 gaps)

**Goal:** Implement right-click context menu with Rename/Download/Delete, inline rename UI, delete confirmation.

**Prerequisite:** Phase B events (OnEdit, OnUpdate, OnDownload, OnDelete)

| # | Gap ID | Description | Type | Action |
|---|--------|-------------|------|--------|
| 1 | SPEC-FM-025 | Right-click context menu with Rename/Download/Delete | spec-ahead | Integrate `MariloContextMenu` |
| 2 | SPEC-FM-026 | Inline rename UI (input overlay on item name) | spec-ahead | Edit state management |
| 3 | SPEC-FM-027 | Delete confirmation dialog | spec-ahead | Integrate `MariloDialog` |
| 4 | SPEC-FM-012 | Wire `OnCreate` to toolbar New Folder (already renamed in Phase B) | spec-ahead | Connect to toolbar |
| 5 | — | Honor `AllowDelete` in markup (declared but unused) | bug fix | Guard delete UI |

**Output:** Full context menu with 3 commands, inline rename flow, delete confirmation.

---

## Phase E — Upload & Preview (3 gaps)

**Goal:** Integrate upload via `<FileManagerSettings>` child pattern; implement preview pane.

**Prerequisite:** Phase C toolbar (upload tool button)

| # | Gap ID | Description | Type | Action |
|---|--------|-------------|------|--------|
| 1 | SPEC-FM-028 | Implement `<FileManagerSettings>` + `<FileManagerUploadSettings>` | spec-ahead | Child components, dialog with embedded MariloUpload |
| 2 | SPEC-FM-029 | Implement preview pane (right panel with file details) | spec-ahead | Conditional right pane, toggled by toolbar |
| 3 | — | Wire toolbar ViewDetails tool to preview pane toggle | integration | Connect toolbar to preview |

**Output:** Upload integration via toolbar tool + dialog; preview pane with file details.

---

## Phase F — Polish (5 gaps)

**Goal:** Sort UI, remaining appearance params, loader, ARIA.

**Prerequisite:** Phases C-E

| # | Gap ID | Description | Type | Action |
|---|--------|-------------|------|--------|
| 1 | SPEC-FM-031 | Sort toolbar tools (sort-by + direction) | spec-ahead | 2 toolbar tools + sort logic |
| 2 | SPEC-FM-008 | `Width` parameter | spec-ahead | Wire to inline style |
| 3 | SPEC-FM-006 | `EnableLoaderContainer` (loading overlay) | spec-ahead | Overlay during async loads |
| 4 | SPEC-FM-032 | ARIA roles and keyboard nav audit | spec-ahead | Post-integration audit |
| 5 | SPEC-FM-009 | Verify `Class` from base class | spec-ahead | Likely already inherited |

**Output:** Complete spec parity.

---

## Spec-Only Updates (no code change)

| # | Gap ID | Description | Action |
|---|--------|-------------|--------|
| 1 | SPEC-FM-002 | Document `ShowFolderTree` in overview spec | Add to spec |
| 2 | SPEC-FM-003 | Document `AllowCreate` in overview spec | Add to spec |
| 3 | SPEC-FM-004 | Document `OnOpen` event in events spec | Add to spec |

---

## Implementation Strategy

This is a **full rewrite** comparable to MariloGantt (95-line scaffold → 20 gaps resolved, 31 tests, 24 commits). The recommended approach:

1. **Phase A as one batch** — foundation rewrite is indivisible (generic + fields + naming + OnRead). Use a git worktree or feature branch.
2. **Phases B + C in parallel** — events and toolbar are independent once Phase A is done.
3. **Phase D depends on B** — context menu needs events.
4. **Phase E depends on C** — upload needs toolbar.
5. **Phase F last** — polish after all UI surfaces exist.

Estimated total: ~20-25 commits, ~35-45 bUnit tests, 5+ demo pages.

---

## Dependency Graph

```
Phase A (Foundation)
  ├──> Phase B (Events & Data)
  │       └──> Phase D (Context Menu & Editing)
  └──> Phase C (Toolbar & Navigation)
          └──> Phase E (Upload & Preview)
                └──> Phase F (Polish)
```
