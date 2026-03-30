---
title: Content
page_title: Wizard Content
description: Content of the Wizard for Blazor.
slug: wizard-structure-content
tags: marilo,blazor,wizard,content
published: True
position: 3
components: ["wizard"]
---
# Wizard Content

The `Content` of the Wizard is a `ChildContent` of the `WizardStep`. While it is a common use case to integrate a Form in the Wizard Content, it can also contain any type of HTML content including various Marilo components as needed to cover your desired scenario.

>caption Wizard step content example

````RAZOR
@* Include the desired content for the Wizard Steps *@

<div style="text-align:center" >
    <MariloWizard Width="600px" Height="300px">
        <WizardSteps>
            <WizardStep Label="Destination" Icon="@SvgIcon.MapMarker">
                <Content>
                    <MariloDropDownList @bind-Value="@SelectedCity"
                                         Data="@Cities" DefaultText="Select City">
                    </MariloDropDownList>
                </Content>
            </WizardStep>
            <WizardStep Label="Dates" Icon="@SvgIcon.Calendar">
                <Content>
                        <MariloDateRangePicker @bind-StartValue="@StartValue"
                                                @bind-EndValue="@EndValue">
                        </MariloDateRangePicker>
                </Content>
            </WizardStep>
            <WizardStep Label="Avalable flights" Icon="@SvgIcon.GlobeOutline">
                <Content>
                    <div>
                        Checking if we can take you to <strong>@SelectedCity</strong> between <strong>@StartValue</strong> and <strong>@EndValue</strong>.
                    </div>
                    <br />
                    <MariloProgressBar Indeterminate="true" Value="10" Max="100" />
                </Content>
            </WizardStep>
        </WizardSteps>
    </MariloWizard>
</div>

@code {
    public string SelectedCity { get; set; }

    public List<string> Cities { get; set; } = new List<string>() { "New York", "Paris", "Rome", "Madrid" };

    public DateTime? StartValue { get; set; } = DateTime.Now.Date;

    public DateTime? EndValue { get; set; } = DateTime.Now.AddDays(10).Date;
}
````


## See Also

* [Live Demos: Wizard Overview](https://demos.marilo.com/blazor-ui/wizard/overview)
