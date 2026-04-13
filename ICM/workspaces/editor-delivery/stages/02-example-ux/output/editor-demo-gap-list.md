# MariloEditor — Stage 02 Example UX: Demo Gap List

**Audit date:** 2026-04-11
**Auditor:** `w-editor-delivery` (orchestrator tick 6, Wave 2 / stage 02-example-ux)
**Source file:** `src/Marilo.Components/Editors/MariloEditor.razor` (21 `[Parameter]` props + 2 inherited)
**Demo page(s) audited:** `samples/Marilo.Demo/Pages/Components/Editor/Editor/Overview.razor` (sole demo file for editor)
**Spec surveyed:** 18 markdown files under `docs/component-specs/editor/**/*.md`
**Scenario-completeness standard:** `ICM/workspaces/editor-delivery/stages/02-example-ux/shared/demo-scenario-format.md`

**STATUS:** Gap list only. Stage 02 checkpoint (step 3) — STOPPED before demo authoring. Awaiting orchestrator review + approval of scope before any new `.razor` scenarios are written to `samples/Marilo.Demo/Pages/Components/Editor/`.

> **Supersedes the 2026-04-10 draft in this same file.** The prior draft was written before the Overview.razor refactor that added Edit Modes, ReadOnly/Disabled, Custom Tools, Adaptive, Table, Import/Export, Events, and Paste Settings sections. All 10 of the prior draft's "proposed demo sections" are now implemented — this Wave 2 audit starts from that updated baseline and catalogs the gaps that remain.

---

## Headline

- **1 demo page exists for 18 spec topics.** Of 18 spec topics, 10 have **zero** demo coverage (9 blocked-by-source + 1 accessibility topic never exercised in a scenario), 5 have **partial** coverage, 3 have **reasonable** coverage.
- **Parameter coverage: 14 of 21 source parameters** (66%) appear in at least one existing scenario. 7 parameters have no demo scenario.
- **Event coverage: 2 of 5 explicit events** (`OnChange`, `OnCommand`) are demonstrated with visible handlers. `OnSelectionChange`, `EditModeChanged`, and `ValueChanged` (as an explicit callback — implicit via `@bind-Value` only) are not demonstrated.
- **Edge cases: Disabled and ReadOnly are demonstrated. Empty state, error state, and form-validation state are not.**
- **No stale code snippets detected in the existing demo page.** Every parameter referenced in the current Overview.razor matches the source API. (The spec has stale/incorrect snippets per Wave 1 gaps C-01, C-02, C-03, W1-05, but those are spec gaps, not demo-page staleness.)
- **Flagship finding carried from Wave 1 (W1-04):** `ImportAsync`/`ExportAsync` + Markdig adapter is demoed in the current Overview page (Import / Export section), but the surrounding narrative and code snippet panel do not explain `AddMariloEditorMarkdownSupport()` DI wiring. Because the spec is silent on Markdig, the demo is currently the *only* consumer-facing documentation of the feature. That makes the demo the only artifact explaining a major source feature — a sync-area risk.
- **Expected outcome confirmed:** the gap list is dominated by category (a) and (d), with (b) empty. Most gaps are source-less spec topics (prosemirror / ai-integration / iframe) that should NOT generate new scenarios — they should be escalated to stage 01 / editor-gap-analysis.

---

## Gap counts by bucket

| Bucket | Count | Description |
|---|---|---|
| **(a) Parameters with no demo scenario** | **7** | 7 of 21 source parameters have zero scenario coverage. |
| **(b) Parameters with a scenario but a stale code snippet** | **0** | All existing snippets reference current parameter names and types. |
| **(c) Events with no demo scenario** | **3** | `OnSelectionChange`, `EditModeChanged`, `ValueChanged` (explicit handler form). |
| **(d) Edge cases not demonstrated** | **4** | Empty state, error state, form-validation state, ARIA-labelling scenario. |
| **(e) Spec-topic coverage gaps (informational)** | **10** | Topics with zero demo coverage. 9 are blocked-by-source and require stage-01 escalation, not demo authoring. 1 (accessibility) could be demoed once an ARIA-labelling scenario is added. |
| **Total actionable gaps for this stage** | **14** | (a)+(c)+(d). Plus 10 informational (e) entries. |

