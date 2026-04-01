# High-Quality Test Examples: bUnit Component Tests

> **Purpose:** Demonstrate bUnit testing patterns for Blazor components following ICM architecture patterns.
> **Governed by:** ADR-0007 (Navigation State Management), ADR-0011 (State Management Architecture), ADR-0012 (UI Component Library Standardization).
> **NuGet:** `bunit`, `bunit.web`, `Moq`, `FluentAssertions` (optional)

## Project Setup

```xml
<!-- Test.csproj additions -->
<PackageReference Include="bunit" Version="1.*" />
<PackageReference Include="bunit.web" Version="1.*" />
<PackageReference Include="Moq" Version="4.*" />
<PackageReference Include="xunit" Version="2.*" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.*" />
```

---

## Example 1: Component Renders Correct State

Testing that a component reflects state correctly and responds to state changes.

```csharp
public class WorkOrderDetailComponentTests : TestContext
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IState<WorkOrderState>> _workOrderState = new();

    public WorkOrderDetailComponentTests()
    {
        Services.AddSingleton(_mediator.Object);
        Services.AddSingleton(_workOrderState.Object);
    }

    [Fact]
    public void Renders_WorkOrderTitle_FromState()
    {
        // Arrange
        _workOrderState
            .Setup(s => s.Value)
            .Returns(new WorkOrderState
            {
                Title = "Replace pump seal on Unit 7",
                Status = WorkOrderStatus.InProgress,
                AssetId = "PUMP-007"
            });

        // Act
        var cut = RenderComponent<WorkOrderDetail>();

        // Assert
        cut.Find("[data-testid='wo-title']")
            .TextContent.MarkupMatches("Replace pump seal on Unit 7");
        cut.Find("[data-testid='wo-status']")
            .TextContent.MarkupMatches("In Progress");
        cut.Find("[data-testid='wo-asset']")
            .TextContent.MarkupMatches("PUMP-007");
    }

    [Fact]
    public void Renders_ActionButtons_BasedOnAvailableTransitions()
    {
        // Arrange
        _workOrderState
            .Setup(s => s.Value)
            .Returns(new WorkOrderState
            {
                Status = WorkOrderStatus.Submitted,
                CanApprove = true,
                CanReject = true,
                CanComplete = false
            });

        // Act
        var cut = RenderComponent<WorkOrderDetail>();

        // Assert
        Assert.NotNull(cut.Find("[data-testid='approve-button']"));
        Assert.NotNull(cut.Find("[data-testid='reject-button']"));
        Assert.Throws<ElementNotFoundException>(
            () => cut.Find("[data-testid='complete-button']"));
    }

    [Fact]
    public void HidesEditControls_WhenStatusIsCompleted()
    {
        // Arrange
        _workOrderState
            .Setup(s => s.Value)
            .Returns(new WorkOrderState
            {
                Status = WorkOrderStatus.Completed,
                IsReadOnly = true
            });

        // Act
        var cut = RenderComponent<WorkOrderDetail>();

        // Assert
        Assert.Throws<ElementNotFoundException>(
            () => cut.Find("[data-testid='edit-title']"));
        Assert.Throws<ElementNotFoundException>(
            () => cut.Find("[data-testid='edit-description']"));
    }
}
```

### Why This Is High Quality

- **Tests rendering from state, not implementation:** Verifies what the user sees.
- **Uses `data-testid` selectors:** Resilient to CSS/layout changes.
- **Tests conditional rendering:** Buttons appear/disappear based on state-driven flags.
- **Tests read-only mode:** Verifies edit controls are hidden when appropriate.

---

## Example 2: Component Dispatches State Actions

Testing that user interactions dispatch the correct state actions via MediatR.

