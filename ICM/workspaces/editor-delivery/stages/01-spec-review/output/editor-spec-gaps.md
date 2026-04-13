# MariloEditor — Stage 01 Spec Review: Gap List

**Audit date:** 2026-04-10
**Source file:** `src/Marilo.Components/Editors/MariloEditor.razor`
**Source parameter count:** 21 (+ 2 inherited from MariloComponentBase)
**Spec parameter count:** 10 (across overview.md, toolbar.md, edit-modes/overview.md, prosemirror-plugins.md)
**Total gaps:** 17

| Gap type | Count |
|----------|-------|
| Undocumented | 11 |
| Spec-ahead | 1 |
| Mismatch | 5 |

---

## Source Inventory

All `[Parameter]` properties declared on `MariloEditor.razor` (lines 128–193) plus inherited base parameters:

| # | Parameter | Type | Default | Category |
|---|-----------|------|---------|----------|
| 1 | `Value` | `string?` | `null` | Core |
| 2 | `ValueChanged` | `EventCallback<string>` | — | Event |
| 3 | `ValueExpression` | `Expression<Func<string>>?` | `null` | Validation |
| 4 | `ReadOnly` | `bool` | `false` | Core |
| 5 | `Disabled` | `bool` | `false` | Core |
| 6 | `EditMode` | `EditorEditMode` | `Edit` | Core |
| 7 | `EditModeChanged` | `EventCallback<EditorEditMode>` | — | Event |
| 8 | `Tools` | `IEnumerable<EditorTool>?` | `null` (uses DefaultTools) | Toolbar |
| 9 | `ToolbarTemplate` | `RenderFragment?` | `null` | Toolbar |
| 10 | `ChildContent` | `RenderFragment?` | `null` | Content |
| 11 | `Placeholder` | `string?` | `null` | Core |
| 12 | `Height` | `string` | `"250px"` | Layout |
| 13 | `Width` | `string?` | `null` | Layout |
| 14 | `DebounceDelay` | `int` | `100` | Behavior |
| 15 | `AriaLabelledBy` | `string?` | `null` | A11y |
| 16 | `AriaDescribedBy` | `string?` | `null` | A11y |
| 17 | `CustomTools` | `IEnumerable<EditorCustomTool>?` | `null` | Toolbar |
| 18 | `Adaptive` | `bool` | `false` | Toolbar |
| 19 | `OnChange` | `EventCallback<string>` | — | Event |
| 20 | `OnSelectionChange` | `EventCallback` | — | Event |
| 21 | `OnCommand` | `EventCallback<string>` | — | Event |
| — | `Class` | `string?` (inherited) | `null` | Base |
| — | `Style` | `string?` (inherited) | `null` | Base |

### Public Methods (source)

| Method | Signature | Spec'd? |
|--------|-----------|---------|
| `ExecuteAsync` | `Task ExecuteAsync(EditorCommandArgs args)` | Yes (overview.md) |
| `ExecuteCommandAsync` | `Task ExecuteCommandAsync(string command)` | No |
| `SetModeAsync` | `Task SetModeAsync(EditorEditMode mode)` | No |
| `GetHtmlAsync` | `Task<string> GetHtmlAsync()` | No |
| `ImportAsync` | `Task ImportAsync(string content, string format)` | No |
| `ExportAsync` | `Task<string> ExportAsync(string format)` | No |

---

## Gap Records

### A. Undocumented (in source, NOT in spec)

#### A-01: `Disabled` parameter
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 145) | No |
| Type | `bool` | — |
| Notes | Disables the editor entirely (greys out, prevents editing). Distinct from `ReadOnly`. No spec mention anywhere. |

**Priority:** P2 — consumers need to know Disabled vs ReadOnly semantics.

---

#### A-02: `EditModeChanged` event
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 149) | No |
| Type | `EventCallback<EditorEditMode>` | — |
| Notes | Two-way binding counterpart for `EditMode`. Not listed in events.md. |

**Priority:** P2 — needed for two-way EditMode binding.

---

#### A-03: `ToolbarTemplate` parameter
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 155) | No |
| Type | `RenderFragment?` | — |
| Notes | Allows fully custom toolbar markup, bypassing built-in toolbar rendering. Not documented in toolbar.md or overview.md. |

