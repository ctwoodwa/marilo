---
title: Overview
page_title: Treeview Overview
description: Overview of the Treeview for Blazor, with feature description.
slug: treeview-overview
tags: marilo,blazor,treeview,overview
published: True
position: 0
components: ["treeview"]
---
# Blazor Treeview Overview

The <a href="https://www.marilo.com/blazor-ui/treeview" target="_blank">Blazor Treeview component</a> displays data in a traditional tree-like structure. The data itself can be flat or hierarchical. In addition to built-in navigation capabilities, you can navigate through the items and their children, define [templates](slug:components/treeview/templates) for the individual nodes, render text, checkboxes and icons, and respond to events.

## Creating Blazor TreeView

1. Use the `MariloTreeView` tag.
1. Set the TreeView `Data` attribute to an `IEnumerable<T>`. The TreeView will automatically recognize property names like `Id`, `ParentId`, `Text` and a few others. Otherwise, [use bindings to configure custom property names](slug:components/treeview/data-binding/overview#treeview-bindings).
1. (optional) Set the [`ExpandedItems`](slug:treeview-expand-items) attribute to a **non-null** `IEnumerable<object>`. Use it to [expand or collapse items programmatically](slug:treeview-expand-items#programmatically-expand-and-collapse-items).

>caption TreeView with flat self-referencing data and icons

````RAZOR
<MariloTreeView Data="@FlatData"
                 @bind-ExpandedItems="@ExpandedItems" />

@code {
    IEnumerable<TreeItem> FlatData { get; set; }
    IEnumerable<object> ExpandedItems { get; set; } = new List<TreeItem>();

    protected override void OnInitialized()
    {
        FlatData = GetFlatData();

        ExpandedItems = FlatData.Where(x => x.HasChildren == true).ToList();
    }

    List<TreeItem> GetFlatData()
    {
        List<TreeItem> items = new List<TreeItem>();

        items.Add(new TreeItem()
        {
            Id = 1,
            Text = "wwwroot",
            ParentId = null,
            HasChildren = true,
            Icon = SvgIcon.Folder
        });
        items.Add(new TreeItem()
        {
            Id = 2,
            Text = "css",
            ParentId = 1,
            HasChildren = true,
            Icon = SvgIcon.Folder
        });
        items.Add(new TreeItem()
        {
            Id = 3,
            Text = "js",
            ParentId = 1,
            HasChildren = true,
            Icon = SvgIcon.Folder
        });
        items.Add(new TreeItem()
        {
            Id = 4,
            Text = "site.css",
            ParentId = 2,
            Icon = SvgIcon.Css
        });
        items.Add(new TreeItem()
        {
            Id = 5,
            Text = "scripts.js",
            ParentId = 3,
            Icon = SvgIcon.Js
        });

        return items;
    }

    public class TreeItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? ParentId { get; set; }
        public bool HasChildren { get; set; }
        public ISvgIcon Icon { get; set; }
    }
}
````

## Data Binding

The [TreeView provides flexible data binding](slug:components/treeview/data-binding/overview) with the following capabilities:

* [Binding to flat data](slug:components/treeview/data-binding/flat-data) (self-referencing data)
* [Binding to hierarchical data](slug:components/treeview/data-binding/hierarchical-data). It is possible to use different types for the items at different levels.
* [Loading child items on demand](slug:components/treeview/data-binding/load-on-demand) to improve performance
* [Setting property names](slug:components/treeview/data-binding/overview#treeview-bindings) for item IDs, text, parent IDs, icons, links, etc.


## Selection

The TreeView supports two selection modes:

* [Standard selection](slug:treeview-selection-overview) via item clicks
* [Checkbox selection](slug:treeview-checkboxes-overview) that allows selecting all child items at once


## Templates

Use [templates to customize the TreeView item rendering](slug:components/treeview/templates). Define a single template for all levels, or a [different template for each level](slug:components/treeview/templates#different-templates-for-different-node-levels).


## Drag and Drop

Users can [drag and drop TreeView items](slug:treeview-drag-drop-overview) within the same TreeView or between different ones.


## Navigate Views

The TreeView can [display links to app views and external pages](slug:treeview-navigation).


## TreeView Parameters

The following table lists TreeView parameters, which are not related to other features on this page. Check the [TreeView API Reference](slug:Marilo.Blazor.Components.MariloTreeView) for a full list of properties, methods and events.


| Parameter | Type and Default&nbsp;Value | Description |
| --- | --- | --- |
| `Class` | `string` | The additional CSS class that will be rendered on the `div.k-treeview` element. Use it to apply custom styles or [override the theme](slug:themes-override). Inherited from `MariloComponentBase`. |
| `Size` | `string` <br /> `"md"` | Affects the TreeView layout, for example the amount of space between items. The possible valid values are `"lg"` (large), `"md"` (medium) and `"sm"` (small). For easier setting, use the predefined string properties in class [`Marilo.Blazor.ThemeConstants.TreeView.Size`](slug:Marilo.Blazor.ThemeConstants.TreeView.Size). |
| `AriaLabel` | `string?` <br /> (`null`) | Accessibility label applied to the root tree element. See [Wai-Aria Support](slug:treeview-wai-aria-support). |
| `Disabled` | `bool` <br /> (`false`) | When `true`, prevents all user interaction with the tree (clicks, keyboard, drag, edit). |
| `ReadOnly` | `bool` <br /> (`false`) | When `true`, shows the current state (selection, expansion, checks) but prevents any user-initiated mutations. |
| `ExpandOnClick` | `bool` <br /> (`false`) | When `true`, clicking anywhere on an item header expands/collapses that node (not only the chevron). |
| `ExpandOnDoubleClick` | `bool` <br /> (`false`) | When `true`, double-clicking an item header expands/collapses the node. Suppressed while `AllowEditing` is `true` so double-click can trigger inline editing instead. |
| `SingleExpand` | `bool` <br /> (`false`) | Accordion behavior — when expanding a node, automatically collapses its sibling nodes so only one branch at each level stays open. |
| `AutoExpand` | `bool` <br /> (`false`) | When `true`, automatically expands all ancestors of the currently selected items so the selection is always visible. |
| `AllowEditing` | `bool` <br /> (`false`) | Enables inline text editing on tree items. A commit raises the `OnItemEdit` event (`EventCallback<TreeItemEditEventArgs>`). Planned full inline-editing spec section is tracked in `fluent-ui-gap-analysis.md`. |
| `FilterFunc` | `Func<object, bool>?` <br /> (`null`) | Predicate run against each data item to determine visibility. Ancestors of any matching item stay visible so matches retain their tree context. Call `ClearFilter()` to reset. |
| `ItemTemplate` | `RenderFragment<object>?` <br /> (`null`) | Template used to render the content of every tree item. Covered in detail under [Templates](slug:components/treeview/templates). |
| `CheckboxTemplate` | `RenderFragment<CheckboxContext>?` <br /> (`null`) | Template used to render each item's checkbox when `CheckBoxMode` is not `None`. |
| `DragThrottleInterval` | `int` <br /> (`0`) | *Planned.* Milliseconds between each firing of the `OnDrag` event during drag operations. Tracked via gap-analysis-resolution. |

> The TreeView source also exposes `EnableDragDrop` and the `OnItemDrop` event today. Telerik-parity names (`Draggable`, `OnDrop`) are **Planned** — see `fluent-ui-gap-analysis.md` and the gap workspace for the renaming/typing decision.


## TreeView Reference and Methods

To execute TreeView methods, obtain reference to the component instance via `@ref`.

The TreeView is a generic component. Its type depends on the type of its model and the type of its `Value`. In case you cannot provide either the `Value` or `Data` initially, you need to [set the corresponding types to the `TItem` and `TValue` parameters](slug:common-features-data-binding-overview#component-type).

The table below lists the TreeView methods. Also consult the [TreeView API](slug:Marilo.Blazor.Components.MariloTreeView).

| Method | Description |
| --- | --- |
| `Rebind` | [Refreshes the component data](slug:treeview-refresh-data#rebind-method). |
| `ExpandAllAsync(bool includeUnloaded = false, int maxDepth = int.MaxValue, CancellationToken ct = default)` | Expands every node in the tree. When `includeUnloaded` is `true`, `LoadChildrenAsync` is invoked for lazy-loaded nodes that have not yet been fetched, honoring `maxDepth` and the supplied cancellation token. |
| `CollapseAllAsync()` | Collapses every expanded node and raises `ExpandedItemsChanged`. |
| `SelectNodeAsync(string id)` | Programmatically navigates to a node: expands all of its ancestors, marks it selected, and sets keyboard focus. |
| `ClearFilter()` | Clears the active `FilterFunc` result so every node is visible again. |
| `GetItemFromDropIndex` <br /> `(string index)` | *Planned.* Will resolve the corresponding `TItem` of the destination TreeView from the passed [`DestinationIndex`](slug:grid-drag-drop-overview#event-arguments). Tracked via gap-analysis-resolution. |

<div class="skip-repl"></div>

````RAZOR
<MariloTreeView @ref="@TreeViewRef" .../>

@code{
    private MariloTreeView TreeViewRef;
}
````

## Next Steps

* [Review TreeView data binding](slug:components/treeview/data-binding/overview)
* [Experiment with TreeView Checkboxes](slug:treeview-checkboxes-overview)


## See Also

* [Live TreeView Demos](https://demos.marilo.com/blazor-ui/treeview/overview)
* [TreeVew API Reference](slug:Marilo.Blazor.Components.MariloTreeView)
* [Enable TreeView Scrolling](slug:treeview-kb-horizontal-scrollbar)
