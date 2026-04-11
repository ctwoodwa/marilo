# Gap Resolution Context -- MariloTreeList

## Target Component

- **Component:** `MariloTreeList<TItem>`
- **Source:** `src/Marilo.Components/DataGrid/MariloTreeList.razor` (199 lines, single file, generic)
- **Spec root:** `docs/component-specs/treelist/` (52 markdown files across 9+ sub-areas)
- **Demo:** `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor` (21 lines — explicit placeholder)
- **Delivery workspace:** `../treelist-delivery/`

## Gap Source

- **GAP_SOURCE:** `assess` (fresh intake — no prior gap analysis file existed)
- **Source file:** `output/stage-01/gap-treelist-inventory.md`
- **Pre-prioritization research:** `output/stage-01/pre-prioritization-research.md` (2026-04-10 — resolves decisions #2 and #3 with codebase evidence: 0 runtime consumers of `List<TreeListColumn>`, and shared DataGrid types already at folder level so no `.Shared` refactor is needed)
- **Source description:** Assessed current MariloTreeList source against the full 52-file spec tree and the single-page placeholder demo. Strategic observation: ≈51% of gaps (22 of 43) are direct MariloDataGrid-parity items suitable for subsystem reuse.

## Resolution Scope

- **Total gaps:** 43
- **By severity:** 6 Critical / 17 High / 14 Medium / 6 Low
- **Scope classification:** `systematic` (cross-cutting across 14 feature areas)
- **Active phase:** 01-intake complete; awaiting Stage 02 prioritization
- **Critical path:** GAP-TREELIST-001 (child-tag `<TreeListColumns>` wrapper) → GAP-TREELIST-002 (Expandable column) → everything else
- **Execution model recommendation:** Phased subagent-driven rewrite mirroring the MariloGantt full-rewrite precedent. Because ≈51% of gaps reuse DataGrid subsystems, effective scope is closer to 20–25 genuinely new work items + 18–22 subsystem-wiring items.

## Resolution Tracking

- **01-intake:** ✅ Complete — 2026-04-10 (assess mode, 43 gaps inventoried)
- **02-prioritize:** Not started — blocked on 7 human decisions
- **03-resolution-design:** Not started
- **04-remediation-plan:** Not started
- **05-implement:** Not started
- **06-validate:** Not started

## Open Human Decisions (before Stage 02 can proceed)

**Evidence-resolved (2026-04-10 via `pre-prioritization-research.md` — pending human ratification):**

- ~~2. `TreeListColumn` backward compat~~ → **Recommend: break cleanly.** Evidence: 0 runtime consumers; the existing `TreeListColumn.cs` is a 17-line POCO with no logic; the only usage sites are the component itself (being rewritten) and the placeholder demo (which doesn't use the parameter at runtime).
- ~~3. DataGrid subsystem reuse strategy~~ → **Recommend: reuse existing shared types in place, mirror `MariloDataSheet` partial-class split.** Evidence: DataGrid folder already has shared types (`GridState`, `GridEventArgs`, `GridColumnFrozenPosition`, `Sizing/*`) at the top-level namespace; `MariloDataSheet` is the precedent for a second grid-family component reusing them via namespace. No `.Shared` refactor or copy-paste needed.

**Still open (require genuine human judgment):**

- **Decision #1 — Branch strategy:** rebuild in place on `workInProgress`, or a dedicated `treelist-rewrite` branch? (Projected larger than Gantt — a branch is probably the right call.)
- **Decision #4 — Flat vs hierarchical data default:** which binding style should the canonical "quick start" demo use?
- **Decision #5 — Editing UX:** built-in popup form (DataGrid-style) or consumer-provided `EditTemplate`?
- **Decision #6 — Virtualization + paging composition:** if both enabled, which wins? (Recommended: paging wraps virtualization.)
- **Decision #7 — Row drag-drop reparenting semantics:** `Into` vs `Before` vs `After` on drop, based on cursor Y-position within target row. (Can be deferred to Phase J.)

## Test Coverage Rollup

- **Batch:** none yet
- **Tests written:** 0
- **Tests passing:** 0

## Constraints

- No Telerik dependencies
- License: MIT / Apache-2.0 / BSD only
- Must inherit from MariloComponentBase
- Must use CssProvider pattern (no hardcoded CSS classes)
- Maximize reuse of existing MariloDataGrid subsystems — do not reinvent paging/sorting/filtering/selection/editing/virtualization/frozen-columns/row-drag-drop
