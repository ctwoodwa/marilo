# Stage 02: API Design

Define the component's public API contract: parameters, events, enums, and CSS provider methods.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../01-discovery/output/component-requirements.md` | Full file | Requirements to design the API for |
| Reference | `../../shared/component-patterns.md` | "Parameter Conventions" and "Event Conventions" | API design rules |
| Reference | `../../shared/css-naming.md` | Full file | CSS class naming for provider methods |

## Process

1. Read the component requirements from discovery output
2. Define the parameter list:
   - Name, type, default value, and description for each parameter
   - Mark which parameters are required vs optional
   - Identify parameters inherited from MariloComponentBase (Class, Style, AdditionalAttributes)
3. Define events using EventCallback<T>:
   - Event name, argument type, and when it fires
   - Create custom EventArgs classes if needed (list fields)
4. Define enums for variant/state parameters:
   - Enum name, values, and which parameter uses each enum
   - Follow existing naming: `[Component]Variant`, `[Component]Size`, etc.
5. Define the IMariloCssProvider method signatures:
   - Method name following pattern: `[Component]Class(...)`
   - Parameters that affect CSS class output (variant, size, state, etc.)
   - Return type is always `string`
6. Define RenderFragment slots if the component uses content projection
7. Define CascadingValue/CascadingParameter relationships if applicable
8. **[Checkpoint]** -- Present the full API spec to the user. Ask: Does this API feel right? Any parameters to add, rename, or remove?
9. Run the audit checks below. If any fail, revise before saving.
10. Write the API design document

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 7 | Full API spec: parameters table, events, enums, CSS provider methods, slots | Whether the API surface is correct and complete |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Parameter completeness | Every requirement from Stage 01 maps to at least one parameter or event |
| Naming consistency | Parameter names follow existing Marilo conventions (PascalCase, no abbreviations) |
| Enum coverage | Every constrained-choice parameter uses an enum, not a string |
| CSS method defined | At least one IMariloCssProvider method is specified with clear parameters |
| No orphaned requirements | Every requirement from Stage 01 is addressed in the API |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| API design | `output/api-design.md` | Parameters table, events, enums, CSS provider methods, RenderFragment slots |