**Priority:** P2 — important customization surface.

---

#### A-04: `Placeholder` parameter
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 161) | No |
| Type | `string?` | — |
| Notes | Rendered as `data-placeholder` on WYSIWYG div and as native `placeholder` on source textarea. The ProseMirror plugins spec shows a custom plugin approach for placeholder but never mentions this built-in parameter. |

**Priority:** P2 — commonly asked-for feature, easy to document.

---

#### A-05: `CustomTools` parameter
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 179) | No |
| Type | `IEnumerable<EditorCustomTool>?` | — |
| Notes | Spec's custom-tools.md describes the `<EditorCustomTools>` child-component approach using ChildContent/RenderFragment. The source also has this separate `CustomTools` parameter accepting a typed collection. The spec does not mention this parameter-based approach. |

**Priority:** P2 — dual API surface needs clear documentation.

---

#### A-06: `ValueExpression` parameter
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 131) | No |
| Type | `Expression<Func<string>>?` | — |
| Notes | Required for EditContext/form validation integration. Overview.md mentions validation conceptually but never documents this parameter. |

**Priority:** P2 — essential for form validation scenarios.

---

#### A-07: `OnChange` event
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 187) | No |
| Type | `EventCallback<string>` | — |
| Notes | Fires after debounce, distinct from `ValueChanged`. Events.md only documents `ValueChanged`. |

**Priority:** P2 — consumers need to understand OnChange vs ValueChanged.

---

#### A-08: `OnSelectionChange` event
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 190) | No |
| Type | `EventCallback` | — |
| Notes | Fires when text selection changes in the editor. No spec mention. |

**Priority:** P3 — useful but not blocking for most scenarios.

---

#### A-09: `OnCommand` event
| | Source | Spec |
|---|--------|------|
| Exists | Yes (line 193) | No |
| Type | `EventCallback<string>` | — |
| Notes | Fires after any command is executed via `ExecuteAsync`. No spec mention. |

**Priority:** P3 — useful for logging/telemetry scenarios.

---

#### A-10: `ImportAsync` / `ExportAsync` methods
| | Source | Spec |
|---|--------|------|
| Exists | Yes (lines 326, 343) | No |
| Signatures | `Task ImportAsync(string content, string format)` / `Task<string> ExportAsync(string format)` | — |
| Notes | Markdig-based import/export via `IEditorFormatConverter` DI. The spec's import-export.md only describes external Document Processing tools and links to a GitHub sample. The built-in `ImportAsync`/`ExportAsync` methods with format converter architecture are completely undocumented. |

**Priority:** P1 — this is a major new feature (Markdig integration) with no spec coverage.

---

#### A-11: `ExecuteCommandAsync`, `SetModeAsync`, `GetHtmlAsync` methods
| | Source | Spec |
|---|--------|------|
| Exists | Yes (lines 278, 284, 309) | No |
| Notes | Three public methods beyond `ExecuteAsync`. Overview.md only lists `ExecuteAsync` in the methods table. |

**Priority:** P2 — `GetHtmlAsync` especially is commonly needed; `SetModeAsync` pairs with the EditMode feature.

---

### B. Spec-Ahead (in spec, NOT in source)

#### B-01: `Plugins` parameter
| | Source | Spec |
|---|--------|------|
| Exists | No | Yes (prosemirror-plugins.md, line 29) |
| Spec type | `string` | — |
| Notes | The spec documents a `Plugins` parameter (string pointing to a JS function name) for custom ProseMirror plugins. This parameter does not exist in the source. The source uses a plain contenteditable div with execCommand, not ProseMirror. |

**Priority:** P1 — spec describes a feature architecture (ProseMirror plugin system) that does not match the implementation. Either the spec is aspirational or the source needs this parameter added.

---

### C. Mismatch (both exist but differ)

#### C-01: `EditMode` enum values
| | Source | Spec |
|---|--------|------|
| Type | `EditorEditMode` | `EditorEditMode` |
| Source values | `Edit`, `Preview`, `Source` (line 148, plus markup lines 67-103) | — |
| Spec values | `Iframe`, `Div` (edit-modes/overview.md, line 17-18) | — |
| Notes | The spec describes Iframe vs Div rendering modes. The source implements Edit (WYSIWYG) / Preview (rendered read-only) / Source (raw HTML textarea) modes. Completely different semantics. |

