# High-Quality Test Examples: Service and CQRS

> **Purpose:** Demonstrate test patterns for service-layer orchestration and CQRS handler testing.
> **Governed by:** ADR-0024 (State vs Service Decision), ADR-0006 (Persistence CQRS Consolidation), ADR-0013 (Data Access Technology Selection).

## Example 1: Service Orchestration Unit Test

Testing a service that orchestrates multiple ports with mocked dependencies.

```csharp
public class WorkOrderApprovalServiceTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<INotificationService> _notifications;
    private readonly Mock<IAuditLogger> _auditLogger;
    private readonly WorkOrderApprovalService _sut;

    public WorkOrderApprovalServiceTests()
    {
        _mediator = new Mock<IMediator>();
        _notifications = new Mock<INotificationService>();
        _auditLogger = new Mock<IAuditLogger>();
        _sut = new WorkOrderApprovalService(
            _mediator.Object,
            _notifications.Object,
            _auditLogger.Object);
    }

    [Fact]
    public async Task ApproveWorkOrder_WhenValid_UpdatesStatusAndNotifiesAndAudits()
    {
        // Arrange
        var workOrderId = "WO-1234";
        var approverId = "user-456";
        var workOrder = new WorkOrderDto
        {
            Id = workOrderId,
            Status = "Submitted",
            RequestedBy = "user-789"
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<GetWorkOrder.Request>(), default))
            .ReturnsAsync(workOrder);

        _mediator
            .Setup(m => m.Send(It.IsAny<ChangeWorkOrderStatus.Request>(), default))
            .ReturnsAsync(OperationResult.Success());

        // Act
        var result = await _sut.ApproveAsync(workOrderId, approverId);

        // Assert
        Assert.True(result.IsSuccess);

        _mediator.Verify(m => m.Send(
            It.Is<ChangeWorkOrderStatus.Request>(r =>
                r.WorkOrderId == workOrderId &&
                r.NewStatus == "Approved"),
            default), Times.Once);

        _notifications.Verify(n => n.SendAsync(
            It.Is<Notification>(n =>
                n.UserId == "user-789" &&
                n.Message.Contains("approved")),
            default), Times.Once);

        _auditLogger.Verify(a => a.LogAsync(
            It.Is<AuditEntry>(e =>
                e.Action == "WorkOrderApproved" &&
                e.ActorId == approverId),
            default), Times.Once);
    }

    [Fact]
    public async Task ApproveWorkOrder_WhenNotInSubmittedStatus_ReturnsFailure()
    {
        // Arrange
        var workOrder = new WorkOrderDto { Id = "WO-1234", Status = "Draft" };
        _mediator
            .Setup(m => m.Send(It.IsAny<GetWorkOrder.Request>(), default))
            .ReturnsAsync(workOrder);

        // Act
        var result = await _sut.ApproveAsync("WO-1234", "user-456");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Cannot approve", result.Error);

        _mediator.Verify(m => m.Send(
            It.IsAny<ChangeWorkOrderStatus.Request>(), default), Times.Never);
        _notifications.Verify(n => n.SendAsync(
            It.IsAny<Notification>(), default), Times.Never);
    }

    [Fact]
    public async Task ApproveWorkOrder_WhenNotificationFails_StillSucceeds()
    {
        // Arrange
        var workOrder = new WorkOrderDto { Id = "WO-1234", Status = "Submitted", RequestedBy = "user-789" };
        _mediator
            .Setup(m => m.Send(It.IsAny<GetWorkOrder.Request>(), default))
            .ReturnsAsync(workOrder);
        _mediator
            .Setup(m => m.Send(It.IsAny<ChangeWorkOrderStatus.Request>(), default))
            .ReturnsAsync(OperationResult.Success());
        _notifications
            .Setup(n => n.SendAsync(It.IsAny<Notification>(), default))
            .ThrowsAsync(new NotificationException("Service unavailable"));

        // Act
        var result = await _sut.ApproveAsync("WO-1234", "user-456");

        // Assert — approval succeeds even if notification fails
        Assert.True(result.IsSuccess);
        _auditLogger.Verify(a => a.LogAsync(
            It.IsAny<AuditEntry>(), default), Times.Once);
    }
}
```

### Why This Is High Quality

- **Tests orchestration, not implementation details:** Verifies call sequence and business rules.
- **Covers happy path, guard condition, and partial failure.**
- **Notification failure doesn't block approval:** Tests the resilience design decision.
- **Verify calls with specific argument matchers:** Ensures correct parameters, not just "was called."

---

## Example 2: CQRS Handler Unit Tests (ADR-0006 / ADR-0013)

Testing parameter building and stored procedure invocation.