---

## Source-to-scenario coverage map

Each row = one source parameter. "Covered by" lists scenario titles in `Overview.razor` that exercise the parameter as its primary focus (P) or incidentally (I).

| # | Parameter | Covered by | Focus | Bucket |
|---|-----------|------------|-------|--------|
| 1 | `Value` | Basic Editor | P | ok |
| 2 | `ValueChanged` | Basic Editor (via `@bind-Value`) | I | **(c)** — no scenario with explicit `ValueChanged` handler; two-way binding demonstrated but not the event itself |
| 3 | `ValueExpression` | — | — | **(a)** — no EditForm / validation scenario |
| 4 | `ReadOnly` | ReadOnly & Disabled | P | ok |
| 5 | `Disabled` | ReadOnly & Disabled | P | ok |
| 6 | `EditMode` | Edit / Source / Preview | P | ok |
| 7 | `EditModeChanged` | — (scenario uses `SetModeAsync()` method instead of two-way binding) | — | **(c)** — the mode-change scenario should also show `@bind-EditMode` to exercise the event |
| 8 | `Tools` | Limited Tools (P), Custom Tools (I), Table Insertion (P) | P | ok |
| 9 | `ToolbarTemplate` | — | — | **(a)** — no scenario shows a custom toolbar template `RenderFragment` |
| 10 | `ChildContent` | Paste Settings (indirectly, via `EditorPasteSettings`) | I | ok — documented through child component usage |
| 11 | `Placeholder` | Basic Editor, Limited Tools, Custom Size | I | ok |
| 12 | `Height` | every scenario | I | ok |
| 13 | `Width` | Custom Size | P | ok |
| 14 | `DebounceDelay` | OnChange & OnCommand | P | ok |
| 15 | `AriaLabelledBy` | — | — | **(a)** — no scenario sets `aria-labelledby` (accessibility topic has no scenario) |
| 16 | `AriaDescribedBy` | — | — | **(a)** — as above |
| 17 | `CustomTools` | Custom Tools | P | ok |
| 18 | `Adaptive` | Adaptive Toolbar | P | ok |
| 19 | `OnChange` | OnChange & OnCommand | P | ok |
| 20 | `OnSelectionChange` | — | — | **(c)** — no scenario subscribes to selection changes |
| 21 | `OnCommand` | OnChange & OnCommand | P | ok |
| — | `Class` (inherited) | — | — | — (base parameter, not normally focus of a scenario) |
| — | `Style` (inherited) | — | — | — (base parameter) |

**Parameter coverage: 14/21 = 66%.** Excluding inherited base parameters.

---

## Bucket (a): Parameters with no demo scenario

### DEMO-editor-a01: `ValueExpression` (form validation)

| Field | Value |
|---|---|
| Parameter | `ValueExpression` (`Expression<Func<string>>?`) |
| Source location | `MariloEditor.razor:131` |
| Spec location | none — undocumented in spec (prior Wave 1 gap A-06) |
| Why a scenario is needed | Core integration surface for `EditForm` / `EditContext` validation. Consumers doing enterprise form work cannot wire up required-field / custom validators without an example. Source cascades `EditContext` (line 128) and computes `FieldIdentifier` from `ValueExpression`. |
| Missing scenario | "Editor inside an `<EditForm>` with a `[Required]` model property, DataAnnotations validator, and visible `ValidationMessage` component. Toggle between a valid and invalid value to show the validation surface updating in real time." |
| Edge cases this also closes | (d) form-validation edge case |
| Priority | P1 — flagship integration gap |

---

### DEMO-editor-a02: `ToolbarTemplate` (custom toolbar)

