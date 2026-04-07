# Priority Framework

Scoring criteria and sequencing rules for gap prioritization.

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

## Priority Bands

| Band | Score Range | Meaning |
|------|-----------|---------|
| **Critical** | 16-20 | Must resolve before any release or milestone |
| **Important** | 10-15 | Resolve in current cycle; schedule proactively |
| **Nice-to-have** | 4-9 | Resolve when convenient; acceptable to defer |

## Sequencing Rules

After scoring, sequence gaps using these rules (in order of precedence):

1. **Dependencies first.** If gap B requires gap A to be resolved first, gap A goes earlier regardless of score.
2. **Cross-cutting before specific.** Gaps that affect a shared base class, utility, or pattern go before gaps in individual components.
3. **Higher score before lower.** Within the same dependency tier, resolve higher-scored gaps first.
4. **Same-area batching.** When scores are tied, batch gaps from the same area together to reduce context switching.

## Phase Assignment

| Phase | Contains | Entry Criteria |
|-------|----------|---------------|
| **1 - Foundation** | Cross-cutting gaps, base class changes, shared types/enums | None (first phase) |
| **2 - Core** | Critical and high-priority component-level gaps | Phase 1 complete |
| **3 - Expansion** | Remaining important gaps | Phase 2 complete |
| **4 - Polish** | Nice-to-have gaps, cosmetic alignment | Phase 3 complete |

## Dependency Notation

In the backlog, express dependencies as:

```
GAP-BUTTON-001 (blocked by: GAP-BASE-001, GAP-BASE-003)
```

If a circular dependency exists, flag it for human resolution:

```
CIRCULAR: GAP-NAV-002 <-> GAP-MENU-005 — [describe the conflict]
```
