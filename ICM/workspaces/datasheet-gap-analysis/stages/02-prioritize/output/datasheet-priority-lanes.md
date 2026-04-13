# DataSheet Priority Lanes — Stage 02 Prioritize

**Worker:** `w-datasheet-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** `02-prioritize` (checkpoint — STOP before resolution design)
**Date:** 2026-04-11
**Component:** MariloDataSheet
**Input:** `stages/01-intake/output/gap-inventory.md` (48 records total)

---

## Record Reconciliation

### Dedup: SA-07 absorbs SA-10

SA-10 was logged as a duplicate cross-reference of SA-07 in Wave 1. Both address the same spec wording issue (`bulk-paste-and-clipboard.md:91` — date coercion culture mismatch). SA-10 is removed from the actionable set.

| Before dedup | After dedup |
|---|---|
| 39 actionable | **38 actionable** |
| SA-07 + SA-10 both present | SA-07 (sole record), SA-10 retired |

### Out-of-pipeline: UD-01 + L1/L2/L3 (orchestrator-only)

Per tick-8 decisions and orchestration rules, the following are **not sequenced in this workspace's remediation lanes**. They are listed for dependency tracking only.

| Record | Description | Status |
|---|---|---|
| UD-01 | `IDataSheetTheme` sub-contract definition | Pending orchestrator dispatch |
| L1 | `IDataSheetTheme` surface definition | Pending orchestrator dispatch |
| L2 | FluentUI provider `IDataSheetTheme` impl | Pending L1 |
| L3 | Material provider 5-line stub | Pending L1 |

**Impact:** VP-datasheet-01 (umbrella) and VP-datasheet-02 through -11 are all gated on UD-01. These VP records appear in lanes below but are marked `BLOCKED-ON-UD-01` — they cannot execute until the orchestrator completes UD-01.

### Deferrals (tracked, not remediation-eligible)

| Record | Deferred reason | Unblocking condition |
|---|---|---|
| VP-datasheet-D01 | DEFERRED-PENDING-ARCHITECTURE (UD-01) | UD-01 implementation lands |
| VP-datasheet-D02 | DEFERRED-PENDING-SOURCE (V03) | V03 range-selection lands |
| VP-datasheet-D03 | DEFERRED-PENDING-SCOPE (UD-02) | UD-02 lands (effective WONTFIX at capture level) |

### CLEAR items (6 — no action, record-of-gate only)

CLEAR-1.5, CLEAR-2.2, CLEAR-2.8, CLEAR-4.1, CLEAR-4.3, CLEAR-5.3. Not sequenced.

---

## Actionable Record Summary (38 post-dedup)

| Priority | Count | IDs |
|---|---:|---|
| P0-blocker | 16 | UD-02, V03, V07.4, SA-01, SA-13, VP-datasheet-01..07, 09, 10, 11, WS-01 |
| P1-primary | 9 | EU-01, EU-02, EU-03, SA-02, SA-03, SA-04, SA-05, SA-08, SA-09 |
| P2-secondary | 11 | SA-06, SA-07, SA-11, SA-12, SA-14, SA-15, EU-04, EU-05, EU-08, VP-datasheet-08, VP-datasheet-12 |
| P3-polish | 2 | SRC-01, NM-01 |
| **Total** | **38** | (SA-10 retired via dedup) |

---

## Remediation Lanes

Lanes are ordered by execution priority. Within a priority tier, lanes with fewer dependencies execute first. Each lane has a unique ID for dispatch tracking.

### Lane 0: WS-01 — Workspace Coverage Audit (schedule early)

| Field | Value |
|---|---|
| **Lane ID** | `lane-ws-01` |
| **Records** | WS-01 |
| **Priority** | P0-blocker |
| **Scope** | single |
| **Sync areas** | gap-plan |
| **Dependencies** | None (this intake already partially satisfies WS-01) |
| **Effort** | XS — populate `_config/coverage-summary.md` with per-parameter test status |
| **Rationale** | Unblocks Wave 4 checklist items 3.1/3.3/3.5/5.1. Must land before any remediation lane so test-coverage planning has a baseline. |
| **Output** | `_config/coverage-summary.md` populated |

---

### Lane 1: SA-01 — Grid Root tabindex (unblocks VP-datasheet-12)

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa-01` |
| **Records** | SA-01 |
| **Priority** | P0-blocker |
| **Scope** | single |
| **Sync areas** | source, spec, tests |
| **Dependencies** | None |
| **Effort** | XS — add `tabindex="0"` to grid root `<div role="grid">`, bUnit assertion |
| **Rationale** | Tiny change, zero risk, unblocks VP-datasheet-12 (keyboard focus styling). Early win. |
| **Output** | Source edit + bUnit test |

