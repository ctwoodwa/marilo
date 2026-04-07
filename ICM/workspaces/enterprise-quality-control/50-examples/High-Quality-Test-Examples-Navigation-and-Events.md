# High-Quality Test Examples: Navigation and Events

> **Purpose:** Demonstrate test patterns for RouteState navigation actions and MediatR eventing.
> **Governed by:** ADR-0007 (Navigation State Management Pattern), ADR-0023 (Inter-Component Communication and MediatR Eventing).

## Part A: Navigation Tests (ADR-0007)

### Example 1: Component Test — Asserting RouteState Action

Testing that a component dispatches the correct navigation action when a user interacts with it.

```csharp
public class WorkOrderListComponentTests
{
    private readonly Mock<IMediator> _mediator;

    public WorkOrderListComponentTests()
    {
        _mediator = new Mock<IMediator>();
    }

    [Fact]
    public void ClickWorkOrderRow_DispatchesChangeRouteToDetail()
    {
        // Arrange
        var workOrderId = "WO-1234";

        using var ctx = new TestContext();
        ctx.Services.AddSingleton(_mediator.Object);

        var component = ctx.RenderComponent<WorkOrderList>(parameters =>
            parameters.Add(p => p.WorkOrders, new[]
            {
                new WorkOrderSummary { Id = workOrderId, Title = "Test WO" }
            }));

        // Act
        component.Find($"[data-work-order-id='{workOrderId}']").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<RouteState.ChangeRouteAction>(a =>
                a.Route == $"/work-orders/{workOrderId}"),
            default), Times.Once);
    }

    [Fact]
    public void ClickCreateButton_DispatchesChangeRouteToNewWorkOrder()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.Services.AddSingleton(_mediator.Object);

        var component = ctx.RenderComponent<WorkOrderList>(parameters =>
            parameters.Add(p => p.WorkOrders, Array.Empty<WorkOrderSummary>()));

        // Act
        component.Find("[data-testid='create-work-order']").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<RouteState.ChangeRouteAction>(a =>
                a.Route == "/work-orders/new"),
            default), Times.Once);
    }

    [Fact]
    public void ClickBackButton_DispatchesGoBackAction()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.Services.AddSingleton(_mediator.Object);

        var component = ctx.RenderComponent<WorkOrderDetail>(parameters =>
            parameters.Add(p => p.WorkOrderId, "WO-1234"));

        // Act
        component.Find("[data-testid='back-button']").Click();

        // Assert
        _mediator.Verify(m => m.Send(
            It.IsAny<RouteState.GoBackAction>(), default), Times.Once);
    }
}
```

### Why This Is High Quality

- **No direct NavigationManager:** Tests enforce the ADR-0007 pattern (all navigation via RouteState).
- **Component-level tests:** Verify the UI triggers the correct action.
- **Specific route assertions:** Not just "navigation happened" but "navigated to the correct route."
- **Uses data-testid attributes:** Robust selectors that don't break with UI changes.

---

### Example 2: Multi-Step Navigation Flow

Testing a wizard-style flow with forward/backward navigation.

```csharp
public class WorkOrderWizardNavigationTests
{
    [Fact]
    public void Handle_NextStepAction_AdvancesToNextStep()
    {
        // Arrange
        var state = new WorkOrderWizardState
        {
            CurrentStep = WizardStep.Details,
            WorkOrderDraft = new WorkOrderDraft { Title = "Test", AssetId = "A-001" }
        };
        var action = new WorkOrderWizardState.NextStepAction();

        // Act
        var newState = WorkOrderWizardState.Reduce(state, action);

        // Assert
        Assert.Equal(WizardStep.Tasks, newState.CurrentStep);
    }

    [Fact]
    public void Handle_NextStepAction_WhenValidationFails_StaysOnCurrentStep()
    {
        // Arrange
        var state = new WorkOrderWizardState
        {
            CurrentStep = WizardStep.Details,
            WorkOrderDraft = new WorkOrderDraft { Title = "" } // Missing required field
        };
        var action = new WorkOrderWizardState.NextStepAction();

        // Act
        var newState = WorkOrderWizardState.Reduce(state, action);

        // Assert
        Assert.Equal(WizardStep.Details, newState.CurrentStep);
        Assert.Contains("Title is required", newState.ValidationErrors);
    }

    [Fact]
    public void Handle_PreviousStepAction_GoesBackOneStep()
    {
        // Arrange
        var state = new WorkOrderWizardState { CurrentStep = WizardStep.Tasks };
        var action = new WorkOrderWizardState.PreviousStepAction();

        // Act
        var newState = WorkOrderWizardState.Reduce(state, action);

        // Assert
        Assert.Equal(WizardStep.Details, newState.CurrentStep);
    }

    [Fact]
    public void Handle_CompleteAction_OnFinalStep_DispatchesRouteChange()
    {
        // Arrange
        var state = new WorkOrderWizardState
        {
            CurrentStep = WizardStep.Review,
            WorkOrderDraft = CreateValidDraft()
        };
        var action = new WorkOrderWizardState.CompleteAction();

        // Act
        var newState = WorkOrderWizardState.Reduce(state, action);

        // Assert
        Assert.True(newState.IsComplete);
        Assert.Equal("/work-orders", newState.NavigateOnComplete);
    }

    private static WorkOrderDraft CreateValidDraft() =>
        new() { Title = "Test WO", AssetId = "A-001", Priority = "Medium" };
}
```

