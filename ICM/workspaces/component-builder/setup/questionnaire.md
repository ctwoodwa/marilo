# Onboarding Questionnaire: Component Builder

Read this file when the user types "setup". Ask ALL questions below in a single conversational pass. The user should be able to answer everything in one message. These answers inform the discovery conversation in Stage 01 -- they are not placeholder replacements.

---

### Q1: What component are you building?
- Examples: "DatePicker", "Rating", "ColorPicker", "Chip", "FileUpload"
- Type: free text
- Purpose: Names the component and sets the scope

### Q2: Which category does it belong to?
- Options: Buttons, Navigation, Forms, Layout, DataDisplay, DataGrid, Overlays, Feedback, Charts, Scheduling, Editors, Media, Utility
- Type: single choice
- Purpose: Determines the folder location under Marilo.Components

### Q3: Describe what the component does in one sentence.
- Examples: "Lets users pick a date from a calendar popup." / "Displays a star rating that users can set or view."
- Type: free text
- Purpose: Gives Stage 01 a starting point for deeper discovery

### Q4: What is the complexity level?
- Options: Simple (single file, few parameters), Medium (code-behind, moderate parameters), Complex (multiple partials, child components, services)
- Type: single choice
- Purpose: Calibrates how many files and stages to expect

### Q5: Does the component need JavaScript interop?
- Options: Yes / No / Not sure
- Type: single choice
- Purpose: Determines whether JS interop scaffolding is needed

### Q6: Are there any existing components to reference for patterns?
- Examples: "Similar to MariloButton for variants" / "Like MariloTreeView for hierarchical data"
- Type: free text (optional)
- Purpose: Helps Stage 01 identify reusable patterns from existing components

---

## After Onboarding

Tell the user:

> Got it. The component-builder will guide you through seven stages:
> 1. Discovery (requirements, use cases, accessibility)
> 2. API Design (parameters, events, enums, CSS provider)
> 3. Implementation (source code, models, enums)
> 4. Theming (FluentUI and Bootstrap providers)
> 5. Demos and Docs (demo pages, API documentation)
> 6. Testing (unit tests, provider tests)
> 7. Workspace Scaffolding (delivery workspace, gap-analysis workspace, spec docs)
>
> When complete, your component will have everything it needs to participate in the full ICM pipeline -- including its own delivery and gap-analysis workspaces.
>
> Ready? Start with Stage 01 -- Discovery.

Then point them to `stages/01-discovery/CONTEXT.md`.
