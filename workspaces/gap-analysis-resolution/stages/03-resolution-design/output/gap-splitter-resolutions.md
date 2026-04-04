# Resolution Design: MariloSplitter

> Date: 2026-04-04
> Source: gap-splitter-inventory.md (10 gaps), gap-splitter-priorities.md
> Stage: 03-resolution-design
> Scope: batch (Splitter component gaps)

---

## Pre-Resolution Code Audit

Before designing resolutions, the current source was audited against each gap. Several gaps that were open at intake time have since been resolved in the implementation.

### Already Resolved (Code Exists)

| Gap | Claimed Missing | Actual Status |
|-----|-----------------|---------------|
| GAP-SPLITTER-002 | GetState/SetState methods | **Already implemented** — `MariloSplitter.razor` lines 248-261, `SplitterState` type in `SplitterTypes.cs` |
| GAP-SPLITTER-003 | Class parameter | **Already inherited** from `MariloComponentBase` base class via `@inherits MariloComponentBase` |
| GAP-SPLITTER-005 | Per-pane Min/Max parameters | **Already implemented** — `MariloSplitterPane.razor` has `Min` and `Max` string parameters, enforced in `ApplyDrag()` and keyboard resize |
| GAP-SPLITTER-008 | Per-pane Resizable parameter | **Already implemented** — `MariloSplitterPane.razor` line 24: `[Parameter] public bool Resizable { get; set; } = true;`, checked in `HandleMouseDown` and `HandleSeparatorKeyDown` |

These 4 gaps require no code changes. They will be marked **Verified (already resolved)** in the plan and closure pipeline.

---

## Batch 1 Resolutions (Critical + High)

### RES-SPLITTER-001: Add MariloSplitterPanes pass-through wrapper

**Resolves:** GAP-SPLITTER-001
**Status:** Proposed

#### Target Pattern

```razor
<MariloSplitter>
    <MariloSplitterPanes>
        <MariloSplitterPane Size="40%">Left</MariloSplitterPane>
        <MariloSplitterPane>Right</MariloSplitterPane>
    </MariloSplitterPanes>
</MariloSplitter>
```

The `MariloSplitterPanes` component is a transparent wrapper that renders its `ChildContent` directly, providing spec-compatible markup structure.

#### Options Considered

**Option A: Pass-through RenderFragment component**
- Approach: Create `MariloSplitterPanes.razor` that only renders `@ChildContent`. No logic.
- Pros: Consumer code matches spec examples. Zero behavior change. Backward compatible (direct children still work via CascadingValue).
- Cons: One more file to maintain (trivial — ~10 lines).
- Effort: S (< 30 minutes)

**Option B: Named RenderFragment on MariloSplitter**
- Approach: Add `[Parameter] public RenderFragment? Panes { get; set; }` to MariloSplitter, render inside the CascadingValue.
- Pros: No new file. Explicit containment.
- Cons: Changes API shape to `<Panes>...</Panes>` instead of `<MariloSplitterPanes>...</MariloSplitterPanes>`. Does not match spec.
- Effort: S

#### Decision

**Chosen:** Option A
**Rationale:** Matches spec API exactly, zero risk to existing functionality, trivially small.

#### Consequences

- New file: `src/Marilo.Components/Layout/MariloSplitterPanes.razor`
- Existing consumers using direct `<MariloSplitterPane>` children still work (non-breaking).
- Demo pages should use the wrapper for spec compliance.

#### Success Criteria

- [ ] `MariloSplitterPanes` component exists and renders ChildContent
- [ ] Panes register correctly when nested inside `<MariloSplitterPanes>` wrapper
- [ ] Existing direct-child pane pattern continues to work (backward compatible)
- [ ] bUnit test confirms both usage patterns

---

### RES-SPLITTER-002: Add SplitterOrientation enum

**Resolves:** GAP-SPLITTER-004
**Status:** Proposed

#### Target Pattern

```csharp
public enum SplitterOrientation
{
    Horizontal,
    Vertical
}
```

The `Orientation` parameter on `MariloSplitter` accepts `SplitterOrientation` (default `Horizontal`).

#### Options Considered

**Option A: New SplitterOrientation enum, replace StackDirection**
- Approach: Create `SplitterOrientation` enum. Change `Orientation` parameter type from `StackDirection` to `SplitterOrientation`. Update internal references.
- Pros: Spec-compatible. Clear semantic meaning. No ambiguity with layout stack direction.
- Cons: Breaking change for any consumers currently using `StackDirection.Horizontal` or `StackDirection.Vertical` on the splitter.
- Effort: S

**Option B: Keep StackDirection, add SplitterOrientation as alias**
- Approach: Create `SplitterOrientation` enum. Accept both types via implicit conversion or separate parameter.
- Pros: Non-breaking.
- Cons: Two ways to do the same thing. Maintenance burden. Confusing API.
- Effort: M

#### Decision

**Chosen:** Option A
**Rationale:** The library is pre-release; breaking changes are acceptable. A splitter's orientation is semantically different from a stack's direction. Clean API is more valuable than backward compatibility at this stage.

#### Consequences

- New enum: `SplitterOrientation` in `src/Marilo.Core/Enums/LayoutEnums.cs` or `src/Marilo.Components/Layout/SplitterTypes.cs`
- `MariloSplitter.razor` parameter type changes from `StackDirection` to `SplitterOrientation`
- Internal usages of `StackDirection.Horizontal`/`Vertical` in the splitter code update to `SplitterOrientation.Horizontal`/`Vertical`
- CSS provider `SplitterClass()` signature may need updating

#### Success Criteria