```csharp
public class GetWorkOrderHandlerTests
{
    private readonly Mock<IDbConnectionFactory> _connectionFactory;
    private readonly Mock<IDbConnection> _connection;
    private readonly GetWorkOrder.Handler _sut;

    public GetWorkOrderHandlerTests()
    {
        _connectionFactory = new Mock<IDbConnectionFactory>();
        _connection = new Mock<IDbConnection>();
        _connectionFactory
            .Setup(f => f.CreateConnection())
            .Returns(_connection.Object);
        _sut = new GetWorkOrder.Handler(_connectionFactory.Object);
    }

    [Fact]
    public async Task Handle_BuildsCorrectParameters()
    {
        // Arrange
        var request = new GetWorkOrder.Request
        {
            WorkOrderId = "WO-1234",
            LogUser = "test-user",
            LogAppName = "UnitTest"
        };

        DynamicParameters capturedParams = null;
        // Capture the parameters passed to Dapper
        _connection
            .Setup(c => c.QuerySingleOrDefaultAsync<WorkOrderDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, CommandType.StoredProcedure))
            .Callback<string, object, IDbTransaction, int?, CommandType?>(
                (sql, param, tx, timeout, type) =>
                    capturedParams = param as DynamicParameters)
            .ReturnsAsync(new WorkOrderDto());

        // Act
        await _sut.Handle(request, default);

        // Assert
        Assert.NotNull(capturedParams);
        Assert.Equal("WO-1234", capturedParams.Get<string>("Identifier"));
        Assert.Equal("test-user", capturedParams.Get<string>("LogUser"));
        Assert.Equal("UnitTest", capturedParams.Get<string>("LogAppName"));
    }

    [Fact]
    public async Task Handle_CallsCorrectStoredProcedure()
    {
        // Arrange
        var request = new GetWorkOrder.Request
        {
            WorkOrderId = "WO-1234",
            LogUser = "test-user",
            LogAppName = "UnitTest"
        };

        string capturedProcName = null;
        _connection
            .Setup(c => c.QuerySingleOrDefaultAsync<WorkOrderDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, CommandType.StoredProcedure))
            .Callback<string, object, IDbTransaction, int?, CommandType?>(
                (sql, param, tx, timeout, type) => capturedProcName = sql)
            .ReturnsAsync(new WorkOrderDto());

        // Act
        await _sut.Handle(request, default);

        // Assert
        Assert.Equal("dbo.GetStoredProcedureWorkOrder", capturedProcName);
    }

    [Fact]
    public async Task Handle_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var request = new GetWorkOrder.Request
        {
            WorkOrderId = "WO-NONEXISTENT",
            LogUser = "test-user",
            LogAppName = "UnitTest"
        };

        _connection
            .Setup(c => c.QuerySingleOrDefaultAsync<WorkOrderDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, CommandType.StoredProcedure))
            .ReturnsAsync((WorkOrderDto)null);

        // Act
        var result = await _sut.Handle(request, default);

        // Assert
        Assert.Null(result);
    }
}
```

### Why This Is High Quality

- **Tests Dapper-first pattern per ADR-0013:** Verifies stored procedure name and parameter building.
- **Captures actual parameters:** Doesn't just assert "method was called" — verifies exact parameter values.
- **Required parameters tested:** LogUser, LogAppName, Identifier always present.
- **Tests null/not-found case:** Important for API consumers.

---

## Example 3: CQRS Integration Test

Testing against a real database connection.

```csharp
public class CreateWorkOrderIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _db;

    public CreateWorkOrderIntegrationTests(DatabaseFixture db) => _db = db;

    [Fact]
    public async Task Handle_CreatesWorkOrderInDatabase()
    {
        // Arrange
        var handler = new CreateWorkOrder.Handler(_db.ConnectionFactory);
        var request = new CreateWorkOrder.Request
        {
            Title = "Integration Test Work Order",
            AssetId = "ASSET-001",
            Priority = "High",
            Description = "Created by integration test",
            LogUser = "integration-test",
            LogAppName = "TestRunner"
        };

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.WorkOrderId);

        // Verify persisted correctly
        var getHandler = new GetWorkOrder.Handler(_db.ConnectionFactory);
        var saved = await getHandler.Handle(new GetWorkOrder.Request
        {
            WorkOrderId = result.WorkOrderId,
            LogUser = "integration-test",
            LogAppName = "TestRunner"
        }, default);

        Assert.Equal("Integration Test Work Order", saved.Title);
        Assert.Equal("ASSET-001", saved.AssetId);
        Assert.Equal("Draft", saved.Status); // New work orders start as Draft
    }
}
```

### Why This Is High Quality

- **Round-trip test:** Creates then reads back to verify persistence.
- **Tests default state:** Asserts new work orders start as "Draft."
- **Uses DatabaseFixture:** Shared test infrastructure, not ad-hoc setup.
- **Realistic parameters:** Includes LogUser and LogAppName per ADR-0013.
