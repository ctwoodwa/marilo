# PM Demo — Current State Assessment

> Derived from inspecting `samples/Marilo.PmDemo` and `SETTINGS_STATUS.md`.
> Last updated: 2026-04-09.

## Solution Structure

```
samples/Marilo.PmDemo/
├── Marilo.PmDemo.AppHost/          Aspire host orchestrator
├── Marilo.PmDemo/                  Blazor Server host (Interactive Server rendering)
├── Marilo.PmDemo.Client/           Razor class library — all pages + client code
├── Marilo.PmDemo.Data/             EF Core DbContext, entities, migrations, seeder
├── Marilo.PmDemo.MigrationService/ One-shot migration worker (DAB waits on this)
├── Marilo.PmDemo.ServiceDefaults/  Serilog + OTEL + health checks
├── Marilo.PmDemo.Tests.Unit/       Seeder smoke tests
├── Marilo.PmDemo.Tests.Integration/ Health check tests
├── Marilo.PmDemo.Tests.Performance/ Perf scaffold (empty)
└── MockOktaService/                Mock OIDC token issuer for dev auth
```

## Existing Pages (Client)

| Route | File | Status |
|---|---|---|
| `/` | `Pages/Home.razor` | Dashboard — exists |
| `/board` | `Pages/Board.razor` | Task board — exists |
| `/tasks` | `Pages/Tasks.razor` | Task list — exists |
| `/timeline` | `Pages/Timeline.razor` | Timeline — exists |
| `/budget` | `Pages/Budget.razor` | Budget — exists |
| `/team` | `Pages/Team.razor` | Team resource — exists |
| `/risk` | `Pages/Risk.razor` | Risk register — exists |
| `/account/details` | `Pages/AccountDetails.razor` | Stub — inline styles, no service binding |
| (404) | `Pages/NotFound.razor` | Custom not-found — exists |

## Shell and Layout

- `MainLayout.razor` renders `MariloAppShell` with sidebar nav, user menu, notification bell.
- Sidebar nav groups: Overview (Dashboard), Planning (Board, Tasks, Timeline), Governance (Budget, Team, Risk).
- Footer: bordered button group — avatar+identity left, bell right.
- `MariloNotificationBell` has "More options" menu (Settings, Delete all read).
- `MariloSnackbarHost` mounted in ChildContent for toast rendering.
- No nested layouts yet — `SettingsLayout` is planned but not created.

## Services — Registered

| Service | Implementation | Scope |
|---|---|---|
| `IUserNotificationService` | `InMemoryUserNotificationService` | Scoped |
| `IUserNotificationToastForwarder` | `MariloToastUserNotificationForwarder` | Scoped |
| `IMariloThemeService` | `ThemeService` | Scoped (via `AddMariloCoreServices`) |
| `IMariloNotificationService` | `MariloNotificationService` | Scoped (via `AddMariloCoreServices`) |
| `IMariloCssProvider` | `FluentUICssProvider` | Scoped (via `UseFluentUI`) |

## Services — Planned but not created

`ICurrentUserContext`, `IAccountService`, `IPreferencesService`, `INotificationPreferencesService`, `IPersonalizationService`, `IAssistantSettingsService`, `IShortcutsService`, `IConnectorService`.

## Data Layer

- EF Core with PostgreSQL (Aspire-wired via `Aspire.Npgsql.EntityFrameworkCore.PostgreSQL`).
- `PmDemoDbContext` in `Marilo.PmDemo.Data` with entities in `Entities/Entities.cs`.
- Migrations run via `MigrationWorker` one-shot worker.
- DAB + GraphQL is the API surface (no hand-rolled CRUD controllers).
- Wolverine for messaging; RabbitMQ for transport; Redis for output caching.

## Notification Architecture (DONE)

Canonical `UserNotification` record → `IUserNotificationService` → two projections:
1. `NotificationFeedProjection.ToFeedItem()` → bell view (`NotificationItem`)
2. `MariloToastUserNotificationForwarder.Forward()` → toast (`NotificationModel`)

8 seed events from multiple PM sources. 8 passing unit tests.

## Settings Area Status

See `samples/Marilo.PmDemo/SETTINGS_STATUS.md` for the canonical live tracking.

Summary: notification pipeline DONE, shell footer DONE, MainLayout wiring DONE, account page stub exists, settings layout and all other settings pages are pending.

## Infrastructure Present

- SignalR hub (`PmDemoHub`) with client interface.
- Wolverine handler (`TaskStatusChangedHandler`).
- Feature flags (`FeatureFlags.cs`).
- Authorization scaffolding (roles, permissions, tenant context).
- Mock Okta token service for dev auth.