---

### Lane 2: UD-02 + EU-01 — Virtualization Threshold (batch)

| Field | Value |
|---|---|
| **Lane ID** | `lane-ud02-eu01` |
| **Records** | UD-02, EU-01 |
| **Priority** | P0-blocker (UD-02) + P1-primary (EU-01) |
| **Scope** | batch |
| **Sync areas** | spec, demo |
| **Dependencies** | None |
| **Effort** | S — spec threshold note + 5k-row demo scenario |
| **Rationale** | Single cohesive lane: both land the same spec text and demo page. UD-02 is the P0 driver; EU-01 is the demo-side of the same requirement. No source changes needed. Effective closure of VP-datasheet-D03 deferral. |
| **Output** | Spec edit (`virtualization-and-performance.md`) + demo page (BulkOperations scenario E or new `Virtualization.razor`) |

---

### Lane 3: SA-08 + EU-03 — Paste-during-save Guard (batch)

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa08-eu03` |
| **Records** | SA-08, EU-03 |
| **Priority** | P1-primary (SA-08 source fix) + P1-primary (EU-03 demo) |
| **Scope** | batch |
| **Sync areas** | source, tests, demo |
| **Dependencies** | None — source fix must land before demo can be truthful (internal ordering within lane) |
| **Effort** | S — early-return guard in `PasteFromClipboard` + bUnit test + demo scenario |
| **Rationale** | EU-03 depends on SA-08 source fix. Bundled as one atomic lane so the demo proves the fix. |
| **Output** | Source edit (`MariloDataSheet.Editing.cs`) + bUnit test + demo scenario |

---

### Lane 4: SA-13 + EU-05 — Missing aria-live Announcements (batch)

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa13-eu05` |
| **Records** | SA-13, EU-05 |
| **Priority** | P0-blocker (SA-13) + P2-secondary (EU-05) |
| **Scope** | batch |
| **Sync areas** | source, spec, demo, tests |
| **Dependencies** | None — source fix must land before demo (internal ordering) |
| **Effort** | S-M — 3 new aria-live announcements in `SaveAllAsync` + bUnit tests + failure/retry demo scenario |
| **Rationale** | SA-13 gates Wave 4 checklist 2.6. EU-05 is the demo proof. Bundled as one lane. |
| **Output** | Source edit (`MariloDataSheet.Data.cs`) + bUnit tests + demo scenario |

---

### Lane 5: V03 + V07.4 — Range Selection Model (large, critical path)

| Field | Value |
|---|---|
| **Lane ID** | `lane-v03` |
| **Records** | V03, V07.4 |
| **Priority** | P0-blocker |
| **Scope** | single (large) |
| **Sync areas** | source, spec, demo, tests |
| **Dependencies** | None — but unblocks VP-datasheet-D02 deferral, SA-06 full fix, EU-04 partial |
| **Effort** | L — new `DataSheetSelection<TItem>` model, keyboard handlers, range-aware clipboard, Ctrl+A |
| **Rationale** | Largest single gap in the inventory. Introduces the entire selection model. V07.4 (Ctrl+A) folds in as a sub-task. Gates multiple downstream items. |
| **Output** | New source files/edits, spec alignment, demo scenario (EU-07 folded), bUnit test suite |
| **Note** | This lane should be decomposed further in Stage 03 (resolution design). Too large for a single atomic dispatch. |

