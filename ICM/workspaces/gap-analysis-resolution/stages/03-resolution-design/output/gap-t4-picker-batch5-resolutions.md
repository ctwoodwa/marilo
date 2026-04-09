# Resolution Records: T4 Pickers Batch 5 — Remote Data + Typed Input

> Date: 2026-04-08
> Source: `stages/01-intake/output/gap-t4-pickers-prioritization.md` (Batch 2/3 leftovers; medium severity)
> Components: MariloMultiSelect, MariloDateTimePicker
> Scope: batch (skips Stage 04 per workspace gap-scope routing)

These are the next medium-severity items after Batch 4 closed. GAP-MSEL-005 (child component API for `<MultiSelectSettings>` / `<MultiSelectPopupSettings>`) is intentionally deferred to its own batch — see § Cross-cutting notes.

---

## RES-T4B5-01: MariloMultiSelect remote-data — OnRead + Rebind() + ValueMapper

**Resolves:** GAP-MSEL-006 (primary). Also closes the deferred remote-data portion of GAP-MSEL-001 by adding `OnRead`.
**Status:** Implemented

### Target Pattern

Three coordinated additions to `MariloMultiSelect<TItem, TValue>`:

1. **OnRead callback** — consumer-provided event that supplies items for the current filter/scroll state. Mirrors the established `GridReadEventArgs<TItem>` pattern from `MariloDataGrid`.

   ```csharp
   /// <summary>
   /// Server-side data callback. When bound, the component invokes this on Open,
   /// Filter changes, and Rebind() instead of paging the local Data parameter.
   /// The handler must set Data and Total on the args.
   /// </summary>
   [Parameter] public EventCallback<MultiSelectReadEventArgs<TItem>> OnRead { get; set; }
   ```

   New args type in `Marilo.Components.Forms.Inputs` (or shared model namespace):

   ```csharp
   public class MultiSelectReadEventArgs<TItem>
   {
       /// <summary>The current filter text the user has typed.</summary>
       public string Filter { get; init; } = string.Empty;

       /// <summary>Cancellation token cancelled if a newer read starts before this one completes.</summary>
       public CancellationToken CancellationToken { get; init; }

       /// <summary>Set this to the items to display in the dropdown.</summary>
       public IEnumerable<TItem> Data { get; set; } = Array.Empty<TItem>();

       /// <summary>Set this to the total number of items available on the server.</summary>
       public int Total { get; set; }
   }
   ```

2. **Rebind() public method** — triggers a fresh OnRead invocation if `OnRead` is bound; otherwise re-reads the local `Data` parameter (matches existing `Refresh()` semantics).

   ```csharp
   /// <summary>
   /// Forces a fresh data read. When OnRead is bound, invokes the callback again.
   /// Otherwise re-syncs from the local Data parameter.
   /// </summary>
   public async Task Rebind()
   {
       if (OnRead.HasDelegate)
           await LoadServerDataAsync();
       else
           Refresh();
   }
   ```

3. **ValueMapper callback** — async resolver that converts pre-selected `TValue`s into the matching `TItem`s when the items are not present in the current `Data` window. Used in remote/virtual scenarios where the consumer hands the component a `Value` list before any data has been read.

   ```csharp
   /// <summary>
   /// Async resolver invoked when Value contains entries that are not present in the
   /// current Data window. Returns the matching TItems so the component can render
   /// the tag chips for pre-selected remote values.
   /// </summary>
   [Parameter] public Func<IEnumerable<TValue>, Task<IEnumerable<TItem>>>? ValueMapper { get; set; }
   ```

   On `OnParametersSet`, after the existing `_selectedItems` reflection-based resolution, any `TValue`s in `Value` that did not resolve to a `TItem` are passed to `ValueMapper`. The returned items are merged into `_selectedItems` so tag rendering works.

### Options Considered

**Option A: OnRead + Rebind() + ValueMapper as three coordinated additions (chosen)**
- Approach: All three additions in one batch since they form a cohesive remote-data feature set.
- Pros: Consumers get a complete remote-data story; matches Telerik shape; closes GAP-MSEL-006 fully.
- Cons: Touches `OnParametersSet` and the load path; medium-sized change.
- Effort: Medium.

**Option B: OnRead alone, defer Rebind/ValueMapper**
- Approach: Just add OnRead, leave Rebind and ValueMapper as future work.
- Pros: Smaller change.
- Cons: Doesn't close GAP-MSEL-006; consumers can't refresh remote data on demand or pre-select remote values; the remote-data story is half-finished.
- Effort: Small.

