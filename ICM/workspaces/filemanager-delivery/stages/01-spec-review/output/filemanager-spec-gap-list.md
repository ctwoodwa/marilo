# MariloFileManager — Stage 01 Spec Review: Gap List (Refreshed)

**Audit date:** 2026-04-11
**Previous audit:** 2026-04-09 (36 gaps; all 36 resolved by filemanager-gap-analysis Phases A–F, closure reports dated 2026-04-09)
**Source file:** `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor.cs`
**Source parameter count:** 40 `[Parameter]` properties + `Rebind()` public method
**Spec directory:** `docs/component-specs/filemanager/`
**Total open gaps:** 8 undocumented + 4 spec-ahead + 3 mismatch = 15
(12 actionable in this workspace; 3 flagged for coordinator escalation)

| Gap type | Count |
|----------|-------|
| Undocumented (in source, not in spec) | 8 |
| Spec-ahead (in spec, not in source) | 4 |
| Mismatch (both exist, differ in name/type/behavior) | 3 |

---

## Source Inventory (current, 2026-04-11)

### Parameters on `MariloFileManager<TItem>`

| # | Name | Type | Default |
|---|------|------|---------|
| 1 | `Data` | `IEnumerable<TItem>` | `Enumerable.Empty<TItem>()` |
| 2 | `OnRead` | `EventCallback<FileManagerReadEventArgs>` | — |
| 3 | `IdField` | `string` | `"Id"` |
| 4 | `ParentIdField` | `string` | `"ParentId"` |
| 5 | `NameField` | `string` | `"Name"` |
| 6 | `PathField` | `string` | `"Path"` |
| 7 | `ExtensionField` | `string` | `"Extension"` |
| 8 | `IsDirectoryField` | `string` | `"IsDirectory"` |
| 9 | `HasDirectoriesField` | `string` | `"HasDirectories"` |
| 10 | `SizeField` | `string` | `"Size"` |
| 11 | `DateCreatedField` | `string` | `"DateCreated"` |
| 12 | `DateCreatedUtcField` | `string` | `"DateCreatedUtc"` |
| 13 | `DateModifiedField` | `string` | `"DateModified"` |
| 14 | `DateModifiedUtcField` | `string` | `"DateModifiedUtc"` |
| 15 | `DirectoriesField` | `string` | `"Directories"` |
| 16 | `ItemsField` | `string` | `"Items"` |
| 17 | `Path` | `string` | `"/"` |
| 18 | `PathChanged` | `EventCallback<string>` | — |
| 19 | `View` | `FileManagerViewType` | `FileManagerViewType.ListView` |
| 20 | `ViewChanged` | `EventCallback<FileManagerViewType>` | — |
| 21 | `ToolBarTemplate` | `RenderFragment?` | `null` |
| 22 | `Height` | `string?` | `null` |
| 23 | `Width` | `string?` | `null` |
| 24 | `EnableLoaderContainer` | `bool` | `false` |
| 25 | `ShowFolderTree` | `bool` | `true` |
| 26 | `FileManagerSettings` | `RenderFragment?` | `null` |
| 27 | `UploadSettings` | `FileManagerUploadSettings?` | `null` |
| 28 | `ShowPreviewPane` | `bool` | `false` |
| 29 | `AllowCreate` | `bool` | `false` |
| 30 | `AllowDelete` | `bool` | `false` |
| 31 | `AllowRename` | `bool` | `false` |
| 32 | `SelectedItems` | `IEnumerable<TItem>` | `Enumerable.Empty<TItem>()` |
| 33 | `SelectedItemsChanged` | `EventCallback<IEnumerable<TItem>>` | — |
| 34 | `OnSelect` | `EventCallback<TItem>` | — |
| 35 | `OnOpen` | `EventCallback<TItem>` | — |
| 36 | `OnCreate` | `EventCallback<FileManagerCreateEventArgs<TItem>>` | — |
| 37 | `OnDelete` | `EventCallback<FileManagerDeleteEventArgs<TItem>>` | — |
| 38 | `OnEdit` | `EventCallback<FileManagerEditEventArgs<TItem>>` | — |
| 39 | `OnUpdate` | `EventCallback<FileManagerUpdateEventArgs<TItem>>` | — |
| 40 | `OnDownload` | `EventCallback<FileManagerDownloadEventArgs<TItem>>` | — |
| 41 | `OnModelInit` | `Func<TItem>?` | `null` |