**Priority:** P1 — fundamental mismatch. Consumers following the spec would use wrong enum values.

---

#### C-02: `Tools` parameter type
| | Source | Spec |
|---|--------|------|
| Source type | `IEnumerable<EditorTool>?` (an enum) | — |
| Spec type | `List<IEditorTool>` (toolbar.md line 32) | — |
| Notes | Source uses an `EditorTool` enum. Spec describes `IEditorTool` interface with class instantiation (`new Bold()`, `new EditorButtonGroup(...)`, etc.). The tool system architecture differs significantly. |

**Priority:** P1 — consumers cannot follow spec code examples as-is. The enum-based approach in source is simpler but incompatible with the spec's OOP tool model.

---

#### C-03: Custom tools API shape
| | Source | Spec |
|---|--------|------|
| Source approach | `CustomTools` parameter accepting `IEnumerable<EditorCustomTool>` (line 179) | — |
| Spec approach | `<EditorCustomTools>` child component with `<EditorCustomTool Name="...">` RenderFragments (custom-tools.md) | — |
| Notes | The spec describes a markup-based child component approach. The source has a parameter-based approach with `EditorCustomTool` objects. The markup does cascade `ChildContent` (line 14) but there is no `EditorCustomTools` component in the source to receive it. |

**Priority:** P2 — API shape mismatch affects developer experience.

---

#### C-04: Import/Export architecture
| | Source | Spec |
|---|--------|------|
| Source approach | Built-in `ImportAsync`/`ExportAsync` with DI-based `IEditorFormatConverter` (Markdig, plaintext) | — |
| Spec approach | External Document Processing library (import-export.md) | — |
| Notes | Spec says import/export is handled externally via Marilo Document Processing tools. Source has built-in converter architecture with `services.AddMariloEditorMarkdownSupport()`. Completely different strategy. |

**Priority:** P1 — consumers would look for external DPL integration when built-in support exists.

---

#### C-05: Table/Image resize documentation
| | Source | Spec |
|---|--------|------|
| Source | Full JS interop for table column/row drag handles (lines 500-584) and image resize handles (lines 586+) | — |
| Spec | Overview.md mentions resizing capability in 2 sentences (lines 43-51) but no parameter, event, or behavioral documentation | — |
| Notes | The source has ~200 lines of JS implementing drag-handle resize for tables and images (Batch 3 interop). The spec only has a brief mention. No parameters control this behavior (e.g., enable/disable resize, min/max constraints). |

**Priority:** P2 — feature is implemented but not controllable via parameters and barely documented.

---

## Summary

### P1 — Blocking (4 gaps)
| ID | Gap | Issue |
|----|-----|-------|
| A-10 | `ImportAsync`/`ExportAsync` | Major feature completely undocumented |
| B-01 | `Plugins` parameter | Spec documents feature that doesn't exist in source |
| C-01 | `EditMode` enum values | Spec and source define entirely different modes |
| C-02 | `Tools` parameter type | Enum vs interface — code examples won't compile |

### P2 — This Phase (10 gaps)
| ID | Gap | Issue |
|----|-----|-------|
| A-01 | `Disabled` | Undocumented parameter |
| A-02 | `EditModeChanged` | Undocumented event |
| A-03 | `ToolbarTemplate` | Undocumented customization surface |
| A-04 | `Placeholder` | Undocumented parameter |
| A-05 | `CustomTools` | Undocumented parameter-based API |
| A-06 | `ValueExpression` | Undocumented validation parameter |
| A-07 | `OnChange` | Undocumented event (confusable with ValueChanged) |
| A-11 | Public methods | 3 methods not in spec |
| C-03 | Custom tools shape | Markup vs parameter mismatch |
| C-05 | Table/image resize | Minimal docs for 200-line feature |

### P3 — Next Phase (3 gaps)
| ID | Gap | Issue |
|----|-----|-------|
| A-08 | `OnSelectionChange` | Undocumented event |
| A-09 | `OnCommand` | Undocumented event |
| C-04 | Import/Export strategy | External vs built-in mismatch |

