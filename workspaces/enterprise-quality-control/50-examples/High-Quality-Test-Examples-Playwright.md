# High-Quality Test Examples: Playwright E2E Tests

> **Purpose:** Demonstrate Playwright end-to-end testing patterns for Blazor applications following ICM architecture patterns.
> **Governed by:** ADR-0007 (Navigation State Management), ADR-0024 (State vs Service Decision), ADR-0012 (UI Component Library).
> **NuGet:** `Microsoft.Playwright`, `Microsoft.Playwright.NUnit` or `Microsoft.Playwright.MSTest`

## Project Setup

```xml
<!-- E2E.Tests.csproj -->
<PackageReference Include="Microsoft.Playwright.NUnit" Version="1.*" />
```

```bash
# Install browsers (run once after package restore)
pwsh bin/Debug/net8.0/playwright.ps1 install
```

### Base Test Class

```csharp
[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class PlaywrightTestBase : PageTest
{
    protected string BaseUrl => TestContext.Parameters["BaseUrl"]
        ?? "https://localhost:5001";

    /// <summary>
    /// Login as a test user before each test.
    /// Adapt to your authentication method (Entra, cookie, token).
    /// </summary>
    protected async Task LoginAsAsync(string role = "operator")
    {
        await Page.GotoAsync($"{BaseUrl}/test-login?role={role}");
        await Page.WaitForURLAsync($"{BaseUrl}/dashboard");
    }
}
```

---

## Example 1: Critical User Journey — Create Work Order

Testing the full create-work-order flow from dashboard to confirmation.

```csharp
[TestFixture]
public class CreateWorkOrderE2ETests : PlaywrightTestBase
{
    [SetUp]
    public async Task Setup() => await LoginAsAsync("operator");

    [Test]
    public async Task CreateWorkOrder_FullFlow_FromDashboardToConfirmation()
    {
        // Navigate to work orders
        await Page.GetByTestId("nav-work-orders").ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders$"));

        // Click create button
        await Page.GetByTestId("create-work-order").ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders/new"));

        // Fill in the form
        await Page.GetByTestId("title-input")
            .FillAsync("Quarterly pump inspection - Unit 7");
        await Page.GetByTestId("asset-input")
            .FillAsync("PUMP-007");
        await Page.GetByTestId("priority-select")
            .SelectOptionAsync("High");
        await Page.GetByTestId("description-input")
            .FillAsync("Scheduled quarterly inspection per maintenance plan MP-2026-Q1");
        await Page.GetByTestId("due-date-input")
            .FillAsync("2026-04-15");

        // Submit the form
        await Page.GetByTestId("submit-button").ClickAsync();

        // Verify confirmation
        await Expect(Page.GetByTestId("success-toast"))
            .ToBeVisibleAsync();
        await Expect(Page.GetByTestId("success-toast"))
            .ToContainTextAsync("Work order created");

        // Verify redirected to detail page
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders/WO-\\d+"));
        await Expect(Page.GetByTestId("wo-title"))
            .ToHaveTextAsync("Quarterly pump inspection - Unit 7");
        await Expect(Page.GetByTestId("wo-status"))
            .ToHaveTextAsync("Draft");
    }

    [Test]
    public async Task CreateWorkOrder_ValidationErrors_PreventSubmission()
    {
        await Page.GetByTestId("nav-work-orders").ClickAsync();
        await Page.GetByTestId("create-work-order").ClickAsync();

        // Submit without filling required fields
        await Page.GetByTestId("submit-button").ClickAsync();

        // Verify validation messages appear
        await Expect(Page.GetByTestId("title-validation"))
            .ToBeVisibleAsync();
        await Expect(Page.GetByTestId("title-validation"))
            .ToContainTextAsync("required");
        await Expect(Page.GetByTestId("asset-validation"))
            .ToBeVisibleAsync();

        // Verify we did NOT navigate away
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders/new"));
    }
}
```

### Why This Is High Quality

