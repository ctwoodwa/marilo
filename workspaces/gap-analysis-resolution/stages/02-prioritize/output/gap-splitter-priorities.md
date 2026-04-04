# Splitter Gap Prioritization

> Date: 2026-04-03
> Source: gap-splitter-inventory.md (10 gaps)
> Stage: 02-prioritize

## Priority Batches

### Batch 1: Core API + Testing (Critical + High)

| Gap | Severity | Description | Effort |
|-----|----------|-------------|--------|
| GAP-SPLITTER-001 | Critical | Missing SplitterPanes wrapper component | S -- pass-through wrapper or document current approach |
| GAP-SPLITTER-002 | Critical | Missing GetState/SetState methods | M -- new SplitterState type + serialize/deserialize pane sizes and collapsed state |
| GAP-SPLITTER-006 | Critical | No test coverage | L -- bUnit tests for resize, collapse, keyboard, pane registration, state |
| GAP-SPLITTER-003 | High | Missing Class parameter | S -- verify base class provides it; add if absent |
| GAP-SPLITTER-004 | High | SplitterOrientation enum alignment (uses StackDirection) | S -- add SplitterOrientation enum or alias |
| GAP-SPLITTER-005 | High | Missing per-pane Min/Max parameters | M -- add parameters + enforce constraints in resize logic |
| GAP-SPLITTER-007 | High | No demo pages | M -- create demo pages for horizontal, vertical, collapsible, state persistence |

### Batch 2: Documentation + Polish (Medium)

| Gap | Severity | Description | Effort |
|-----|----------|-------------|--------|
| GAP-SPLITTER-008 | Medium | Missing per-pane Resizable parameter verification | S -- audit pane parameters; add if absent |
| GAP-SPLITTER-009 | Medium | Missing 100%-height layout guidance | S -- add full-viewport demo example |
| GAP-SPLITTER-010 | Medium | Missing nested splitter support verification | S -- verify nesting works; add nested splitter test |

## Recommended Sequence

Start with GAP-SPLITTER-001 (wrapper alignment) and GAP-SPLITTER-004 (enum alignment) to stabilize the public API shape, then GAP-SPLITTER-002 (state) and GAP-SPLITTER-005 (Min/Max) to complete the feature set, then GAP-SPLITTER-006 (tests) and GAP-SPLITTER-007 (demos) to validate everything. Batch 2 items (008-010) can follow in any order as polish work once the core API is settled.
