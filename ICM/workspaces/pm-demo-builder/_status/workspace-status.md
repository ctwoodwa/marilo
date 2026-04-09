# PM Demo Builder — Status Snapshot

> Updated: 2026-04-09

## Pipeline

```
[01-current-state]  -->  [02-ia-and-shell]  -->  [03-domain-modeling]  -->  [04-page-build]  -->  [05-integration]  -->  [06-review]
     PENDING                 PENDING                  PENDING                 PENDING              PENDING              PENDING
```

## Current PM Demo State (summary)

- **Shell**: MariloAppShell with sidebar, user menu, notification bell — DONE
- **Core pages**: Dashboard, Board, Tasks, Timeline, Budget, Team, Risk — DONE
- **Notification pipeline**: Canonical UserNotification model + IUserNotificationService — DONE
- **Settings pages**: 1 stub (/account/details), 9 more planned — IN PROGRESS
- **Settings layout**: Not yet created
- **ICurrentUserContext**: Not yet created
- **Asset management**: Not started — in planning
- **Dynamic forms**: Not started — in planning
- **Inspections/deficiencies**: Not started — in planning

## Active Build Order (from SETTINGS_STATUS.md)

| Step | Status |
|---|---|
| 1. Component audit | DONE |
| 2. Shared infra (notification pipeline) | DONE |
| 3. DAB migrations | Pending |
| 4. Settings shell (SettingsLayout, SettingsNav) | Pending |
| 5. Account page | Pending (stub exists) |
| 6. Preferences page | Pending |
| 7. Notifications page | Pending |
| 8–12. Remaining pages | Pending |

## Canonical Source of Truth

`samples/Marilo.PmDemo/SETTINGS_STATUS.md` — always read this file, not this snapshot, for current settings progress.