| Field | Value |
|---|---|
| Parameter | `ToolbarTemplate` (`RenderFragment?`) |
| Source location | `MariloEditor.razor:155` |
| Spec location | none — undocumented in spec (prior Wave 1 gap A-03) |
| Why a scenario is needed | `ToolbarTemplate` bypasses the built-in toolbar entirely and lets the consumer render arbitrary markup. Distinct from `Tools` (filtering built-in toolbar) and `CustomTools` (adding buttons to the built-in toolbar). No scenario shows the full-replacement option. |
| Missing scenario | "Custom Toolbar Template — replace the entire toolbar with a consumer-designed markup (e.g. a mini floating toolbar with only Bold / Italic / Save, rendered as `mar-btn` buttons that call `ExecuteAsync`)." |
| Priority | P2 |

---

### DEMO-editor-a03: `AriaLabelledBy`

| Field | Value |
|---|---|
| Parameter | `AriaLabelledBy` (`string?`) |
| Source location | `MariloEditor.razor:173` |
| Spec location | `accessibility/wai-aria-support.md` (mentioned conceptually but no parameter documented; Wave 1 gap W1-06 flags selector mismatch) |
| Why a scenario is needed | Accessibility spec topic currently has zero demo coverage. Consumers writing accessible forms need an example that associates an external `<label>` with the editor. |
| Missing scenario | "Labelled Editor — a visible `<label id='editor-label'>Description</label>` associated with the editor via `AriaLabelledBy=\"editor-label\"`. Demonstrates proper WAI-ARIA labelling flow." |
| Priority | P2 — pairs with DEMO-editor-a04 |

---

### DEMO-editor-a04: `AriaDescribedBy`

| Field | Value |
|---|---|
| Parameter | `AriaDescribedBy` (`string?`) |
| Source location | `MariloEditor.razor:176` |
| Spec location | `accessibility/wai-aria-support.md` (as above) |
| Why a scenario is needed | Same accessibility gap as a03. Describedby is used to tie help-text / error-text to the editor. |
| Missing scenario | Combine with a03 into a single "Accessible Editor with Label + Help Text" scenario: visible label → `AriaLabelledBy`, visible help text → `AriaDescribedBy`. Use a browser dev-tools screenshot or a narrated checklist of what a screen reader announces. |
| Priority | P2 |

---

### DEMO-editor-a05: Empty-state content

| Field | Value |
|---|---|
| Parameter | `Value` / `Placeholder` — empty-state UX |
| Source location | `MariloEditor.razor:161` (Placeholder), `:138` (Value) |
| Spec location | none |
| Why a scenario is needed | Every existing scenario initializes `Value` with sample content. The placeholder behaviour only runs when `Value == null` or empty, and is not actually shown in a dedicated scenario (Basic Editor uses Placeholder + a pre-filled Value, which hides the placeholder). A reviewer cannot see what the placeholder looks like without editing the demo page. |
| Missing scenario | "Empty Editor — `Value` deliberately null, `Placeholder` set. Demonstrates the placeholder rendering when there is no content. Include a 'Reset to empty' button so the user can return to empty state after typing." |
| Priority | P2 — doubles as edge case (d) empty-state |

---

### DEMO-editor-a06: Error / failure state

| Field | Value |
|---|---|
| Parameter | `ImportAsync` / `ExportAsync` error behavior |
| Source location | `MariloEditor.razor:326` (`ImportAsync`), `:343` (`ExportAsync`), `:357` (error message path) |
| Spec location | `import-export.md` — spec is wrong per Wave 1 W1-04; source throws `InvalidOperationException` when converter not registered |
| Why a scenario is needed | The existing Import / Export scenario already catches `InvalidOperationException` and displays a fallback message, but it treats the error as a silent `_exportedText` assignment. There is no dedicated scenario demonstrating a **deliberately empty DI container** or an **unsupported format**. Consumers do not learn how the component signals failure. |
| Missing scenario | "Unsupported Format — call `ExportAsync(\"latex\")` (never registered); show the `InvalidOperationException` surfaced in a red error panel and explain the `AddMariloEditorMarkdownSupport()` DI registration step. This is also the place the Markdig decision (Wave 1 W1-04) should be explicitly documented in the demo until the spec catches up." |
| Priority | P1 — doubles as edge case (d) error-state AND carries Markdig provenance for Wave 1 W1-04 |

