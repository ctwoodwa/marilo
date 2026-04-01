# High-Quality Test Examples: State

> **Purpose:** Demonstrate test patterns for state action handlers, derivations, and RouteState navigation.
> **Governed by:** ADR-0011 (State Management Architecture), ADR-0005 (State File Organization), ADR-0024 (State vs Service Decision).

## Example 1: State Transition Test

Testing a state action handler that transitions work order status.

```csharp
public class WorkOrderStatusTransitionTests
{
    [Fact]
    public void Handle_SubmitAction_WhenDraftWithRequiredFields_TransitionsToSubmitted()
    {
        // Arrange
        var initialState = new WorkOrderState
        {
            Status = WorkOrderStatus.Draft,
            Title = "Replace pump seal",
            AssetId = "PUMP-001",
            Priority = Priority.Medium
        };
        var action = new WorkOrderState.SubmitAction();

        // Act
        var newState = WorkOrderState.Reduce(initialState, action);

        // Assert
        Assert.Equal(WorkOrderStatus.Submitted, newState.Status);
        Assert.NotNull(newState.SubmittedAt);
    }

    [Fact]
    public void Handle_SubmitAction_WhenDraftMissingTitle_RemainsInDraftWithError()
    {
        // Arrange
        var initialState = new WorkOrderState
        {
            Status = WorkOrderStatus.Draft,
            Title = "",  // Missing required field
            AssetId = "PUMP-001"
        };
        var action = new WorkOrderState.SubmitAction();

        // Act
        var newState = WorkOrderState.Reduce(initialState, action);

        // Assert
        Assert.Equal(WorkOrderStatus.Draft, newState.Status);
        Assert.Contains("Title is required", newState.ValidationErrors);
    }

    [Fact]
    public void Handle_ApproveAction_WhenStatusIsDraft_RejectsTransition()
    {
        // Arrange
        var initialState = new WorkOrderState
        {
            Status = WorkOrderStatus.Draft
        };
        var action = new WorkOrderState.ApproveAction(approverId: "user-123");

        // Act
        var newState = WorkOrderState.Reduce(initialState, action);

        // Assert
        Assert.Equal(WorkOrderStatus.Draft, newState.Status);
        Assert.Contains("Cannot approve from Draft status", newState.ValidationErrors);
    }
}
```

### Why This Is High Quality

- **One behavior per test:** Each test verifies a single transition or guard.
- **Descriptive names:** Method name tells you the scenario and expected outcome.
- **Arrange-Act-Assert:** Clear structure, easy to read.
- **Tests both valid and invalid transitions:** Covers happy path and rejection.

---

## Example 2: Derivation Test

Testing computed values derived from state.

```csharp
public class WorkOrderDerivationTests
{
    [Theory]
    [InlineData(WorkOrderStatus.Draft, true, false, false)]
    [InlineData(WorkOrderStatus.Submitted, false, true, false)]
    [InlineData(WorkOrderStatus.InProgress, false, false, true)]
    [InlineData(WorkOrderStatus.Completed, false, false, false)]
    public void AvailableActions_DependOnCurrentStatus(
        WorkOrderStatus status,
        bool canSubmit,
        bool canApprove,
        bool canComplete)
    {
        // Arrange
        var state = new WorkOrderState { Status = status };

        // Assert
        Assert.Equal(canSubmit, state.CanSubmit);
        Assert.Equal(canApprove, state.CanApprove);
        Assert.Equal(canComplete, state.CanComplete);
    }

    [Fact]
    public void OverdueFlag_WhenPastDueDate_ReturnsTrue()
    {
        // Arrange
        var state = new WorkOrderState
        {
            Status = WorkOrderStatus.InProgress,
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        // Assert
        Assert.True(state.IsOverdue);
    }

    [Fact]
    public void OverdueFlag_WhenCompleted_ReturnsFalseRegardlessOfDueDate()
    {
        // Arrange
        var state = new WorkOrderState
        {
            Status = WorkOrderStatus.Completed,
            DueDate = DateTime.UtcNow.AddDays(-7)
        };

        // Assert
        Assert.False(state.IsOverdue);
    }
}
```

### Why This Is High Quality

- **Theory/InlineData:** Tests multiple status→actions mappings efficiently.
- **Edge cases:** Tests overdue flag with completed status (business rule: completed items are never overdue).
- **Pure derivation tests:** No side effects, just input → output verification.

---

## Example 3: RouteState Navigation Test (ADR-0007)

Testing navigation via RouteState actions.

```csharp
public class RouteStateNavigationTests
{
    [Fact]
    public void Handle_ChangeRouteAction_UpdatesCurrentAndPreviousRoute()
    {
        // Arrange
        var initialState = new RouteState
        {
            CurrentRoute = "/work-orders",
            PreviousRoute = null
        };
        var action = new RouteState.ChangeRouteAction("/work-orders/WO-1234");

        // Act
        var newState = RouteState.Reduce(initialState, action);

        // Assert
        Assert.Equal("/work-orders/WO-1234", newState.CurrentRoute);
        Assert.Equal("/work-orders", newState.PreviousRoute);
    }

    [Fact]
    public void Handle_ChangeRouteAction_WithParameters_PreservesRouteParams()
    {
        // Arrange
        var initialState = new RouteState { CurrentRoute = "/dashboard" };
        var action = new RouteState.ChangeRouteAction(
            route: "/work-orders",
            parameters: new Dictionary<string, string>
            {
                ["filter"] = "overdue",
                ["sort"] = "priority"
            });

        // Act
        var newState = RouteState.Reduce(initialState, action);

        // Assert
        Assert.Equal("/work-orders", newState.CurrentRoute);
        Assert.Equal("overdue", newState.RouteParameters["filter"]);
        Assert.Equal("priority", newState.RouteParameters["sort"]);
    }

    [Fact]
    public void Handle_GoBackAction_RestoresPreviousRoute()
    {
        // Arrange
        var initialState = new RouteState
        {
            CurrentRoute = "/work-orders/WO-1234",
            PreviousRoute = "/work-orders"
        };
        var action = new RouteState.GoBackAction();

        // Act
        var newState = RouteState.Reduce(initialState, action);

        // Assert
        Assert.Equal("/work-orders", newState.CurrentRoute);
    }
}
```

### Why This Is High Quality

- **Tests actual RouteState pattern from ADR-0007:** No direct NavigationManager usage.
- **Covers forward navigation, parameters, and back-navigation.**
- **State-in, state-out:** Pure reducer tests with no external dependencies.
