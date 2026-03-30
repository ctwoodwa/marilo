---
title: Refresh Data
page_title: ComboBox Refresh Data
description: Refresh ComboBox Data using Observable Data or creating a new Collection reference.
slug: combobox-refresh-data
tags: marilo,blazor,combobox,observable,data,new,collection
published: True
position: 35
components: ["combobox"]
---
# ComboBox - Refresh Data


In this article:

- [Rebind Method](#rebind-method)
- [Observable Data](#observable-data)
- [New Collection Reference](#new-collection-reference)


## Rebind Method

To refresh the ComboBox data when using [`OnRead`](slug:components/combobox/events#onread), call the `Rebind` method of the MariloComboBox reference. This will fire the `OnRead` event and execute the business logic in the handler.

````RAZOR
@using Marilo.DataSource.Extensions

<MariloButton OnClick="@RebindComboBox">Remove First Data Item and Rebind</MariloButton>

<MariloComboBox @ref="@ComboBoxRef"
                 @bind-Value="@ComboBoxValue"
                 OnRead="@OnComboBoxRead"
                 TItem="ListItem"
                 TValue="int"
                 ValueField="@nameof(ListItem.Id)"
                 TextField="@nameof(ListItem.Text)"
                 Width="200px">
</MariloComboBox>

@code{
    public MariloComboBox<ListItem, int>? ComboBoxRef { get; set; }
    private List<ListItem> AllData { get; set; } = new();
    private int ComboBoxValue { get; set; }

    private void RebindComboBox()
    {
        if (AllData.Count > 1)
        {
            AllData.RemoveAt(0);
        }

        ComboBoxValue = AllData.FirstOrDefault()?.Id ?? default;

        ComboBoxRef?.Rebind();
    }

    private async Task OnComboBoxRead(ComboBoxReadEventArgs args)
    {
        var result = await AllData.ToDataSourceResultAsync(args.Request);
        args.Data = result.Data;
        args.Total = result.Total;
    }

    protected override void OnInitialized()
    {
        for (int i = 1; i <= 5; i++)
        {
            AllData.Add(new ListItem()
            {
                Id = i,
                Text = $"ListItem {i}"
            });
        }

        ComboBoxValue = AllData.First().Id;
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
````


## Observable Data


>caption Bind the ComboBox component to an ObservableCollection, so it can react to collection changes.

````RAZOR
@* Add/remove an option to see how the ComboBox reacts to the change. *@

@using System.Collections.ObjectModel

<h4>Add option</h4>
<MariloTextBox @bind-Value="@ValuetoAdd"></MariloTextBox>

<MariloButton OnClick="@AddOption">Add option</MariloButton>
<br />

<h4>Remove the last option</h4>
<MariloButton OnClick="@RemoveOption">Remove the last option</MariloButton>
<br />

<h4>ComboBox options: @myDdlData.Count</h4>
<br />

<MariloComboBox Data="@myDdlData" TextField="MyTextField" ValueField="MyValueField" @bind-Value="@selectedValue">
</MariloComboBox>

@code {
    string ValuetoAdd { get; set; }

    int selectedValue { get; set; }

    ObservableCollection<MyDdlModel> myDdlData = new ObservableCollection<MyDdlModel>(Enumerable.Range(1, 5).Select(x => new MyDdlModel { MyTextField = "item " + x, MyValueField = x }));

    void AddOption()
    {
        if (!string.IsNullOrWhiteSpace(ValuetoAdd))
        {
            myDdlData.Add(
            new MyDdlModel { MyTextField = ValuetoAdd, MyValueField = myDdlData.Count + 1 }
            );
            ValuetoAdd = string.Empty;
        }
    }

    void RemoveOption()
    {
        if (myDdlData.Count > 0)
        {
        myDdlData.RemoveAt(myDdlData.Count - 1);
        }
    }

    public class MyDdlModel
    {
        public int MyValueField { get; set; }
        public string MyTextField { get; set; }
    }
}
````


## New Collection Reference


>caption Create new collection reference to refresh the ComboBox data.

````RAZOR
@* Add/remove an option to see how the ComboBox reacts to the change. *@

<h4>Add option</h4>
<MariloTextBox @bind-Value="@ValuetoAdd"></MariloTextBox>

<MariloButton OnClick="@AddOption">Add option</MariloButton>
<br />

<h4>Remove the last option</h4>
<MariloButton OnClick="@RemoveOption">Remove the last option</MariloButton>
<br />

<h4>Load new collection</h4>
<MariloButton OnClick="@LoadNewData">Load data</MariloButton>
<br />

<h4>ComboBox options: @myDdlData.Count</h4>
<br />

<MariloComboBox Data="@myDdlData" TextField="MyTextField" ValueField="MyValueField" @bind-Value="@selectedValue">
</MariloComboBox>

@code {
    string ValuetoAdd { get; set; }

    int selectedValue { get; set; }

    List<MyDdlModel> myDdlData = new List<MyDdlModel>(Enumerable.Range(1, 5).Select(x => new MyDdlModel { MyTextField = "item " + x, MyValueField = x }));

    void AddOption()
    {
        if (!string.IsNullOrWhiteSpace(ValuetoAdd))
        {
            myDdlData.Add(
            new MyDdlModel { MyTextField = ValuetoAdd, MyValueField = myDdlData.Count + 1 }
            );
            myDdlData = new List<MyDdlModel>(myDdlData);
            ValuetoAdd = string.Empty;
        }
    }

    void RemoveOption()
    {
        if (myDdlData.Count > 0)
        {
            myDdlData.RemoveAt(myDdlData.Count - 1);
            myDdlData = new List<MyDdlModel>(myDdlData);
        }
    }

    void LoadNewData()
    {
        var newData = new List<MyDdlModel>(Enumerable.Range(6, 5).Select(x => new MyDdlModel { MyTextField = "item " + x, MyValueField = x }));

        myDdlData = new List<MyDdlModel>(newData);

        Console.WriteLine("New data collection loaded.");
    }

    public class MyDdlModel
    {
        public int MyValueField { get; set; }
        public string MyTextField { get; set; }
    }
}
````

## See Also

  * [ObservableCollection](slug:common-features-observable-data)
  * [INotifyCollectionChanged Interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=netframework-4.8)
  * [Live Demos](https://demos.marilo.com/blazor-ui)