```csharp
public class WorkOrderFormComponentTests : TestContext
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IState<WorkOrderState>> _workOrderState = new();

    public WorkOrderFormComponentTests()
    {
        Services.AddSingleton(_mediator.Object);
        Services.AddSingleton(_workOrderState.Object);

        _workOrderState
            .Setup(s => s.Value)
            .Returns(new WorkOrderState
            {
                Status = WorkOrderStatus.Draft,
                Title = "",
                AssetId = "",
                Priority = Priority.Medium
            });
    }

    [Fact]
    public void SubmitButton_DispatchesSubmitAction_WhenFormIsValid()
    {
        // Arrange
        var cut = RenderComponent<WorkOrderForm>();

        cut.Find("[data-testid='title-input']")
            .Change("Replace pump seal");
        cut.Find("[data-testid='asset-input']")
            .Change("PUMP-007");

        // Act
        cut.Find("[data-testid='submit-button']").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.IsAny<WorkOrderState.SubmitAction>(), default), Times.Once);
    }

    [Fact]
    public void SubmitButton_ShowsValidationErrors_WhenFormIsInvalid()
    {
        // Arrange
        var cut = RenderComponent<WorkOrderForm>();
        // Leave title empty (required field)

        // Act
        cut.Find("[data-testid='submit-button']").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.IsAny<WorkOrderState.SubmitAction>(), default), Times.Never);

        var validationMessage = cut.Find("[data-testid='title-validation']");
        Assert.Contains("required", validationMessage.TextContent,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void PriorityDropdown_DispatchesChangePriorityAction()
    {
        // Arrange
        var cut = RenderComponent<WorkOrderForm>();

        // Act
        cut.Find("[data-testid='priority-select']").Change("High");

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<WorkOrderState.ChangePriorityAction>(a =>
                a.Priority == Priority.High),
            default), Times.Once);
    }

    [Fact]
    public void CancelButton_DispatchesNavigateBackAction()
    {
        // Arrange
        var cut = RenderComponent<WorkOrderForm>();

        // Act
        cut.Find("[data-testid='cancel-button']").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.IsAny<RouteState.GoBackAction>(), default), Times.Once);
    }
}
```

### Why This Is High Quality

- **Tests user interactions → state actions:** The component's job is to translate UI events to actions.
- **Validates client-side validation:** Submit is blocked and errors shown for invalid input.
- **Tests navigation per ADR-0007:** Cancel button uses RouteState, not NavigationManager.

---

## Example 3: Component with Cascading Parameters and Child Components

Testing a parent component that passes data down and receives events from children.

```csharp
public class WorkOrderListWithFiltersTests : TestContext
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IState<WorkOrderListState>> _listState = new();

    public WorkOrderListWithFiltersTests()
    {
        Services.AddSingleton(_mediator.Object);
        Services.AddSingleton(_listState.Object);
    }

    [Fact]
    public void Renders_CorrectNumberOfRows()
    {
        // Arrange
        _listState
            .Setup(s => s.Value)
            .Returns(new WorkOrderListState
            {
                WorkOrders = new[]
                {
                    new WorkOrderSummary { Id = "WO-1", Title = "Fix pump" },
                    new WorkOrderSummary { Id = "WO-2", Title = "Replace valve" },
                    new WorkOrderSummary { Id = "WO-3", Title = "Inspect tank" }
                },
                IsLoading = false
            });

        // Act
        var cut = RenderComponent<WorkOrderList>();

        // Assert
        var rows = cut.FindAll("[data-testid='wo-row']");
        Assert.Equal(3, rows.Count);
    }

    [Fact]
    public void ShowsLoadingIndicator_WhenStateIsLoading()
    {
        // Arrange
        _listState
            .Setup(s => s.Value)
            .Returns(new WorkOrderListState
            {
                WorkOrders = Array.Empty<WorkOrderSummary>(),
                IsLoading = true
            });

        // Act
        var cut = RenderComponent<WorkOrderList>();

        // Assert
        Assert.NotNull(cut.Find("[data-testid='loading-spinner']"));
        Assert.Throws<ElementNotFoundException>(
            () => cut.Find("[data-testid='wo-row']"));
    }

    [Fact]
    public void ShowsEmptyState_WhenNoWorkOrdersAndNotLoading()
    {
        // Arrange
        _listState
            .Setup(s => s.Value)
            .Returns(new WorkOrderListState
            {
                WorkOrders = Array.Empty<WorkOrderSummary>(),
                IsLoading = false
            });

        // Act
        var cut = RenderComponent<WorkOrderList>();

        // Assert
        Assert.NotNull(cut.Find("[data-testid='empty-state']"));
    }

    [Fact]
    public void FilterChange_DispatchesFilterAction()
    {
        // Arrange
        _listState
            .Setup(s => s.Value)
            .Returns(new WorkOrderListState
            {
                WorkOrders = Array.Empty<WorkOrderSummary>(),
                IsLoading = false,
                ActiveFilter = "all"
            });

        var cut = RenderComponent<WorkOrderList>();

        // Act
        cut.Find("[data-testid='filter-overdue']").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<WorkOrderListState.SetFilterAction>(a =>
                a.Filter == "overdue"),
            default), Times.Once);
    }
}
```