**Option C: Rebind() as alias for Refresh(), no OnRead**
- Approach: Add `Rebind()` as a no-op alias for the existing `Refresh()` method.
- Pros: Tiny change.
- Cons: Useless without OnRead — there's nothing to "rebind" to. The gap report explicitly says "Rebind() triggers OnRead re-fetch".
- Effort: Trivial.

### Decision

**Chosen:** Option A.
**Rationale:** GAP-MSEL-006 explicitly couples Rebind() to OnRead ("Rebind() triggers OnRead re-fetch"). Adding Rebind without OnRead is a no-op; adding OnRead without Rebind leaves consumers unable to manually refresh. The three additions form an indivisible feature. The cancellation token in `MultiSelectReadEventArgs` mirrors the existing `GridReadEventArgs<TItem>.CancellationToken` so consumers writing handlers for both components see the same shape.

ValueMapper is included in this batch because pre-selected remote values are the immediate next thing a remote-data consumer hits — without it, the tags for pre-selected remote items render as empty (their `TValue` is in `_selectedValues` but no matching `TItem` exists in `_allItems`).

### Consequences

- New parameter `OnRead` on `MariloMultiSelect`.
- New parameter `ValueMapper` on `MariloMultiSelect`.
- New public method `Rebind()` on `MariloMultiSelect`.
- New args type `MultiSelectReadEventArgs<TItem>` in `Marilo.Components.Forms.Inputs`.
- New private `LoadServerDataAsync()` helper that builds args, invokes `OnRead`, copies returned `Data` into `_allItems`, copies returned `Total` into a new `_serverTotal` field (currently informational; reserved for future virtual-scroll page-fetch cycles).
- Existing `OnParametersSet` extended with a `ValueMapper` resolution pass (only runs if ValueMapper is set and `Value` has entries not yet in `_allItems`). The pass is `async` so the component now needs `OnParametersSetAsync` or an async-fire-and-forget pattern; chosen approach is to override `OnParametersSetAsync` instead of the sync version.
- `OpenDropdown` invokes `LoadServerDataAsync` if `OnRead` is bound and `_allItems` is empty (lazy first read).
- `OnFilterInput` invokes `LoadServerDataAsync` instead of local `ApplyFilter` when `OnRead` is bound. Existing local-filtering behavior preserved when `OnRead` is not bound.
- A `CancellationTokenSource _readCts` field mirrors the existing `_filterDebounce` pattern — each new read cancels the previous one.
- Existing tests unchanged; new bUnit tests cover `Rebind()`, OnRead invocation, and ValueMapper resolution.

### Success Criteria

- [x] `OnRead` parameter exists.
- [x] `MultiSelectReadEventArgs<TItem>` type exists with Filter, CancellationToken, Data, Total.
- [x] `Rebind()` public method invokes OnRead when bound, falls back to Refresh otherwise.
- [x] `ValueMapper` parameter exists.
- [x] Pre-selected `Value` entries not present in `_allItems` are resolved via ValueMapper and rendered as tags.
- [x] Cancellation token cancels in-flight reads when a newer one starts.
- [x] Local-data path (no OnRead bound) is unchanged.
- [x] bUnit tests cover OnRead invocation, Rebind triggering OnRead, and ValueMapper resolution.

---

## RES-T4B5-02: MariloDateTimePicker typed input parsing

**Resolves:** GAP-DTP-003
**Status:** Implemented

### Target Pattern

Remove the `readonly="true"` attribute on the DateTimePicker input and add a typed-input parsing path:

```razor
<input id="@Id"
       type="text"
       class="@CssProvider.TextBoxClass(false, !Enabled)"
       value="@_inputText"
       placeholder="@Placeholder"
       disabled="@(!Enabled)"
       readonly="@ReadOnly"
       tabindex="@TabIndex"
       inputmode="@InputMode"
       autocomplete="@AutoComplete"
       role="combobox"
       aria-label="Date and time"
       aria-haspopup="dialog"
       aria-expanded="@_isPopupOpen.ToString().ToLowerInvariant()"
       aria-controls="@(_isPopupOpen ? _popupId : null)"
       @oninput="OnInputChanged"
       @onclick="OpenPopup"
       @onblur="HandleBlur" />
```

