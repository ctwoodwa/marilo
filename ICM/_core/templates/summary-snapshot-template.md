# Workspace Status

<!-- SUMMARY SNAPSHOT -- read at session start for fast orientation only.
     NOT authoritative. If this file contradicts a stage output, the stage output wins.
     Update this file after completing a stage or a significant batch of work.
     Do not append -- replace the content and update the date.
     Keep this file under 50 lines. See Pattern 16 in _core/CONVENTIONS.md. -->

## Header

| Field | Value |
|-------|-------|
| Workspace | {{WORKSPACE_NAME}} |
| Last updated | {{YYYY-MM-DD}} |
| Current phase | {{STAGE_NAME}} (Stage {{N}} of {{TOTAL}}) |

## Pipeline Status

<!-- Mark each stage: [x] complete, [ ] pending, [~] in progress, [!] blocked -->

```
Stage 01 -- [x] {{STAGE_1_NAME}}
Stage 02 -- [~] {{STAGE_2_NAME}}   <-- in progress
Stage 03 -- [ ] {{STAGE_3_NAME}}
Stage 04 -- [ ] {{STAGE_4_NAME}}
```

Key outputs so far:

- `stages/01-{{STAGE_1_NAME}}/output/{{ARTIFACT}}.md` -- done

## Next Actions

<!-- 1-3 actions only. Be specific. -->

1. {{NEXT_ACTION_1}}
2. {{NEXT_ACTION_2}}

## Blockers

<!-- Remove this section if there are no blockers. -->

- {{BLOCKER_DESCRIPTION}} -- waiting on {{OWNER_OR_INPUT}}