Inherited from `MariloComponentBase`:
- `Class` (`string?`, default `null`)

Public methods:
- `Rebind()` → `Task` — documented in `overview.md#reference-and-methods`

---

## Gap Records

### A. Undocumented (in source, missing from spec)

---

**ID:** SPEC-filemanager-101
**Type:** undocumented
**Parameter/Event:** `ShowFolderTree`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `ShowFolderTree` |
| Type | missing | `bool` |
| Default | missing | `true` |
| Description | missing | Controls visibility of the left folder-tree (TreeView) sidebar |

**Recommended action:** Add to `overview.md` parameters table; cross-reference `navigation.md`.
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-102
**Type:** undocumented
**Parameter/Event:** `ShowPreviewPane`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `ShowPreviewPane` |
| Type | missing | `bool` |
| Default | missing | `false` |
| Description | missing | Gates visibility of the preview-pane toggle button and the preview pane itself |

**Note:** `preview-pane.md` describes the feature but does not mention a parameter to opt-in. Without it the feature is invisible.

**Recommended action:** Add to `overview.md` and `preview-pane.md`; note default `false`.
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-103
**Type:** undocumented
**Parameter/Event:** `AllowCreate`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `AllowCreate` |
| Type | missing | `bool` |
| Default | missing | `false` |
| Description | missing | Shows/hides the New Folder button in the default toolbar; gates `OnCreate` |

**Recommended action:** Add to `overview.md` and `toolbar.md`.
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-104
**Type:** undocumented
**Parameter/Event:** `AllowDelete`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `AllowDelete` |
| Type | missing | `bool` |
| Default | missing | `false` |
| Description | missing | Gates the Delete context-menu command and the delete-confirmation dialog |

**Recommended action:** Add to `overview.md` and `context-menu.md` (near Delete).
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-105
**Type:** undocumented
**Parameter/Event:** `AllowRename`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `AllowRename` |
| Type | missing | `bool` |
| Default | missing | `false` |
| Description | missing | Gates the Rename context-menu command and the inline-rename input |

**Recommended action:** Add to `overview.md` and `context-menu.md` (near Rename).
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-106
**Type:** undocumented
**Parameter/Event:** `OnSelect`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing (spec uses only `SelectedItemsChanged`) | `OnSelect` |
| Type | missing | `EventCallback<TItem>` |
| Default | — | — |
| Description | missing | Fires on single-click of an item; passes the clicked item directly. Distinct from `SelectedItemsChanged` (which emits the full selection set) |

**Note:** Both events exist and serve different purposes. Spec should describe both.

**Recommended action:** Add an `OnSelect` section to `events.md` and clarify the relationship with `SelectedItemsChanged`.
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-107
**Type:** undocumented
**Parameter/Event:** `OnOpen`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `OnOpen` |
| Type | missing | `EventCallback<TItem>` |
| Default | — | — |
| Description | missing | Fires on double-click of a file (non-directory). Folder double-click navigates internally without firing this event |

**Recommended action:** Add an `OnOpen` section to `events.md`; clarify relationship to `OnDownload`.
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-108
**Type:** undocumented
**Parameter/Event:** `ToolBarTemplate`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing (spec describes `<FileManagerToolBar>` child-tag component pattern) | `ToolBarTemplate` |
| Type | missing | `RenderFragment?` |
| Default | missing | `null` |
| Description | missing | When non-null, replaces the default toolbar content. Receives no cascading context (consumers must bind back to the FileManager via `@ref`) |

**Note:** This is both an undocumented parameter AND the source's chosen approach for toolbar customization, which diverges from the spec's `<FileManagerToolBar>` child-tag pattern. See mismatch SPEC-filemanager-203.

**Recommended action:** Document `ToolBarTemplate` in `toolbar.md` as the supported extensibility surface; keep `<FileManagerToolBar>` listed as a roadmap item.
**Delegated to:** spec update only (in-scope); architectural divergence flagged under SPEC-filemanager-203.

---

### B. Spec-Ahead (documented but not implemented in source)

---