---

## 2026-04-11 Orchestrator Wave 1

**Auditor:** `w-editor-delivery` (orchestrator tick 6, wave 1 / stage 01-spec-review)
**Audit date:** 2026-04-11
**Source surveyed:**
- `src/Marilo.Components/Editors/MariloEditor.razor` (main component, 21 `[Parameter]` props)
- `src/Marilo.Components/Editors/EditorPasteSettings.razor` (child component, 7 parameters)
- `src/Marilo.Components/Editors/EditorCustomTool.cs`
- `src/Marilo.Components/Editors/EditorCommandArgs.cs`
- `src/Marilo.Components/Editors/EditorFormatConverter.cs` (Markdig + plaintext converters, DI extension)

**Spec surveyed:** all 18 markdown files under `docs/component-specs/editor/**/*.md` (verified via `find` → 18 files).
**Prior audit context:** Section above (2026-04-10) produced 17 gaps. This pass does NOT duplicate them — it adds topic-level coverage classification for the 18 spec files as a whole and records gaps the prior pass missed (paste-cleanup child component, accessibility k- vs mar- prefix, Markdig decision provenance, prosemirror-schema blockers, ai-integration blockers, edit-modes iframe-vs-div semantic conflict at a higher level).

### Headline

- **Spec/source alignment is roughly 20%.** Of 18 spec topics, 2 are fully Covered, 3 are Partial, 9 are Blocked-by-source (feature absent from implementation), 4 are Orphan-source (source feature has no spec counterpart).
- **Human decision 2026-04-09 ("Editor = Markdig as bounded adapter") is NOT reflected in any spec file.** Source has `EditorFormatConverter.cs` with a `Markdig` using directive and `AddMariloEditorMarkdownSupport()` DI extension, but the import-export spec still describes a separate "Document Processing" library with no mention of Markdig. This is the single highest-priority documentation gap in Wave 1.
- The entire **ProseMirror** narrative (5 spec files: prosemirror-plugins, prosemirror-schema/overview, prosemirror-schema/create-new-schema, prosemirror-schema/modify-default-schema, edit-modes/iframe) describes an architecture the source does not use — the source is a `contenteditable` div with `document.execCommand`, not ProseMirror. These topics are Blocked-by-source and should be marked aspirational in the spec or removed until source adopts ProseMirror.
- The entire **ai-integration** topic set (3 spec files) is Blocked-by-source — `AIPrompt` integration is not implemented. Source's own `GAP_ANALYSIS.md` and `RESOLUTION_STATUS.md` already note "AI integration — Depends on AIPrompt component. Deferred."
- **Paste-cleanup** has a parameter shape mismatch not captured in the prior audit: spec lists `StripTags: List<string>` and `RemoveAttributes: List<string>`, source exposes them as `string?` (CSV). Spec also describes an `<EditorSettings>` wrapper that does not exist in source.

### Coverage Table (18 spec topics)