- **Tests the complete user journey:** Dashboard → list → create → fill → submit → confirmation.
- **Uses `GetByTestId`:** Resilient selectors that survive CSS/layout refactors.
- **Tests validation separately:** Ensures form validation prevents bad submissions.
- **Verifies navigation per ADR-0007:** URL changes confirm RouteState is working.
- **Verifies initial state:** New work orders start as "Draft."

---

## Example 2: Status Transition Flow

Testing the approval workflow through the UI.

```csharp
[TestFixture]
public class WorkOrderApprovalE2ETests : PlaywrightTestBase
{
    [Test]
    public async Task ApproveWorkOrder_FullFlow_StatusChangesAndNotificationShows()
    {
        // Login as operator, create and submit a work order
        await LoginAsAsync("operator");
        var workOrderId = await CreateAndSubmitWorkOrderAsync();

        // Login as approver
        await LoginAsAsync("approver");
        await Page.GotoAsync($"{BaseUrl}/work-orders/{workOrderId}");

        // Verify the approve button is visible for submitted work orders
        await Expect(Page.GetByTestId("approve-button")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("reject-button")).ToBeVisibleAsync();

        // Approve the work order
        await Page.GetByTestId("approve-button").ClickAsync();

        // Confirm the approval dialog
        await Expect(Page.GetByTestId("confirm-dialog")).ToBeVisibleAsync();
        await Page.GetByTestId("confirm-yes").ClickAsync();

        // Verify status changed
        await Expect(Page.GetByTestId("wo-status"))
            .ToHaveTextAsync("Approved");

        // Verify approve/reject buttons are gone
        await Expect(Page.GetByTestId("approve-button"))
            .Not.ToBeVisibleAsync();

        // Verify audit trail entry appears
        await Expect(Page.GetByTestId("audit-log"))
            .ToContainTextAsync("Approved by");
    }

    [Test]
    public async Task ApproveWorkOrder_AsOperator_ApproveButtonNotVisible()
    {
        await LoginAsAsync("operator");
        var workOrderId = await CreateAndSubmitWorkOrderAsync();

        await Page.GotoAsync($"{BaseUrl}/work-orders/{workOrderId}");

        // Operator should NOT see the approve button
        await Expect(Page.GetByTestId("approve-button"))
            .Not.ToBeVisibleAsync();
    }

    private async Task<string> CreateAndSubmitWorkOrderAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders/new");
        await Page.GetByTestId("title-input").FillAsync("E2E Test Work Order");
        await Page.GetByTestId("asset-input").FillAsync("ASSET-E2E");
        await Page.GetByTestId("priority-select").SelectOptionAsync("Medium");
        await Page.GetByTestId("submit-button").ClickAsync();
        await Expect(Page.GetByTestId("wo-status")).ToHaveTextAsync("Draft");

        // Submit the draft
        await Page.GetByTestId("submit-for-approval-button").ClickAsync();
        await Expect(Page.GetByTestId("wo-status")).ToHaveTextAsync("Submitted");

        // Extract work order ID from URL
        var url = Page.Url;
        return url.Split('/').Last();
    }
}
```

### Why This Is High Quality

- **Tests role-based access:** Approver sees approve button, operator does not.
- **Tests the full approval flow:** Submit → approve → status change → audit trail.
- **Tests UI guards:** Confirmation dialog prevents accidental approvals.
- **Helper method reuse:** `CreateAndSubmitWorkOrderAsync` keeps tests focused.

---

## Example 3: Navigation and Deep Links

Testing that navigation works correctly via RouteState (ADR-0007).