**ID:** SPEC-filemanager-201
**Type:** spec-ahead
**Parameter/Event:** `<FileManagerToolBar>` composite child-tag + 7 built-in tool tags
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `FileManagerToolBar`, `FileManagerToolBarNewFolderTool`, `FileManagerToolBarUploadTool`, `FileManagerToolBarSortDirectionTool`, `FileManagerToolBarSortTool`, `FileManagerToolBarFileViewTool`, `FileManagerToolBarViewDetailsTool`, `FileManagerToolBarSearchTool` | missing |
| Type | child component tags | missing (source uses `ToolBarTemplate` `RenderFragment` instead) |
| Default | all shown by default | default toolbar is hard-coded in markup |
| Description | Composable toolbar tools for new folder, upload, sort direction/by, view toggle, preview-pane toggle, search | source exposes a single `RenderFragment` for full replacement |

**Recommended action:** Either (a) update the spec to document `ToolBarTemplate` as the canonical extensibility mechanism and mark `<FileManagerToolBar>` tags as roadmap, or (b) implement the composite child-tag pattern in a follow-up phase. Preference is (a) for the current phase.
**Delegated to:** filemanager-gap-analysis intake (if implementing) OR spec update only (if formalizing `ToolBarTemplate`). Default: spec update.

---

**ID:** SPEC-filemanager-202
**Type:** spec-ahead
**Parameter/Event:** Breadcrumb (embedded `MariloBreadcrumb` component)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MariloBreadcrumb` (embedded) | hand-rendered breadcrumb in markup via `GetBreadcrumbSegments()` |
| Type | child Marilo component | inline HTML segments |
| Default | shown by default | shown by default |
| Description | Uses `MariloBreadcrumb` for consistent styling, ARIA, and interaction | Source hand-rolls interactive breadcrumb markup with click handlers |

**Note:** Behaviorally equivalent but not using the shared component. Cross-component consistency gap.

**Recommended action:** Refactor source to render `MariloBreadcrumb` (composition). Tracked as future polish.
**Delegated to:** filemanager-gap-analysis intake (follow-up phase)

---

**ID:** SPEC-filemanager-203
**Type:** spec-ahead
**Parameter/Event:** `FileManagerUploadSettings.RemoveUrl`, `OnUpload`, `OnRemove`, `OnSuccess`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SaveUrl`, `RemoveUrl`, `Multiple`, `OnUpload`, `OnRemove`, `OnSuccess` | `SaveUrl`, `AllowedExtensions`, `MaxFileSize`, `Multiple` |
| Type | POCO / config | POCO |
| Default | — | — |
| Description | Matches `MariloUpload` API surface with event hooks | Source lacks `RemoveUrl` and all three event callbacks |

**Recommended action:** Extend `FileManagerUploadSettings` to expose `RemoveUrl` and `EventCallback<UploadEventArgs>` surface matching `MariloUpload`.
**Delegated to:** **COORDINATOR ESCALATION** — the `FileManagerUploadSettings` class lives in `src/Marilo.Core/Models/FileManagerModels.cs`, which is outside this worktree's editable scope. Must be resolved by filemanager-gap-analysis partner with Core contract update.

---