| # | Spec file | Source counterpart | Classification | Notes |
|---|-----------|-------------------|----------------|-------|
| 1 | `overview.md` | `MariloEditor.razor` (21 params) | **Partial** | Core behavior documented but parameter surface mismatch — see prior gaps A-01..A-11, C-01..C-05. |
| 2 | `toolbar.md` | `Tools`, `ToolbarTemplate`, `Adaptive`, `CustomTools` | **Partial** | Parameter present but type shape differs (enum vs `IEditorTool`). See prior C-02. |
| 3 | `built-in-tools.md` | `EditorTool` enum in source (19 DefaultTools) | **Partial** | Enum values exist but are not the OOP tool classes the spec describes. |
| 4 | `custom-tools.md` | `EditorCustomTool` + `CustomTools` parameter | **Partial** | Parameter-based API exists; markup-based `<EditorCustomTools>` API from spec does not. See prior C-03. |
| 5 | `events.md` | `ValueChanged`, `OnChange`, `OnCommand`, `OnSelectionChange`, `EditModeChanged` | **Partial** | Spec covers only `ValueChanged`; source has 4 additional events. See prior A-02, A-07, A-08, A-09. |
| 6 | `edit-modes/overview.md` | `EditorEditMode` enum (Edit, Preview, Source) | **Mismatch** | Spec enumerates Iframe/Div; source enumerates Edit/Preview/Source. Same enum name, entirely different values. See prior C-01. |
| 7 | `edit-modes/iframe.md` | — | **Blocked-by-source** | Source does not render an iframe at all — the Edit mode is a `contenteditable` div. |
| 8 | `edit-modes/div.md` | `contenteditable` div on WYSIWYG branch | **Covered** (by accident) | Source implements what the spec calls "div mode" as its only Edit mode, under a different EditMode value. |
| 9 | `import-export.md` | `ImportAsync`/`ExportAsync` + `IEditorFormatConverter` + Markdig | **Mismatch** | Spec describes external Document Processing library; source has built-in Markdig converter registered via `AddMariloEditorMarkdownSupport()`. See prior A-10 + C-04. New gap W1-04 covers the Markdig-decision provenance. |
| 10 | `paste-cleanup.md` | `EditorPasteSettings.razor` | **Partial** | 5 of 7 parameters align; `StripTags`/`RemoveAttributes` type mismatch and missing `<EditorSettings>` wrapper component. See new gap W1-05. |
| 11 | `prosemirror-plugins.md` | — | **Blocked-by-source** | Source is not ProseMirror-based; `Plugins` parameter does not exist. See prior B-01. |
| 12 | `prosemirror-schema/overview.md` | — | **Blocked-by-source** | Source has no ProseMirror schema concept. |
| 13 | `prosemirror-schema/create-new-schema.md` | — | **Blocked-by-source** | Same as above. |
| 14 | `prosemirror-schema/modify-default-schema.md` | — | **Blocked-by-source** | Same as above. |
| 15 | `accessibility/wai-aria-support.md` | `role=textbox`, `aria-multiline`, `aria-readonly`, `aria-labelledby/describedby` | **Partial** | Source implements WAI-ARIA attributes on the contenteditable div. Spec uses `.k-editor` (Kendo) class selectors — Marilo uses `mar-editor-*`. Selectors in spec are wrong. See new gap W1-06. |
| 16 | `ai-integration/overview.md` | — | **Blocked-by-source** | No AIPrompt wiring in source. See new gap W1-07. |
| 17 | `ai-integration/integration-with-aiprompt.md` | — | **Blocked-by-source** | Same as above. |
| 18 | `ai-integration/integration-with-inline-prompt.md` | — | **Blocked-by-source** | Same as above. |

**Plus orphan-source topics (source has features, spec has no topic at all):**

| # | Source surface | Classification | Notes |
|---|----------------|----------------|-------|
| O-1 | Adaptive toolbar overflow popup (lines 18–64, `Adaptive` parameter, `_overflowStartIndex`, ResizeObserver wiring) | **Orphan-source** | No spec file for adaptive/overflow toolbar. See new gap W1-08. |
| O-2 | Table/image resize JS drag handles (~200 lines) | **Orphan-source** | Only mentioned in 2 sentences in `overview.md`. See prior C-05. |
| O-3 | `EditContext` / `ValueExpression` validation integration (line 128 cascade, line 131 parameter) | **Orphan-source** | Spec mentions validation conceptually but no form-integration topic file. See prior A-06. |
| O-4 | Keyboard shortcuts (Ctrl+B/I/U/Z/Y hardcoded in JS) | **Orphan-source** | `accessibility/wai-aria-support.md` links to an external "keyboard navigation demo" but does not enumerate the actually-supported shortcuts. See new gap W1-09. |

### New Gap Records

