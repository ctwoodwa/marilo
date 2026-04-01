# ICM Quality Control Playbook

> **Purpose:** One-page operational guide for using the Quality Control workspace to drive test coverage through every ICM decision.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## The Workflow

```
1. Start with ADR        → Author or review an ADR (system or module)
2. Run through gates     → Apply quality gates with testing hooks
3. Apply checklists      → Use boundary-specific test checklists
4. Create test tasks     → Generate JIRA sub-tasks for each seam
5. Implement with TDD    → Write tests at chosen seams before/during implementation
6. Track metrics         → Monitor coverage in dashboards
7. Verify at completion  → Confirm all tests pass and coverage targets met
```

---

## Step-by-Step

### 1. Start with the ADR

Every feature that changes architecture, boundaries, or patterns starts with an ADR.

- **System-level decision?** → Use `20-checklists/ADR-Author-Checklist.md`.
- **Module-level decision?** → Use `20-checklists/Module-ADR-Checklist.md`.
- Ensure the Validation section includes test types, seams, and scenarios.

### 2. Run Through Quality Gates

Open `10-quality-gates/ICM-Quality-Gates.md` and work through each gate:

| Gate | You should have | Test output |
|------|----------------|-------------|
| Problem Framing | Problem statement + affected modules | Seam classification (State/Service/CQRS/Nav/Events) |
| Options & Drivers | Options analysis | Behavior list + test types needed |
| Decision & Boundaries | ADR decision + boundary choice | Boundary-to-test mapping |
| Implementation Plan | Phased tasks | Test work items per phase |
| Validation | Completed implementation | All tests passing |

### 3. Apply the Right Checklists

Based on the boundary decision in Gate 3, use the corresponding checklist:

| Boundary | Checklist |
|----------|-----------|
| State vs Service decision | `20-checklists/State-Service-Decision-and-Test-Checklist.md` |
| CQRS/persistence handlers | `20-checklists/Persistence-CQRS-Test-Checklist.md` |
| Navigation or eventing | `20-checklists/Navigation-and-Eventing-Test-Checklist.md` |

Work through the checklist items. Each checkbox represents a test or verification that should exist.

### 4. Create Test Tasks in JIRA

Follow `10-quality-gates/ICM-JIRA-Workflow-Mapping.md`:

- Create "Test Task" sub-issues for each selected seam.
- Link sub-issues to the parent story/epic.
- Include acceptance criteria from the checklist.

### 5. Implement with TDD

Use `40-playbooks/TDD-and-Test-Seams-Playbook.md` for guidance on writing tests first:

- Identify the seam (State, Service, CQRS, Navigation, Eventing).
- Write a failing test that captures the expected behavior.
- Implement the minimum code to make it pass.
- Refactor while keeping tests green.

Use `50-examples/` for concrete examples at each layer.

### 6. Track Metrics

Reference `30-metrics-and-reporting/Test-Coverage-Metrics-and-Dashboards.md`:

- Monitor diff coverage on each PR.
- Track module coverage trends on sprint dashboards.
- Review ADR-governed feature coverage quarterly.

### 7. Verify at Completion

Before marking the story "Done":

- [ ] All test sub-tasks are completed.
- [ ] CI pipeline passes with coverage thresholds met.
- [ ] Relevant checklist items are all checked.
- [ ] ADR status updated to "accepted" (if applicable).
- [ ] Coverage report shows improvement or maintenance.

---

## Quick Reference Card

```
Feature starts  →  Write ADR  →  Gate 1-3 (decide + classify seams)
                                     ↓
                    Gate 4 (plan tests)  →  Create JIRA test sub-tasks
                                     ↓
                    TDD at each seam  →  Apply checklist
                                     ↓
                    Gate 5 (validate)  →  Coverage report  →  Done
```