**ID:** SPEC-filemanager-204
**Type:** spec-ahead
**Parameter/Event:** `FileManagerDownloadEventArgs` settable `Stream`/`MimeType`/`FileName`
**Priority:** P1 (blocking for `OnDownload` usability)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Stream`, `MimeType`, `FileName` | same |
| Type | `Stream? { get; set; }`, `string? { get; set; }`, `string? { get; set; }` | `init`-only setters |
| Default | — | — |
| Description | Spec examples (`events.md:242`) assign `args.Stream = ...`, `args.MimeType = ...`, `args.FileName = ...` inside the handler | Source `init;` setters make post-construction assignment a compile error against the current spec example |

**Note:** Compile-breaking mismatch between spec examples and source contract.

**Recommended action:** Change `FileManagerDownloadEventArgs<TItem>` to expose `{ get; set; }` on `Stream`, `MimeType`, `FileName`.
**Delegated to:** **COORDINATOR ESCALATION** — lives in `src/Marilo.Core/Models/FileManagerModels.cs`. Out of scope for this worktree.

---

### C. Mismatch (both exist, differ in name/type/behavior)

---

**ID:** SPEC-filemanager-301
**Type:** mismatch
**Parameter/Event:** `FileManagerViewMode` enum (obsolete)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `FileManagerViewType` (spec) | `FileManagerViewType` + `FileManagerViewMode` `[Obsolete]` |
| Type | enum | enum |
| Default | `ListView` | `ListView` |
| Description | Single canonical type | Source exposes a second `[Obsolete]` enum `FileManagerViewMode` that does not appear in spec |

**Note:** Documentation consistency — the obsolete enum is still public surface that users may search for.

**Recommended action:** Add a short note to `views.md` about the legacy `FileManagerViewMode` enum and the migration path. Source action (removing the enum) is a follow-up breaking change.
**Delegated to:** spec update only (in-scope)

---

**ID:** SPEC-filemanager-302
**Type:** mismatch
**Parameter/Event:** Toolbar extensibility pattern (`<FileManagerToolBar>` vs `ToolBarTemplate`)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `<FileManagerToolBar>` child-tag component | `ToolBarTemplate` RenderFragment |
| Type | composite component with nested tool tags | single replacement `RenderFragment?` |
| Default | composable additive model | full replacement |
| Description | Spec lets users add/reorder tools; source lets users replace the entire toolbar | Behavioral delta |

**Recommended action:** Resolve via either (a) update the spec to match source (`ToolBarTemplate`) for this release, or (b) ship both. Default: (a) for this phase; track (b) as a follow-up.
**Delegated to:** spec update only if resolving as (a); filemanager-gap-analysis if resolving as (b).

---

**ID:** SPEC-filemanager-303
**Type:** mismatch
**Parameter/Event:** `FileManagerReadEventArgs.Data` vs spec example type
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Data` on `FileManagerReadEventArgs` | same |
| Type | spec example assigns `IEnumerable<TItem>` | source uses `IEnumerable<object>?` then `args.Data.OfType<TItem>()` |
| Default | — | `null` |
| Description | Spec example wants a strongly typed collection; source boxes through `object` | Type-safety delta |

**Note:** Not a compile break, but forces consumers to cast if they want compile-time safety in the handler.

**Recommended action:** Consider making `FileManagerReadEventArgs` generic (`FileManagerReadEventArgs<TItem>`). Breaking change.
**Delegated to:** **COORDINATOR ESCALATION** — Core-model change. Out of scope for this worktree.

---

## Priority Summary

| Priority | Gap IDs | Count |
|----------|---------|-------|
| P1 — blocking | 204 | 1 (escalated) |
| P2 — this phase | 101, 102, 103, 104, 105, 108, 203, 302 | 8 (6 in-scope spec edits, 2 escalated) |
| P3 — next phase | 106, 107, 201, 202, 301, 303 | 6 |

## In-Scope vs Escalated

| Status | IDs |
|--------|-----|
| **In-scope spec updates (this worktree can close)** | 101, 102, 103, 104, 105, 106, 107, 108, 201, 301, 302 (11 gaps — all require only spec-file edits under `docs/component-specs/filemanager/`) |
| **Coordinator escalations (Core model / shared contracts)** | 203, 204, 303 (3 gaps touching `src/Marilo.Core/Models/FileManagerModels.cs`) |
| **Delegated to filemanager-gap-analysis (source changes)** | 202 (breadcrumb refactor) — follow-up phase |

---

## Recommended Resolution Order

1. **Close undocumented params (spec-only):** Add 101–108 to `overview.md` parameters table and relevant feature pages. Single pass.
2. **Resolve toolbar pattern (302):** Decide `ToolBarTemplate` vs `<FileManagerToolBar>`; update `toolbar.md` accordingly.
3. **Escalate Core changes (203, 204, 303):** Hand off to filemanager-gap-analysis to touch Core models.
4. **Follow-ups (106, 107, 201, 202, 301):** Defer to next phase.

---

## Audit Checklist

| Check | Status |
|-------|--------|
| All source parameters inventoried (40 + inherited Class + Rebind method) | PASS |
| All spec parameters inventoried (40+ across all feature files) | PASS |
| Every gap record has a type classification (a/b/c) | PASS |
| Priority order justified | PASS |
| Output references spec, does not copy it | PASS |