Key changes:
- `readonly="true"` removed; replaced with `readonly="@ReadOnly"` so the existing `ReadOnly` parameter still works.
- A new `_inputText` field holds the displayed text (decoupled from `Value` so partial typing doesn't lose user input mid-keystroke).
- `@onfocus="OpenPopup"` removed; only `@onclick` opens. This avoids the popup auto-opening when the user Tabs into the field to type, which would steal focus from the input.
- New `OnInputChanged(ChangeEventArgs)` handler attempts `DateTime.TryParseExact(input, Format, CultureInfo.CurrentCulture, ...)` first, then `DateTime.TryParse(input, ...)` as a fallback. If parsing succeeds and the result is within `Min`/`Max`, `Value` is updated and `ValueChanged` is fired. If parsing fails, `_inputText` retains the user's text but `Value` is not changed (consumer sees no spurious value updates from invalid input).
- `FormatValue()` is replaced by `_inputText` initialization in `OnParametersSet` — when `Value` changes externally, `_inputText` is reset to the formatted value.

### Options Considered

**Option A: Decoupled `_inputText` field, parse on every input event (chosen)**
- Approach: Local field tracks display text; parse attempt updates `Value` only on successful parse.
- Pros: Partial typing preserved; no debounce needed; matches how `MariloTimePicker` already works (it has a similar `_inputText` field).
- Cons: One new field; small extra logic in `OnParametersSet`.
- Effort: Small.

**Option B: Direct `Value`-bound input with parse-or-revert**
- Approach: Bind `value="@FormatValue()"` and parse `e.Value` on every input. Revert to last valid value if parse fails.
- Pros: No new field.
- Cons: Reverts mid-typing — unusable. Type "2026-" and the field jumps back to the last valid value.
- Effort: Small.

**Option C: Parse only on blur**
- Approach: Don't parse on input — only when the input loses focus.
- Pros: Less parsing work.
- Cons: User has no indication their input is being processed; clicking the popup to pick a date after typing is confusing because the typed text stays stale.
- Effort: Small.

### Decision

**Chosen:** Option A.
**Rationale:** Mirrors the existing `MariloTimePicker._inputText` pattern so both pickers behave the same when the user types directly. Option B is broken UX. Option C is inconsistent with how TimePicker works. The added `_inputText` field is one new piece of state — minimal cost.

Removing `@onfocus="OpenPopup"` is a small UX change: previously, Tab-focusing the input opened the popup. With typed input enabled, that would steal focus and prevent typing. Click-to-open is the standard behavior for combobox inputs that support typing. Documented in closure report as a minor behavior change.

### Consequences

- New private `_inputText` field on `MariloDateTimePicker`.
- New `OnInputChanged` handler.
- `FormatValue()` replaced by `_inputText` initialization in `OnParametersSet` (override `OnParametersSet` to sync `_inputText` from `Value`).
- `readonly="true"` → `readonly="@ReadOnly"` — `ReadOnly` parameter now actually does something on the input element.
- `@onfocus="OpenPopup"` removed; only `@onclick` opens the popup.
- Parse uses `DateTime.TryParseExact` against the configured `Format` first, then falls back to `DateTime.TryParse` for tolerant input.
- Min/Max clamp applied after successful parse.
- Existing tests that rely on the input being read-only need verification — none should because they all interact with the popup via `cut.Find("input").Click()` then act on the popup.
- New bUnit tests cover typed valid input updates Value, typed invalid input does not update Value, and `ReadOnly=true` blocks input.

### Success Criteria

- [x] Input has `readonly="@ReadOnly"` (no longer hardcoded true).
- [x] Typing a valid date string in the configured Format updates Value.
- [x] Typing an invalid string leaves Value unchanged but preserves the typed text.
- [x] Typing a value outside Min/Max clamps to the boundary.
- [x] `ReadOnly=true` still prevents typing (browser-enforced via the attribute).
- [x] Setting `Value` externally updates the displayed text.
- [x] Tab focus does not auto-open the popup; click does.
- [x] bUnit tests cover valid parse, invalid parse, ReadOnly block.

---

## Cross-cutting notes

- **Scope:** batch (skips Stage 04 remediation plan per workspace `Gap Scope Routing`).
- **GAP-MSEL-005 deferred to its own batch:** The `<MultiSelectSettings>` / `<MultiSelectPopupSettings>` child component API requires cascading-parameter wiring with `[CascadingParameter]` registration. The cerebrum already documents a `MariloWizard CascadingValue bug` class — bringing the same pattern into MultiSelect is a non-trivial design decision (lifecycle ordering, child registration timing, what-if-no-parent fallback). Worth its own resolution record. Filed as an explicit follow-up, not a deferred gap.
- **GAP-MSEL-001 partial-resolved status:** This batch closes the `OnRead` portion. `OnChange` and `OnItemRender` remain open but are orthogonal to remote data and can be a separate batch.
- **GAP-DTP-001 typed-input touch:** This batch removes `@onfocus="OpenPopup"`. No existing test should break — all tests use `Click()` not `Focus()`. Verified by inspection of `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs`.
- **No CSS provider changes** required.
- **No third-party dependencies** added.
