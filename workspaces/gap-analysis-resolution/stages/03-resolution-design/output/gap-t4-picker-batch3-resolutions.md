# Resolution Records: T4 Pickers Batch 3 — Cross-Cutting Polish

> Date: 2026-04-05
> Source: `stages/01-intake/output/gap-t4-pickers-prioritization.md` Batch 3
> Components: MariloColorPicker, MariloDateRangePicker, MariloDateTimePicker, MariloTimePicker, MariloFileUpload, MariloUpload, MariloMultiSelect

---

## RES-T4B3-01: Shared AdaptiveMode enum + parameter on all 7 pickers

**Status:** Ready for implementation

### Target Pattern

Create shared `AdaptiveMode` enum (None, Auto) in `Marilo.Core.Enums.ComponentEnums`. Add `[Parameter] public AdaptiveMode AdaptiveMode { get; set; } = AdaptiveMode.None;` to all 7 T4 pickers.

### Decision

Use a shared enum (not per-component enums like `AutoCompleteAdaptiveMode`). The shared enum unifies the API across pickers. Default `None` preserves existing behavior. The parameter is additive — no rendering changes yet. Rendering behavior for `Auto` mode will be implemented when responsive layout infrastructure is added.

### Success Criteria
- [ ] `AdaptiveMode` enum with None and Auto values
- [ ] All 7 pickers accept the parameter
- [ ] Default is None (no behavior change)

---

## RES-T4B3-02: ARIA combobox pattern on popup-based pickers

**Status:** Ready for implementation

### Target Pattern

Add WAI-ARIA 1.2 combobox attributes to inputs on DateRangePicker, DateTimePicker, and TimePicker:
- `role="combobox"` on the text input
- `aria-haspopup="dialog"` (upgrade from `"true"`)
- `aria-controls="<popup-id>"` when popup is open
- Unique `id` on popup div for `aria-controls` targeting

### Decision

Generate popup IDs via `$"mar-{prefix}-{Guid.NewGuid():N}"`. Only set `aria-controls` when popup is open (null when closed). MultiSelect already has full combobox ARIA. ColorPicker uses button trigger, not combobox.

### Success Criteria
- [ ] TimePicker input has `role="combobox"` and `aria-controls`
- [ ] DateTimePicker input has `role="combobox"` and `aria-controls`
- [ ] DateRangePicker both inputs have `role="combobox"` and `aria-controls`
- [ ] Popup divs have unique `id` attributes

---

## RES-T4B3-03: CSS provider methods for DateRangePicker and DateTimePicker

**Status:** Ready for implementation

### Target Pattern

Add to `IMariloCssProvider`:
```csharp
string DateRangePickerClass();
string DateRangePickerPopupClass();
string DateTimePickerClass();
string DateTimePickerPopupClass();
```

Implement in FluentUI and Bootstrap providers with `mar-date-range-picker` / `mar-datetime-picker` CSS class names (matching existing markup).

### Success Criteria
- [ ] 4 new methods in IMariloCssProvider
- [ ] FluentUI provider implements all 4
- [ ] Bootstrap provider implements all 4
- [ ] Class names match existing component markup

---

## RES-T4B3-04: MaxVisibleTags vs MaxAllowedTags naming

**Status:** Won't fix

### Analysis

Spec references `MaxAllowedTags` but code uses `MaxVisibleTags`. The code name is more accurate — it controls how many tags are *visible* before showing a summary count, not how many can be selected. Users can still select unlimited items.

### Decision: Keep `MaxVisibleTags`

The current name is more descriptive and self-documenting. No change needed.
