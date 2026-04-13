# Stage 05 Wave B Report: MariloAllocationScheduler

**Worker:** `w-allocation-scheduler-gap-analysis`
**Stage:** 05-implement (second cycle, Wave B)
**Date:** 2026-04-12
**Build:** `dotnet build Marilo.slnx` exit 0 (0 warnings, 0 errors)

---

## Task Results

### ASC-W4-B01: R2 Scrollbar Fix (FluentUI) -- DONE

**File:** `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss`

- Replaced `scrollbar-width: none` with `scrollbar-width: thin`
- Added `scrollbar-color: var(--marilo-color-border, #d1d1d1) transparent`
- Replaced `::-webkit-scrollbar { display: none }` with 6px thumb, border-radius 3px, transparent track
- Hidden scrollbar CSS fully removed
- All 4 acceptance criteria met

### ASC-W4-B02: R2 Scrollbar Fix (Bootstrap) -- DONE

**File:** `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss`

- Same pattern as B-01 using `var(--bs-border-color)` tokens
- Replaced `scrollbar-width: none` with `scrollbar-width: thin`
- Added WebKit fallback with `--bs-border-color` thumb
- Hidden scrollbar CSS fully removed
- All 4 acceptance criteria met

### ASC-W4-B03: R3 AccessibilityDemo.razor -- DONE

**File:** `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AccessibilityDemo.razor` (new)

- Page routable at `/components/allocation-scheduler/accessibility`
- 4 demo sections: Keyboard Navigation, ARIA Roles & Landmarks, Screen-Reader Live Region, High-Contrast Mode
- Uses `ComponentDemoLayout` with `<PageSection>` + `<DemoSection>` pattern
- Follows `AdvancedFeatures.razor` layout conventions
- Builds without error
- All 5 acceptance criteria met

### ASC-W4-B04: R4 ThemingDemo.razor -- DONE

**File:** `samples/Marilo.Demo/Pages/Components/AllocationScheduler/ThemingDemo.razor` (new)

- Page routable at `/components/allocation-scheduler/theming`
- 3 demo sections: Dark/Light Toggle, Provider Swap, Custom Token Overrides
- Uses `ComponentDemoLayout` with `<PageSection>` + `<DemoSection>` pattern
- Dark/light toggle wraps component in `data-marilo-theme` attribute
- Custom token section includes interactive color pickers for `--marilo-color-primary`, `--marilo-color-surface`, `--marilo-color-border`
- Builds without error
- All 5 acceptance criteria met

### ASC-W4-B05: R8 Spec Re-Audit Batch -- DONE

12 spec records applied across 7 spec files:

| Record | File | Status | Notes |
|---|---|---|---|
| SPEC-AS-W1-010 | `scenario-planning.md` | DONE | All `AllocationScenarioStatus` -> `ScenarioStatus` (enum heading, code block, data model, 2 code examples) |
| SPEC-AS-W1-011 | `events.md` | VERIFIED | Events.md already uses `ScenarioStatusChangedArgs` correctly. No wrong type name found. Closed as correct. |
| SPEC-AS-W1-012 | `events.md` | DONE | Added `OnTimeColumnResized` event section with `Dictionary<int, int>` payload docs and usage example |
| SPEC-AS-W1-013 | `overview.md` | DONE | `DefaultRangeLength` marked deprecated with migration note to use `MinVisibleColumns` + `DefaultRangeUnit` |
| SPEC-AS-W1-014 | `editing.md` | DONE | Drag-fill wording updated to describe inset box-shadow preview. Marked `<!-- pending R10 update -->` for dashed outline change |
| SPEC-AS-W1-015 | `overview.md` | DONE | `ShowJumpToDate` parameter added to Parameters table with description |
| SPEC-AS-W1-016 | `templates.md` | DONE | Grouped Headers section added documenting `__header-group-row` and `__header-group-cell` BEM classes |
| SPEC-AS-W1-017 | `overview.md` | DONE | Current-Period Column Highlight section added documenting `__col-current` CSS class |
| SPEC-AS-W1-018 | `editing.md` | DONE | Column Width and Layout section added documenting `calc(100%/N)` fluid sizing and fixed-width behavior |
| SPEC-AS-W1-019 | `splitter-layout.md` | DONE | Enhanced BEM class descriptions for `--left`, `--right`, `--collapsed` with collapse lifecycle narrative |
| SPEC-AS-W1-020 | `scenario-planning.md` | VERIFIED | Already documented at line 31: "When `DisplayLabel` is non-null it takes precedence over the auto-formatted label." Closed as already correct. |
| SPEC-AS-W1-021 | `theming.md` | DONE | Dark theme token table added with 6 token entries and their dark-mode values/purposes |

---

## Files Modified

| File | Task | Change Type |
|---|---|---|
| `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` | B-01 | SCSS edit (scrollbar rules) |
| `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` | B-02 | SCSS edit (scrollbar rules) |
| `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AccessibilityDemo.razor` | B-03 | New file |
| `samples/Marilo.Demo/Pages/Components/AllocationScheduler/ThemingDemo.razor` | B-04 | New file |
| `docs/component-specs/allocation-scheduler/scenario-planning.md` | B-05 | Spec text (010, 020) |
| `docs/component-specs/allocation-scheduler/events.md` | B-05 | Spec text (011, 012) |
| `docs/component-specs/allocation-scheduler/editing.md` | B-05 | Spec text (014, 018) |
| `docs/component-specs/allocation-scheduler/overview.md` | B-05 | Spec text (013, 015, 016, 017) |
| `docs/component-specs/allocation-scheduler/templates.md` | B-05 | Spec text (016) |
| `docs/component-specs/allocation-scheduler/splitter-layout.md` | B-05 | Spec text (019) |
| `docs/component-specs/allocation-scheduler/theming.md` | B-05 | Spec text (021) |

---

## Verification

- **Build:** `dotnet build Marilo.slnx` exit 0, 0 warnings, 0 errors
- **Task count:** 5/5 complete
- **R-lane coverage:** R2 (B-01, B-02), R3 (B-03), R4 (B-04), R8 (B-05) -- all Wave B lanes
- **Spec record count:** 12/12 addressed (10 edited, 2 verified-already-correct)
- **First-cycle artifacts:** NOT touched
- **Sync areas covered:** source (SCSS), demo (2 new pages), spec (7 files edited)
- **Skill discipline:**
  - `verification-before-completion`: build exit 0 confirmed in current turn
  - `requesting-code-review`: result follows template with per-task acceptance mapping

**Wave B Gate: PASS. All 5 tasks complete. Ready for review.**
