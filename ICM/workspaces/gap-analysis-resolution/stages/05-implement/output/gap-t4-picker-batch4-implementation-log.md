# Implementation Log: T4 Pickers Batch 4 — GroupField + DateTimePickerSteps

> Date: 2026-04-08
> Resolutions: `stages/03-resolution-design/output/gap-t4-picker-batch4-resolutions.md`
> Components: `MariloMultiSelect`, `MariloDateTimePicker`
> Scope: batch (Stage 04 skipped per workspace gap-scope routing)

---

## RES-T4B4-01: MariloMultiSelect.GroupField

### Files modified

- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`

### Changes

1. **New parameter** (`MariloMultiSelect.razor` `@code` block, alongside `AllowCustom`):
   ```csharp
   /// <summary>
   /// Property name on TItem used to group items in the dropdown. When set, items
   /// are grouped alphabetically by the property value and a sticky group header
   /// is rendered above each group. Ignored when EnableVirtualization is true.
   /// </summary>
   [Parameter] public string? GroupField { get; set; }
   ```

2. **New helper** (`MariloMultiSelect.razor`, alongside `GetText`/`GetValue`):
   ```csharp
   private string? GetGroupKey(TItem item)
   {
       if (item is null || string.IsNullOrEmpty(GroupField)) return null;
       return typeof(TItem).GetProperty(GroupField)?.GetValue(item)?.ToString();
   }
   ```

3. **Rewritten non-virtualized list loop** to support grouping:
   - Replaced the flat `@for (int i = 0; i < _filteredItems.Count; i++)` with a precomputed `orderedIndices` list.
   - When `GroupField` is set, `orderedIndices` sorts the flat indices by the reflected group key (`StringComparer.CurrentCultureIgnoreCase`).
   - When `GroupField` is not set, `orderedIndices` is `0..Count` (no behavior change).
   - A `lastGroupKey` tracker emits a `<div class="mar-multiselect__group-header">` whenever the key changes.
   - Group header carries inline `style="position:sticky;top:0;z-index:1;background:inherit;"` so it works without provider CSS changes.
   - Original `_highlightedIndex` semantics preserved — option `id` and `aria-activedescendant` still use the flat `_filteredItems` index.
   - Virtualized path is unchanged. A code comment documents that `EnableVirtualization=true` ignores `GroupField`.

### Verification (manual code-trace)

- Grouped path (`GroupField="Region"`, 5 items across 3 regions): produces 3 group headers + 5 options. Indices into `_filteredItems` preserved.
- Ungrouped path (default): zero group headers, identical to pre-change rendering. Manual diff confirms identical option markup.
- Empty `_filteredItems`: `orderedIndices` is empty list, no headers, falls through to existing "no items found" rendering.
- Filter applied first (`ApplyFilter()` runs in `OnParametersSet`), grouping operates on `_filteredItems` after filtering — correct precedence.

---

## RES-T4B4-02: MariloDateTimePicker tumbler step parameters

### Files modified

- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`

### Changes

1. **Three new parameters** (`MariloDateTimePicker.razor` `@code` block, after `AdaptiveMode`):
   ```csharp
   // Tumbler step parameters (matching DateTimePickerSteps child component concept).
   // Mirrors MariloTimePicker.razor:222-224 to keep both pickers consistent.

   /// <summary>Increment step for the hour tumbler. Defaults to 1. Values less than 1 are clamped to 1.</summary>
   [Parameter] public int HourStep { get; set; } = 1;

   /// <summary>Increment step for the minute tumbler. Defaults to 1. Values less than 1 are clamped to 1.</summary>
   [Parameter] public int MinuteStep { get; set; } = 1;

   /// <summary>Increment step for the second tumbler. Defaults to 1. Values less than 1 are clamped to 1.</summary>
   [Parameter] public int SecondStep { get; set; } = 1;
   ```

2. **Three private clamp helpers** (clamp zero/negative to 1):
   ```csharp
   private int HourStepClamped => Math.Max(1, HourStep);
   private int MinuteStepClamped => Math.Max(1, MinuteStep);
   private int SecondStepClamped => Math.Max(1, SecondStep);
   ```