```csharp
[TestFixture]
public class NavigationE2ETests : PlaywrightTestBase
{
    [SetUp]
    public async Task Setup() => await LoginAsAsync("operator");

    [Test]
    public async Task DeepLink_ToWorkOrderDetail_LoadsCorrectData()
    {
        // Navigate directly to a known work order
        await Page.GotoAsync($"{BaseUrl}/work-orders/WO-1001");

        // Verify the correct work order loads
        await Expect(Page.GetByTestId("wo-title"))
            .Not.ToBeEmptyAsync();
        await Expect(Page.GetByTestId("wo-id"))
            .ToHaveTextAsync("WO-1001");
    }

    [Test]
    public async Task BackButton_ReturnsToList_FromDetail()
    {
        // Start at the list
        await Page.GotoAsync($"{BaseUrl}/work-orders");
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders$"));

        // Navigate to a detail
        await Page.GetByTestId("wo-row").First.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders/WO-"));

        // Click back
        await Page.GetByTestId("back-button").ClickAsync();

        // Verify we're back at the list
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders$"));
    }

    [Test]
    public async Task BrowserBackButton_WorksWithRouteState()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders");
        await Page.GetByTestId("wo-row").First.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders/WO-"));

        // Use browser back button
        await Page.GoBackAsync();

        // RouteState should handle browser back correctly
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders$"));
    }

    [Test]
    public async Task DeepLink_WithInvalidId_ShowsNotFound()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders/WO-INVALID");

        await Expect(Page.GetByTestId("not-found-message"))
            .ToBeVisibleAsync();
    }
}
```

---

## Example 4: Telerik Grid Interactions

Testing Telerik grid sorting, filtering, and paging.

```csharp
[TestFixture]
public class WorkOrderGridE2ETests : PlaywrightTestBase
{
    [SetUp]
    public async Task Setup() => await LoginAsAsync("operator");

    [Test]
    public async Task Grid_SortByPriority_ReordersRows()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders");

        // Click the Priority column header to sort
        await Page.Locator(".k-header")
            .Filter(new() { HasText = "Priority" })
            .ClickAsync();

        // Verify sort indicator appears
        await Expect(Page.Locator(".k-header .k-sort-icon"))
            .ToBeVisibleAsync();

        // Verify first row has highest priority
        var firstRowPriority = await Page
            .GetByTestId("wo-row").First
            .GetByTestId("wo-priority")
            .TextContentAsync();

        Assert.That(firstRowPriority, Is.EqualTo("Critical").Or.EqualTo("High"));
    }

    [Test]
    public async Task Grid_FilterByStatus_ShowsOnlyMatchingRows()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders");

        // Open status filter
        await Page.GetByTestId("filter-status").ClickAsync();
        await Page.GetByTestId("filter-option-overdue").ClickAsync();

        // Wait for grid to refresh
        await Page.WaitForResponseAsync(resp =>
            resp.Url.Contains("/api/") && resp.Status == 200);

        // Verify all visible rows show overdue indicator
        var rows = await Page.GetByTestId("wo-row").AllAsync();
        foreach (var row in rows)
        {
            await Expect(row.GetByTestId("overdue-badge")).ToBeVisibleAsync();
        }
    }

    [Test]
    public async Task Grid_Pagination_NavigatesBetweenPages()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders");

        // Verify page 1 indicator
        await Expect(Page.Locator(".k-pager-numbers .k-selected"))
            .ToHaveTextAsync("1");

        // Get first row text on page 1
        var page1FirstRow = await Page
            .GetByTestId("wo-row").First
            .GetByTestId("wo-id")
            .TextContentAsync();

        // Navigate to page 2
        await Page.Locator(".k-pager-numbers")
            .GetByText("2")
            .ClickAsync();

        // Verify page changed
        await Expect(Page.Locator(".k-pager-numbers .k-selected"))
            .ToHaveTextAsync("2");

        // Verify different data on page 2
        var page2FirstRow = await Page
            .GetByTestId("wo-row").First
            .GetByTestId("wo-id")
            .TextContentAsync();

        Assert.That(page2FirstRow, Is.Not.EqualTo(page1FirstRow));
    }
}
```

---

## Example 5: Responsive Layout and Accessibility

Testing that the application works on different viewports and meets accessibility basics.

