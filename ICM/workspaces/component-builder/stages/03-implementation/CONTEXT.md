# Stage 03: Implementation

Build the core infrastructure (enums, models, contracts) and component source files.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../02-api-design/output/api-design.md` | Full file | The API contract to implement |
| Reference | `../../shared/component-patterns.md` | "Base Class" and "CssProvider Integration" | Implementation patterns |
| Reference | `../../shared/file-organization.md` | Full file | Where to place each file in the repo |

## Process

1. Read the API design from Stage 02 output
2. Create enum files in `src/Marilo.Core/Enums/`:
   - One file per enum defined in the API design
   - Follow existing enum naming and documentation patterns
3. Create model files in `src/Marilo.Core/Models/` if needed:
   - EventArgs classes for custom events
   - Configuration or state model classes
4. Add CSS provider method signatures to `IMariloCssProvider.cs`:
   - Add the method(s) defined in the API design to the interface
5. Create the component file(s) in `src/Marilo.Components/[Category]/`:
   - Simple: single `.razor` file with `@code` block
   - Medium/Complex: `.razor` markup + `.razor.cs` code-behind
   - Inherit from `MariloComponentBase`
   - Inject `IMariloCssProvider` via `CssProvider` (inherited)
   - Implement all parameters, events, and rendering logic
   - Apply ARIA attributes using `SetAria()` helper
   - Use `CombineClasses()` and `CombineStyles()` for class/style merging
6. If JS interop is needed, create the interop file in the component's `wwwroot/js/` folder
7. **[Checkpoint]** -- Present the implementation plan (file list with locations) before writing. Ask: Does this structure look correct?
8. Run the audit checks below. If any fail, fix before saving.
9. Write the implementation summary listing all files created

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 6 | Implementation plan: list of files to create with their locations | Whether the file structure is correct before writing code |

## Audit

| Check | Pass Condition |
|-------|---------------|
| API fidelity | Every parameter, event, and enum from the API design is implemented |
| Base class | Component inherits from MariloComponentBase |
| CssProvider usage | Component uses CssProvider for all CSS class generation, no hardcoded classes |
| Accessibility | ARIA role and attributes are applied per the requirements |
| No hardcoded styles | No inline styles for things that should come from the provider |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Implementation summary | `output/implementation-summary.md` | List of files created with paths, plus any implementation notes |
