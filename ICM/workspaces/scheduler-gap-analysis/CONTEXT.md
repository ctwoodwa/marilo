# Component Gap-Analysis Workspace -- MariloScheduler

Structured gap tracking and resolution for MariloScheduler.

## Task Routing

| Task Type | Entry Point | Trigger Keyword |
|-----------|-------------|-----------------|
| Import/discover gaps | stages/01-intake/CONTEXT.md | ingest |
| Score and sequence | stages/02-prioritize/CONTEXT.md | prioritize |
| Design resolutions | stages/03-resolution-design/CONTEXT.md | resolve |
| Plan remediation | stages/04-remediation-plan/CONTEXT.md | plan |
| Execute changes | stages/05-implement/CONTEXT.md | implement |
| Verify and close | stages/06-validate/CONTEXT.md | close |

## Shared Resources

| Resource | Location | Section to Load |
|----------|----------|-----------------|
| Gap context | _config/gap-context.md | Full file |
| Coverage summary | _config/coverage-summary.md | Full file |
| Gap record format | shared/gap-record-format.md | Full file |
| Delivery workspace | ../scheduler-delivery/CLAUDE.md | Routing table only |
