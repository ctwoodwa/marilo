---
title: Templates
page_title: AllocationScheduler Templates
description: How to use ResourceTemplate, ItemTemplate, CellTemplate, and other RenderFragment slots in the AllocationScheduler.
slug: allocation-scheduler-templates
tags: marilo,blazor,allocation-scheduler,templates,render-fragment
published: True
position: 5
components: ["allocation-scheduler"]
---

# AllocationScheduler Templates

## AllocationResourceColumns

Define resource metadata columns using child `AllocationResourceColumn` tags.

```razor
<MariloAllocationScheduler TResource="StaffResource" ...>
    <AllocationResourceColumns>
        <AllocationResourceColumn TResource="StaffResource" Field="Name" Title="Resource" Width="200px" />
        <AllocationResourceColumn TResource="StaffResource" Field="Role" Title="Role" Width="150px" />
    </AllocationResourceColumns>
</MariloAllocationScheduler>
```

## Column Template

Each `AllocationResourceColumn` accepts a `Template` RenderFragment for custom cell rendering.

```razor
<AllocationResourceColumn TResource="StaffResource" Field="Name" Title="Resource" Width="200px">
    <Template>
        <div style="display:flex;align-items:center;gap:0.5rem">
            <img src="@context.AvatarUrl" style="width:24px;height:24px;border-radius:50%" />
            <span>@context.Name</span>
        </div>
    </Template>
</AllocationResourceColumn>
```

## CellTemplate

Customize the rendering of individual allocation cells.

```razor
<MariloAllocationScheduler TResource="StaffResource" ...>
    <CellTemplate>
        @if (context.Record is not null)
        {
            <div class="custom-cell @(context.IsConflict ? "conflict" : "")">
                <span>@context.Record.Value h</span>
            </div>
        }
    </CellTemplate>
</MariloAllocationScheduler>
```

## EmptyTemplate

Shown when no allocations are bound.

```razor
<MariloAllocationScheduler TResource="StaffResource" ...>
    <EmptyTemplate>
        <p>No allocations yet. Click a cell to start planning.</p>
    </EmptyTemplate>
</MariloAllocationScheduler>
```

## ResourceRowTemplate

Fallback template for resource metadata cells when no column-level `Template` is set. Receives the `TResource` instance as `context`.

```razor
<MariloAllocationScheduler TResource="StaffResource" ...>
    <ResourceRowTemplate>
        <div class="resource-badge">
            <strong>@context.Name</strong>
            <span class="role-tag">@context.Role</span>
        </div>
    </ResourceRowTemplate>
</MariloAllocationScheduler>
```

> When both `ResourceRowTemplate` and a column-level `Template` are set, the column-level template wins for that column.

## ToolbarTemplate

Append custom content to the built-in toolbar.

```razor
<MariloAllocationScheduler TResource="StaffResource" ...>
    <ToolbarTemplate>
        <button @onclick="ExportToExcel">Export</button>
    </ToolbarTemplate>
</MariloAllocationScheduler>
```
