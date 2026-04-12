using Marilo.Core.Base;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Marilo.Components.DataGrid;

public partial class MariloTreeList<TItem> : MariloComponentBase, IColumnHost
{
    [Parameter] public IEnumerable<TItem> Data { get; set; } = Enumerable.Empty<TItem>();
    [Parameter] public string? IdField { get; set; }
    [Parameter] public string? ParentIdField { get; set; }
    [Parameter] public string? ItemsField { get; set; }
    [Parameter] public string? HasChildrenField { get; set; }

    /// <summary>
    /// POCO-based column definitions. Prefer &lt;MariloTreeListColumn&gt; child components instead.
    /// When both child columns and this parameter are provided, child columns take precedence.
    /// </summary>
#pragma warning disable CS0618 // Obsolete usage is intentional for backward compat
    [Parameter]
    [Obsolete("Use <MariloTreeListColumn> child components instead.")]
    public List<TreeListColumn>? Columns { get; set; }
#pragma warning restore CS0618

    [Parameter] public EventCallback<TItem> OnRowClick { get; set; }

    /// <summary>
    /// Child content that accepts MariloTreeListColumn components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private List<TreeListNode> _rootItems = new();
    private HashSet<string> _expandedIds = new();

    // Column registration from child MariloTreeListColumn components
    private readonly List<MariloColumnBase> _registeredColumns = new();

    private record TreeListNode(string Id, TItem Item, List<TreeListNode> Children, bool HasChildren);

    // ── IColumnHost ─────────────────────────────────────────────────────

    void IColumnHost.RegisterColumn(MariloColumnBase column)
    {
        if (!_registeredColumns.Contains(column))
        {
            _registeredColumns.Add(column);
            InvokeAsync(StateHasChanged);
        }
    }

    void IColumnHost.UnregisterColumn(MariloColumnBase column)
    {
        if (_registeredColumns.Remove(column))
        {
            InvokeAsync(StateHasChanged);
        }
    }

    // ── Effective Columns ───────────────────────────────────────────────

    /// <summary>
    /// Returns the effective column list. Child-component columns take precedence
    /// over the old POCO Columns parameter.
    /// </summary>
    internal List<IColumnDescriptor> EffectiveColumns
    {
        get
        {
            if (_registeredColumns.Count > 0)
                return _registeredColumns.Cast<IColumnDescriptor>().ToList();

            // Map legacy POCO columns to internal representation
#pragma warning disable CS0618
            if (Columns is { Count: > 0 })
            {
                return Columns.Select(c => (IColumnDescriptor)new LegacyColumnAdapter(c)).ToList();
            }
#pragma warning restore CS0618

            return new();
        }
    }

    // ── Tree Building ───────────────────────────────────────────────────

    protected override void OnParametersSet()
    {
        _rootItems = BuildTree();
    }

    private List<TreeListNode> BuildTree()
    {
        var items = Data.ToList();
        if (!items.Any()) return new();

        if (!string.IsNullOrEmpty(ItemsField))
            return BuildHierarchical(items.Cast<object>(), 0);

        if (!string.IsNullOrEmpty(IdField) && !string.IsNullOrEmpty(ParentIdField))
            return BuildFlat(items);

        int idx = 0;
        return items.Select(i => new TreeListNode($"auto-{idx++}", i, new(), false)).ToList();
    }

    private List<TreeListNode> BuildHierarchical(IEnumerable<object> items, int depth)
    {
        var result = new List<TreeListNode>();
        int idx = 0;
        foreach (var item in items)
        {
            var id = GetProp(item, IdField) ?? $"h-{depth}-{idx++}";
            var children = new List<TreeListNode>();
            var childItems = item.GetType().GetProperty(ItemsField!)?.GetValue(item);
            if (childItems is System.Collections.IEnumerable en)
            {
                var list = en.Cast<object>().ToList();
                if (list.Any()) children = BuildHierarchical(list, depth + 1);
            }
            var hasKids = children.Any();
            if (!string.IsNullOrEmpty(HasChildrenField))
            {
                var v = item.GetType().GetProperty(HasChildrenField)?.GetValue(item);
                if (v is bool b) hasKids = b;
            }
            result.Add(new TreeListNode(id, (TItem)item, children, hasKids));
        }
        return result;
    }

