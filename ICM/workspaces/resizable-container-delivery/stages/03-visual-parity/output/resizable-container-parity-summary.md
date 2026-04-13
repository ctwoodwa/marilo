# Visual Parity Summary — MariloResizableContainer

**Stage:** 03-visual-parity
**Date:** 2026-04-11
**Method:** Design-time SCSS source review (no live browser available)
**Sources reviewed:**
- `src/Marilo.Providers.FluentUI/Styles/_resizable-container.scss`
- `src/Marilo.Providers.FluentUI/Styles/components/_resizable-container.scss` (identical content — duplicate file, see GAP-VP-005)
- `src/Marilo.Providers.FluentUI/Styles/foundation/_colors.scss` (token definitions, light + dark)
- `src/Marilo.Providers.Bootstrap/Styles/_bridge-resizable-container.scss`
- `src/Marilo.Providers.Bootstrap/Styles/components/_resizable-container.scss` (identical to bridge file — duplicate, see GAP-VP-005)
- `src/Marilo.Providers.Bootstrap/Styles/_tokens-dark.scss`
- `src/Marilo.Providers.Material/Styles/components/_resizable-container.scss` (placeholder only)
- `src/Marilo.Components/Layout/ResizableContainer/MariloResizableContainer.razor`

---

## Score Matrix

Themes: **Fluent Light**, **Fluent Dark**, **Bootstrap Light**, **Bootstrap Dark**, **Material** (N/A — not yet implemented)

| Scenario | Fluent Light | Fluent Dark | Bootstrap Light | Bootstrap Dark |
|----------|-------------|-------------|-----------------|----------------|
| 1. Container default | PASS | PASS | PASS | PASS |
| 2. Resize handle hover | PASS | PASS | PASS | PASS |
| 3. Resize handle active (drag) | PARTIAL | PARTIAL | PARTIAL | PARTIAL |
| 4. Min/max constraint indicator | FAIL | FAIL | FAIL | FAIL |
| 5. Corner handles | PARTIAL | PARTIAL | PARTIAL | PARTIAL |

Material: all scenarios N/A — runtime provider is a placeholder scaffold only.

---

## Scenario Detail

### Scenario 1 — Container Default

The container root uses:
- `border: 1px solid var(--marilo-color-border, #e1dfdd)` (Fluent) — correct token, dark override at `_colors.scss:161` sets `--marilo-color-border: #484644` — PASS
- `background: var(--marilo-color-surface, #ffffff)` (Fluent) — correct token, dark override at `_colors.scss:155` — PASS
- `border: 1px solid var(--bs-border-color, …)` (Bootstrap) — Bootstrap 5.3 CSS variable; dark mode inherited from Bootstrap's own color-mode system — PASS
- `background: var(--bs-body-bg, #fff)` (Bootstrap) — Bootstrap native token, dark-mode handled by Bootstrap — PASS

**Both Fluent and Bootstrap: PASS across light and dark.**

---

### Scenario 2 — Resize Handle Hover

Fluent (`_resizable-container.scss` line 184):
```scss
&:hover::after {
  background: var(--marilo-color-primary, #0f6cbd) !important;
}
```
Token `--marilo-color-primary` has both light (`#0078d4`, `_colors.scss:8`) and dark (`#60cdff`, `_colors.scss:134`) overrides. Correct token family. PASS.

Bootstrap (`_bridge-resizable-container.scss` line 184):
```scss
&:hover::after {
  background: var(--bs-primary, #0d6efd) !important;
}
```
Bootstrap's `--bs-primary` adjusts via Bootstrap's `color-mode()`. Correct Bootstrap-native token. PASS.

**All active themes: PASS.**

---

### Scenario 3 — Resize Handle Active (Drag State)

The `--active` modifier class is applied via `CssProvider.ResizableContainerHandleClass(edge, isDragging: true, isFocused)` (source `razor.cs` line 183). When `_isDragging` is true the handle gets `--active` class, which triggers the same rule as `:hover::after`:

```scss
&:hover::after,
&--active::after {
  background: var(--marilo-color-primary, #0f6cbd) !important;
}
```

