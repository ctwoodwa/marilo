---
title: Decade Cell
page_title: Calendar - Decade Cell Template
description: Use custom Decade cell template in the Calendar for Blazor.
slug: calendar-templates-decade
tags: marilo,blazor,calendar,templates,Decade
published: True
position: 4
components: ["calendar"]
---
# Decade Cell Template

The Decade Cell Template controls what the calendar will render in the `<td>` element for each year in the Decade view that lists the years.

The template receives the `DateTime` corresponding to its cell.

>caption Mark some years on the calendar decade view

![calendar decade cell template](images/calendar-decade-template.png)

````RAZOR
@* This example adds an icon for certain years *@

<MariloCalendar @bind-Date="@startDate" @bind-View="@theView">
    <DecadeCellTemplate>
        @if (yearsWithEvents.Contains(context.Year))
        {
            <MariloSvgIcon Icon="@SvgIcon.ExclamationCircle"></MariloSvgIcon>
        }
        @context.Year
    </DecadeCellTemplate>
</MariloCalendar>

@code{
    DateTime startDate { get; set; } = new DateTime(2021, 4, 1);

    CalendarView theView { get; set; } = CalendarView.Decade;
    List<int> yearsWithEvents { get; set; } = new List<int>() { 2020, 2021 };
}
````


## See Also

 * [Calendar Templates Overview](slug:calendar-templates-overview)
 * [Live Demo: Calendar Templates](https://demos.marilo.com/blazor-ui/calendar/templates)
 

