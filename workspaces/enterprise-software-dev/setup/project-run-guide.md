# How to Run a Named Project

## Overview

The workspace is a factory. Each project run produces its own named output files.
You do not clear the output folders between projects - you use a project slug instead.

## Before You Start

1. Run setup trigger once if you have not already. This fills all placeholders in _config/.
2. Choose a short lowercase project slug. Examples: crm, portal, dataplatform, adminapi.
   The slug becomes a prefix on all output filenames for this project run.

## Running the Pipeline

Open the workspace in Claude Code and say:
> Start a new project run for [project name]. The slug is [slug].

The agent will:
1. Confirm the slug and record it for this session.
2. Walk you through stages 01 to 05 in order.
3. Name every output file with the slug prefix: [slug]-problem-brief.md, [slug]-architecture.md, etc.
4. At the end of Stage 05, produce [slug]-copilot-instructions.md ready to copy to your project repo.

## After the Pipeline

Copy these files to your .NET solution repo:

| Source in ICM | Destination in project repo |
|---|---|
| stages/05-.../output/[slug]-copilot-instructions.md | .github/copilot-instructions.md |
| stages/05-.../output/[slug]-solution-structure.md | docs/architecture.md |
| stages/05-.../output/[slug]-checklists.md | docs/checklists.md |
| stages/03-.../output/[slug]-coding-standards.md | docs/coding-standards.md |
| stages/05-.../output/scaffold-templates/ | copy stubs into src/ as starting points |

## Git History

All project outputs are committed to enterprise-icm. You have a full design history for every
project ever run through this factory. Diffs show where projects made different architecture decisions.