---

### DEMO-editor-a07: Two-way `EditMode` binding scenario

| Field | Value |
|---|---|
| Parameter | `EditMode` + `EditModeChanged` (two-way binding) |
| Source location | `MariloEditor.razor:148-149` |
| Spec location | none for `EditModeChanged` (Wave 1 A-02) |
| Why a scenario is needed | The existing "Edit / Source / Preview" scenario drives the mode via `SetModeAsync()` on a `@ref`. It never demonstrates the **declarative two-way** form `@bind-EditMode="_mode"`. The `EditModeChanged` event is therefore never demoed. |
| Missing scenario | Either (a) refactor the existing scenario to use `@bind-EditMode` and drop the `@ref + SetModeAsync` path, or (b) add a second "Two-way bound EditMode" scenario alongside. Prefer (a) — the current scenario uses a more complex pattern than necessary. If (a) is chosen, note it as a **refactor** of the existing scenario, not a net-new one. |
| Priority | P2 — pairs with bucket (c) c02 below |

---

## Bucket (b): Parameters with a stale code snippet

**None.** Every parameter referenced in `Overview.razor` matches the current source API. Verified against `MariloEditor.razor:128-194` and `EditorPasteSettings.razor:13-25`:

- `@bind-Value` ↔ source `Value` / `ValueChanged` ✔
- `ReadOnly`, `Disabled`, `EditMode`, `Tools`, `CustomTools`, `Adaptive`, `Width`, `Height`, `Placeholder`, `DebounceDelay`, `OnChange`, `OnCommand` all name-match ✔
- `EditorTool` enum values (`Bold`, `Italic`, `Underline`, `Link`, `OrderedList`, `UnorderedList`, `Image`, `Table`, `Undo`, `Redo`, `AlignLeft`, `AlignCenter`, `AlignRight`, `Strikethrough`) all resolve against the source enum ✔
- `EditorPasteSettings` parameters in the Paste Settings scenario (`RemoveHtmlComments`, `RemoveMsClasses`, `RemoveMsStyles`, `StripTags`) all name-match the source. `StripTags="meta,link,style"` matches the source's `string?` CSV shape. Note: **this IS a mismatch against the spec** (Wave 1 W1-05 — spec says `List<string>`), but since the demo must reflect the actual source API, the demo is correct and the spec is wrong. **No stale snippet in the demo.**
- `EditorCustomTool` public surface (`Name`, `Tooltip`, `OnClick`) matches the `EditorCustomTool` class used by `Custom Tools` scenario ✔

If Wave 1 W1-05 is resolved by changing the source (making `StripTags` a `List<string>`), the Paste Settings scenario snippet will become stale at that point and should move to bucket (b). It is NOT stale today.

---

## Bucket (c): Events with no demo scenario

### DEMO-editor-c01: `OnSelectionChange`

| Field | Value |
|---|---|
| Event | `OnSelectionChange` (`EventCallback`) |
| Source location | `MariloEditor.razor:190` |
| Spec location | none (Wave 1 A-08) |
| Why a scenario is needed | Consumers building inline toolbars, selection-aware formatting indicators, or collaborative-editing cursors need to know when the selection changes. No scenario shows this event firing. |
| Missing scenario | "Selection Indicator — subscribe to `OnSelectionChange`, display 'Last selection: HH:MM:SS' and a running count. Also demonstrate that the event fires on click, keyboard arrow, and drag." |
| Priority | P3 |

---

### DEMO-editor-c02: `EditModeChanged` (two-way event)

