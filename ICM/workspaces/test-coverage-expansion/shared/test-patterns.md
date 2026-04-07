# Test Patterns

Reference patterns for NUnit, bunit, and Aspire.Hosting.Testing in a .NET 10 solution.

## NUnit — Unit Test Structure

```csharp
[TestFixture]
public class GreedySchedulerTests
{
    private GreedyScheduler _scheduler = null!;

    [SetUp]
    public void SetUp()
    {
        // Construct the unit under test with value object dependencies only
        _scheduler = new GreedyScheduler(new SchedulerOptions { MaxYears = 5 });
    }

    [Test]
    public void Schedule_WithSufficientBudget_AssignsAllProjects()
    {
        // Arrange
        var projects = new[]
        {
            new PrioritizedProject { Id = 1, AnnualCost = 100_000, Priority = 1 },
            new PrioritizedProject { Id = 2, AnnualCost = 80_000, Priority = 2 }
        };
        var budget = new AnnualBudget(year: 2025, amount: 250_000);

        // Act
        var schedule = _scheduler.Schedule(projects, budget);

        // Assert
        Assert.That(schedule.AssignedProjects, Has.Count.EqualTo(2));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void Schedule_WithInvalidBudget_ThrowsArgumentException(decimal budget)
    {
        Assert.Throws<ArgumentException>(() =>
            _scheduler.Schedule(Array.Empty<PrioritizedProject>(), new AnnualBudget(2025, budget)));
    }
}
```

## NUnit — Async Tests

```csharp
[Test]
public async Task GetProjectsAsync_ReturnsAllProjects()
{
    // Arrange
    var repo = new FakeProjectRepository(seedWith: [new Project { Id = 1, Name = "Test" }]);

    // Act
    var projects = await repo.GetAllAsync();

    // Assert
    Assert.That(projects, Has.Count.EqualTo(1));
    Assert.That(projects[0].Name, Is.EqualTo("Test"));
}
```

## Fake Pattern (no mocking frameworks)

```csharp
// Prefer hand-written fakes over Moq/NSubstitute for unit tests
public class FakeProjectRepository : IProjectRepository
{
    private readonly List<Project> _projects;

    public FakeProjectRepository(IEnumerable<Project>? seedWith = null)
        => _projects = seedWith?.ToList() ?? [];

    public Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Project>>(_projects);

    public Task<Project?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_projects.FirstOrDefault(p => p.Id == id));

    public Task<Project> AddAsync(Project project, CancellationToken ct = default)
    {
        _projects.Add(project);
        return Task.FromResult(project);
    }

    public Task UpdateAsync(Project project, CancellationToken ct = default)
        => Task.CompletedTask;
}
```

## bunit — Component Test Structure

```csharp
[TestFixture]
public class ProjectListPageTests : BunitTestContext
{
    [SetUp]
    public void SetUp()
    {
        // Register mocked services on the bunit TestContext
        Services.AddSingleton<IProjectApiClient>(new FakeProjectApiClient());
        Services.AddSingleton<NavigationManager>(new FakeNavigationManager());
    }

    [Test]
    public void Renders_ProjectListPage_WithoutError()
    {
        var cut = RenderComponent<ProjectListPage>();
        cut.MarkupMatches(cut.Markup); // renders without throwing
    }

    [Test]
    public void ClickingProject_NavigatesToDetail()
    {
        var cut = RenderComponent<ProjectListPage>();
        var row = cut.Find("[data-testid='project-row-1']");

        row.Click();

        var nav = Services.GetRequiredService<FakeNavigationManager>();
        Assert.That(nav.Uri, Does.Contain("/projects/1"));
    }
}

// Base class to reduce boilerplate
public abstract class BunitTestContext : TestContext, IDisposable
{
    void IDisposable.Dispose() => Dispose();
}
```

## bunit — Testing Telerik Components

Telerik components require the TelerikRootComponent and service registration:

```csharp
[SetUp]
public void SetUp()
{
    Services.AddTelerikBlazor();

    // Wrap the component under test in TelerikRootComponent
    RenderComponent<TelerikRootComponent>(parameters =>
        parameters.AddChildContent<MyComponent>());
}
```

## Aspire.Hosting.Testing — Integration Test

```csharp
[TestFixture]
public class ProjectEndpointTests
{
    private DistributedApplication _app = null!;
    private HttpClient _apiClient = null!;

    [OneTimeSetUp]
    public async Task StartAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Parsons_Comet_AppHost>();

        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        _apiClient = _app.CreateHttpClient("apiservice");
    }

    [OneTimeTearDown]
    public async Task StopAsync()
    {
        await _app.DisposeAsync();
        _apiClient.Dispose();
    }

    [Test]
    public async Task GetProjects_ReturnsOk()
    {
        var response = await _apiClient.GetAsync("/api/projects");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task PostProject_CreatesAndReturns201()
    {
        var payload = new { Name = "Test Project", CategoryId = 1 };
        var response = await _apiClient.PostAsJsonAsync("/api/projects", payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
}
```

## NUnit Solver Pipeline Test Pattern

For the four-solver pipeline (Prioritize → Schedule → Project → Solve):

```csharp
[TestFixture]
public class FullPipelineTests
{
    [Test]
    public void Pipeline_WithRealisticInput_ProducesCompleteOutput()
    {
        // Arrange — use realistic domain values, not minimal stubs
        var projects = ProjectFixtures.RealisticProjectSet(count: 20);
        var settings = GlobalSettingsFixtures.Default();

        // Act — run each stage in sequence, passing output to next
        var prioritized = new PrioritizationEngine().Prioritize(projects, settings);
        var scheduled = new GreedyScheduler().Schedule(prioritized, settings.BudgetPlan);
        var projected = new ProjectionEngine().Project(scheduled, settings.PlanningWindow);
        var solution = new ScenarioSolver().Solve(projected, settings.TargetCondition);

        // Assert — verify the output shape, not exact values
        Assert.That(solution.Schedule, Is.Not.Empty);
        Assert.That(solution.Projections, Has.Count.EqualTo(settings.PlanningWindow.Years));
        Assert.That(solution.TargetAchieved, Is.True);
    }
}
```
