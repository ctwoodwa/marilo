# Stage 03 - Evolution Design

Define which standards to adopt for the project and how to apply them safely in a brownfield environment.

## Inputs

| Source              | File/Location                                              | Section/Scope                                      | Why                             |
| ------------------- | ---------------------------------------------------------- | -------------------------------------------------- | ------------------------------- |
| Previous stage      | ../02-gap-analysis/output/[slug]-standards-gap-analysis.md | Full file                                          | Primary gap and priority input  |
| Canonical standards | ../../references/layer-3-defaults-index.md                 | Domain, architecture, governance, release channels | Confirms target-state decisions |

## Process

1. Review prioritized gaps and identify candidate standards to adopt.
2. For each selected standard, define `Adopt?` (`Yes`, `Partial`, `No`) with rationale.
3. Define scope (`Whole solution` or `Specific services/components`) for each decision.
4. Define approach (`Incremental` or `Big-bang`) and legacy compatibility strategy.
5. Identify dependencies, sequencing constraints, and known risks.
6. Save outputs.

## Outputs

| Artifact                 | Location                        | Format   |
| ------------------------ | ------------------------------- | -------- |
| Upgrade design decisions | output/[slug]-upgrade-design.md | Markdown |

## Checkpoints

| After Step | Agent Presents                                        | Human Decides                         |
| ---------- | ----------------------------------------------------- | ------------------------------------- |
| 2          | Per-standard adoption decision matrix                 | Approve adoption scope and exceptions |
| 4          | Proposed migration approaches and compatibility notes | Confirm rollout strategy direction    |

## Audit

| Check                  | Pass Condition                                                               |
| ---------------------- | ---------------------------------------------------------------------------- |
| Decision completeness  | Every prioritized Stage 02 gap has an explicit Adopt/Scope/Approach decision |
| Rationale quality      | `Partial` and `No` decisions include documented rationale                    |
| Compatibility planning | Legacy compatibility implications are explicit where relevant                |
| Output naming          | Artifact matches `output/[slug]-upgrade-design.md`                           |
