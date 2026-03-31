# Design Language Token Values

Default token values extracted from the corporate presentation template and design language reference.
These are the canonical defaults used when generating SCSS scaffold files in Stage 05.
Override any value during setup to match a specific product or project.

---

## Color Tokens

### Light Theme

| Token | Value | Source |
|---|---|---|
| color-bg-page | #F3F3F3 | Slide background (light gray, not pure white) |
| color-bg-surface | #FFFFFF | Card, panel, dialog backgrounds |
| color-bg-surface-alt | #F9F9F9 | Alternating row, subtle nested surfaces |
| color-border-subtle | #E0E0E0 | Card borders, table dividers, separators |
| color-text-primary | #1E1E1E | Main text and headings |
| color-text-muted | #7F7F7F | Secondary text, labels, helper copy (from PPT inline style) |
| color-text-disabled | #BDBDBD | Disabled inputs and labels |
| color-accent | #0078D4 | Primary calls to action, links, focused states |
| color-accent-hover | #005FA3 | Accent hover state |
| color-accent-subtle | #EBF4FB | Accent tint for selected rows and backgrounds |
| color-success | #107C10 | Success states and positive indicators |
| color-success-subtle | #DFF6DD | Success pill backgrounds |
| color-warning | #F7630C | Cautionary states, soft alerts |
| color-warning-subtle | #FFF4CE | Warning pill backgrounds |
| color-error | #C50F1F | Errors and critical alerts |
| color-error-subtle | #FDE7E9 | Error pill backgrounds |

### Dark Theme

| Token | Value | Source |
|---|---|---|
| color-bg-page | #111318 | Dark page background |
| color-bg-surface | #1C1F26 | Dark card/panel backgrounds |
| color-bg-surface-alt | #22262F | Dark alternating rows |
| color-border-subtle | #2E3038 | Dark borders |
| color-text-primary | #F3F3F3 | Light primary text |
| color-text-muted | #9E9E9E | Dark theme secondary text |
| color-text-disabled | #5C5C5C | Dark disabled text |
| color-accent | #4CA3DD | Lighter accent for dark backgrounds |
| color-accent-hover | #6DB8E8 | Dark hover accent |
| color-accent-subtle | #1A3A52 | Dark accent tint |
| color-success | #6CCB5F | Dark success |
| color-success-subtle | #1E3A1E | Dark success background |
| color-warning | #FCB040 | Dark warning |
| color-warning-subtle | #3A2A00 | Dark warning background |
| color-error | #F1707A | Dark error |
| color-error-subtle | #3A0A0F | Dark error background |

---

## Typography Tokens

| Token | Value | Source |
|---|---|---|
| font-family-base | 'Segoe UI', system-ui, -apple-system, sans-serif | Corporate Windows/web standard |
| font-family-mono | 'Cascadia Code', 'Consolas', monospace | Code and technical values |
| font-size-h1 | 2rem (32px) | Page title - strong and clear per PPT title style |
| font-size-h2 | 1.5rem (24px) | Section title |
| font-size-h3 | 1.25rem (20px) | Subsection title |
| font-size-body | 0.875rem (14px) | Main content and form input text |
| font-size-caption | 0.75rem (12px) | Secondary text, labels, helper copy (PPT subtitle style) |
| line-height-heading | 1.25 | Tight for display text |
| line-height-body | 1.5 | Comfortable for reading |
| font-weight-regular | 400 | Default body weight |
| font-weight-medium | 500 | Labels and emphasized body |
| font-weight-semibold | 600 | Subheadings and strong labels (PPT slide heading style) |
| font-weight-bold | 700 | Page titles and critical emphasis |

---

## Spacing Tokens (4px base)

| Token | Value |
|---|---|
| space-0 | 0 |
| space-1 | 4px |
| space-2 | 8px |
| space-3 | 12px |
| space-4 | 16px |
| space-5 | 24px |
| space-6 | 32px |
| space-7 | 48px |
| space-8 | 64px |

---

## Radii Tokens

| Token | Value | Use |
|---|---|---|
| radius-none | 0 | Sharp corners for data-dense tables |
| radius-sm | 2px | Inputs, buttons, small controls |
| radius-md | 4px | Cards and panels |
| radius-lg | 8px | Large surfaces, callouts, modals |
| radius-pill | 9999px | Pill badges and chips |

---

## Shadow Tokens

| Token | Value | Use |
|---|---|---|
| shadow-none | none | Flat surfaces |
| shadow-subtle | 0 1px 3px rgba(0,0,0,0.08) | Cards and panels (restrained per PPT style) |
| shadow-modal | 0 4px 16px rgba(0,0,0,0.16) | Dialogs and popovers |
| shadow-raised | 0 2px 8px rgba(0,0,0,0.12) | Dropdown menus, tooltips |

---

## Chart Color Palette

Follows the PPT graph template: restrained accent colors, mostly neutral, clear labels.

| Token | Value | Use |
|---|---|---|
| chart-color-1 | #0078D4 | Primary series (accent) |
| chart-color-2 | #50B0F0 | Secondary series (light accent) |
| chart-color-3 | #107C10 | Success/positive series |
| chart-color-4 | #F7630C | Warning/cautionary series |
| chart-color-5 | #5C2D91 | Tertiary series |
| chart-color-6 | #00B7C3 | Quaternary series |
| chart-neutral | #9E9E9E | Baseline/comparison series |