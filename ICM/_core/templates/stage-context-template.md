# [Stage Name]

[One sentence: what this stage does.]

## Inputs

<!-- List every file the agent needs. Be specific about which sections. -->
<!-- Prefer Layer 3 reference docs for quality standards over Layer 4 outputs. See Pattern 14. -->
<!-- Before adding a row: is this already defined elsewhere? Point there, don't duplicate. See Pattern 5. -->
<!-- Build note: Only list files this stage actually needs. Do not load other stages'
     CONTEXT.md files or outputs unless they are a direct dependency of this stage.
     Loading adjacent stages "for context" is the primary source of context overload. -->

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../0N-prev/output/artifact.md` | Full file | The artifact to work from |
| Reference | `references/example.md` | "Relevant Section" | What it provides |

## Process

<!-- Build note: when referencing large files in steps, specify the section, not the
     whole file (Pattern 4 -- Selective Section Loading). Example: instead of "read
     shared/standards.md", write "read shared/standards.md §Naming Conventions". -->
<!-- Numbered steps. Each step is one concrete action. Be specific enough that
     two different agents following these steps would produce structurally similar
     outputs.

     Too vague: "Write the script"
     Good: "Write the full script in one pass, then audit against the voice
            hard constraints and value brief"

     Too vague: "Generate ideas"
     Good: "Propose 3-5 concept angles, each as a single sentence. Tag each
            with its value type and format." -->

1. Read the input artifact from the previous stage
2. [Step two]
3. [Step three]
4. Save to output/

## Checkpoints

<!-- Points where the agent pauses for human input before continuing.
     Not every stage needs checkpoints. Linear stages (extract, render, validate)
     often run straight through. Creative stages (writing, design, ideation)
     benefit from at least one.

     Format: after which process step, what the agent presents, what the human decides.
     Delete this section for linear stages. See Pattern 11. -->

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| [step #] | [what options/output to show] | [what direction to choose] |

## Audit

<!-- Quality checks before the output is considered done. The agent runs these
     after completing the process steps. If any check fails, revise before saving.

     Not every stage needs an audit. Data extraction or file conversion stages
     may not benefit. Creative and build stages almost always do.
     Delete this section for linear/extraction stages. See Pattern 12. -->

| Check | Pass Condition |
|-------|---------------|
| [Check name] | [What "passing" looks like] |

## Outputs

<!-- What this stage produces and where it goes. -->

| Artifact | Location | Format |
|----------|----------|--------|
| [Name] | `output/[slug]-[type].md` | [Description of the format] |

<!-- Target: 25-80 lines. Trim if longer. -->
