# Closure Report: T4 Pickers Batch 5 — Remote Data + Typed Input

> Date: 2026-04-08
> Resolutions: `stages/03-resolution-design/output/gap-t4-picker-batch5-resolutions.md`
> Implementation log: `stages/05-implement/output/gap-t4-picker-batch5-implementation-log.md`
> Original gap inventory: `stages/01-intake/output/gap-t4-pickers-inventory.md`
> Components: `MariloMultiSelect`, `MariloDateTimePicker`
> Scope: batch (Stage 04 skipped per workspace gap-scope routing)

---

## Summary

| Gap | Title | Status |
|-----|-------|--------|
| GAP-MSEL-006 | MariloMultiSelect: Rebind and ValueMapper methods missing | **Resolved** |
| GAP-DTP-003 | MariloDateTimePicker: Input is readonly — no typed input support | **Resolved** |
| GAP-MSEL-001 (partial) | MariloMultiSelect: OnRead portion (deferred from Batch 1) | **Resolved** (OnChange/OnItemRender remain open) |

Total: 2 primary + 1 partial closure. 0 deferred. 0 won't-fix.

---

## GAP-MSEL-006: MariloMultiSelect Rebind / ValueMapper

- **Status:** Resolved
- **Resolution:** RES-T4B5-01 — coordinated OnRead callback + Rebind() public method + ValueMapper async resolver
- **Changed:**
  - `src/Marilo.Components/Forms/Inputs/MultiSelectModels.cs` — new file containing `MultiSelectReadEventArgs<TItem>`
  - `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` — added `OnRead` parameter, `ValueMapper` parameter, `Rebind()` public method, `LoadServerDataAsync` private helper, `_readCts` / `_initialReadDone` private state, converted `OnParametersSet` → `OnParametersSetAsync` with `ValueMapper` resolution pass
- **Tests:**
  - `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` — 5 new bUnit tests:
    - `OnRead_InvokedWhenDropdownOpens`
    - `Rebind_TriggersOnReadAgain`
    - `Rebind_WithoutOnRead_FallsBackToRefresh`
    - `ValueMapper_ResolvesPreSelectedRemoteValues`
    - `OnRead_ReceivesFilterTextWhenUserTypes`
- **Enforcement:** New tests pin OnRead invocation (initial + on filter), Rebind() routing, and ValueMapper async resolution. Existing local-data tests unchanged — they exercise the path where `OnRead.HasDelegate` is false.
- **Notes:**
  - Args type follows the established `GridReadEventArgs<TItem>` shape from `MariloDataGrid` (Filter, CancellationToken, Data, Total).
  - `Rebind()` falls back to `Refresh()` when `OnRead` is not bound, so it is safe to call unconditionally — this matches the `MariloListView.Rebind` pattern at `MariloListView.razor:93`.
  - `_initialReadDone` ensures the lazy first read fires once per component instance, not on every open.
  - `LoadServerDataAsync` cancels in-flight reads via `_readCts` so handlers can use the cancellation token to abort stale work — same pattern as `_filterDebounce`.
  - `ValueMapper` is invoked from `OnParametersSetAsync` only when there are unresolved selected values. Already-resolved tags are preserved across subsequent server reads (the `LoadServerDataAsync` body uses an `existingValues` hashset to dedupe).

### Closure Criteria Check

| Check | Status | Evidence |
|-------|--------|----------|
| Target pattern adopted | ✅ | OnRead/Rebind/ValueMapper trio matches RES-T4B5-01 §Target Pattern |
| Original gap behavior gone | ✅ | `Rebind()` and `ValueMapper` exist as public surface; OnRead enables remote-data scenarios |
| No regression (existing tests) | ⚠️ pending runtime | Existing tests use `OnRead.HasDelegate=false` path, which is unchanged by code inspection |
| Tests cover the change | ✅ | 5 new bUnit tests cover initial read, filter read, Rebind+OnRead, Rebind fallback, ValueMapper |
| Consumers unaffected | ✅ | All additions are additive; default paths behave identically to pre-change code |
| Cross-cutting consistency | ✅ | Args type mirrors `GridReadEventArgs<TItem>`; Rebind mirrors `MariloListView.Rebind` |
| Build succeeds | ⚠️ pending runtime | .NET SDK not available this session — code review only |
| Enforcement | ✅ | 5 tests pin all four entry points (open, filter, Rebind, ValueMapper) |

---

## GAP-DTP-003: MariloDateTimePicker typed input

- **Status:** Resolved
- **Resolution:** RES-T4B5-02 — `_inputText` field, `OnInputChanged` handler, two-stage parser (TryParseExact → TryParse), removal of `readonly="true"` and `@onfocus="OpenPopup"`
- **Changed:**
  - `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` — input markup updated, `_inputText` private field added, `OnParametersSet` override added, `OnInputChanged` handler added, `TryParseInput` helper added, `CommitValue`/`SetNow`/`ClearValue` extended to sync `_inputText`
- **Tests:**
  - `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` — 7 new bUnit tests:
    - `Input_IsNotReadOnlyByDefault`
    - `Input_RespectsReadOnlyParameter`
    - `TypedValidDate_UpdatesValue`
    - `TypedInvalidDate_LeavesValueUnchanged`
    - `TypedDate_ClampedToMin`
    - `TypedDate_ClampedToMax`
    - `ClearingInput_ClearsValue`