---

### Lane 6: SA-03 — AddRow ActivateCell

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa-03` |
| **Records** | SA-03 |
| **Priority** | P1-primary |
| **Scope** | single |
| **Sync areas** | source, tests |
| **Dependencies** | None |
| **Effort** | XS — add `ActivateCell` call at end of `AddRowAsync` + bUnit test |
| **Output** | Source edit + bUnit test |

---

### Lane 7: SA-04 — Reset Clears Undo Buffer

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa-04` |
| **Records** | SA-04 |
| **Priority** | P1-primary |
| **Scope** | single |
| **Sync areas** | source, tests |
| **Dependencies** | None |
| **Effort** | XS — add `_undoBuffer.Clear()` to `ResetAsync` + bUnit regression test |
| **Output** | Source edit + bUnit test |

---

### Lane 8: EU-02 — Copy-Paste Round-Trip Demo

| Field | Value |
|---|---|
| **Lane ID** | `lane-eu-02` |
| **Records** | EU-02 |
| **Priority** | P1-primary |
| **Scope** | single |
| **Sync areas** | demo |
| **Dependencies** | None |
| **Effort** | XS — new demo scenario exercising Ctrl+C / Ctrl+V round-trip |
| **Output** | Demo page edit |

---

### Lane 9: VP Fluent / Bootstrap / Material SCSS Lanes (BLOCKED-ON-UD-01)

These lanes cannot execute until the orchestrator completes UD-01 (`IDataSheetTheme`). Listed here for sequencing visibility. Each becomes a separate atomic dispatch once UD-01 lands.

| Sub-lane | Records | Provider | Theme | Effort |
|---|---|---|---|---|
| `vp-datasheet-fluent-light` | VP-datasheet-02, VP-datasheet-08 (hover, folded in) | FluentUI | Light | S |
| `vp-datasheet-fluent-dark` | VP-datasheet-03 | FluentUI | Dark | XS |
| `vp-datasheet-fluent-selection` | VP-datasheet-04, VP-datasheet-09 | FluentUI | Both | S |
| `vp-datasheet-fluent-editor` | VP-datasheet-05 | FluentUI | Both | XS |
| `vp-datasheet-fluent-validation` | VP-datasheet-06 | FluentUI | Both | S |
| `vp-datasheet-frozen-column` | VP-datasheet-07 (SCSS half) | FluentUI + all | Both | S |
| `vp-datasheet-bootstrap-bridge` | VP-datasheet-10 | Bootstrap | Both | S |
| `vp-datasheet-material` | VP-datasheet-11 | Material | Both | XS (stub) |
| `vp-datasheet-focus-visible` | VP-datasheet-12 | FluentUI | Both | XS (blocked on SA-01) |
| `vp-datasheet-umbrella` | VP-datasheet-01 | — | — | Satisfied when all child lanes complete |

**Total VP SCSS records:** 10 child records + 1 umbrella = 11 (all P0-blocker or P2-secondary)

**VP-datasheet-07 source half** (Frozen column `Frozen` property + sticky class) is NOT in the SCSS lanes. It is a separate source lane:

| Field | Value |
|---|---|
| **Lane ID** | `lane-vp07-source` |
| **Records** | VP-datasheet-07 (source half only) |
| **Priority** | P0-blocker |
| **Scope** | single |
| **Sync areas** | source, spec, tests |
| **Dependencies** | None (source-side is independent of UD-01) |
| **Effort** | S — add `Frozen` property to `MariloDataSheetColumn.razor`, render sticky class, bUnit test |
| **Output** | Source edit + spec alignment + bUnit test |

---

### Lane 10: Escalation Candidates — Pending Orchestrator Confirmation

These 3 records require orchestrator arbitration on spec-vs-source direction. Worker proposes a direction for each; orchestrator confirms or overrides before these enter Stage 03.

