# Resolution Record Format

Standard shape for resolution decisions. Each resolved gap (or batch of related gaps) gets one resolution record.

## Record Shape

```markdown
### RES-[AREA]-[NNN]: [Short title]

**Resolves:** [GAP-IDs this resolution addresses, comma-separated]
**Status:** Proposed | Approved | Implemented | Verified

#### Target Pattern

[Describe what the code/config/process should look like when the gap is closed.
Include code snippets, API signatures, or configuration examples as needed.]

#### Options Considered

**Option A: [Name]**
- Approach: [How it works]
- Pros: [Benefits]
- Cons: [Drawbacks]
- Effort: [Estimate]

**Option B: [Name]**
- Approach: [How it works]
- Pros: [Benefits]
- Cons: [Drawbacks]
- Effort: [Estimate]

#### Decision

**Chosen:** Option [X]
**Rationale:** [Why this option wins]

#### Consequences

- [What changes downstream]
- [What other files/components are affected]
- [What migration is needed, if any]
- [What documentation must be updated]

#### Success Criteria

- [ ] [Specific, testable criterion 1]
- [ ] [Specific, testable criterion 2]
```

## ID Convention

- Format: `RES-[AREA]-[NNN]`
- Matches the area convention from gap records.
- NNN is sequential within the area, independent of gap numbering.
- A single resolution can address multiple gaps (e.g., a cross-cutting theme).

## Batching Rules

Batch related gaps into a single resolution when:
- They share the same root cause (e.g., missing base class parameter).
- The fix is identical across all affected components.
- Resolving them separately would create redundant records.

Do NOT batch when:
- Gaps require different solution approaches.
- Components have unique constraints that affect the resolution.
- The gaps are in different priority phases.

## ADR Threshold

Produce a formal ADR (alongside the resolution record) when a resolution:
- Changes a shared interface or base class.
- Introduces a new pattern that all future components should follow.
- Renames public API surface (breaking change).
- Requires a migration across multiple existing components.

Use the ADR template from canonical defaults (`docs/icm-defaults/040-governance/adr-template.md`).
