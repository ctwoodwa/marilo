# Priority Framework -- ResizableContainer

Scoring criteria and sequencing rules for gap prioritization.

## Priority Levels

| Priority | Label | Meaning |
|----------|-------|---------|
| **P1** | Blocking | Blocks core ResizableContainer functionality or downstream consumers. Must resolve before any release. |
| **P2** | This phase | Required for Phase 1 (initial build) completion. Resolve in current cycle. |
| **P3** | Next phase | Important but can wait for a subsequent phase. Schedule proactively. |
| **P4** | Backlog | Nice-to-have. Resolve when convenient; acceptable to defer indefinitely. |

## Scoring Dimensions

Score each gap from 1 (low) to 5 (high) on four dimensions:

| Dimension | 1 (Low) | 3 (Medium) | 5 (High) |
|-----------|---------|------------|----------|
| **Risk** | No risk if left unresolved | Workarounds exist but are fragile | Causes failures, data issues, or compliance violations |
| **User impact** | Internal-only, no external visibility | Affects developer experience or API consumers | Blocks end-user workflows or causes visible defects |
| **Architectural importance** | Cosmetic or isolated | Affects one integration boundary or pattern | Cross-cutting; impacts multiple modules or future extensibility |
| **Effort** (inverted) | Large effort, multi-sprint | Moderate effort, 1-2 days | Quick fix, under a day |

**Composite score** = Risk + User Impact + Architectural Importance + Effort (inverted).
Range: 4-20. Higher = higher priority.

## Score-to-Priority Mapping

| Score Range | Priority | Action |
|-------------|----------|--------|
| 16-20 | P1 (Blocking) | Must resolve before any release or milestone |
| 10-15 | P2 (This phase) | Resolve in current cycle; schedule proactively |
| 6-9 | P3 (Next phase) | Resolve in a subsequent phase |
| 4-5 | P4 (Backlog) | Resolve when convenient; acceptable to defer |

## Sequencing Rules

After scoring, sequence gaps using these rules (in order of precedence):

1. **Dependencies first.** If gap B requires gap A to be resolved first, gap A goes earlier regardless of score.
2. **Cross-cutting before specific.** Gaps that affect shared infrastructure go before component-specific gaps.
3. **Higher score before lower.** Within the same dependency tier, resolve higher-scored gaps first.
4. **Same-category batching.** When scores are tied, batch gaps from the same category together to reduce context switching.

## Phase Assignment

| Phase | Contains | Entry Criteria |
|-------|----------|---------------|
| **1 - Foundation** | Cross-cutting gaps, base class changes, shared types | None (first phase) |
| **2 - Core** | P1 and P2 component-level gaps | Phase 1 complete |
| **3 - Expansion** | Remaining P2 and P3 gaps | Phase 2 complete |
| **4 - Polish** | P4 gaps, cosmetic alignment | Phase 3 complete |

## Dependency Notation

```
GAP-RESIZABLE-CONTAINER-001 (blocked by: GAP-RESIZABLE-CONTAINER-003)
```
