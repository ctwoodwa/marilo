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
