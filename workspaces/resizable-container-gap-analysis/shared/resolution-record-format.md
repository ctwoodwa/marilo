# Resolution Record Format -- ResizableContainer

Standard shape for resolution decisions. Each resolved gap (or batch of related gaps) gets one resolution record.

## Record Shape

```markdown
### RES-RESIZABLE-CONTAINER-[NNN]: [Short title]

**Resolution ID:** RES-RESIZABLE-CONTAINER-[NNN]
**Gap ID:** [GAP-RESIZABLE-CONTAINER-NNN IDs this resolution addresses, comma-separated]
**Approach:** [Brief summary of the chosen approach]
**Affected files:** [File paths that will be modified]
**Risk:** Low | Medium | High
**Test plan:** [How the resolution will be verified]

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

#### Success Criteria

- [ ] [Specific, testable criterion 1]
- [ ] [Specific, testable criterion 2]
```

## ID Convention

- Format: `RES-RESIZABLE-CONTAINER-[NNN]`
- NNN: zero-padded sequential number (e.g., 001, 002)
- A single resolution can address multiple gaps (e.g., a cross-cutting theme).

## Standard Fields

| Field | Required | Description |
|-------|----------|-------------|
| resolution id | Yes | Unique identifier following the convention above |
| gap id | Yes | One or more GAP-RESIZABLE-CONTAINER IDs this resolution addresses |
| approach | Yes | Brief summary of the chosen approach |
| affected files | Yes | Source file paths that will be modified |
| risk | Yes | Low, Medium, or High impact risk |
| test plan | Yes | How the resolution will be verified (unit tests, integration tests, manual) |

## Batching Rules

Batch related gaps into a single resolution when:
- They share the same root cause.
- The fix is identical across all affected areas.
- Resolving them separately would create redundant records.

Do NOT batch when:
- Gaps require different solution approaches.
- The gaps are in different priority phases.
