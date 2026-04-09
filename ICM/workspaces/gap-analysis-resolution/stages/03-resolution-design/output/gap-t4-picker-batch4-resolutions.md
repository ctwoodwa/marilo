# Resolution Records: T4 Pickers Batch 4 — GroupField + DateTimePickerSteps

> Date: 2026-04-08
> Source: `stages/01-intake/output/gap-t4-pickers-prioritization.md` (Batch 2 leftovers)
> Components: MariloMultiSelect, MariloDateTimePicker
> Scope: batch (related gaps in T4 pickers area; runs 01 → 02 → 03 → 05 → 06)

These were the two remaining "Next Actions" after Batch 3 closed (see `_status/workspace-status.md`):
1. T4 Pickers remaining: GroupField (MultiSelect), DateTimePickerSteps.

Both are pure C# additions, no JS interop, no third-party dependencies, no human decisions.

---

## RES-T4B4-01: MariloMultiSelect GroupField with sticky group headers

**Resolves:** GAP-MSEL-003
**Status:** Implemented

### Target Pattern

Add a single new parameter:

```csharp
/// <summary>
/// Property name on TItem used to group items in the dropdown.
/// When set, items are grouped by the property value and a sticky group header
/// is rendered above each group.
/// </summary>
[Parameter] public string? GroupField { get; set; }
```

When `GroupField` is non-empty, the non-virtualized dropdown body renders items as:

```
<div class="mar-multiselect__list-container">
  <div class="mar-multiselect__group-header">[Group label A]</div>
  <div role="option" ...>...</div>
  <div role="option" ...>...</div>
  <div class="mar-multiselect__group-header">[Group label B]</div>
  <div role="option" ...>...</div>
  ...
</div>
```

Group headers carry `position: sticky; top: 0; z-index: 1` from a small style attribute on the element so they remain visible during scroll without depending on a CSS provider change. Group key resolution mirrors the existing `GetText`/`GetValue` reflection helpers — a private `GetGroupKey(TItem)` returns the group property value as `string?` (null/empty groups fall under an unlabeled bucket and render as a header with empty text suppressed).

Filtering still applies first (`_filteredItems`), then grouping is computed inline in markup using `GroupBy(...).OrderBy(g => g.Key)`. The `_highlightedIndex` semantics are preserved — indices map to the flat `_filteredItems` order regardless of grouping, so keyboard navigation and `aria-activedescendant` stay correct.

When `GroupField` is null/empty, behavior is identical to today (no grouping, no header overhead).

### Options Considered

**Option A: Inline GroupBy in markup (chosen)**
- Approach: Add `GroupField` parameter; in the non-virtualized loop, group `_filteredItems` inline by reflected property and emit a header before each group.
- Pros: Minimal code change; no new state; preserves `_highlightedIndex` flat-index semantics; matches Telerik public API shape.
- Cons: Reflection per render (already done for TextField/ValueField, so consistent with current cost model).
- Effort: Small.

**Option B: Pre-grouped state with flat index map**
- Approach: Compute `_groupedItems` as `List<(string? key, List<TItem> items)>` in `OnParametersSet` and `ApplyFilter`; rebuild a parallel index map for highlight tracking.
- Pros: Avoids per-render LINQ.
- Cons: Two parallel data structures to keep in sync; risks index drift; more invasive change for a feature most consumers won't use.
- Effort: Medium.

**Option C: Defer to consumer via grouping template slot**
- Approach: Don't add GroupField; expose `ItemTemplate` and let consumers render their own group headers.
- Pros: Zero new API.
- Cons: Doesn't satisfy spec; group headers can't be sticky from a per-item template; consumers can't replicate the visual pattern Telerik delivers.
- Effort: None — but doesn't close the gap.

### Decision

**Chosen:** Option A.
**Rationale:** Smallest delta to satisfy the spec, keeps the existing `_filteredItems` flat-index model intact, and the reflection cost is negligible because the same pattern is already used for `TextField`/`ValueField`. Virtualized path is explicitly out of scope for this resolution — sticky headers under `Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize` require a different approach (overlay header tracking the visible window) and there is no current consumer asking for grouping + virtualization. Documented as a future enhancement, not a deferred gap.

### Consequences

- New `GroupField` parameter on `MariloMultiSelect` (additive, no breaking change).
- New private helper `GetGroupKey(TItem)` mirroring `GetText`/`GetValue` reflection.
- New CSS hook `mar-multiselect__group-header` — initial inline `style="position:sticky;..."` so styling works even before providers add the class. CSS providers may later add their own theming for this hook (out of scope here).
- Existing tests unchanged; new bUnit tests cover the grouping + ordering + ungrouped fallback paths.
- Virtualized path is unchanged — `EnableVirtualization=true` + `GroupField` set means virtualized rendering wins and no headers are emitted. A `// note:` code comment documents this.

### Success Criteria

