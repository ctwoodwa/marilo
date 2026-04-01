# MWP Conventions Reference

The canonical MWP conventions live at `/_core/CONVENTIONS.md`. Read that file for the full specification.

This file is a pointer, not a copy. Do not duplicate content from CONVENTIONS.md here.

## Quick Reference

The most relevant conventions for the component-builder workspace:

- **Five-layer routing**: CLAUDE.md (Layer 0) -> CONTEXT.md (Layer 1) -> Stage CONTEXT.md (Layer 2) -> Reference material (Layer 3) -> Working artifacts (Layer 4)
- **Stage contracts**: Every stage CONTEXT.md has Inputs, Process, Outputs sections
- **Stage handoffs**: Stage N writes to output/, Stage N+1 reads from there
- **One-way references**: If A references B, B must not reference A
- **Selective loading**: CONTEXT.md tables specify sections within files, not just filenames
- **One canonical home**: Every piece of information lives in one place
- **CONTEXT.md = routing only**: No definitions, rules, or extended content in CONTEXT.md files
- **Checkpoints**: Creative stages pause for human steering between process steps
- **Stage audits**: Quality checklist before writing to output/
- **Shared constants**: Code-producing workspaces define shared constant files
- **Quality rules**: CONTEXT.md under 80 lines, reference files under 200 lines, no em dashes, lowercase-with-hyphens naming
