# Implementation Guardrails

Agreed rules that every build pass must follow. Violations should be caught in Stage 06 review.

## Layout Rules

1. **`MainLayout` is the only component that renders `MariloAppShell`.** No other layout, page, or component may render `MariloAppShell`. Sub-layouts (e.g., `SettingsLayout`) declare `@layout MainLayout` and render only their inner frame.
2. **Nested layouts render content only.** A sub-layout emits its own chrome (sidebar nav, page header) and `@Body`. It never re-renders the app shell, sidebar, or footer.

## State Management

3. **Shared state lives in scoped services.** Theme, notification, and user-context state live behind DI-registered interfaces. No layout or page stores shared state in private fields.
4. **Event subscribers implement `IDisposable`.** Every component that subscribes to a service event (`Changed`, `ThemeChanged`, etc.) must implement `@implements IDisposable` and unsubscribe in `Dispose()`.

## Auth and Permissions

5. **Demo auth seams are explicit.** `ICurrentUserContext` (or equivalent) is annotated "DEMO ONLY" with replacement guidance. All plan/admin gating routes through this service — no scattered hardcoded checks.

## Data Layer

6. **DTO vs VM separation.** Transport DTOs in dedicated files; page-facing VMs carry validation and UI state. UI never binds to EF entities or DAB/GraphQL types directly.
7. **Serilog + OTEL + DAB is the intended stack.** No hand-rolled CRUD controllers. Entity APIs are served by Data API Builder with GraphQL as the standard surface.
8. **Migrations via MigrationService.** Schema changes go through the one-shot worker so DAB can `WaitForCompletion` before reading the schema.

## Components

9. **Marilo-native inputs only.** Settings forms and domain pages use Marilo components from `src/Marilo.Components/`. If a control is missing, create it upstream in the component library rather than pulling in another library or inlining HTML inputs.
10. **Inherit `MariloComponentBase`.** All new Marilo components inherit `MariloComponentBase` (provides Class/Style/AdditionalAttributes, ClassBuilder, CssProvider, ThemeService, dispose pattern).

## Shortcuts

11. **`Ctrl+,` / `Cmd+,` is reserved globally** for opening settings. No other action may bind to this chord.

## CSS

12. **SCSS not CSS.** FluentUI provider styles are edited in SCSS source files, not compiled CSS.
13. **Component scoped vars.** Shell-style components use `.razor.css` with `--mar-{abbr}-*` local vars falling back to `--marilo-*` tokens. Form inputs use CssProvider classes, not scoped CSS.