#### SA-02 — AddRow append vs. prepend

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa-02` |
| **Records** | SA-02 |
| **Priority** | P1-primary |
| **Scope** | single |
| **Sync areas** | `spec` OR `source`+`tests` (depends on arbitration) |
| **Dependencies** | Orchestrator confirmation |
| **Status** | `pending-orchestrator-confirmation` |
| **Worker proposal** | **Recommend spec fix** (change "appended to the end" to "prepended at the top"). Rationale: the current source behavior (`Insert(0, newItem)`) is a deliberate UX choice — new rows appearing at the top of a data entry sheet is the more common spreadsheet pattern (Excel inserts above). Changing source would break existing demo muscle-memory and require retesting all AddRow scenarios. A spec-side wording fix is lower risk and lower effort. |
| **Effort if spec fix** | XS |
| **Effort if source fix** | S (source change + demo re-verification + bUnit test update) |

#### SA-05 — Saving->Saved transition ownership

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa-05` |
| **Records** | SA-05 |
| **Priority** | P1-primary |
| **Scope** | single |
| **Sync areas** | `spec` OR `source`+`tests` (depends on arbitration) |
| **Dependencies** | Orchestrator confirmation |
| **Status** | `pending-orchestrator-confirmation` |
| **Worker proposal** | **Recommend spec fix** (describe the component-driven `Task.Delay(_savedStateDurationMs)` transition as the intended behavior). Rationale: the component-driven timer is a polished UX pattern — the cell flashes green for `_savedStateDurationMs` then auto-clears. Making the transition consumer-keyed (observing `IsSaving` changes) would require an entirely new state-machine and break the current auto-feedback loop. The spec wording "IsSaving set to false" is ambiguous about WHO sets it; clarifying that the component drives the `Saving -> Saved` visual transition after `SaveAllAsync` completes is accurate and non-breaking. |
| **Effort if spec fix** | XS |
| **Effort if source fix** | M (new state machine + bUnit tests) |

