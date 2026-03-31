# Resolution Records: MariloIcon

## Summary

MariloIcon has 3 gaps, all documentation-vs-code mismatches. The component implementation is correct and complete. All gaps are resolved as documentation fixes — no code changes required.

---

### RES-ICON-001: Document IconFlip.Both and IconSize.ExtraLarge

**Resolves:** Code→Spec GAP-1 (IconFlip.Both undocumented), Code→Spec GAP-2 (IconSize.ExtraLarge partially documented)
**Status:** Approved

#### Target Pattern

Documentation updates only:
- Add `Both` to the `IconFlip` enum documentation: "None, Horizontal, Vertical, Both"
- Add an `ExtraLarge` example to the icon appearance documentation alongside existing Small/Medium/Large examples

#### Options Considered

**Option A: Documentation fix only**
- Approach: Update docs to reflect the existing enum values
- Pros: Code is already correct and consistent; no risk of breaking changes
- Cons: None
- Effort: Trivial

**Option B: Remove undocumented values**
- Approach: Remove `Both` from `IconFlip` and `ExtraLarge` from `IconSize`
- Pros: Tighter API surface
- Cons: Breaking change for any consumers already using these values; `Both` is a logical completion of flip options; `ExtraLarge` is already referenced in the overview docs
- Effort: Small but risky

#### Decision

**Chosen:** Option A
**Rationale:** Both values are intentional, useful, and partially documented already. `IconFlip.Both` is the logical union of Horizontal+Vertical. `IconSize.ExtraLarge` is referenced in the overview feature list. Removing them would be a breaking change with no benefit.

#### Consequences

- Documentation must be updated (out of scope for code resolution)
- No code changes, no migration needed

#### Success Criteria

- [ ] Flagged for documentation update in resolution status
- [ ] No code modifications required

---

### RES-ICON-002: Reconcile IconThemeColor.Error vs Danger

**Resolves:** Code→Spec GAP-3 (docs use `Error`, enum defines `Danger`)
**Status:** Approved

#### Target Pattern

The enum value `Danger` is correct and consistent with:
- `MariloColorPalette.Danger` property name
- Bootstrap's `text-danger` CSS class mapping in `BootstrapCssProvider`
- FluentUI provider's `themeColor.ToString().ToLower()` pattern producing `"danger"` CSS class
- Industry convention (Bootstrap, Fluent UI, Material all use "danger" for destructive/error states)

The documentation should be corrected: `IconThemeColor.Error` → `IconThemeColor.Danger`.

#### Options Considered

**Option A: Fix documentation to use `Danger`**
- Approach: Change doc examples from `IconThemeColor.Error` to `IconThemeColor.Danger`
- Pros: Consistent with enum, color palette, and CSS providers; no code change
- Cons: None
- Effort: Trivial

**Option B: Add `Error` alias to the enum (`Error = Danger`)**
- Approach: Add `Error = 5` (same int as `Danger`) to `IconThemeColor`
- Pros: Both names compile; forward-compatible with docs
- Cons: C# enum aliasing is confusing (`Error.ToString()` returns "Danger"); breaks FluentUI provider's `ToString().ToLower()` pattern in unpredictable ways; adds API surface bloat for a doc typo
- Effort: Small but creates confusion

**Option C: Rename `Danger` to `Error` everywhere**
- Approach: Rename enum value, update providers, update `MariloColorPalette.Danger` to `Error`
- Pros: Matches some frameworks' naming
- Cons: Breaking change across the entire library; `Danger` is already the established convention matching Bootstrap/Fluent/Material; affects `MariloColorPalette` and all providers
- Effort: Large and risky

#### Decision

**Chosen:** Option A
**Rationale:** `Danger` is the correct, consistent name across the design system. The documentation's use of `Error` is a typo. Adding an alias creates more confusion than it solves. Renaming would be a breaking change for no real benefit.

#### Consequences

- Documentation must be corrected: `IconThemeColor.Error` → `IconThemeColor.Danger`
- No code changes, no migration needed
- Users following current docs would get a compile error; this is the doc bug being flagged

#### Success Criteria

- [ ] Flagged for documentation correction in resolution status
- [ ] No code modifications required
- [ ] Enum remains `Danger` consistent with `MariloColorPalette.Danger`