```csharp
[TestFixture]
public class ResponsiveAndAccessibilityE2ETests : PlaywrightTestBase
{
    [SetUp]
    public async Task Setup() => await LoginAsAsync("operator");

    [Test]
    public async Task MobileViewport_ShowsHamburgerMenu()
    {
        // Set mobile viewport
        await Page.SetViewportSizeAsync(375, 812);
        await Page.GotoAsync($"{BaseUrl}/work-orders");

        // Desktop nav should be hidden
        await Expect(Page.GetByTestId("desktop-nav")).Not.ToBeVisibleAsync();

        // Hamburger menu should be visible
        await Expect(Page.GetByTestId("mobile-menu-toggle")).ToBeVisibleAsync();

        // Open mobile menu and navigate
        await Page.GetByTestId("mobile-menu-toggle").ClickAsync();
        await Expect(Page.GetByTestId("mobile-nav")).ToBeVisibleAsync();

        await Page.GetByTestId("nav-work-orders").ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-orders"));
    }

    [Test]
    public async Task KeyboardNavigation_TabThroughFormFields()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders/new");

        // Tab through form fields and verify focus order
        await Page.GetByTestId("title-input").FocusAsync();
        await Page.Keyboard.PressAsync("Tab");

        await Expect(Page.GetByTestId("asset-input")).ToBeFocusedAsync();

        await Page.Keyboard.PressAsync("Tab");
        await Expect(Page.GetByTestId("priority-select")).ToBeFocusedAsync();

        await Page.Keyboard.PressAsync("Tab");
        await Expect(Page.GetByTestId("description-input")).ToBeFocusedAsync();
    }

    [Test]
    public async Task AriaLabels_ExistOnInteractiveElements()
    {
        await Page.GotoAsync($"{BaseUrl}/work-orders");

        // Verify ARIA labels on key interactive elements
        await Expect(Page.GetByTestId("create-work-order"))
            .ToHaveAttributeAsync("aria-label", new Regex("create", RegexOptions.IgnoreCase));

        await Expect(Page.GetByTestId("filter-status"))
            .ToHaveAttributeAsync("aria-label", new Regex("filter|status", RegexOptions.IgnoreCase));
    }
}
```

---

## Playwright CI Integration

### Pipeline Configuration

```yaml
# Example: Azure DevOps or GitHub Actions
- name: Run Playwright E2E tests
  run: |
    dotnet build tests/E2E.Tests/
    pwsh tests/E2E.Tests/bin/Debug/net8.0/playwright.ps1 install --with-deps
    dotnet test tests/E2E.Tests/ \
      --logger "trx;LogFileName=e2e-results.trx" \
      -- TestRunParameters.Parameter.BaseUrl=${{ env.APP_URL }}
  env:
    APP_URL: https://localhost:5001
```

### Test Categorization for CI

```csharp
[Category("smoke")]    // Run on every PR — critical user journeys only
[Category("full")]     // Run nightly — all E2E tests
[Category("visual")]   // Run weekly — screenshot comparisons
```

### Recommended Test Distribution

| Category | When to run | Count target | Timeout |
|----------|------------|--------------|---------|
| Smoke | Every PR | 5–10 tests | 5 min |
| Full E2E | Nightly | 30–50 tests | 15 min |
| Visual regression | Weekly | 10–20 pages | 10 min |
| Performance | Weekly | 3–5 scenarios | 10 min |

---

## Common Playwright Patterns Reference

| Pattern | Usage |
|---------|-------|
| `Page.GetByTestId("x")` | Find by `data-testid` attribute (preferred) |
| `Page.GetByRole(AriaRole.Button, new() { Name = "Submit" })` | Find by ARIA role |
| `Page.GetByText("text")` | Find by visible text |
| `Expect(locator).ToBeVisibleAsync()` | Assert element is visible |
| `Expect(locator).ToHaveTextAsync("x")` | Assert text content |
| `Expect(locator).Not.ToBeVisibleAsync()` | Assert element is hidden |
| `Expect(Page).ToHaveURLAsync(regex)` | Assert navigation occurred |
| `Page.WaitForResponseAsync(pred)` | Wait for API response |
| `Page.SetViewportSizeAsync(w, h)` | Test responsive layouts |
| `Page.Keyboard.PressAsync("Tab")` | Test keyboard navigation |