- [x] `[Parameter] public string? GroupField { get; set; }` exists.
- [x] When `GroupField` is set, dropdown renders one `mar-multiselect__group-header` element per distinct group value, in alphabetical order.
- [x] When `GroupField` is null/empty, no group headers are rendered (existing behavior).
- [x] Flat option indices preserved — `aria-activedescendant` still resolves correctly.
- [x] Group headers carry sticky positioning style.
- [x] bUnit tests cover: grouped rendering, ungrouped fallback, distinct group count.

---

## RES-T4B4-02: MariloDateTimePicker tumbler step parameters

**Resolves:** GAP-DTP-002
**Status:** Implemented

### Target Pattern

Add three flat step parameters to `MariloDateTimePicker`, mirroring the existing `MariloTimePicker` pattern documented at `MariloTimePicker.razor:221`:

```csharp
// Step parameters (matching DateTimePickerSteps child component concept)
[Parameter] public int HourStep { get; set; } = 1;
[Parameter] public int MinuteStep { get; set; } = 1;
[Parameter] public int SecondStep { get; set; } = 1;
```

The existing tumbler increment/decrement methods change from incrementing by 1 to incrementing by the configured step, with modulo wrap-around preserved:

```csharp
private void IncrementHour() => _hour = (_hour + Math.Max(1, HourStep)) % 24;
private void DecrementHour() => _hour = (_hour - Math.Max(1, HourStep) + 24) % 24;
private void IncrementMinute() => _minute = (_minute + Math.Max(1, MinuteStep)) % 60;
private void DecrementMinute() => _minute = (_minute - Math.Max(1, MinuteStep) + 60) % 60;
private void IncrementSecond() => _second = (_second + Math.Max(1, SecondStep)) % 60;
private void DecrementSecond() => _second = (_second - Math.Max(1, SecondStep) + 60) % 60;
```

`Math.Max(1, ...)` defends against zero/negative step misconfiguration causing infinite loops or value freezes.

### Options Considered

**Option A: Flat HourStep / MinuteStep / SecondStep parameters (chosen)**
- Approach: Three plain `int` parameters on the parent component, used directly by the tumbler increment methods.
- Pros: Mirrors `MariloTimePicker` exactly (already documented as "matching TimePickerSteps child component concept"); no new types; no cascading-parameter wiring; consumers don't need a child component.
- Cons: Flat parameters look slightly less Telerik-shaped than a `<DateTimePickerSteps>` child tag.
- Effort: Tiny.

**Option B: New `DateTimePickerSteps` child component with `CascadingValue`**
- Approach: Build a `DateTimePickerSteps : ComponentBase` child that registers itself with a cascading `MariloDateTimePicker` parent on `OnInitialized`.
- Pros: Visually closer to the Telerik markup shape `<DateTimePickerSteps Hour="2" Minute="15" />`.
- Cons: Adds a child component, parent registration plumbing, lifecycle ordering risk (child registers after first parent render — see the cerebrum entry on `MariloWizard CascadingValue bug`); MariloTimePicker already chose flat parameters, so introducing the child component here would split the pattern in half.
- Effort: Medium.

**Option C: Both flat parameters AND child component**
- Approach: Flat parameters work standalone; child component sets the same fields via cascading registration.
- Pros: Maximum API surface.
- Cons: Two ways to do the same thing; documentation churn; higher test surface; YAGNI — no consumer has asked for the child component shape.
- Effort: Medium-High.

### Decision

**Chosen:** Option A.
**Rationale:** Consistency with `MariloTimePicker` (which uses the exact same pattern at `MariloTimePicker.razor:222-224`) is more valuable than visual fidelity to a Telerik child-tag shape that nobody is migrating from. The cerebrum already documents a `CascadingValue` bug class on `MariloWizard` — avoiding new cascading registrations on input components reduces that risk. If a future consumer demands the child-tag shape, it can be layered on top of the flat parameters as a thin wrapper without breaking anyone.

### Consequences

- Three new parameters on `MariloDateTimePicker` (additive, no breaking change).
- Six existing tumbler increment/decrement methods updated to use the step.
- `Math.Max(1, step)` defensive normalization prevents zero/negative configuration from breaking the tumblers.
- `MariloTimePicker` parity confirmed — both pickers now expose `HourStep` / `MinuteStep` / `SecondStep`.
- bUnit tests cover the step propagation through both increment and decrement directions.

### Success Criteria

- [x] `HourStep`, `MinuteStep`, `SecondStep` parameters exist on `MariloDateTimePicker`.
- [x] All three default to `1` (existing behavior unchanged).
- [x] Increment/decrement methods respect the configured step.
- [x] Modulo wrap-around preserved (24h/60m/60s).
- [x] Zero/negative step values clamped to 1.
- [x] bUnit tests cover step increment, step decrement, and default behavior.

---

## Cross-cutting notes

- **Scope:** batch (skips Stage 04 remediation plan per workspace `Gap Scope Routing`).
- **Test ownership:** new tests live in `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` and `tests/Marilo.Tests.Unit/Selection/DateTimePickerTests.cs` next to existing T4 picker tests.
- **No CSS provider changes** required by either resolution. Group headers use inline sticky style; the existing `IMariloCssProvider.MultiSelectPopupClass()` method still wraps the popup.
- **No documentation changes** owned by this batch — gap closure tracked in the closure report only.