**Gap:** The `--active` state has **no distinct visual feedback** from the hover state. Both hover and drag use the same `--marilo-color-primary` color. Industry standard for a drag-active handle is a distinct "pressed" state (e.g., `--marilo-color-primary-active`, scale transform, or opacity reduction) so users receive clear confirmation that the drag has engaged. This is PARTIAL — the active modifier class exists and the color changes from rest, but hover and drag states are visually identical.

**File/line:** `src/Marilo.Providers.FluentUI/Styles/_resizable-container.scss` lines 184–187 (and identical Bootstrap file).

**Score: PARTIAL (both Fluent and Bootstrap, light and dark).**

---

### Scenario 4 — Min/Max Constraint Indicator

No SCSS exists for a constraint-hit indicator in any provider. No CSS class is emitted by the component when min/max is reached (source `_containerStyle` applies min/max only via inline `min-width`/`min-height` style values — there is no constraint-reached class). The spec (`appearance.md`) documents ghost outline behavior but does not document a constraint indicator. There is no visual feedback when a drag hits a min or max boundary.

**Gap:** No constraint indicator implemented or specced. The component relies entirely on the CSS `min-width`/`max-width` enforcement (the browser stops the resize at the boundary) but emits no visual "hit" signal to the user.

**File/line:** `src/Marilo.Components/Layout/ResizableContainer/MariloResizableContainer.razor.cs` lines 131–138 (`_containerStyle` — min/max applied as style, no class emitted). No SCSS rule exists in any provider.

**Score: FAIL (all active themes). Acceptable at initial delivery — constraint indicator is not in current spec. Tracked for next phase.**

---

### Scenario 5 — Corner Handles

Corner handle variants (`--top-left`, `--top-right`, `--bottom-left`) are present in SCSS with correct cursor values (`nwse-resize`, `nesw-resize`). However, these corner variants have **no `::after` pseudo-element** — unlike the edge handles (`--right`, `--bottom`, `--top`, `--left`) and the `--bottom-right` corner, the three non-default corners render as invisible hit areas with no visible indicator (no grip dots, no diagonal stripe).

Specifically:
- `--top-left` (Fluent `_resizable-container.scss` lines 158–164): cursor only, no `::after`
- `--top-right` (lines 166–172): cursor only, no `::after`
- `--bottom-left` (lines 174–180): cursor only, no `::after`

The default `--bottom-right` corner does have a `::after` with a correct diagonal stripe pattern.

**Gap:** Three of four corner handle variants are invisible at rest. Only the default `--bottom-right` corner is discoverable. This is a PARTIAL — the handles are functional (correct cursor, correct hit area) but not visually discoverable without hovering, at which point they become primary-colored rectangles with no directional indicator.

**File/line:** `src/Marilo.Providers.FluentUI/Styles/_resizable-container.scss` lines 158–180.

**Score: PARTIAL (all active themes).**

---

## Gap Records

### GAP-VP-001 — Active drag state indistinguishable from hover

**Severity:** Minor
**Scenario:** 3 — Resize handle active
**Themes affected:** Fluent Light, Fluent Dark, Bootstrap Light, Bootstrap Dark
**File:** `src/Marilo.Providers.FluentUI/Styles/_resizable-container.scss` lines 184–187; `src/Marilo.Providers.Bootstrap/Styles/_bridge-resizable-container.scss` lines 184–187
**Description:** The `--active` CSS modifier class shares the same background color as the `:hover` pseudo-class. Users who rely on visual feedback to confirm drag engagement receive no distinct signal beyond the cursor change.
**Recommended fix:** Add a distinct `background` value for `&--active::after` (e.g., `var(--marilo-color-primary-active)` for Fluent, `var(--bs-primary-active)` / a darkened primary for Bootstrap) and optionally add a subtle scale transform on the `--active` state.
**Remediation target:** gap-analysis workspace — SCSS update only, no component API change.

---

### GAP-VP-002 — No constraint-hit visual indicator

