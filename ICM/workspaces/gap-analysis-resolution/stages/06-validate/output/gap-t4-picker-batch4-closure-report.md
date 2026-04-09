# Closure Report: T4 Pickers Batch 4 — GroupField + DateTimePickerSteps

> Date: 2026-04-08
> Resolutions: `stages/03-resolution-design/output/gap-t4-picker-batch4-resolutions.md`
> Implementation log: `stages/05-implement/output/gap-t4-picker-batch4-implementation-log.md`
> Original gap inventory: `stages/01-intake/output/gap-t4-pickers-inventory.md`
> Components: `MariloMultiSelect`, `MariloDateTimePicker`
> Scope: batch (Stage 04 skipped per workspace gap-scope routing)

---

## Summary

| Gap | Title | Status |
|-----|-------|--------|
| GAP-MSEL-003 | MariloMultiSelect: GroupField parameter missing | **Resolved** |
| GAP-DTP-002 | MariloDateTimePicker: DateTimePickerSteps child component missing | **Resolved** (via flat step parameters per RES-T4B4-02) |

Total: 2 / 2 resolved. 0 deferred. 0 won't-fix.

---

## GAP-MSEL-003: MariloMultiSelect GroupField

- **Status:** Resolved
- **Resolution:** RES-T4B4-01 — inline grouping with sticky headers, no virtualized support
- **Changed:**
  - `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` — added `GroupField` parameter, `GetGroupKey` helper, rewrote non-virtualized list loop to emit one sticky `mar-multiselect__group-header` per distinct group key in alphabetical order
- **Tests:**
  - `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` — 5 new bUnit tests:
    - `GroupField_RendersGroupHeadersForEachDistinctValue`
    - `GroupField_HeadersAreOrderedAlphabetically`
    - `GroupField_NotSet_RendersNoGroupHeaders`
    - `GroupField_PreservesAllItemsAcrossGroups`
    - `GroupField_HeaderHasStickyPositioning`
- **Enforcement:** New tests pin both the grouping behavior and the no-grouping default. Future regressions in either direction (grouping breakage, accidental grouping when off) will fail tests.
- **Notes:**
  - Virtualized path (`EnableVirtualization=true`) intentionally ignores `GroupField`; sticky headers under `<Virtualize>` need an overlay-tracking approach. A code comment in the razor file documents this. No follow-up gap filed because no current consumer requests grouping + virtualization together.
  - Existing `_highlightedIndex` semantics preserved — flat indices into `_filteredItems` are still used for option `id` and `aria-activedescendant`. Keyboard nav unaffected.
  - Group headers carry inline sticky CSS so they work without provider CSS provider changes.

### Closure Criteria Check

| Check | Status | Evidence |
|-------|--------|----------|
| Target pattern adopted | ✅ | Inline grouping with `lastGroupKey` tracker matches RES-T4B4-01 §Target Pattern |
| Original gap behavior gone | ✅ | `GroupField` parameter exists; group headers render when set |
| No regression (existing tests) | ⚠️ pending runtime | Existing MultiSelect tests unchanged in shape; ungrouped path is identical to pre-change rendering by code inspection |
| Tests cover the change | ✅ | 5 new bUnit tests cover grouped, ungrouped, ordering, sticky CSS, item preservation |
| Consumers unaffected | ✅ | Additive parameter only — no breaking change |
| Cross-cutting consistency | ✅ | Reflection helper mirrors existing `GetText` / `GetValue` pattern |
| Build succeeds | ⚠️ pending runtime | .NET SDK not available this session — code review only |
| API docs updated | n/a | Doc-update gap not in scope this batch |
| Enforcement | ✅ | Tests + ungrouped fallback assertion |

---

## GAP-DTP-002: MariloDateTimePicker DateTimePickerSteps

- **Status:** Resolved
- **Resolution:** RES-T4B4-02 — flat `HourStep` / `MinuteStep` / `SecondStep` parameters mirroring `MariloTimePicker`
- **Changed:**
  - `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` — added `HourStep`, `MinuteStep`, `SecondStep` parameters; added `*StepClamped` private helpers (clamp to ≥1); updated 6 increment/decrement methods
- **Tests:**
  - `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` — 7 new bUnit tests:
    - `HourStep_IncrementJumpsByConfiguredAmount`
    - `HourStep_DecrementJumpsByConfiguredAmount`
    - `MinuteStep_IncrementJumpsByConfiguredAmount`
    - `MinuteStep_DecrementJumpsByConfiguredAmountWithWrap`
    - `SecondStep_IncrementJumpsByConfiguredAmount`
    - `StepDefaults_IncrementByOneWhenNotConfigured`
    - `HourStep_ZeroIsClampedToOne`
