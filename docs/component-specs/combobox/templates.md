---
title: Templates
page_title: ComboBox - Templates
description: Templates in the ComboBox for Blazor.
slug: components/combobox/templates
tags: marilo,blazor,combo,combobox,templates
published: True
position: 25
components: ["combobox"]
---
# ComboBox Templates

The ComboBox component allows you to change what is rendered in its items, header and footer through templates.

>caption In this article:

* [Item Template](#item-template)
* [Header Template](#header-template)
* [Footer Template](#footer-template)
* [No Data Template](#no-data-template)
* [Example](#example)


## Item Template


## Header Template


## Footer Template


## No Data Template


## Example

>caption Using ComboBox Templates

````RAZOR
@* ComboBox component with HeaderTemplate, ItemTemplate, FooterTemplate and NoDataTemplate *@

<p>
    <MariloCheckBox @bind-Value="@IsDataAvailable" OnChange="@OnCheckBoxChangeHandler" />
    ComboBox has data
</p>

<MariloComboBox Data="@ComboBoxData" @bind-Value="@Role" Placeholder="Write your position">
    <HeaderTemplate>
        <strong>Select one of the following:</strong>
    </HeaderTemplate>
    <ItemTemplate>
        Are you a <strong>@context</strong>
    </ItemTemplate>
    <FooterTemplate>
        <h6>Total Positions: @ComboBoxData.Count()</h6>
    </FooterTemplate>
    <NoDataTemplate>
        <div class="no-data-template">
            <MariloSvgIcon Size="@ThemeConstants.SvgIcon.Size.Large" Icon="@SvgIcon.FilesError"></MariloSvgIcon>
            <p>No items available</p>
        </div>
    </NoDataTemplate>
</MariloComboBox>

@code {
    private string Role { get; set; }

    private bool IsDataAvailable { get; set; } = true;

    private List<string> ComboBoxData { get; set; }

    private List<string> SourceData { get; set; } = new List<string> { "Manager", "Developer", "QA", "Technical Writer", "Support Engineer", "Sales Agent", "Architect", "Designer" };

    protected override void OnInitialized()
    {
        ComboBoxData = SourceData;
    }

    private void OnCheckBoxChangeHandler()
    {
        if (IsDataAvailable)
        {
            ComboBoxData = new List<string>(SourceData);
        }else{
            ComboBoxData = new List<string>();
        }
    }
}
````

## See Also

  * [Live Demo: ComboBox Templates](https://demos.marilo.com/blazor-ui/combobox/templates)
   
  