**Severity:** Minor (not in current spec — track for next phase)
**Scenario:** 4 — Min/max constraint indicator
**Themes affected:** All
**File:** No SCSS or source class emitted — gap is in component behavior, not a styling error
**Description:** When a user drags to a min/max boundary, the browser's CSS enforcement silently stops the resize. No visual "snap" or "bounce" indicator is emitted. This is an unspecced behavior gap, not a token-usage error.
**Recommended fix:** Either (a) add a `--at-constraint` modifier class in the component source emitted when the computed size equals a min/max constraint, then style it in SCSS with a brief ring animation; or (b) explicitly accept the current silent behavior as correct for v1 and document it in the spec.
**Remediation target:** Spec decision first — if indicator is desired, component source + SCSS update + spec update required. Delegate to gap-analysis workspace.

---

### GAP-VP-003 — Corner handles (top-left, top-right, bottom-left) have no visual indicator

**Severity:** Minor
**Scenario:** 5 — Corner handles
**Themes affected:** Fluent Light, Fluent Dark, Bootstrap Light, Bootstrap Dark
**File:** `src/Marilo.Providers.FluentUI/Styles/_resizable-container.scss` lines 158–180; `src/Marilo.Providers.Bootstrap/Styles/_bridge-resizable-container.scss` lines 158–180
**Description:** Three corner handle variants render as invisible hit areas. Only `--bottom-right` has a `::after` diagonal stripe. Users cannot discover that these corners are resizable without hovering first.
**Recommended fix:** Add a `::after` pseudo-element to `--top-left`, `--top-right`, and `--bottom-left` with an appropriate directional indicator (rotate/mirror the existing diagonal stripe, or add corner dots consistent with the `--bottom-right` pattern).
**Remediation target:** gap-analysis workspace — SCSS update only.

---

### GAP-VP-004 — Material provider is a placeholder (N/A)

**Severity:** N/A — known, pre-acknowledged blocker
**Scenario:** All
**Themes affected:** Material Light, Material Dark
**File:** `src/Marilo.Providers.Material/Styles/components/_resizable-container.scss` (6-line TODO comment only)
**Description:** The Material 3 provider has no runtime project and no SCSS content for this component. This was noted in the visual parity plan as a known unknown.
**Recommended fix:** When the Material runtime provider is created, score this component against Material 3 design tokens (md-sys-color-surface, md-sys-color-outline, etc.) and update this summary.
**Remediation target:** Future delivery cycle — out of scope for Phase 1.

---

### GAP-VP-005 — Duplicate SCSS files between root Styles/ and Styles/components/

**Severity:** Minor — maintenance risk, not a visual gap
**File:** `src/Marilo.Providers.FluentUI/Styles/_resizable-container.scss` and `src/Marilo.Providers.FluentUI/Styles/components/_resizable-container.scss` are byte-for-byte identical. Same duplication in Bootstrap provider.
**Description:** Two SCSS files contain identical rules. If either file is updated independently the two will diverge without warning. The `_index.scss` likely imports only one of these; the other is dead code.
**Recommended fix:** Confirm which file is imported by `_index.scss`, remove or archive the unused copy, and add a comment to the canonical file preventing re-duplication.
**Remediation target:** Housekeeping — SCSS cleanup, no visual impact.

---

## Verdict

**AMBER**

No critical gaps block delivery. The component is visually functional across Fluent and Bootstrap themes in both light and dark modes, with correct token usage throughout. All fallback values are appropriate hardcoded Fluent/Bootstrap colors and are only used when their respective design tokens are unavailable.

Three PARTIAL/FAIL items are present but none are delivery blockers for Phase 1:

- GAP-VP-001 (active vs. hover distinction) — minor UX polish, correct tokens used
- GAP-VP-002 (constraint indicator) — unspecced; acceptable as silent behavior for v1
- GAP-VP-003 (corner handle discoverability) — functional handles, minor visual completeness gap

GAP-VP-004 (Material) and GAP-VP-005 (duplicate files) are tracked but do not affect delivery quality for Fluent or Bootstrap consumers.

**Recommended action before shipping:** Address GAP-VP-003 (corner handle `::after` elements) as it is a straightforward SCSS addition that improves discoverability with no API change. GAP-VP-001 can follow in a polish pass. Both are SCSS-only changes delegated to the gap-analysis workspace.
