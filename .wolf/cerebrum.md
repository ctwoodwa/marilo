# Cerebrum

> OpenWolf's learning memory. Updated automatically as the AI learns from interactions.
> Do not edit manually unless correcting an error.
> Last updated: 2026-04-03

## User Preferences

<!-- How the user likes things done. Code style, tools, patterns, communication. -->

## Key Learnings

- **Project:** marilo
- **Description:** Marilo - Provider-first Blazor component library
- **PopupEventArgs pattern:** All popup-bearing components should use `PopupEventArgs` from `Marilo.Core.Models` for cancellable `OnOpen`/`OnClose` events. Existing `ColorPickerOpenEventArgs`/`ColorPickerCloseEventArgs` are legacy — new components use the shared type.
- **_Imports.razor includes Marilo.Core.Models:** No per-file `@using` needed for model types.
- **MultiSelect input area vs toggle:** `OnInputAreaClick` only opens (not toggles). The arrow button (`.mar-multiselect__arrow`) toggles. Overlay (`.mar-multiselect__overlay`) closes.
- **DateTimePicker/DateRangePicker open pattern:** Both use `@onfocus`/`@onclick` on the input element (no toggle button). Tests should click `input` to open.
- **TimePicker Set button class:** `.mar-timepicker__btn--set` (not `__set-btn`)
- **MariloWizard has CascadingValue bug:** WizardStep expects cascading `MariloWizard` parent but parent never provides one — step registration is fundamentally broken (GAP-WIZARD-018, Critical)
- **DataGrid naming mismatch:** Spec says `MariloGrid`/`GridColumn`, code says `MariloDataGrid`/`MariloGridColumn` — blocking gap for CDW handoff

## Do-Not-Repeat

<!-- Mistakes made and corrected. Each entry prevents the same mistake recurring. -->
<!-- Format: [YYYY-MM-DD] Description of what went wrong and what to do instead. -->
- [2026-04-03] When writing bUnit tests, always check the actual component markup for CSS class names before assuming selectors (e.g., `.mar-timepicker__set-btn` vs `.mar-timepicker__btn--set`). Read the component's markup template first.
- [2026-04-03] When making a sync method async (e.g., `OpenDropdown()` → `async Task OpenDropdown()`), audit ALL callers in the file — event handlers, other methods, public API methods. Every call site needs `await`.

## Decision Log

<!-- Significant technical decisions with rationale. Why X was chosen over Y. -->
- [2026-04-03] **Shared PopupEventArgs over per-component args (RES-T4B1-001):** All popup components use the same `PopupEventArgs` class rather than component-specific args types. Rationale: identical lifecycle needs (open/close/cancel), reduces type proliferation, consistent consumer API. Existing `ColorPickerOpenEventArgs`/`ColorPickerCloseEventArgs` retained for backward compat.
- [2026-04-03] **TimePicker OnOpen/OnClose breaking change:** Upgraded from `EventCallback` (no args) to `EventCallback<PopupEventArgs>`. This is a minor breaking change for existing consumers but necessary for cancellation support. Documented in closure report.