3. **Six tumbler increment/decrement methods** updated to use clamped step:
   ```csharp
   private void IncrementHour() => _hour = (_hour + HourStepClamped) % 24;
   private void DecrementHour() => _hour = (_hour - HourStepClamped + 24) % 24;
   private void IncrementMinute() => _minute = (_minute + MinuteStepClamped) % 60;
   private void DecrementMinute() => _minute = (_minute - MinuteStepClamped + 60) % 60;
   private void IncrementSecond() => _second = (_second + SecondStepClamped) % 60;
   private void DecrementSecond() => _second = (_second - SecondStepClamped + 60) % 60;
   ```

### Verification (manual code-trace)

- `HourStep=3`, current 10 → `(10+3)%24 = 13` ✓
- `HourStep=4`, current 10 → `(10-4+24)%24 = 6` ✓
- `MinuteStep=15`, current 0 → `(0+15)%60 = 15` ✓
- `MinuteStep=10`, current 5 → `(5-10+60)%60 = 55` ✓ (wrap)
- `SecondStep=30`, current 0 → `(0+30)%60 = 30` ✓
- Default `HourStep=1`, current 30 → `31` ✓ (no behavior change)
- `HourStep=0`, clamped to 1, current 10 → `11` ✓ (defends against freeze)

---

## Tests

### Added tests

#### `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` (5 new tests)

| Test | Purpose |
|------|---------|
| `GroupField_RendersGroupHeadersForEachDistinctValue` | Verifies one header per distinct group value |
| `GroupField_HeadersAreOrderedAlphabetically` | Verifies header ordering (Americas, Asia, Europe) |
| `GroupField_NotSet_RendersNoGroupHeaders` | Verifies opt-in behavior — default has no headers |
| `GroupField_PreservesAllItemsAcrossGroups` | Verifies all items still render as options under headers |
| `GroupField_HeaderHasStickyPositioning` | Verifies inline sticky CSS on group header |

Test fixture: `GroupedCountry` record with `Region` field; 5 countries across 3 regions.

#### `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` (7 new tests)

| Test | Purpose |
|------|---------|
| `HourStep_IncrementJumpsByConfiguredAmount` | `HourStep=3`, 10 → 13 |
| `HourStep_DecrementJumpsByConfiguredAmount` | `HourStep=4`, 10 → 6 |
| `MinuteStep_IncrementJumpsByConfiguredAmount` | `MinuteStep=15`, 0 → 15 |
| `MinuteStep_DecrementJumpsByConfiguredAmountWithWrap` | `MinuteStep=10`, 5 → 55 (wrap) |
| `SecondStep_IncrementJumpsByConfiguredAmount` | `SecondStep=30`, 0 → 30 |
| `StepDefaults_IncrementByOneWhenNotConfigured` | Default behavior preserved |
| `HourStep_ZeroIsClampedToOne` | Defends against zero/negative misconfiguration |

### Test execution

Test runtime not executed in this session — `.NET SDK not available` per `_config/coverage-summary.md` Active Blockers ("Cannot run `dotnet test` to verify test pass/fail"). All 12 tests are written following existing conventions in the same file (same `Render<>` API, same `MariloTestBase`, same selector strategies). Tests added to existing files alongside passing tests of identical shape.

Verification by code inspection only this session — runtime test pass/fail recorded as **pending** in the closure report, matching the tracking convention used for splitter / wizard / chart-batch1 / chart-batch2 / editor-batch1 / datagrid-phase1 / datagrid-phase2 batches in `_config/coverage-summary.md`.

---

## Files written

- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`
- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`
- `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs`
- `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs`

## Files read (target project)

- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` (full read)
- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` (full read)
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor` (sections to confirm step pattern)
- `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` (existing tests for fixture style)
- `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` (existing tests for selector style)

## No opportunistic changes

Every modified file traces directly to GAP-MSEL-003 or GAP-DTP-002. No drive-by refactors, no formatting churn, no style-only edits.
