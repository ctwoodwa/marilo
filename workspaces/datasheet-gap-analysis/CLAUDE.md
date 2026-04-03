# Gap Analysis -- MariloDataSheet

**Status: STUB -- no gap phases started**

Resolves documented gaps from gap analysis through a structured lifecycle: intake, prioritization, resolution design, remediation planning, implementation, and validation.

This workspace is initialized. Run intake (Stage 01) to begin gap analysis. Feature areas are tracked in ../datasheet-delivery/_config/delivery-context.md.

## Folder Map

```
datasheet-gap-analysis/
├── CLAUDE.md              (you are here)
├── _config/
│   ├── gap-context.md     (scope, target project, resolution tracking)
│   └── coverage-summary.md (test and closure tracking)
└── stages/
    ├── 01-intake/output/
    ├── 02-prioritize/output/
    ├── 03-resolution-design/output/
    ├── 04-remediation-plan/output/
    ├── 05-implement/output/
    └── 06-validate/output/
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Fast path: paste/point to gap analysis file, jump to Stage 01 |
| `resolve` | Start or continue resolution design (Stage 03) |
| `close` | Jump to validation (Stage 06) for a specific gap |

## Routing

| Task | Go To |
|------|-------|
| Import or create a gap analysis | stages/01-intake/output/ |
| Prioritize and sequence gaps | stages/02-prioritize/output/ |
| Design a resolution for a gap | stages/03-resolution-design/output/ |
| Plan remediation tasks and phases | stages/04-remediation-plan/output/ |
| Implement changes | stages/05-implement/output/ |
| Validate closure and enforce | stages/06-validate/output/ |
| Return to delivery workspace | ../datasheet-delivery/CLAUDE.md |