---

## Example 4: Component with Telerik UI Controls

Testing components that use Telerik Blazor controls (per ADR-0012).

```csharp
public class WorkOrderGridComponentTests : TestContext
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IState<WorkOrderListState>> _listState = new();

    public WorkOrderGridComponentTests()
    {
        Services.AddSingleton(_mediator.Object);
        Services.AddSingleton(_listState.Object);

        // Register Telerik Blazor services for bUnit
        Services.AddTelerikBlazor();

        // Register the TelerikRootComponent as a cascading value
        var rootComponent = new TelerikRootComponent();
        RenderComponent<TelerikRootComponent>();
    }

    [Fact]
    public void Grid_RendersColumns_ForRequiredFields()
    {
        // Arrange
        _listState
            .Setup(s => s.Value)
            .Returns(CreateListStateWithSampleData());

        // Act
        var cut = RenderComponent<WorkOrderGrid>();

        // Assert — verify grid column headers exist
        var headers = cut.FindAll(".k-header");
        var headerTexts = headers.Select(h => h.TextContent.Trim()).ToList();

        Assert.Contains("Work Order", headerTexts);
        Assert.Contains("Asset", headerTexts);
        Assert.Contains("Status", headerTexts);
        Assert.Contains("Priority", headerTexts);
        Assert.Contains("Due Date", headerTexts);
    }

    [Fact]
    public void Grid_RowClick_DispatchesNavigateToDetail()
    {
        // Arrange
        _listState
            .Setup(s => s.Value)
            .Returns(CreateListStateWithSampleData());

        var cut = RenderComponent<WorkOrderGrid>();

        // Act — simulate grid row selection
        cut.Find(".k-grid tbody tr").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<RouteState.ChangeRouteAction>(a =>
                a.Route.StartsWith("/work-orders/")),
            default), Times.Once);
    }

    private static WorkOrderListState CreateListStateWithSampleData() =>
        new()
        {
            WorkOrders = new[]
            {
                new WorkOrderSummary
                {
                    Id = "WO-1001",
                    Title = "Quarterly pump inspection",
                    AssetId = "PUMP-007",
                    Status = "In Progress",
                    Priority = "High",
                    DueDate = DateTime.UtcNow.AddDays(3)
                }
            },
            IsLoading = false
        };
}
```

### Why This Is High Quality

- **Tests Telerik integration:** Registers `TelerikBlazor` services and root component in bUnit context.
- **Tests grid rendering:** Verifies expected columns are present.
- **Tests row interaction → RouteState:** Grid row click triggers navigation per ADR-0007.

---

## Common bUnit Patterns Reference

| Pattern | When to use | Example |
|---------|------------|---------|
| `RenderComponent<T>()` | Render a component for testing | `var cut = RenderComponent<MyComponent>();` |
| `cut.Find("[data-testid='x']")` | Find element by test ID | Resilient to CSS changes |
| `element.Click()` | Simulate click | `cut.Find("button").Click();` |
| `element.Change("value")` | Simulate input change | `cut.Find("input").Change("text");` |
| `element.TextContent` | Get rendered text | Assert against expected display text |
| `element.MarkupMatches("html")` | Assert markup | For exact HTML comparisons |
| `cut.FindAll("selector")` | Find multiple elements | `var rows = cut.FindAll("tr");` |
| `Assert.Throws<ElementNotFoundException>` | Assert element absent | Verify conditional rendering |
| `Services.AddSingleton(mock.Object)` | Inject mocked service | Register in constructor |
| `cut.InvokeAsync(() => ...)` | Run async operations | Trigger async state updates |

### Test Organization Convention

```
Tests/
  Components/
    WorkOrders/
      WorkOrderDetailComponentTests.cs
      WorkOrderFormComponentTests.cs
      WorkOrderListComponentTests.cs
      WorkOrderGridComponentTests.cs
    Assets/
      AssetDetailComponentTests.cs
      ...
  State/
    WorkOrderStateTests.cs
    ...
  Services/
    WorkOrderApprovalServiceTests.cs
    ...
```