- [ ] `SplitterOrientation` enum exists with `Horizontal` and `Vertical` values
- [ ] `MariloSplitter.Orientation` parameter uses `SplitterOrientation` type
- [ ] Horizontal layout renders correctly
- [ ] Vertical layout renders correctly
- [ ] bUnit tests verify both orientations

---

### RES-SPLITTER-003: bUnit test suite for MariloSplitter

**Resolves:** GAP-SPLITTER-006
**Status:** Proposed

#### Target Pattern

A `SplitterTests.cs` file in the test project covering:
1. Pane registration and rendering (2+ panes)
2. Collapse/expand toggle via button
3. Keyboard resize (arrow keys)
4. GetState/SetState round-trip
5. Min/Max enforcement
6. Resizable=false disables drag
7. Nested splitter rendering
8. SplitterPanes wrapper compatibility
9. SplitterOrientation enum rendering
10. Event callbacks (OnResize, OnCollapse, OnExpand)

#### Options Considered

**Option A: Single test class with categorized test methods**
- Approach: One `SplitterTests.cs` file with `[Fact]` methods grouped by feature area.
- Pros: Consistent with existing test structure (e.g., TreeView tests). Easy to find and run.
- Cons: File may grow large.
- Effort: L

**Option B: Multiple test classes per feature area**
- Approach: Separate files for resize, collapse, state, accessibility.
- Pros: Smaller files. Parallel execution.
- Cons: Inconsistent with existing project test structure.
- Effort: L

#### Decision

**Chosen:** Option A
**Rationale:** Matches existing test organization patterns in this project. Single file keeps all splitter tests discoverable.

#### Consequences

- New file: `tests/Marilo.Components.Tests/Layout/SplitterTests.cs`
- Depends on RES-SPLITTER-001 (wrapper) and RES-SPLITTER-002 (enum) being implemented first
- Tests will use bUnit `TestContext` with `RenderComponent<MariloSplitter>`

#### Success Criteria

- [ ] SplitterTests.cs exists with ≥15 test methods
- [ ] Tests cover: pane registration, collapse, keyboard, state, min/max, resizable, events
- [ ] All tests pass (`dotnet test --filter SplitterTests`)

---

### RES-SPLITTER-004: Demo pages for MariloSplitter

**Resolves:** GAP-SPLITTER-007, GAP-SPLITTER-009
**Status:** Proposed

#### Target Pattern

Demo pages at `samples/Marilo.Demo/Pages/Components/Splitter/`:
- `Overview.razor` — Basic horizontal and vertical splitters
- `Collapsible.razor` — Collapsible panes with toggle
- `StatePersistence.razor` — GetState/SetState demo with localStorage
- `FullViewport.razor` — 100%-height layout with header/footer/sidebar (addresses GAP-009)

#### Options Considered

**Option A: Four separate demo pages**
- Approach: One page per scenario, consistent with other component demo patterns.
- Pros: Clear navigation. Each page demonstrates one concept.
- Cons: More files.
- Effort: M

**Option B: Single overview page with sections**
- Approach: One long page with all scenarios.
- Pros: Fewer files.
- Cons: Hard to navigate. Inconsistent with other component demos.
- Effort: M

#### Decision

**Chosen:** Option A
**Rationale:** Matches established demo page pattern in the project.

#### Consequences

- New directory: `samples/Marilo.Demo/Pages/Components/Splitter/`
- 4 new `.razor` files
- Navigation menu update may be needed

#### Success Criteria

- [ ] 4 demo pages exist and compile
- [ ] Each page renders the splitter correctly
- [ ] FullViewport demo shows 100%-height layout pattern
- [ ] Demo uses `<MariloSplitterPanes>` wrapper (spec-compliant API)

---

### RES-SPLITTER-005: Verify and test nested splitter support

**Resolves:** GAP-SPLITTER-010
**Status:** Proposed

#### Target Pattern

Nested splitters work correctly because `CascadingValue IsFixed="true"` scopes the cascade to the immediate parent splitter. A bUnit test confirms a splitter inside a pane registers its own child panes independently.

#### Options Considered

**Option A: Verification test only**
- Approach: Write a bUnit test that nests a splitter inside a pane. Verify inner panes register to the inner splitter, not the outer one.
- Pros: If it works (likely, given IsFixed=true), no code changes needed.
- Cons: None.
- Effort: S

**Option B: Code change + test**
- Approach: If verification fails, fix the CascadingValue scoping.
- Pros: Ensures correctness.
- Cons: May not be needed.
- Effort: S-M

#### Decision

**Chosen:** Option A (escalate to B if test fails)
**Rationale:** The `IsFixed="true"` attribute should correctly scope the cascade. Test first, fix only if needed.

#### Consequences

- One additional test method in SplitterTests.cs
- If nesting fails, the CascadingValue approach may need redesign (unlikely)

#### Success Criteria

- [ ] bUnit test renders nested splitter with inner panes
- [ ] Inner panes register to inner splitter only
- [ ] Outer pane count is unaffected by nested children

---

## Summary

| Resolution | Gaps Resolved | Status | Effort |
|------------|--------------|--------|--------|
| RES-SPLITTER-001 | GAP-001 | Proposed | S |
| RES-SPLITTER-002 | GAP-004 | Proposed | S |
| RES-SPLITTER-003 | GAP-006 | Proposed | L |
| RES-SPLITTER-004 | GAP-007, GAP-009 | Proposed | M |
| RES-SPLITTER-005 | GAP-010 | Proposed | S |
| (Pre-resolved) | GAP-002, GAP-003, GAP-005, GAP-008 | Verified | — |

Total implementation effort: ~L (primarily test writing + demo pages)
