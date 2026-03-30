---
uid: component-tabs-events
title: Tabs Events
description: Handle tab change events from the MariloTabs component.
---

# Tabs Events

## ActiveIndexChanged

The `ActiveIndexChanged` parameter is an `EventCallback<int>` that fires when the user selects a different tab. It is typically used via `@bind-ActiveIndex`:

```razor
<MariloTabs @bind-ActiveIndex="activeTab">
    <MariloTabPanel Title="Tab 1">Content 1</MariloTabPanel>
    <MariloTabPanel Title="Tab 2">Content 2</MariloTabPanel>
</MariloTabs>

@code {
    private int activeTab = 0;
}
```

For manual handling:

```razor
<MariloTabs ActiveIndex="@activeTab" ActiveIndexChanged="@OnTabChanged">
    <MariloTabPanel Title="Tab 1">Content 1</MariloTabPanel>
    <MariloTabPanel Title="Tab 2">Content 2</MariloTabPanel>
</MariloTabs>

@code {
    private int activeTab = 0;

    private void OnTabChanged(int index)
    {
        activeTab = index;
        // Perform additional logic, e.g. lazy-load data
    }
}
```

## See Also

- [Tabs Overview](xref:component-tabs-overview)
