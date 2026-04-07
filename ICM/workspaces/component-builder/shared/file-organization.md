# File Organization

Authoritative reference for where each artifact belongs in the Marilo repository.

---

## Core Files

| Artifact | Location |
|----------|----------|
| Enums | `src/Marilo.Core/Enums/[EnumName].cs` |
| Models / EventArgs | `src/Marilo.Core/Models/[ClassName].cs` |
| CSS provider contract | `src/Marilo.Core/Contracts/IMariloCssProvider.cs` (add methods) |
| Service interfaces | `src/Marilo.Core/Contracts/I[ServiceName].cs` |
| Service implementations | `src/Marilo.Core/Services/[ServiceName].cs` |

---

## Component Files

| Artifact | Location |
|----------|----------|
| Simple component | `src/Marilo.Components/[Category]/Marilo[Name].razor` |
| Code-behind | `src/Marilo.Components/[Category]/Marilo[Name].razor.cs` |
| Additional partials | `src/Marilo.Components/[Category]/Marilo[Name].[Aspect].cs` |
| JS interop | `src/Marilo.Components/wwwroot/js/[name].js` |

Categories: Buttons, Navigation, Forms, Layout, DataDisplay, DataGrid, Overlays, Feedback, Charts, Scheduling, Editors, Media, Utility

---

## Provider Files

| Artifact | Location |
|----------|----------|
| FluentUI CSS provider | `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` (add methods) |
| FluentUI SCSS | `src/Marilo.Providers.FluentUI/Styles/_[component].scss` |
| FluentUI main SCSS | `src/Marilo.Providers.FluentUI/Styles/marilo-fluentui.scss` (add import) |
| Bootstrap CSS provider | `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` (add methods) |
| Bootstrap SCSS | `src/Marilo.Providers.Bootstrap/Styles/_bridge-[component].scss` |
| Bootstrap main SCSS | `src/Marilo.Providers.Bootstrap/Styles/marilo-bootstrap.scss` (add import) |

---

## Docs Files

| Artifact | Location |
|----------|----------|
| Spec folder | `docs/component-specs/[component-name]/` |
| Overview | `docs/component-specs/[component-name]/overview.md` |
| Appearance | `docs/component-specs/[component-name]/appearance.md` |
| Events | `docs/component-specs/[component-name]/events.md` |
| Accessibility | `docs/component-specs/[component-name]/accessibility/overview.md` |
| Table of contents | `docs/component-specs/[component-name]/toc.yml` |

---

## Demo Files

| Artifact | Location |
|----------|----------|
| Base demo page | `samples/Marilo.Demo/Pages/Components/[Name]/Overview.razor` |
| FluentUI demo | `samples/Marilo.Demo.FluentUI/Pages/Components/[Name]/Overview.razor` |
| Bootstrap demo | `samples/Marilo.Demo.Bootstrap/Pages/Components/[Name]/Overview.razor` |

---

## Test Files

| Artifact | Location |
|----------|----------|
| Unit tests | `tests/Marilo.Tests.Unit/[Category]/Marilo[Name]Tests.cs` |
| Provider tests | `tests/Marilo.Tests.Unit/[Category]/[Provider]CssProvider[Name]Tests.cs` |
| Integration tests | `tests/Marilo.Tests.Integration/[Category]/Marilo[Name]IntegrationTests.cs` |

---

## Build Commands

| Task | Command |
|------|---------|
| Build SCSS | `npm run scss:build` |
| Watch SCSS | `npm run scss:watch` |
| Build docs | `npm run docs:build` |
| Run tests | `dotnet test` |
| Full build | `dotnet build && npm run build` |