| Field | Value |
|---|---|
| Event | `EditModeChanged` (`EventCallback<EditorEditMode>`) |
| Source location | `MariloEditor.razor:149` |
| Spec location | none (Wave 1 A-02) |
| Why a scenario is needed | See DEMO-editor-a07 above. The existing Edit/Source/Preview scenario uses `SetModeAsync` and bypasses the event. A two-way binding example via `@bind-EditMode` would fire `EditModeChanged` visibly. |
| Missing scenario | Same as DEMO-editor-a07. |
| Priority | P2 |

---

### DEMO-editor-c03: `ValueChanged` (explicit handler, not just @bind)

| Field | Value |
|---|---|
| Event | `ValueChanged` (`EventCallback<string>`) |
| Source location | `MariloEditor.razor:139` |
| Spec location | `overview.md` mentions conceptually |
| Why a scenario is needed | Every current scenario uses `@bind-Value`, which hides the `ValueChanged` callback. Consumers who need to run logic in the setter (validation, word count, autosave) must subscribe to `ValueChanged` directly (`Value=".." ValueChanged="OnVal"`). This pattern is never demoed. Note that `OnChange` is a separate, debounced event — these are NOT the same surface. |
| Missing scenario | "Word Counter — `Value`+`ValueChanged` (no `@bind`), strip HTML tags and show a running word count. Contrast with the OnChange scenario (which is debounced)." |
| Priority | P3 |

---

## Bucket (d): Edge cases not demonstrated

### DEMO-editor-d01: Empty state

See DEMO-editor-a05 (combined — empty state is both a parameter gap and an edge case).

### DEMO-editor-d02: Error state

See DEMO-editor-a06 (combined — error state is both a method-surface gap and an edge case).

### DEMO-editor-d03: Form-validation state

See DEMO-editor-a01 (combined — form validation is both a parameter gap and an edge case).

### DEMO-editor-d04: ARIA-labelled state

See DEMO-editor-a03 + a04 (combined — the accessibility edge case is an ARIA-labelled scenario that also covers `AriaLabelledBy`/`AriaDescribedBy`).

**Net new edge-case scenarios (not double-counted with bucket a):** 0 — every edge-case gap is closed by one of the bucket-(a) scenarios above.

**Remaining edge cases already demonstrated:**

- `Disabled` state — covered (ReadOnly & Disabled scenario)
- `ReadOnly` state — covered (ReadOnly & Disabled scenario)

---

## Bucket (e): Spec topics with zero demo coverage (informational)

For completeness — these 10 of 18 spec topics have no scenario in the current demo page. **Most are NOT actionable at this stage because the source does not implement them.** Do not auto-generate scenarios for blocked-by-source topics — escalate them back to stage 01 / editor-gap-analysis.

| # | Spec topic | Demo coverage | Reason | Actionable at stage 02? |
|---|---|---|---|---|
| 1 | `edit-modes/iframe.md` | zero | Source has no iframe branch (Wave 1 W1-01) | No — escalate |
| 2 | `prosemirror-plugins.md` | zero | Source not ProseMirror-based (Wave 1 B-01) | No — escalate |
| 3 | `prosemirror-schema/overview.md` | zero | As above | No — escalate |
| 4 | `prosemirror-schema/create-new-schema.md` | zero | As above | No — escalate |
| 5 | `prosemirror-schema/modify-default-schema.md` | zero | As above | No — escalate |
| 6 | `ai-integration/overview.md` | zero | `MariloAIPrompt` not implemented (Wave 1 W1-03) | No — escalate |
| 7 | `ai-integration/integration-with-aiprompt.md` | zero | As above | No — escalate |
| 8 | `ai-integration/integration-with-inline-prompt.md` | zero | As above | No — escalate |
| 9 | `built-in-tools.md` | partial only (`EditorTool` enum values shown in Limited Tools and Custom Tools) | Not every built-in tool has its own focused scenario (e.g. `Link`, `Image`, `Color`, `Formatting`, `FontName`, `FontSize`, `Indent`/`Outdent`, `Strikethrough`, `Subscript`/`Superscript`). These DO exist in source but are bundled into generic scenarios. | Partial — a "Tool Gallery" scenario could focus on each built-in tool, but this is P3 and should follow approval of the P1/P2 items above. |
| 10 | `accessibility/wai-aria-support.md` | zero | Closed by DEMO-editor-a03 + a04 above | Yes — the only (e)-bucket topic that IS actionable at stage 02 |