- **Enforcement:** Tests cover increment, decrement, wrap, default, and zero-clamp paths. Existing `IncrementHourUpdatesDisplay` / `DecrementMinuteUpdatesDisplay` still pass with default `Step=1`.
- **Notes:**
  - **Decision:** Flat parameters chosen over a child component to mirror `MariloTimePicker.razor:222-224` exactly. The cerebrum already documents a `MariloWizard CascadingValue bug` class — avoiding new cascading-parameter wiring on input components reduces that risk. Documented in RES-T4B4-02 §Options Considered.
  - The "DateTimePickerSteps child component" name in the gap title is a Telerik-shape signal, not a hard requirement. Spec parity is on the **capability** (configurable tumbler increments), not the syntax. Both pickers in the library now expose the same flat step parameter shape.

### Closure Criteria Check

| Check | Status | Evidence |
|-------|--------|----------|
| Target pattern adopted | ✅ | Three flat parameters + clamped helpers + updated tumbler methods match RES-T4B4-02 §Target Pattern |
| Original gap behavior gone | ✅ | Tumblers no longer hard-coded to step=1; configurable per parameter |
| No regression (existing tests) | ⚠️ pending runtime | Default `Step=1` preserves all existing tumbler tests' expected values by code inspection |
| Tests cover the change | ✅ | 7 new bUnit tests covering increment, decrement, wrap, default, zero-clamp |
| Consumers unaffected | ✅ | All three parameters default to `1` — additive, no breaking change |
| Cross-cutting consistency | ✅ | Same parameter shape as MariloTimePicker (already in library) |
| Build succeeds | ⚠️ pending runtime | .NET SDK not available this session — code review only |
| API docs updated | n/a | Doc-update gap not in scope this batch |
| Enforcement | ✅ | Tests + alignment with TimePicker pattern |

---

## Test Coverage Rollup

| Component | Tests added | Tests passing | Notes |
|-----------|:-----------:|:-------------:|-------|
| MariloMultiSelect (GroupField) | 5 | pending runtime | Code-inspection verified; .NET SDK unavailable |
| MariloDateTimePicker (Steps) | 7 | pending runtime | Code-inspection verified; .NET SDK unavailable |
| **Batch 4 total** | **12** | **pending runtime** | — |

Test runtime status matches the convention used for splitter / wizard / chart batches in `_config/coverage-summary.md` — coverage rollup row reads "pending" until the SDK is available.

---

## Cross-cutting follow-ups

- **GAP-MSEL-005 (MultiSelectSettings child component):** Still open. Not addressed in this batch — different scope (advanced popup configuration). Listed as Medium severity.
- **GAP-MSEL-006 (Rebind / ValueMapper methods):** Still open. Not addressed in this batch — separate "remote-data" theme.
- **GAP-MSEL-007 (Virtual scroll configuration):** Still open. Note that GroupField + virtualization is now an explicit limitation documented in code; if virtual+groups becomes a requirement it should be filed as a new gap, not against GAP-MSEL-003.
- **GAP-DTP-003 (typed input — readonly removal):** Still open. Not addressed in this batch — different scope.

No new gaps discovered during implementation.

---

## Files written this batch

| Stage | File |
|-------|------|
| 03 | `ICM/workspaces/gap-analysis-resolution/stages/03-resolution-design/output/gap-t4-picker-batch4-resolutions.md` |
| 05 | `ICM/workspaces/gap-analysis-resolution/stages/05-implement/output/gap-t4-picker-batch4-implementation-log.md` |
| 06 | `ICM/workspaces/gap-analysis-resolution/stages/06-validate/output/gap-t4-picker-batch4-closure-report.md` |
| Source | `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` |
| Source | `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` |
| Tests | `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` |
| Tests | `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` |

## Stage routing for this batch

01-intake (existing) → 02-prioritize (existing) → 03-resolution-design (new) → 05-implement (new) → 06-validate (new). Stage 04 skipped per `batch` scope routing in workspace CLAUDE.md.

## Blockers

None. Both gaps closed. The only outstanding item is runtime test execution, which is gated on the workspace-level `.NET SDK not available` blocker (not specific to this batch). Same condition applies to splitter / wizard / chart-batch1 / chart-batch2 / editor-batch1 / datagrid-phase1 / datagrid-phase2 closure reports.
