# Design Language Patterns

Shell, layout, and component behavior patterns. Companion to design-language-reference.md.
These patterns are technology-agnostic. Stack-specific implementation belongs in separate guides.

---

## 4. Application Shell and Layout Patterns

### 4.1 Global Shell

All applications share a common shell structure.

Top bar:
- Left: product logo/name and optional module name.
- Center: breadcrumb or key context.
- Right: global utilities - search, help, notifications, user profile.

Navigation:
- Left navigation rail with icons and labels.
- Supports grouping (primary sections vs. admin/tools).
- Collapsible to icons-only; state remembered per user.

Content stack:
1. Breadcrumb - shows current location.
2. Page header - title (H1), key metadata (chips/pills), primary actions.
3. Content canvas - main working area (cards, tables, forms, charts).

### 4.2 Screen-Level Patterns

#### Explorer (List View)

Use when browsing, filtering, and selecting entities.
- Layout: filters (top or side), list or table, row actions.
- Interactions: search, sort, filter, pagination or infinite scroll.
- Row click opens a detail view using the Workspace pattern.

#### Workspace (Detail View)

Use when viewing or editing a single entity or related data.
- Layout: page header; tabs or sections (Overview, Details, Related, Activity).
- May use side-by-side layout (form left, results or visualization right).
- Supports contextual panels (inspectors, help, history) without breaking the main flow.

#### Admin Hub (Settings)

Use for configuration and administration.
- Layout: tabs across the top for each admin section.
- Master-detail: list/grid left, detail pane right.
- Reuse standard forms, tables, and cards.

---

## 5. Core Component Patterns

### 5.1 Cards

- Header: title and optional primary/secondary actions.
- Body: content (forms, lists, charts).
- Optional footer: secondary actions or metadata.
- Consistent padding (space-3 / space-4). Prefer page-level scrolling over scrollable card bodies.

### 5.2 Tables / Grids

Column order: Name/ID, Status, Owner, Dates, Metrics, Actions.
- Name: left-aligned, slightly stronger weight.
- Numeric: right-aligned.
- Actions: grouped right as icons or overflow menu.

Interactions: sorting, filtering, pagination or virtual scroll, optional row selection for bulk actions.

Visuals: subtle header background, caption-sized labels, optional light striping,
clear hover/selection states using neutral or accent tints.

### 5.3 Forms

- One or two columns depending on density and screen size.
- Labels above inputs; helper text below.
- Group fields into logical sections with headings.
- Required fields clearly marked. Inline validation with concise messages.
- Advanced fields collapsed under expander.
- Primary action (Save) right-most and visually emphasized.

### 5.4 Buttons

- Primary: main call to action (color-accent, high contrast text).
- Secondary: supporting actions (neutral background, subtle border).
- Tertiary/text: low-emphasis.
- Icon-only: compact actions in tables or toolbars.
- One primary button per surface. Button groups place primary action to the right.

### 5.5 Pills, Chips, and Badges

- Status pills: high-contrast text on lightly tinted status-color background.
- Chips: neutral background, muted text, optionally removable.
- Badges: small indicators anchored to icons (e.g., notification count).

### 5.6 Charts and Data Visualization

- Use accent and status colors for key series; keep everything else muted.
- Subtle axes and gridlines. Readable labels and legends.
- Meaningful titles and subtitles. Tooltips for detailed values.

---

## 6. Responsive Behavior

Applications are primarily desktop-focused but adapt gracefully.

| Breakpoint | Width | Behavior |
|---|---|---|
| Desktop | >= 1200px | Full layouts with side-by-side panels |
| Tablet | 768-1199px | Stacked panels; navigation may collapse |
| Mobile | < 768px | Single-column layouts; simplified workflows |

Rules:
- Navigation collapses to icons or a drawer on smaller screens.
- Complex tables degrade gracefully (summaries or card lists).
- Primary actions remain visible without horizontal scrolling.