#### SA-09 — Double-click edit entry

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa-09` |
| **Records** | SA-09 |
| **Priority** | P1-primary |
| **Scope** | single |
| **Sync areas** | `spec` OR `source`+`tests` (depends on arbitration) |
| **Dependencies** | Orchestrator confirmation |
| **Status** | `pending-orchestrator-confirmation` |
| **Worker proposal** | **Recommend source fix** (add `ondblclick` handler). Rationale: double-click-to-edit is a universal spreadsheet convention (Excel, Google Sheets, Telerik). The current single-click entry is unusual and already has a "click-click" delay path in `OnCellClick` that suggests the original intent was double-click. Adding `ondblclick` alongside the existing single-click path (which enters navigate-then-edit mode) gives users both affordances. The spec is correct; the source is incomplete. |
| **Effort if source fix** | S (add handler + bUnit test) |
| **Effort if spec fix** | XS |

---

### Lane 11: Spec Wording Fixes (batch — all P2-secondary, no source changes)

| Field | Value |
|---|---|
| **Lane ID** | `lane-spec-fixes` |
| **Records** | SA-07, SA-11, SA-12, SA-14, SA-15 |
| **Priority** | P2-secondary |
| **Scope** | batch |
| **Sync areas** | spec |
| **Dependencies** | SA-12, SA-14, SA-15 need orchestrator direction on spec-vs-source (but worker recommends spec fix for all three; if confirmed, they fold into this lane cleanly) |
| **Effort** | S — 5 spec file edits, no source/tests/demo |
| **Rationale** | All 5 are spec-side wording corrections where the source behavior is intentional or acceptable. Batching minimizes dispatch overhead. |

**Per-record recommendations (for orchestrator confirmation):**

| Record | Recommendation | Spec file | Change |
|---|---|---|---|
| SA-07 | Spec fix | `bulk-paste-and-clipboard.md:91` | Change "current culture" to "InvariantCulture (matches invariant round-trip used by `data-raw-value`)" |
| SA-11 | Spec fix | `editing-and-validation.md:139` | Rewrite to describe actual short-circuit: required failure prevents `column.Validate` from running |
| SA-12 | Spec fix (recommended) | `editing-and-validation.md:193` | Drop "invalid-only rows" clause; source counts all dirty rows including invalid-AND-dirty |
| SA-14 | Spec fix (recommended) | `columns-and-schema.md:118` | Drop "or zero" clause; source only checks null |
| SA-15 | Spec fix (recommended) | `columns-and-schema.md:231` | Mirror SA-14: drop `default(DateTime)` rejection clause |

---

### Lane 12: Demo-Only Additions (P2-secondary, no source dependencies)

| Field | Value |
|---|---|
| **Lane ID** | `lane-demo-p2` |
| **Records** | EU-04, EU-08 |
| **Priority** | P2-secondary |
| **Scope** | batch |
| **Sync areas** | demo |
| **Dependencies** | EU-04 partially gated on V03 (Ctrl+A) but Delete-key portion is independent |
| **Effort** | XS — 2 small demo additions |
| **Rationale** | Neither requires source changes. EU-04 (Delete-key scenario) and EU-08 (CellTemplate scenario) are independent demo edits. |
| **Output** | Demo page edits |

---

### Lane 13: SA-06 — Fill-Down Editable Filter (deferred decision)

| Field | Value |
|---|---|
| **Lane ID** | `lane-sa-06` |
| **Records** | SA-06 |
| **Priority** | P2-secondary |
| **Scope** | single |
| **Sync areas** | demo, spec |
| **Dependencies** | Partially depends on V03 (range selection). Full fix requires range-selection model. |
| **Effort** | XS (wording softening now) or S (full fix after V03) |
| **Worker proposal** | Soften demo wording now (doc-only fix), defer full behavioral fix until V03 lands. Two-step approach. |

---

### Lane 14: P3 Polish (lowest priority)

| Field | Value |
|---|---|
| **Lane ID** | `lane-p3-polish` |
| **Records** | SRC-01, NM-01 |
| **Priority** | P3-polish |
| **Scope** | batch |
| **Sync areas** | spec |
| **Dependencies** | None |
| **Effort** | XS — 2 minor spec wording tweaks |
| **Rationale** | Skeleton row count description (SRC-01) and inherited parameter documentation (NM-01). Lowest priority; can be folded into any future spec-editing lane for zero marginal cost. |
| **Output** | Spec file edits |

---

## Execution Sequence (recommended)

```
Phase A — Unblocked, no-dependency lanes (parallel-eligible)
  lane-ws-01      WS-01         P0  gap-plan only         XS
  lane-sa-01      SA-01         P0  source+spec+tests     XS
  lane-ud02-eu01  UD-02+EU-01   P0  spec+demo             S
  lane-sa08-eu03  SA-08+EU-03   P1  source+tests+demo     S
  lane-sa13-eu05  SA-13+EU-05   P0  source+spec+demo+tests S-M
  lane-sa-03      SA-03         P1  source+tests           XS
  lane-sa-04      SA-04         P1  source+tests           XS
  lane-eu-02      EU-02         P1  demo                   XS

Phase B — Escalation-gated (waiting orchestrator confirmation)
  lane-sa-02      SA-02         P1  spec OR source+tests   XS-S
  lane-sa-05      SA-05         P1  spec OR source+tests   XS-M
  lane-sa-09      SA-09         P1  spec OR source+tests   XS-S

Phase C — Large / complex (may decompose in Stage 03)
  lane-v03        V03+V07.4     P0  source+spec+demo+tests L
  lane-vp07-source VP-07 src    P0  source+spec+tests      S

Phase D — Blocked on UD-01 (orchestrator-dispatched dependency)
  9 VP SCSS sub-lanes           P0  source(SCSS)           XS-S each
  VP-datasheet-12               P2  source(SCSS)           XS (also needs SA-01)

