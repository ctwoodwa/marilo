# Implementation Log: T4 Pickers Batch 5 — Remote Data + Typed Input

> Date: 2026-04-08
> Resolutions: `stages/03-resolution-design/output/gap-t4-picker-batch5-resolutions.md`
> Components: `MariloMultiSelect`, `MariloDateTimePicker`
> Scope: batch (Stage 04 skipped per workspace gap-scope routing)

---

## RES-T4B5-01: MariloMultiSelect.OnRead + Rebind() + ValueMapper

### Files modified

- `src/Marilo.Components/Forms/Inputs/MultiSelectModels.cs` (new file)
- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`

### Changes

1. **New args type** (`MultiSelectModels.cs`):
   ```csharp
   public class MultiSelectReadEventArgs<TItem>
   {
       public string Filter { get; init; } = string.Empty;
       public CancellationToken CancellationToken { get; init; }
       public IEnumerable<TItem> Data { get; set; } = Array.Empty<TItem>();
       public int Total { get; set; }
   }
   ```
   Mirrors `GridReadEventArgs<TItem>` from `MariloDataGrid` for consistency.

2. **New parameters** (`MariloMultiSelect.razor` `@code`):
   - `[Parameter] EventCallback<MultiSelectReadEventArgs<TItem>> OnRead` — server-side data callback
   - `[Parameter] Func<IEnumerable<TValue>, Task<IEnumerable<TItem>>>? ValueMapper` — pre-selection resolver

3. **New private state**:
   - `CancellationTokenSource? _readCts` — cancels stale reads when newer one starts
   - `bool _initialReadDone` — tracks whether the lazy first read has fired

4. **`OnParametersSet` → `OnParametersSetAsync`**: required because `ValueMapper` is async. Existing local-resolution path runs first, then if `ValueMapper` is set and there are unresolved selected values, the mapper is invoked and returned items are merged into `_selectedItems`.

5. **New `LoadServerDataAsync` helper**: creates `MultiSelectReadEventArgs<TItem>` with current `_filterText`, awaits `OnRead.InvokeAsync(args)`, copies `args.Data` into `_allItems` and `_filteredItems`, recomputes `_selectedItems` against the new server window while preserving previously-resolved tags (e.g., from ValueMapper). Cancels any in-flight read via `_readCts`.

6. **`OpenDropdown` extended**: when `OnRead.HasDelegate && !_initialReadDone`, invokes `LoadServerDataAsync` instead of `ApplyFilter` on first open. `_initialReadDone` flag ensures the lazy read fires only once per component instance.

7. **`OnFilterInput` extended**: after debounce, routes to `LoadServerDataAsync` (when `OnRead` bound) or `ApplyFilter` (local). Existing local-filtering path is unchanged.

8. **New `Rebind()` public method**: when `OnRead` is bound, calls `LoadServerDataAsync` and `InvokeAsync(StateHasChanged)`. Otherwise falls back to existing `Refresh()` (which re-reads local `Data`). Pattern matches `MariloListView.Rebind` at `MariloListView.razor:93`.

9. **`Dispose` extended**: disposes `_readCts` alongside `_filterDebounce`.

### Verification (manual code-trace)

| Scenario | Expected | Trace |
|---|---|---|
| Local data, no OnRead | Existing behavior unchanged | `OnRead.HasDelegate` is false → all branches fall through to legacy paths |
| OnRead bound, first open | Invokes OnRead with empty filter | `OpenDropdown` → `_initialReadDone=false` → `LoadServerDataAsync` → `OnRead.InvokeAsync` |
| OnRead bound, second open | Does NOT re-invoke OnRead | `_initialReadDone=true` → falls into `ApplyFilter` branch (no read) |
| OnRead bound, user types | OnRead invoked with filter text after debounce | `OnFilterInput` → debounce → `LoadServerDataAsync` |
| OnRead bound, Rebind() called | Fresh OnRead invocation | `Rebind` → `LoadServerDataAsync` |
| Rebind() without OnRead | Falls back to Refresh() | `OnRead.HasDelegate` false → `Refresh()` |
| ValueMapper resolves remote pre-selection | Tags render for items not in Data | `OnParametersSetAsync` → `_selectedValues.Count > _selectedItems.Count` → `ValueMapper(unresolved)` → merge into `_selectedItems` |
| Two reads in flight | Older read cancelled | `_readCts.Cancel()` before assigning new CTS |

---

## RES-T4B5-02: MariloDateTimePicker typed input

### Files modified

- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`

### Changes

1. **Input attribute changes** (markup):
   - `value="@FormatValue()"` → `value="@_inputText"`
   - `readonly="true"` → `readonly="@ReadOnly"` (the `ReadOnly` parameter now actually does something)
   - `@onfocus="OpenPopup"` removed; only `@onclick="OpenPopup"` opens the popup
   - `@oninput="OnInputChanged"` added

2. **New private state**: `private string _inputText = string.Empty;`

3. **`OnInitialized` extended**: `_inputText = FormatValue() ?? string.Empty;` so the displayed text is correct on first render when `Value` is bound.

4. **New `OnParametersSet` override**: sync displayed text from external `Value` updates, but only when the user is not currently mid-edit. Specifically: if `_inputText` does not parse to the current `Value`, overwrite it with the formatted value. This preserves user typing across re-renders triggered by parent state changes.

5. **New `OnInputChanged(ChangeEventArgs)` handler**:
   - Empty string → clears `Value` to null and fires `ValueChanged`.
   - Calls `TryParseInput` to parse text. On success, clamps to Min/Max, updates `Value`, syncs `_hour/_minute/_second/_displayMonth`, fires `ValueChanged`.
   - On parse failure: `_inputText` retains the user's text but `Value` is unchanged.

