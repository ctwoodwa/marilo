using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloTreeListEditingTests : MariloTestBase
{
    private class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
    }

    private static List<Employee> SampleData() =>
    [
        new() { Id = 1, Name = "Alice", Department = "Engineering" },
        new() { Id = 2, Name = "Bob", Department = "Marketing" },
        new() { Id = 3, Name = "Carol", Department = "Engineering" },
    ];

    // â”€â”€ Wave 4: Inline Editing Tests â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public void EditMode_Inline_Renders_Command_Column_Header()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Equal(3, headers.Count); // Name, Department, Commands
        Assert.Contains("Commands", headers[2].TextContent);
    }

    [Fact]
    public void EditMode_None_Does_Not_Render_Command_Column()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.None)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Single(headers);
        Assert.DoesNotContain("Commands", headers[0].TextContent);
    }

    [Fact]
    public void DoubleClick_Row_Enters_Edit_Mode_Renders_Inputs()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        // No edit inputs initially
        var editInputsBefore = cut.FindAll("input.mar-treelist__edit-input");
        Assert.Empty(editInputsBefore);

        // Double-click the first row
        var row = cut.Find("tr.mar-treelist__row");
        row.DoubleClick();

        // Edit inputs should appear
        var editInputs = cut.FindAll("input.mar-treelist__edit-input");
        Assert.Equal(2, editInputs.Count); // Name + Department
    }

    [Fact]
    public void Edit_Mode_Row_Shows_Save_And_Cancel_Buttons()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Double-click to enter edit mode
        var row = cut.Find("tr.mar-treelist__row");
        row.DoubleClick();

        var saveBtn = cut.Find("button.mar-treelist__cmd-btn--save");
        var cancelBtn = cut.Find("button.mar-treelist__cmd-btn--cancel");
        Assert.NotNull(saveBtn);
        Assert.NotNull(cancelBtn);
        Assert.Contains("Save", saveBtn.TextContent);
        Assert.Contains("Cancel", cancelBtn.TextContent);
    }

    [Fact]
    public void Save_Fires_OnUpdate_With_Edited_Item()
    {
        TreeListCommandEventArgs<Employee>? receivedArgs = null;

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .Add(p => p.OnUpdate, (TreeListCommandEventArgs<Employee> args) => { receivedArgs = args; })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Double-click to enter edit mode
        var row = cut.Find("tr.mar-treelist__row");
        row.DoubleClick();

        // Click Save
        var saveBtn = cut.Find("button.mar-treelist__cmd-btn--save");
        saveBtn.Click();

        Assert.NotNull(receivedArgs);
        Assert.Equal("Alice", receivedArgs!.Item.Name);
        Assert.False(receivedArgs.IsNew);
    }

    [Fact]
    public void Cancel_Reverts_Edit_Mode()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Double-click to enter edit mode
        var row = cut.Find("tr.mar-treelist__row");
        row.DoubleClick();

        // Verify we're in edit mode
        Assert.NotEmpty(cut.FindAll("input.mar-treelist__edit-input"));

        // Click Cancel
        var cancelBtn = cut.Find("button.mar-treelist__cmd-btn--cancel");
        cancelBtn.Click();

        // Should no longer be in edit mode
        var editInputs = cut.FindAll("input.mar-treelist__edit-input");
        Assert.Empty(editInputs);
    }

    [Fact]
    public void Delete_Button_Fires_OnDelete()
    {
        TreeListCommandEventArgs<Employee>? receivedArgs = null;

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .Add(p => p.OnDelete, (TreeListCommandEventArgs<Employee> args) => { receivedArgs = args; })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Click Delete button on first row (non-editing rows show Delete)
        var deleteBtn = cut.Find("button.mar-treelist__cmd-btn--delete");
        deleteBtn.Click();

        Assert.NotNull(receivedArgs);
        Assert.Equal("Alice", receivedArgs!.Item.Name);
    }

    [Fact]
    public void Editing_Row_Has_Editing_Css_Class()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Double-click first row
        var row = cut.Find("tr.mar-treelist__row");
        row.DoubleClick();

        var editingRow = cut.Find("tr.mar-treelist__row--editing");
        Assert.NotNull(editingRow);
    }

    // â”€â”€ Wave 4: Toolbar Tests â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public void TreeListToolbar_Renders_Toolbar_Div()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .AddChildContent<TreeListToolbar>(toolbar => toolbar
                .AddChildContent("Toolbar Content")));

        var toolbar = cut.Find("div.mar-treelist__toolbar");
        Assert.NotNull(toolbar);
        Assert.Contains("Toolbar Content", toolbar.TextContent);
    }

    [Fact]
    public void TreeListToolbarButton_Add_Invokes_BeginAdd()
    {
        TreeListCommandEventArgs<Employee>? receivedArgs = null;

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .Add(p => p.OnCreate, (TreeListCommandEventArgs<Employee> args) => { receivedArgs = args; })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<TreeListToolbar>(toolbar => toolbar
                .AddChildContent<TreeListToolbarButton>(btn => btn
                    .Add(b => b.Command, TreeListToolbarCommand.Add)
                    .AddChildContent("Add"))));

        // Click the Add toolbar button
        var addBtn = cut.Find("button.mar-treelist__toolbar-btn");
        addBtn.Click();

        // A new row should appear
        var newRow = cut.FindAll("tr.mar-treelist__row--new");
        Assert.Single(newRow);

        // Save the new row
        var saveBtn = cut.Find("button.mar-treelist__cmd-btn--save");
        saveBtn.Click();

        Assert.NotNull(receivedArgs);
        Assert.True(receivedArgs!.IsNew);
    }

    [Fact]
    public void TreeListToolbarButton_Save_Invokes_Save()
    {
        TreeListCommandEventArgs<Employee>? receivedArgs = null;

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .Add(p => p.OnUpdate, (TreeListCommandEventArgs<Employee> args) => { receivedArgs = args; })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<TreeListToolbar>(toolbar => toolbar
                .AddChildContent<TreeListToolbarButton>(btn => btn
                    .Add(b => b.Command, TreeListToolbarCommand.Save)
                    .AddChildContent("Save"))));

        // First enter edit mode
        var row = cut.Find("tr.mar-treelist__row");
        row.DoubleClick();

        // Click the toolbar Save button
        var toolbarBtns = cut.FindAll("button.mar-treelist__toolbar-btn");
        toolbarBtns[0].Click();

        Assert.NotNull(receivedArgs);
        Assert.Equal("Alice", receivedArgs!.Item.Name);
    }

    [Fact]
    public void TreeListToolbarButton_Cancel_Invokes_Cancel()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, SampleData())
            .Add(p => p.EditMode, TreeListEditMode.Inline)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<TreeListToolbar>(toolbar => toolbar
                .AddChildContent<TreeListToolbarButton>(btn => btn
                    .Add(b => b.Command, TreeListToolbarCommand.Cancel)
                    .AddChildContent("Cancel"))));

        // Enter edit mode
        var row = cut.Find("tr.mar-treelist__row");
        row.DoubleClick();

        // Verify in edit mode
        Assert.NotEmpty(cut.FindAll("input.mar-treelist__edit-input"));

        // Click toolbar Cancel
        var toolbarBtns = cut.FindAll("button.mar-treelist__toolbar-btn");
        toolbarBtns[0].Click();

        // Should exit edit mode
        Assert.Empty(cut.FindAll("input.mar-treelist__edit-input"));
    }
}