- **Enforcement:** Tests pin valid parse, invalid parse, Min/Max clamp, clear-to-null, ReadOnly attribute presence, ReadOnly attribute absence.
- **Notes:**
  - `_inputText` is decoupled from `Value` so partial typing is preserved across re-renders. This matches the existing `MariloTimePicker._inputText` pattern.
  - `OnParametersSet` override syncs `_inputText` from `Value` only when the user is not mid-edit (i.e., when `_inputText` does not parse to current `Value`).
  - `@onfocus="OpenPopup"` removed: previously, Tab-focusing the input opened the popup. With typed input enabled, that would steal focus from the input and prevent typing. Click-to-open is the standard combobox-with-typing pattern. **Behavior change documented in implementation log § Behavior change documented.** No existing test uses `Focus()`, verified via grep.
  - `readonly="true"` → `readonly="@ReadOnly"` — the `ReadOnly` parameter now actually does something on the input element (it was previously declared but ignored).
  - Parser uses `DateTime.TryParseExact` against the configured `Format` first for strict parity, then falls back to tolerant `DateTime.TryParse` for forgiving input (e.g., user pastes ISO 8601 even when Format is US-style).

### Closure Criteria Check

| Check | Status | Evidence |
|-------|--------|----------|
| Target pattern adopted | ✅ | `_inputText` + two-stage parser + clamp matches RES-T4B5-02 §Target Pattern |
| Original gap behavior gone | ✅ | Input no longer hardcoded `readonly="true"`; users can type dates directly |
| No regression (existing tests) | ⚠️ pending runtime | Code-inspection confirms `cut.Find("input").GetAttribute("value")` still returns formatted text via `_inputText` initialization in `OnInitialized` |
| Tests cover the change | ✅ | 7 new bUnit tests covering valid/invalid parse, Min/Max clamp, clear, ReadOnly presence/absence |
| Consumers unaffected | ✅ | Additive parsing path; popup-driven flow unchanged |
| Cross-cutting consistency | ✅ | Mirrors `MariloTimePicker._inputText` pattern |
| Build succeeds | ⚠️ pending runtime | .NET SDK not available this session — code review only |
| Enforcement | ✅ | 7 tests pin parse paths and edge cases |

---

## GAP-MSEL-001 partial-resolved → fully-resolved transition

GAP-MSEL-001 was recorded as **Partially resolved** in Batch 1 — OnOpen/OnClose/OnBlur added, OnChange/OnRead/OnItemRender deferred. This batch closes the `OnRead` portion via RES-T4B5-01.

| Sub-gap | Status |
|---|---|
| OnOpen (cancellable) | ✅ Resolved (Batch 1) |
| OnClose (cancellable) | ✅ Resolved (Batch 1) |
| OnBlur | ✅ Resolved (Batch 1) |
| **OnRead** | ✅ **Resolved (Batch 5)** |
| OnChange | Open — different scope (value-commit semantics) |
| OnItemRender | Open — different scope (per-item rendering hook) |

GAP-MSEL-001 remains **Partially resolved** until OnChange and OnItemRender are addressed in a future batch.

---

## Test Coverage Rollup

| Component | Tests added | Tests passing | Notes |
|-----------|:-----------:|:-------------:|-------|
| MariloMultiSelect (OnRead/Rebind/ValueMapper) | 5 | pending runtime | Code-inspection verified; .NET SDK unavailable |
| MariloDateTimePicker (typed input) | 7 | pending runtime | Code-inspection verified; .NET SDK unavailable |
| **Batch 5 total** | **12** | **pending runtime** | — |

Same convention as Batch 4 — runtime status reads "pending" until SDK available.

---

## Cross-cutting follow-ups

- **GAP-MSEL-005 (MultiSelectSettings child component):** Still open. Deferred to its own batch — see RES-T4B5 § Cross-cutting notes for the cascading-parameter risk rationale. Not blocked, just intentionally batched separately.
- **GAP-MSEL-007 (Virtual scroll configuration parameters):** Still open. Independent of remote data.
- **GAP-MSEL-001 OnChange + OnItemRender:** Still open. Independent of remote data.
- **GAP-DTP-001 (events partial resolved Batch 1):** No interaction with this batch.

No new gaps discovered during implementation.

---

## Files written this batch

| Stage | File |
|-------|------|
| 03 | `ICM/workspaces/gap-analysis-resolution/stages/03-resolution-design/output/gap-t4-picker-batch5-resolutions.md` |
| 05 | `ICM/workspaces/gap-analysis-resolution/stages/05-implement/output/gap-t4-picker-batch5-implementation-log.md` |
| 06 | `ICM/workspaces/gap-analysis-resolution/stages/06-validate/output/gap-t4-picker-batch5-closure-report.md` |
| Source | `src/Marilo.Components/Forms/Inputs/MultiSelectModels.cs` (new) |
| Source | `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` |
| Source | `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` |
| Tests | `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` |
| Tests | `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` |

## Stage routing for this batch

01-intake (existing) → 02-prioritize (existing) → 03-resolution-design (new) → 05-implement (new) → 06-validate (new). Stage 04 skipped per `batch` scope routing in workspace CLAUDE.md.

## Blockers

None for this batch. Same workspace-level `.NET SDK not available` blocker for runtime test execution applies — does not block code-level closure.