6. **New `TryParseInput(string text, out DateTime? result)` helper**:
   - First tries `DateTime.TryParseExact(text, Format, CurrentCulture, None)` for strict format match.
   - Falls back to `DateTime.TryParse(text, CurrentCulture, None)` for tolerant input.

7. **`CommitValue`, `SetNow`, `ClearValue` extended**: each now updates `_inputText` after mutating `Value` so the input element reflects popup-driven changes.

### Verification (manual code-trace)

| Scenario | Expected | Trace |
|---|---|---|
| Render with no Value | Empty input text | `OnInitialized: _inputText = FormatValue() ?? "" = ""` |
| Render with Value + Format | Formatted text in input | `_inputText = "2026-05-20 14:30"` |
| Type valid date | Value updated, ValueChanged fired | `OnInputChanged → TryParseExact → success → clamp → Value = parsed` |
| Type invalid date | Value unchanged, _inputText keeps text | `TryParseExact false, TryParse false → no Value mutation` |
| Type date below Min | Value clamped to Min | `if (dt < Min) dt = Min` |
| Type date above Max | Value clamped to Max | `if (dt > Max) dt = Max` |
| ReadOnly=true | Browser blocks typing via attribute | `readonly="@ReadOnly"` emits readonly attribute when true |
| Clear input (empty string) | Value → null | `if (string.IsNullOrWhiteSpace(text))` → `Value = null; ValueChanged.InvokeAsync(null)` |
| Tab into input | Popup does NOT auto-open | `@onfocus` removed |
| Click input | Popup opens | `@onclick="OpenPopup"` preserved |
| Popup commits Value | _inputText syncs to formatted value | `CommitValue → _inputText = FormatValue()` |
| Click × clear button | _inputText cleared | `ClearValue → _inputText = string.Empty` |

### Behavior change documented

Removing `@onfocus="OpenPopup"` is a minor behavior change: previously, Tab-focusing the input opened the popup. With typed input enabled, that would steal focus and prevent typing. Click-to-open is the standard combobox-with-typing pattern. No existing test in `DateTimePickerTests.cs` uses `Focus()` (verified by grep), so no test regressions.

---

## Tests

### Added tests

#### `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` (5 new tests)

| Test | Purpose |
|------|---------|
| `OnRead_InvokedWhenDropdownOpens` | Initial lazy read fires on first open with empty filter; dropdown shows handler-supplied items |
| `Rebind_TriggersOnReadAgain` | `Rebind()` invokes OnRead a second time |
| `Rebind_WithoutOnRead_FallsBackToRefresh` | `Rebind()` is safe when OnRead is not bound |
| `ValueMapper_ResolvesPreSelectedRemoteValues` | Tag renders for an id not present in local Data, resolved via async ValueMapper |
| `OnRead_ReceivesFilterTextWhenUserTypes` | Typing filter text triggers a second OnRead invocation with the typed filter |

#### `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` (7 new tests)

| Test | Purpose |
|------|---------|
| `Input_IsNotReadOnlyByDefault` | Default `ReadOnly=false` produces no `readonly` attribute |
| `Input_RespectsReadOnlyParameter` | `ReadOnly=true` produces a `readonly` attribute |
| `TypedValidDate_UpdatesValue` | Typing a date in the configured format updates Value |
| `TypedInvalidDate_LeavesValueUnchanged` | Garbage input does not fire ValueChanged |
| `TypedDate_ClampedToMin` | Below-Min input clamps to Min |
| `TypedDate_ClampedToMax` | Above-Max input clamps to Max |
| `ClearingInput_ClearsValue` | Empty string clears Value |

### Test execution

Test runtime not executed in this session — `.NET SDK not available` per `_config/coverage-summary.md` Active Blockers. All 12 tests written following existing conventions in the same file (same `Render<>` API, same `MariloTestBase`, same selector strategies). Tests added to existing files alongside passing tests of identical shape.

Verification by code inspection only — runtime test pass/fail recorded as **pending** in the closure report, matching the convention used by Batch 4 and other recent batches.

---

## Files written

- `src/Marilo.Components/Forms/Inputs/MultiSelectModels.cs` (new)
- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`
- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`
- `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs`
- `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs`

## Files read (target project)

- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` (full read)
- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` (full read)
- `src/Marilo.Components/Forms/Inputs/UploadModels.cs` (pattern reference for new model file)
- `src/Marilo.Components/DataGrid/GridEventArgs.cs` (pattern reference for ReadEventArgs shape)
- `src/Marilo.Components/DataDisplay/MariloListView.razor` (pattern reference for Rebind/OnRead)
- `src/Marilo.Core/Data/DataRequest.cs` (checked for reuse — not used; MultiSelect args are simpler)
- `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` (existing tests for fixture style)
- `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` (existing tests for selector style)

## No opportunistic changes

Every modified file traces directly to GAP-MSEL-006 or GAP-DTP-003. No drive-by refactors, no formatting churn.

## Side-effect: closes deferred portion of GAP-MSEL-001

GAP-MSEL-001 (Core events missing) was previously **partially resolved** in Batch 1 — `OnOpen`, `OnClose`, `OnBlur` added, but `OnChange`, `OnRead`, `OnItemRender` deferred. This batch closes the `OnRead` portion. `OnChange` and `OnItemRender` remain open and are orthogonal — they belong in a future batch focused on item-level events.