Phase E — Low-priority cleanup (parallel-eligible, anytime)
  lane-spec-fixes  SA-07+11+12+14+15  P2  spec            S
  lane-demo-p2     EU-04+EU-08        P2  demo             XS
  lane-sa-06       SA-06              P2  demo+spec        XS (partial)
  lane-p3-polish   SRC-01+NM-01      P3  spec             XS
```

### Phase notes

- **Phase A** can run all 8 lanes in parallel (disjoint file ownership). Total: 22 records dispatched.
- **Phase B** requires orchestrator confirmation on 3 direction decisions before dispatch. Worker has proposed spec-fix for SA-02 and SA-05, source-fix for SA-09.
- **Phase C** is the critical path. V03 is the largest gap (introduces the entire selection model). Should be decomposed in Stage 03 resolution design before dispatch. VP-07 source half (Frozen column) is independent and can run alongside.
- **Phase D** is entirely blocked on UD-01 orchestrator lanes (L1/L2/L3). Once UD-01 lands, 10 VP SCSS records become dispatchable across ~9 sub-lanes.
- **Phase E** is low-priority spec/demo cleanup. Can run anytime files are available. Zero source risk.

---

## Lane Coverage Verification

### All 38 actionable records accounted for

| Lane | Records | Count |
|---|---|---:|
| lane-ws-01 | WS-01 | 1 |
| lane-sa-01 | SA-01 | 1 |
| lane-ud02-eu01 | UD-02, EU-01 | 2 |
| lane-sa08-eu03 | SA-08, EU-03 | 2 |
| lane-sa13-eu05 | SA-13, EU-05 | 2 |
| lane-v03 | V03, V07.4 | 2 |
| lane-sa-03 | SA-03 | 1 |
| lane-sa-04 | SA-04 | 1 |
| lane-eu-02 | EU-02 | 1 |
| VP SCSS sub-lanes (9) | VP-01..07, 08, 09, 10, 11 | 10 |
| VP-datasheet-12 | VP-12 | 1 |
| lane-vp07-source | VP-07 (source half, separate from SCSS) | (shared with VP SCSS — VP-07 is dual-lane, counted once) |
| lane-sa-02 | SA-02 | 1 |
| lane-sa-05 | SA-05 | 1 |
| lane-sa-09 | SA-09 | 1 |
| lane-spec-fixes | SA-07, SA-11, SA-12, SA-14, SA-15 | 5 |
| lane-demo-p2 | EU-04, EU-08 | 2 |
| lane-sa-06 | SA-06 | 1 |
| lane-p3-polish | SRC-01, NM-01 | 2 |
| **SA-10** | **Retired (dedup into SA-07)** | **-1** |
| **Total** | | **38** |

### Retired record

| Record | Reason |
|---|---|
| SA-10 | Duplicate of SA-07. Merged during dedup. |

### Out-of-pipeline (not counted in 38)

| Record | Reason |
|---|---|
| UD-01 + L1/L2/L3 | Orchestrator-only implementation |
| VP-datasheet-D01 | Deferral (unblocks with UD-01) |
| VP-datasheet-D02 | Deferral (unblocks with V03) |
| VP-datasheet-D03 | Deferral (effective WONTFIX, unblocks with UD-02) |
| CLEAR-1.5 through CLEAR-5.3 | Passed delivery gate, no action |

---

## Checkpoint

**STOP — end of Stage 02 prioritize.**

- Stage 03 (resolution-design) is **NOT** executed in this turn.
- 38 actionable records clustered into 14 lanes + 9 VP SCSS sub-lanes across 5 execution phases.
- 3 records flagged `pending-orchestrator-confirmation` (SA-02, SA-05, SA-09) with worker-proposed directions.
- 1 record retired (SA-10 deduped into SA-07).
- UD-01 + L1/L2/L3 confirmed out-of-pipeline.
- WS-01 scheduled as Lane 0 (earliest dispatch).