**Count: 10 of 18 spec topics have zero demo coverage.** 8 of those are blocked-by-source (not actionable). 1 is partial (built-in tools gallery, P3). 1 is actionable via the accessibility scenario proposed in bucket (a).

---

## Actionable summary (for orchestrator approval)

If the checkpoint is approved, the following scenarios should be authored in Wave 2 step 5:

| # | Scenario title | Closes gaps | Priority |
|---|---|---|---|
| 1 | EditForm Validation — `<EditForm>` with DataAnnotations + `ValidationMessage` | a01, d03 | P1 |
| 2 | Unsupported Format / Error State — `ExportAsync("latex")` fail path + Markdig DI wiring callout | a06, d02, (carries W1-04) | P1 |
| 3 | Custom Toolbar Template — full `RenderFragment` replacement | a02 | P2 |
| 4 | Accessible Editor — labelled + described, keyboard walkthrough | a03, a04, d04, (e)-10 | P2 |
| 5 | Empty Editor — `Value=null`, placeholder visible, reset button | a05, d01 | P2 |
| 6 | Two-way bound EditMode (refactor of existing) — `@bind-EditMode` + `EditModeChanged` handler | a07, c02 | P2 (refactor, not net-new) |
| 7 | Selection Indicator — `OnSelectionChange` handler | c01 | P3 |
| 8 | Word Counter — `Value` / `ValueChanged` explicit (no `@bind`) | c03 | P3 |

**Total new scenarios to write: 7 (plus 1 refactor of the existing Edit/Source/Preview scenario). Do NOT auto-generate scenarios for (e)-bucket entries 1-8 — those require stage 01 escalation first.**

**Scenarios that should NOT be authored this wave:**

- All blocked-by-source topics (iframe, prosemirror-*, ai-integration-*)
- Tool-gallery scenarios for every single `EditorTool` enum value (P3, schedule after P1/P2)
- Any scenario that would require changing the source API (e.g. converting `StripTags` to `List<string>` from Wave 1 W1-05 — that is a gap-analysis / source change, not a demo change)

---

## Checkpoint

**Step 3 CHECKPOINT per `02-example-ux/CONTEXT.md` — STOPPED HERE.**

Orchestrator review required before any scenario authoring. Specific questions for the reviewer:

1. Approve the 7 new scenarios + 1 refactor above?
2. Confirm that (e)-bucket blocked-by-source topics should be escalated back to stage 01, not demoed?
3. Confirm the DEMO-editor-a06 error-state scenario is the correct place to document the Markdig adapter decision (Wave 1 W1-04) until the spec catches up, or should that remain a pure spec fix?
4. Confirm the scope for DEMO-editor-a07 — refactor the existing Edit/Source/Preview scenario OR add a second scenario?

---

## Verification

- `dotnet build Marilo.slnx` → **exit 0**, 0 warnings, 0 errors. Build output inspected in this turn.
- Source API cross-checked against `MariloEditor.razor:128-194` and `EditorPasteSettings.razor:13-25` (re-read this turn).
- Demo page cross-checked against `samples/Marilo.Demo/Pages/Components/Editor/Editor/Overview.razor` (re-read this turn — structure unchanged since Wave 1 audit).
- Spec topic count cross-checked against `docs/component-specs/editor/**/*.md` — 18 files, consistent with Wave 1.
- **No new files created in `samples/Marilo.Demo/Pages/Components/Editor/`.** Stage 02 checkpoint respected.