(Numbered `W1-##` to avoid collision with the prior audit's A/B/C scheme.)

### SPEC-editor-W1-01

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-01 |
| Feature area | Edit modes (topic coverage) |
| Parameter/event | `edit-modes/iframe.md` |
| Gap type | spec-ahead |
| Source location | `MariloEditor.razor:67-103` (no iframe branch exists) |
| Spec location | `docs/component-specs/editor/edit-modes/iframe.md` |
| Description | Entire spec file describes iframe-based editing. Source never creates an `<iframe>`; the Edit mode is a `contenteditable` div. File is aspirational or copied from Kendo baseline. |
| Priority | P1 |
| Priority rationale | Developers following the spec will try to configure iframe mode and find it silently ignored. Compounds prior C-01 `EditorEditMode` enum mismatch. |
| Suggested resolution | Either (a) remove/mark-as-aspirational the iframe.md file, or (b) merge edit-modes/overview + div + iframe into a single topic aligned with actual Edit/Preview/Source enum. |

### SPEC-editor-W1-02

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-02 |
| Feature area | ProseMirror schema (topic cluster) |
| Parameter/event | `prosemirror-schema/*.md` (3 files) |
| Gap type | spec-ahead |
| Source location | n/a — no ProseMirror references anywhere in `src/Marilo.Components/Editors/**/*` |
| Spec location | `docs/component-specs/editor/prosemirror-schema/{overview,create-new-schema,modify-default-schema}.md` |
| Description | Three spec files describe ProseMirror schema customisation: nodes, marks, tag nesting, HTML attribute ordering. Source implementation is `contenteditable` + `document.execCommand`, no ProseMirror runtime. Topic cluster has zero implementation footing. |
| Priority | P1 |
| Priority rationale | Entire architectural narrative is incorrect. Any consumer who reads these pages gets a wrong mental model of the component. |
| Suggested resolution | Mark all three files as aspirational/roadmap and link to `editor-gap-analysis` workspace, OR delete until source adopts ProseMirror. Decision should be coordinated with editor-gap-analysis intake. |

### SPEC-editor-W1-03

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-03 |
| Feature area | AI integration (topic cluster) |
| Parameter/event | `ai-integration/*.md` (3 files) |
| Gap type | spec-ahead |
| Source location | n/a — no AIPrompt references in editor source |
| Spec location | `docs/component-specs/editor/ai-integration/{overview,integration-with-aiprompt,integration-with-inline-prompt}.md` |
| Description | Spec documents AIPrompt integration, inline prompt popup, and built-in commands (Rewrite, Fix Mistakes, Tone, Polish Formatting, Adjust Length, Translate). Source's own `GAP_ANALYSIS.md` line 46 marks AI integration "Not implemented" and `RESOLUTION_STATUS.md` line 58 marks it "Deferred". No `MariloAIPrompt` component exists in the repo. |
| Priority | P2 |
| Priority rationale | Lower than W1-01/W1-02 because this feature cluster is openly marked deferred in the source's own gap log — consumers who find that log will not be misled. Still P2 because the spec text itself does not say "deferred". |
| Suggested resolution | Add a "Status: planned, not yet implemented" banner to all three AI-integration spec files, citing source GAP_ANALYSIS.md line 46. |

### SPEC-editor-W1-04

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-04 |
| Feature area | Import/export — Markdig decision provenance |
| Parameter/event | `ImportAsync(string, string)` / `ExportAsync(string)` + `IEditorFormatConverter` + `AddMariloEditorMarkdownSupport()` |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/Editors/EditorFormatConverter.cs:3` (`using Markdig;`), line 30 (`MarkdownFormatConverter`), line 146 (`AddMariloEditorMarkdownSupport`); `MariloEditor.razor:326` (`ImportAsync`), line 343 (`ExportAsync`), line 357 (error message referencing the DI extension). |
| Spec location | missing — grep confirms zero matches for `Markdig` or `markdown` (case-insensitive) in `docs/component-specs/editor/**/*.md` |
| Description | Human decision 2026-04-09 recorded in user memory file `project_human_decisions_2026_04_09.md`: "Editor = Markdig as bounded adapter." Source implements this decision (Markdig pipeline in `EditorFormatConverter.cs`, DI extension, full-fidelity Markdown→HTML). Spec `import-export.md` still describes a separate "Document Processing" library and does not mention Markdig anywhere. This is the Wave 1 flagship documentation gap. |
| Priority | P1 |
| Priority rationale | The decision has been made AND implemented. Spec is the only artifact still out of sync. Blocking for any developer trying to enable markdown import/export — they will look for an external library that does not exist. |
| Suggested resolution | Rewrite `docs/component-specs/editor/import-export.md` to describe the Markdig-backed bounded adapter pattern: `services.AddMariloEditorMarkdownSupport()`, `IEditorFormatConverter`, `ImportAsync("...", "markdown")`, known asymmetry (Markdig for md→html, custom `BasicHtmlToMarkdown` for html→md). Keep the "bounded adapter" framing from the human decision record. |

### SPEC-editor-W1-05

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-05 |
| Feature area | Paste cleanup |
| Parameter/event | `StripTags`, `RemoveAttributes`, missing `<EditorSettings>` wrapper |
| Gap type | mismatch |
| Source location | `src/Marilo.Components/Editors/EditorPasteSettings.razor:19` (`StripTags: string?`), line 22 (`RemoveAttributes: string?`); `MariloEditor.razor:13` (ChildContent cascade, no `<EditorSettings>` intermediate). |
| Spec location | `docs/component-specs/editor/paste-cleanup.md:29-35` (spec shows `<EditorSettings>` wrapper), line 67 (`StripTags: List<string>`), line 69 (`RemoveAttributes: List<string>`). |
| Description | Two independent mismatches in the same topic: (1) source exposes `StripTags`/`RemoveAttributes` as `string?` CSV, spec documents them as `List<string>`. Code samples in the spec will not compile. (2) Spec wraps `<EditorPasteSettings>` inside `<EditorSettings>`; source has no `EditorSettings` component — `EditorPasteSettings` cascades directly from `MariloEditor`. Minor bonus: spec has typo `ConvertMsList` (singular) vs source `ConvertMsLists` (plural). |
| Priority | P1 |
| Priority rationale | Samples from this spec file will fail compilation. Paste cleanup is a headline feature for the editor and one of the more polished parts of the source. |
| Suggested resolution | Update paste-cleanup.md to drop `<EditorSettings>` wrapper, change parameter types to `string?` with CSV semantics (or change source to `List<string>` — ADR needed), fix `ConvertMsList` → `ConvertMsLists`. If CSV semantics are kept, add an example showing `"span,div"` syntax. |

### SPEC-editor-W1-06

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-06 |
| Feature area | Accessibility — WAI-ARIA |
| Parameter/event | CSS selectors in accessibility docs |
| Gap type | mismatch |
| Source location | `MariloEditor.razor:7` (class from `CssProvider.EditorClass()` → `mar-editor-*`), line 93 (`contenteditable="true"`), line 94 (`role="textbox"`), line 96 (`aria-readonly`) |
| Spec location | `docs/component-specs/editor/accessibility/wai-aria-support.md:29` (`.k-editor.k-readonly div[contenteditable=false]`) |
| Description | Accessibility spec uses Kendo CSS class selectors (`.k-editor`, `.k-readonly`). Marilo editor renders with `mar-editor-*` classes via `IMariloCssProvider`. Any consumer writing custom styles or axe-core rules against the documented selectors will match nothing. Also: spec says `div[contenteditable=false]` when readonly; source renders `contenteditable="false"` on the same element (via `@(!ReadOnly && !Disabled)` expression on line 93), so the selector *shape* is right but the class prefix is wrong. |
| Priority | P2 |
| Priority rationale | WAI-ARIA behavior is correctly implemented in source; the gap is purely documentation selectors. Not blocking functionality but blocking automated a11y tests written from spec. |
| Suggested resolution | Replace every `k-*` selector in `wai-aria-support.md` with the equivalent `mar-editor-*` class. Add a note that actual class names come from the configured `IMariloCssProvider`. |

### SPEC-editor-W1-07

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-07 |
| Feature area | AI integration — status banner |
| Parameter/event | All 3 `ai-integration/*.md` files |
| Gap type | spec-ahead |
| Source location | `src/Marilo.Components/Editors/GAP_ANALYSIS.md:46` and `resolution/RESOLUTION_STATUS.md:58` both note AI integration is deferred. |
| Spec location | `docs/component-specs/editor/ai-integration/{overview,integration-with-aiprompt,integration-with-inline-prompt}.md` |
| Description | Narrower restatement of W1-03: the spec does not disclose the deferred status. A "Status: planned" banner is the minimum-cost documentation fix — does not require deleting or rewriting the files. |
| Priority | P2 |
| Priority rationale | P2 (not P1) because W1-03 already captures the cluster-level gap; W1-07 is the tracked remediation action for it. |
| Suggested resolution | Add a frontmatter `status: planned` key and a `> Not yet implemented. See editor-gap-analysis workspace.` callout at the top of each file. |

### SPEC-editor-W1-08

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-08 |
| Feature area | Toolbar — adaptive overflow |
| Parameter/event | `Adaptive: bool`, overflow popup |
| Gap type | undocumented |
| Source location | `MariloEditor.razor:26-61` (overflow-items rendering), line 122 (`_overflowStartIndex`), line 123 (`_showOverflowPopup`), line 124 (`_toolbarWidth`), line 182 (`Adaptive` parameter), plus `IResizeObserverService` / `IElementMeasurementService` injection (lines 4-5). |
| Spec location | missing — `toolbar.md` does not mention `Adaptive` or overflow |
| Description | Source has a complete adaptive-toolbar implementation: ResizeObserver-driven measurement, overflow-index tracking, ⋯ popup button with `role="menu"`, focus-out handling. None of this is in the spec. Prior audit A-05-ish noted `Adaptive` as a parameter but did not cover the overflow popup behavior or the underlying services. |
| Priority | P2 |
| Priority rationale | Working feature, no demo references it explicitly. Not blocking but meaningful for consumers building tight-width toolbars. |
| Suggested resolution | Add a "Responsive / Overflow" subsection to `toolbar.md` documenting `Adaptive=true`, the ⋯ overflow popup, and a11y implications (role=menu, focus management). |

### SPEC-editor-W1-09

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-09 |
| Feature area | Accessibility — keyboard navigation |
| Parameter/event | Ctrl+B/I/U/Z/Y shortcuts |
| Gap type | undocumented |
| Source location | `MariloEditor.razor:407-416` (JS `onKeyDown` handler — hardcoded Ctrl+B bold, Ctrl+I italic, Ctrl+U underline, Ctrl+Z undo, Ctrl+Y redo, meta key support for macOS). |
| Spec location | `docs/component-specs/editor/accessibility/wai-aria-support.md:60-62` links to an external demo page but enumerates nothing. |
| Description | Source has exactly 5 hardcoded keyboard shortcuts. Spec's "Keyboard Navigation" section punts to an external demo URL and does not list them. WCAG 2.2 AA compliance claimed in the spec (line 18) is undermined by the omission. |
| Priority | P3 |
| Priority rationale | Shortcuts are standard browser defaults and work whether documented or not. Primarily a documentation-completeness concern. |
| Suggested resolution | Inline a `## Supported Keyboard Shortcuts` table in `wai-aria-support.md` listing the 5 hardcoded shortcuts, noting that additional shortcuts depend on `document.execCommand` browser support. |

### SPEC-editor-W1-10

| Field | Value |
|-------|-------|
| ID | SPEC-editor-W1-10 |
| Feature area | Demos — demo presence check |
| Parameter/event | Demo coverage for 18 spec topics |
| Gap type | spec-ahead |
| Source location | `samples/Marilo.Demo/Pages/Components/Editor/Editor/Overview.razor` (1 file — overview demo only) |
| Spec location | all 18 spec files imply demo links (`Live Demo: ...` hyperlinks near the bottom) |
| Description | Only 1 demo page exists for the editor (overview). Spec files routinely link to live demos for paste-cleanup, edit-modes, toolbar, AI integration, etc. All those demo links resolve to `demos.marilo.com` externally but nothing local exists to verify against in Wave 2 (`02-example-ux`). |
| Priority | P2 |
| Priority rationale | Not a spec-vs-source gap per se, but a Wave 2 blocker — example-UX stage will have almost nothing to audit. Flagging here so the orchestrator can plan Wave 2 accordingly. |
| Suggested resolution | Wave 2 should create baseline demo pages for at least: paste-cleanup, edit-modes (Edit/Preview/Source), custom tools, adaptive toolbar, Markdig import/export. Coordinate with `editor-gap-analysis` for source-side prerequisites. |

---

**Wave 1 totals (this section only):** 10 new gap records. Combined with the 2026-04-10 prior audit, total editor spec gaps = 27.

| New priority | Count |
|--------------|-------|
| P1 | 4 (W1-01, W1-02, W1-04, W1-05) |
| P2 | 5 (W1-03, W1-06, W1-07, W1-08, W1-10) |
| P3 | 1 (W1-09) |
