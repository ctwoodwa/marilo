# Test Summary: SignalRConnectionStatus

## Test Files

- `tests/Marilo.Tests.Unit/Feedback/SignalRConnectionStatusTests.cs` -- 12 component tests + mock registry
- `tests/Marilo.Tests.Unit/FluentUICssProviderTests.cs` -- 6 new CSS provider tests added

## Test Count

| Category | Count |
|----------|-------|
| Component render tests | 12 |
| CSS provider tests | 6 |
| **New tests total** | **14** (4 Theory tests expand to multiple) |

## Component Tests

1. Renders_Default_WithNoHubs -- healthy state, aria attributes present
2. Shows_Healthy_State_When_All_Critical_Connected -- healthy class, correct count
3. Shows_Offline_State_When_Critical_Hub_Disconnected -- offline class, correct count
4. Shows_Degraded_State_When_Critical_Hub_Reconnecting -- degraded class
5. Hides_Counts_When_ShowCounts_False -- count span absent
6. Compact_Mode_Applies_Class -- compact modifier present
7. Popup_Not_Rendered_By_Default -- no popup in initial markup
8. Click_Opens_Popup -- popup appears, aria-expanded="true", role="dialog"
9. Popup_Shows_Hub_Rows -- hub names, health labels, error text visible
10. Popup_Filters_NonCritical_When_IncludeNonCritical_False -- only critical hubs shown
11. Popup_Shows_Reconnect_Button_For_Offline_Hub -- reconnect action present
12. Popup_Shows_Custom_Title -- custom title text rendered
13. Registry_Changed_Updates_UI -- count updates on Changed event
14. Tooltip_Shows_Summary -- tooltip text matches unhealthy count

## CSS Provider Tests (Theory)

- SignalRStatusClass -- all 4 aggregate states return correct class
- SignalRStatusClass_Compact -- compact modifier applied
- SignalRPopupClass -- returns expected class
- SignalRRowClass -- all 5 health states return correct class
- SignalRBadgeClass -- healthy and offline return correct class

## Results

`dotnet test` -- **118 passed, 0 failed, 0 skipped**
