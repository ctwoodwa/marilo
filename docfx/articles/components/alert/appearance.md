---
uid: component-alert-appearance
title: Alert Appearance
description: Customize MariloAlert severity levels and visual presentation.
---

# Alert Appearance

## Severity levels

The `Severity` parameter determines the color scheme of the alert:

```razor
<MariloAlert Severity="AlertSeverity.Info">Informational message.</MariloAlert>
<MariloAlert Severity="AlertSeverity.Success">Operation completed.</MariloAlert>
<MariloAlert Severity="AlertSeverity.Warning">Please review before continuing.</MariloAlert>
<MariloAlert Severity="AlertSeverity.Error">An error occurred.</MariloAlert>
```

Each severity maps to a distinct CSS class via `IMariloCssProvider.AlertClass(severity)`. The provider determines the exact colors, icons, and border styles.

## Dismissible alerts

When `IsDismissible` is `true`, a close button appears inside the alert:

```razor
<MariloAlert Severity="AlertSeverity.Warning" IsDismissible="true" OnDismiss="@HideAlert">
    This alert can be dismissed.
</MariloAlert>
```

## See Also

- [Alert Overview](xref:component-alert-overview)
