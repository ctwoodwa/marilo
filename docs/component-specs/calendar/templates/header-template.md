---
title: Header Template
page_title: Calendar - Header Template
description: Use custom custom rendering in the header of the Calendar for Blazor.
slug: calendar-templates-header
tags: marilo,blazor,calendar,templates,header
published: True
position: 10
components: ["calendar"]
---
# Header Template

The `<HeaderTemplate>` allows you to customize the header of the calendar. If the application defines this template, the component will not render any of the built-in buttons and labels in the header area.

The example below is using a [Calendar reference and methods](slug:components/calendar/overview#calendar-reference-and-methods).

>caption Use custom rendering in the Calendar header

````RAZOR
<MariloCalendar @bind-Value="@CalendarValue" @bind-Date="@CalendarDate">
    <HeaderTemplate>

        <MariloButton OnClick="@GoToPrevious" Icon="@SvgIcon.ArrowLeft" Title="Go to Previous Month"></MariloButton>
        <MariloButton OnClick="@SelectToday">Today</MariloButton>
        <MariloButton OnClick="@GoToNext" Icon="@SvgIcon.ArrowRight" Title="Go to Next Month"></MariloButton>

        <MariloSvgIcon Icon="@SvgIcon.ParameterDateTime" /> @CalendarValue.ToShortDateString()

    </HeaderTemplate>
</MariloCalendar>

@code {
    DateTime CalendarValue { get; set; } = DateTime.Now;
    DateTime CalendarDate { get; set; } = DateTime.Now;

    void GoToPrevious()
    {
        CalendarDate = CalendarDate.AddMonths(-1);
    }

    void SelectToday()
    {
        CalendarValue = DateTime.Today;
        CalendarDate = DateTime.Today;
    }

    void GoToNext()
    {
        CalendarDate = CalendarDate.AddMonths(1);
    }
}
````


## See Also

* [Calendar Templates Overview](slug:calendar-templates-overview)
* [Live Demo: Calendar Templates](https://demos.marilo.com/blazor-ui/calendar/templates)