---

## Part B: Eventing Tests (ADR-0023)

### Example 3: Notification Handler Unit Test

Testing a MediatR notification handler with fake dependencies.

```csharp
public class WorkOrderApprovedNotificationHandlerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IAuditLogger> _auditLogger;
    private readonly WorkOrderApprovedHandler _sut;

    public WorkOrderApprovedNotificationHandlerTests()
    {
        _mediator = new Mock<IMediator>();
        _auditLogger = new Mock<IAuditLogger>();
        _sut = new WorkOrderApprovedHandler(_mediator.Object, _auditLogger.Object);
    }

    [Fact]
    public async Task Handle_WhenWorkOrderApproved_UpdatesSchedulingState()
    {
        // Arrange
        var notification = new WorkOrderApprovedNotification
        {
            WorkOrderId = "WO-1234",
            ApprovedBy = "user-456",
            ApprovedAt = DateTime.UtcNow
        };

        // Act
        await _sut.Handle(notification, default);

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<SchedulingState.WorkOrderReadyAction>(a =>
                a.WorkOrderId == "WO-1234"),
            default), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenWorkOrderApproved_CreatesAuditEntry()
    {
        // Arrange
        var notification = new WorkOrderApprovedNotification
        {
            WorkOrderId = "WO-1234",
            ApprovedBy = "user-456",
            ApprovedAt = new DateTime(2026, 3, 15, 10, 30, 0, DateTimeKind.Utc)
        };

        // Act
        await _sut.Handle(notification, default);

        // Assert
        _auditLogger.Verify(a => a.LogAsync(
            It.Is<AuditEntry>(e =>
                e.Action == "WorkOrderApproved" &&
                e.EntityId == "WO-1234" &&
                e.ActorId == "user-456"),
            default), Times.Once);
    }

    [Fact]
    public async Task Handle_IsIdempotent_WhenCalledTwice()
    {
        // Arrange
        var notification = new WorkOrderApprovedNotification
        {
            WorkOrderId = "WO-1234",
            ApprovedBy = "user-456",
            ApprovedAt = DateTime.UtcNow
        };

        // Act
        await _sut.Handle(notification, default);
        await _sut.Handle(notification, default);

        // Assert — scheduling state action sent twice is safe (idempotent handler)
        _mediator.Verify(m => m.Send(
            It.IsAny<SchedulingState.WorkOrderReadyAction>(), default),
            Times.Exactly(2));

        // Audit logged twice is acceptable (each invocation is a valid event)
        _auditLogger.Verify(a => a.LogAsync(
            It.IsAny<AuditEntry>(), default), Times.Exactly(2));
    }
}
```

### Why This Is High Quality

- **Tests handler behavior, not implementation details.**
- **Idempotency test:** Explicitly verifies the handler is safe to re-execute.
- **Cross-module effect:** Tests that a notification in one module triggers an action in another (SchedulingState).
- **Small, focused handler:** Each test verifies one aspect of the handler's behavior.

---

### Example 4: Cross-Module Integration Test

Testing that a notification published in one module is received and processed by another.

```csharp
public class WorkOrderApprovalIntegrationTests : IClassFixture<MediatRFixture>
{
    private readonly MediatRFixture _fixture;

    public WorkOrderApprovalIntegrationTests(MediatRFixture fixture)
        => _fixture = fixture;

    [Fact]
    public async Task PublishApprovalNotification_TriggersSchedulingAndAudit()
    {
        // Arrange
        var notification = new WorkOrderApprovedNotification
        {
            WorkOrderId = "WO-5678",
            ApprovedBy = "user-admin",
            ApprovedAt = DateTime.UtcNow
        };

        // Act
        await _fixture.Mediator.Publish(notification);

        // Assert — verify cross-module effects
        var schedulingState = _fixture.GetState<SchedulingState>();
        Assert.Contains("WO-5678", schedulingState.ReadyWorkOrders);

        var auditEntries = _fixture.GetAuditEntries();
        Assert.Contains(auditEntries,
            e => e.Action == "WorkOrderApproved" && e.EntityId == "WO-5678");
    }

    [Fact]
    public async Task PublishApprovalNotification_WhenHandlerThrows_OtherHandlersContinue()
    {
        // Arrange — configure one handler to throw
        _fixture.ConfigureHandlerToFail<AuditNotificationHandler>();

        var notification = new WorkOrderApprovedNotification
        {
            WorkOrderId = "WO-9999",
            ApprovedBy = "user-admin",
            ApprovedAt = DateTime.UtcNow
        };

        // Act
        await _fixture.Mediator.Publish(notification);

        // Assert — scheduling still processed despite audit handler failure
        var schedulingState = _fixture.GetState<SchedulingState>();
        Assert.Contains("WO-9999", schedulingState.ReadyWorkOrders);
    }
}
```

### Why This Is High Quality

- **End-to-end notification flow:** Tests the full MediatR pipeline, not just individual handlers.
- **Cross-module verification:** Confirms that Module A's notification updates Module B's state.
- **Handler isolation test:** Verifies that one handler failing doesn't block others.
- **Uses shared fixture:** Consistent test infrastructure with real MediatR registration.