    private List<TreeListNode> BuildFlat(List<TItem> items)
    {
        var lookup = new Dictionary<string, TreeListNode>();
        var roots = new List<TreeListNode>();
        foreach (var item in items)
        {
            var id = GetProp(item!, IdField) ?? "";
            var hasKids = false;
            if (!string.IsNullOrEmpty(HasChildrenField))
            {
                var v = item!.GetType().GetProperty(HasChildrenField)?.GetValue(item);
                if (v is bool b) hasKids = b;
            }
            lookup[id] = new TreeListNode(id, item, new(), hasKids);
        }
        foreach (var item in items)
        {
            var id = GetProp(item!, IdField) ?? "";
            var parentId = GetProp(item!, ParentIdField);
            if (string.IsNullOrEmpty(parentId) || !lookup.ContainsKey(parentId))
                roots.Add(lookup[id]);
            else
                lookup[parentId].Children.Add(lookup[id]);
        }
        return roots;
    }

    private string? GetProp(object item, string? propName)
    {
        if (string.IsNullOrEmpty(propName)) return null;
        return item.GetType().GetProperty(propName)?.GetValue(item)?.ToString();
    }

    // ── Rendering ───────────────────────────────────────────────────────

    private RenderFragment RenderRows(List<TreeListNode> nodes, int depth) => builder =>
    {
        int seq = 0;
        var columns = EffectiveColumns;

        foreach (var node in nodes)
        {
            var isExpanded = _expandedIds.Contains(node.Id);
            var hasKids = node.Children.Any() || node.HasChildren;
            var nodeId = node.Id;

            builder.OpenElement(seq++, "tr");
            builder.AddAttribute(seq++, "class", "mar-treelist__row");
            builder.AddAttribute(seq++, "role", "row");
            builder.AddAttribute(seq++, "aria-level", depth + 1);
            builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => OnRowClick.InvokeAsync(node.Item)));

            for (var ci = 0; ci < columns.Count; ci++)
            {
                var col = columns[ci];
                builder.OpenElement(seq++, "td");
                builder.AddAttribute(seq++, "class", "mar-treelist__td");

                if (ci == 0)
                {
                    builder.OpenElement(seq++, "span");
                    builder.AddAttribute(seq++, "style", $"padding-left: {depth * 20}px; display: inline-flex; align-items: center; gap: 4px;");

                    if (hasKids)
                    {
                        builder.OpenElement(seq++, "button");
                        builder.AddAttribute(seq++, "type", "button");
                        builder.AddAttribute(seq++, "class", "mar-tree-item__toggle");
                        builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => ToggleExpand(nodeId)));
                        builder.AddEventStopPropagationAttribute(seq++, "onclick", true);
                        builder.AddContent(seq++, isExpanded ? "\u25BC" : "\u25B6");
                        builder.CloseElement();
                    }
                    else
                    {
                        builder.OpenElement(seq++, "span");
                        builder.AddAttribute(seq++, "style", "width: 20px;");
                        builder.CloseElement();
                    }

                    builder.AddContent(seq++, col.GetDisplayValue(node.Item));
                    builder.CloseElement(); // span
                }
                else
                {
                    builder.AddContent(seq++, col.GetDisplayValue(node.Item));
                }

                builder.CloseElement(); // td
            }

            builder.CloseElement(); // tr

            if (hasKids && isExpanded)
                builder.AddContent(seq++, RenderRows(node.Children, depth + 1));
        }
    };

    private void ToggleExpand(string id)
    {
        if (!_expandedIds.Remove(id))
            _expandedIds.Add(id);
    }
}
