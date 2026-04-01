# Stage 01: Discovery

Understand the component requirements, use cases, and accessibility needs through conversation.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| User | (conversation) | Component description and requirements | The component to build |
| Reference | `../../shared/component-patterns.md` | Full file | Know the Marilo component architecture |
| Reference | `../../references/examples/button-walkthrough.md` | Full file | Concrete example of a completed component |

## Process

1. Ask the user to describe the component's purpose and primary use cases. What problem does it solve?
2. Identify the visual states and variants. What does it look like in each state? (default, hover, active, disabled, focused, error)
3. Identify interactive behavior. What events does it emit? What user actions trigger state changes?
4. Identify composition patterns. Does it have child components? Does it accept RenderFragment content? Does it participate in a parent-child relationship?
5. Research accessibility requirements:
   - What ARIA role should it use?
   - What keyboard interactions are expected? (Tab, Enter, Space, Arrow keys, Escape)
   - What aria-* attributes are needed for state communication?
   - What screen reader announcements should occur?
6. Identify data binding needs. Does it support two-way binding (@bind)? What data types does it work with?
7. Scan existing Marilo components for reusable patterns. If the user named reference components in onboarding, read those to identify shared approaches.
8. Identify theme considerations. What visual properties vary between FluentUI and Bootstrap? (spacing, borders, shadows, animations)
9. **[Checkpoint]** -- Present the requirements summary to the user. Ask: Are all use cases captured? Any missing states or interactions?
10. Run the audit checks below. If any fail, revise before saving.
11. Write the component requirements document.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 8 | Requirements summary: use cases, states, events, composition, accessibility, theming | Whether the requirements are complete and correctly scoped |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Use case clarity | Every use case has a concrete scenario, not just a feature name |
| State coverage | All visual states are identified (default, hover, active, disabled, focused, plus component-specific) |
| Accessibility complete | ARIA role, keyboard interactions, and screen reader behavior are all specified |
| Theme awareness | Visual properties that vary between providers are called out |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Component requirements | `output/component-requirements.md` | Structured doc: use cases, states, events, composition, accessibility, theming notes |
