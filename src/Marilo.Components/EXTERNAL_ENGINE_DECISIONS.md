# External Engine Decision Matrix

Approved library/engine selections for deferred components requiring JS interop or external dependencies.

---

## Decisions

| Component | Engine | License | Bundle | Interop |
|-----------|--------|---------|--------|---------|
| **MariloEditor** | Tiptap + ProseMirror (`@tiptap/core`, `@tiptap/starter-kit`) | MIT (ProseMirror); check Tiptap per-package | Medium–High | High |
| **MariloChart + ChartSeries** | Chart.js v4+ | MIT | Medium | Medium |
| **MariloWindow** | Custom JS pointer/resize module | MIT (own code) | Low–Medium | Medium |
| **MariloColorPicker** | Custom HSV canvas interop | MIT (own code) | Low–Medium | Medium |
| **MariloDateRangePicker** | Floating UI + custom dual-calendar | MIT (Floating UI) | Medium | Medium–High |
| **MariloDateTimePicker** | Floating UI + shared calendar + custom time tumblers | MIT | Medium–High | High |
| **MariloTimePicker** | Custom tumbler/wheel UI | MIT (own code) | Low–Medium | Medium |
| **MariloFileUpload + Upload** | tus-based resumable upload (`tus-js-client` pattern) | MIT | Medium–High | High |
| **MariloMultiSelect** | TanStack Virtual for virtualized lists | MIT | Low–Medium | Medium |
| **DataGrid Pass 2 (Editing)** | Custom declarative command architecture | MIT (own code) | Low | Medium |

---

## Public API Principles

Each component's .NET API **must be engine-agnostic**. The JS engine is an implementation detail.

### What to Expose

| Component | Public API Surface |
|-----------|--------------------|
| MariloEditor | Value binding, modes (view/edit), formatting commands, toolbar config, read-only/disabled, OnChange/OnSelectionChange |
| MariloChart | Series collections, chart type, axes, data binding, color/palette, animation, EnableDataDecimation |
| MariloWindow | Position, Size, State (normal/max/min), modal, drag/resize flags, OnMove/OnResize/OnStateChanged, focus/z-order |
| MariloColorPicker | Color value (RGBA/HEX/HSLA), two-way binding, disabled/read-only, presets, OnColorChanged/OnPreviewChanged |
| MariloDateRangePicker | Value (start/end), min/max, disabled rules, presets, culture/format, OnRangeChanged/OnApply/OnCancel |
| MariloDateTimePicker | Value (DateTime), min/max, time granularity, 12/24h, culture/format, OnValueChanged/OnConfirm |
| MariloTimePicker | Time value, granularity, min/max, culture (AM/PM), visual mode (wheel/dropdown/stepper) |
| MariloFileUpload | File selection, validation, multi-file, upload strategy, progress/cancel/pause/resume, events |
| MariloMultiSelect | Items, selection mode, virtualization toggle, filtering, max visible tags, OnChange/OnFilter |
| DataGrid Editing | Command definitions (Id/Text/Icon/rules), placement (cell/menu/row), async handlers, edit modes |

### What to Hide

- Concrete JS engine types, option objects, plugin registrations
- DOM element references, canvas refs, JS instance IDs
- Internal state machines, pointer math, scroll positions
- Protocol-level details (tus tokens, chunk sizes, retry strategies)
- ProseMirror transactions, node schemas, mark/extension names
- Chart.js dataset objects, canvas lifecycle, plugin wiring
- Floating UI middleware, positioning algorithm internals

---

## Fallback Plans

| Component | Fallback if Engine Becomes Unsuitable |
|-----------|--------------------------------------|
| MariloEditor | Raw ProseMirror packages or minimal internal editor; keep .NET API editor-agnostic |
| MariloChart | Swap to ECharts or SVG engine; Marilo's series/axes model maps to new backend |
| MariloWindow | Evolve internal JS without changing Blazor API; add optional layout provider layer |
| MariloColorPicker | DOM-based picker with reduced fidelity behind same .NET API |
| MariloDateRangePicker | Swap positioning engine; .NET API exposes popup behavior abstractly |
| MariloDateTimePicker | Split DatePicker + TimePicker or simple datetime textbox with masking |
| MariloTimePicker | Stepper-based time picker or dropdown selector behind same API |
| MariloFileUpload | Simple multipart/form-data; configurable upload strategy interface |
| MariloMultiSelect | Replace virtualizer while keeping "virtualized list" interface constant |
