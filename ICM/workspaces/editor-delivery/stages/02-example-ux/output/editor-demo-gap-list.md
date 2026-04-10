# MariloEditor — Demo Gap List

**Audit date:** 2026-04-10
**Existing demo page:** `samples/Marilo.Demo/Pages/Components/Editor/Editor/Overview.razor`
**Current scenario count:** 2 (basic editor, limited tools)
**Target scenario count:** 10

---

## Current Coverage

| # | Existing Scenario | Parameters Covered | Events Covered |
|---|---|---|---|
| 1 | Basic Editor | Value, Placeholder, Height | ValueChanged (via @bind) |
| 2 | Limited Tools | Tools | None |

---

## Demo Gaps

### Category A — Missing scenarios for implemented parameters

| # | Gap | Parameter(s) | Priority |
|---|-----|-------------|----------|
| A1 | No edit modes demo | EditMode, EditModeChanged | P1 |
| A2 | No readonly/disabled demo | ReadOnly, Disabled | P2 |
| A3 | No custom tools demo | CustomTools | P2 |
| A4 | No adaptive toolbar demo | Adaptive | P2 |
| A5 | No sizing demo | Width, Height | P3 |

### Category B — Missing scenarios for implemented events/features

| # | Gap | Feature | Priority |
|---|-----|---------|----------|
| B1 | No import/export demo | ImportAsync, ExportAsync | P1 |
| B2 | No table insertion demo | EditorTool.Table, table resize | P2 |
| B3 | No image insertion demo | EditorTool.Image, image resize | P2 |
| B4 | No paste settings demo | EditorPasteSettings child component | P3 |

### Category C — Missing edge cases

| # | Gap | Scenario | Priority |
|---|-----|---------|----------|
| C1 | No validation integration demo | ValueExpression + EditContext | P3 |

---

## Proposed Demo Sections (10 scenarios)

| # | Section Title | Gaps Covered |
|---|---|---|
| 1 | Basic Editor (existing, enhanced) | A5 |
| 2 | Limited Tools (existing) | — |
| 3 | Edit Modes | A1 |
| 4 | ReadOnly & Disabled | A2 |
| 5 | Custom Tools | A3 |
| 6 | Adaptive Toolbar | A4 |
| 7 | Tables | B2 |
| 8 | Import / Export | B1 |
| 9 | Events | OnChange, OnCommand |
| 10 | Paste Settings | B4 |
