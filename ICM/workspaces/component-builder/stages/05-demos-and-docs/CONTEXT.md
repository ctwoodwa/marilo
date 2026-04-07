# Stage 05: Demos and Documentation

Create demo pages and API documentation for the new component.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| API design | `../02-api-design/output/api-design.md` | Full file | Parameters, events, enums to document |
| Implementation | `../03-implementation/output/implementation-summary.md` | Full file | Actual file paths and implementation notes |
| Reference | `../../shared/file-organization.md` | "Docs Files" and "Demo Files" sections | Where to place docs and demos |

## Process

1. Read the API design and implementation summary
2. Create the component spec folder in `docs/component-specs/[component-name]/`:
   - `overview.md` -- Purpose, basic usage, parameters table, code examples
   - `appearance.md` -- Variant options, size options, styling parameters
   - `events.md` -- Event handlers with code examples
   - `accessibility/overview.md` -- Keyboard interactions, ARIA attributes, screen reader notes
   - `toc.yml` -- Table of contents for DocFx
3. Add YAML front matter to each doc file:
   - title, page_title, description, slug, tags, published, position
4. Create demo pages in `samples/Marilo.Demo/Pages/Components/[ComponentName]/`:
   - `Overview.razor` -- Basic usage, variants, sizes, events, accessibility info
   - Use `PageSection`, `DemoSection`, and `AccessibilityInfo` components
   - Include inline code samples as string constants
5. If provider-specific demos differ, add pages to FluentUI and Bootstrap demo projects
6. **[Checkpoint]** -- Present the doc outline and demo sections before writing. Ask: Are all features covered? Any sections to add or remove?
7. Run the audit checks below. If any fail, fix before saving.
8. Write the docs and demos summary

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 5 | Doc outline (sections and content summary) and demo section list | Whether documentation coverage is complete |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Parameter coverage | Every public parameter appears in the overview.md parameters table |
| Code examples | At least one code example per major feature (basic usage, each variant, events) |
| Accessibility section | Keyboard interactions and ARIA attributes are documented |
| Demo completeness | Demo page shows basic usage, variants, sizes, and event handling |
| Front matter | Every doc file has valid YAML front matter with slug and tags |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Docs and demos summary | `output/docs-demos-summary.md` | List of doc files and demo pages created with paths |
