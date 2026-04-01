# ICM JIRA Workflow Mapping

> **Role in ICM:** Quality control reference — maps ICM quality gates to JIRA statuses, transitions, and required fields to enforce test coverage in the development workflow.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## Purpose

This document maps each ICM quality gate to concrete JIRA workflow states, ensuring that test planning and execution are enforced through the tracking system rather than relying on manual discipline alone.

---

## Workflow States and Gate Mapping

| JIRA Status | ICM Gate | Required Fields / Links | Test Requirement |
|---|---|---|---|
| **Backlog** | — | Epic link, acceptance criteria | — |
| **Ready for Refinement** | Gate 1: Problem Framing | ADR link (draft), affected modules | Seam classification documented |
| **Refined** | Gate 2: Options & Drivers | ADR options section completed | Behavior list and test types identified |
| **Ready for Dev** | Gate 3: Decision & Boundaries | ADR decision finalized, boundary selection (State/Service/CQRS) | Boundary-to-test mapping complete |
| **In Progress** | Gate 4: Implementation Plan | At least one linked "Test Task" sub-issue | Test work items exist per seam |
| **Ready for Review** | Gate 4→5 transition | Checklist confirming tests written/updated | Tests exist and pass locally |
| **In Review** | — | PR link, CI status | CI green, coverage thresholds met |
| **Done** | Gate 5: Validation | All sub-tasks closed, coverage report | Tests passing, coverage criteria met |

---

## Required Sub-Issue Types

For each story or task that passes through Gate 3 (boundary selection), create sub-issues:

| Boundary Selected | Sub-Issue Title Pattern | Description |
|---|---|---|
| State | `[Feature] State transition tests` | Unit tests for state actions, derivations, RouteState |
| Service | `[Feature] Service orchestration tests` | Unit tests with mocked ports, integration tests |
| CQRS | `[Feature] CQRS handler tests` | Handler unit tests + DB integration tests |
| Navigation | `[Feature] Navigation tests` | Component tests for RouteState actions |
| Eventing | `[Feature] Event handler tests` | Notification handler unit + integration tests |

---

## Transition Rules

### Backlog → Ready for Refinement
- **Requires:** Epic link and acceptance criteria present.
- **Validation:** None.

### Ready for Refinement → Refined
- **Requires:** ADR link with at least context and options sections completed.
- **Validation:** Test types identified in ADR or linked document.

### Refined → Ready for Dev
- **Requires:** ADR decision section finalized with boundary selection.
- **Validation:** Boundary-to-test mapping documented. Test sub-issues created.

### Ready for Dev → In Progress
- **Requires:** At least one "Test Task" sub-issue linked.
- **Validation:** Developer confirms test plan before starting implementation.

### In Progress → Ready for Review
- **Requires:** Implementation complete, tests written.
- **Validation checklist:**
  - [ ] Tests exist at each selected seam.
  - [ ] Tests pass locally.
  - [ ] Code coverage for changed files meets threshold.
  - [ ] Relevant checklist (from `20-Checklists/`) completed.

### Ready for Review → In Review
- **Requires:** PR created with CI passing.
- **Validation:** CI pipeline green, coverage report attached or linked.

### In Review → Done
- **Requires:** PR approved and merged. All sub-tasks closed.
- **Validation:**
  - [ ] CI green on main/target branch after merge.
  - [ ] Coverage thresholds met for changed code.
  - [ ] ADR status updated if applicable.

---

## Automation Opportunities

| Automation | Trigger | Action |
|---|---|---|
| Auto-create test sub-issues | Story moves to "Ready for Dev" with boundary tags | Create sub-issues per selected boundary |
| Coverage gate | PR created | Block merge if diff coverage < threshold |
| ADR link check | Story moves to "Ready for Refinement" | Warn if no ADR link present |
| Checklist enforcement | Story moves to "Ready for Review" | Require all checklist items checked |
