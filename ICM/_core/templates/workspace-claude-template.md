<!-- Target: ~800 tokens. Trim if longer. -->
<!-- Build note: this file is Layer 0 routing only. No reference content, no rule
     definitions, no extended examples. If you are adding prose, definitions, or
     domain knowledge here, move it to CONTEXT.md or a reference file instead. -->
# [Workspace Name]

[One sentence: what this workspace does.]

## Folder Map

```
[workspace-name]/
├── CLAUDE.md          (you are here)
├── CONTEXT.md         (start here for task routing)
├── setup/             (onboarding questionnaire)
├── skills/            (bundled Claude skills for domain knowledge)
├── [context-folder]/  (shared context files)
├── stages/
│   ├── 01-[name]/     ([brief description])
│   ├── 02-[name]/     ([brief description])
│   └── 03-[name]/     ([brief description])
└── shared/            (cross-stage reference files)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
<!-- Custom trigger examples -- add workspace-specific triggers below setup/status:
| `ingest`    | Load a source artifact and run Stage 01 analysis |
| `resolve`   | Jump to Stage 03 with an open gap item as input  |
| `convert`   | Jump to Stage 04 with a specific input file      |
| `inventory` | Run Stage 01 in inventory-only mode (no output)  |
-->

## Routing

| Task | Go To |
|------|-------|
| [Task type 1] | `stages/01-[name]/CONTEXT.md` |
| [Task type 2] | `stages/02-[name]/CONTEXT.md` |
| [Task type 3] | `stages/03-[name]/CONTEXT.md` |
| Run onboarding / check tool prerequisites | `setup/questionnaire.md` |

<!-- Build note: before adding cross-references, verify no circular
     dependencies. Target files must not reference back. See Pattern 3. -->
<!-- Build note: creative/build stages need Checkpoints and Audit sections.
     Linear/extraction stages can omit them. See Patterns 11 and 12. -->

## What to Load

<!-- Map each task to its minimal file set. Loading more files dilutes quality.
     The context window is working memory, not storage. -->
<!-- Every "Do NOT Load" entry must explain WHY those files are excluded,
     not just list them. This prevents accidental loading. See Pattern 1. -->
<!-- CRITICAL: Agents must never load stages they are not actively executing.
     Loading upstream stages "for context" is the most common cause of context
     overload. Each stage's CONTEXT.md defines its own inputs -- trust it. -->

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| [Task 1] | [minimal file list] | [what to skip and why -- e.g., "Stage 02 CONTEXT.md: not executing it"] |
| [Task 2] | [minimal file list] | [what to skip and why -- e.g., "shared/ reference files: not needed for extraction"] |
<!-- example: | Build stage | stages/03-build/CONTEXT.md, skills/[name]/SKILL.md | Stages 01-02 (already complete, outputs are inputs), unrelated skills (bloat) | -->

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file, the next stage picks up your edits.
