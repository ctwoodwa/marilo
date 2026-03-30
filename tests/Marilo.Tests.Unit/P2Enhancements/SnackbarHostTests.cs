using Bunit;
using Marilo.Components.Feedback;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Marilo.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Marilo.Tests.Unit.P2Enhancements;

public class SnackbarHostTests : MariloTestBase
{
    [Fact]
    public void SnackbarHost_RendersNotificationsFromService()
    {
        // Arrange
        var service = Services.GetRequiredService<IMariloNotificationService>();
        service.Show(new NotificationModel { Id = "n1", Message = "Hello", Severity = ToastSeverity.Info, CloseAfterMs = 0 });
        service.Show(new NotificationModel { Id = "n2", Message = "World", Severity = ToastSeverity.Success, CloseAfterMs = 0 });

        // Act
        var cut = Render<MariloSnackbarHost>();

        // Assert
        var messages = cut.FindAll(".mar-snackbar__message");
        Assert.Equal(2, messages.Count);
        Assert.Contains("Hello", messages[0].TextContent);
        Assert.Contains("World", messages[1].TextContent);
    }

    [Fact]
    public void SnackbarHost_RespectsMaxCount()
    {
        // Arrange
        var service = Services.GetRequiredService<IMariloNotificationService>();
        for (var i = 0; i < 10; i++)
        {
            service.Show(new NotificationModel { Id = $"n{i}", Message = $"Msg {i}", CloseAfterMs = 0 });
        }

        // Act
        var cut = Render<MariloSnackbarHost>(p => p.Add(h => h.MaxCount, 3));

        // Assert — should only show 3
        var messages = cut.FindAll(".mar-snackbar__message");
        Assert.Equal(3, messages.Count);
    }

    [Fact]
    public void SnackbarHost_DismissButtonRemovesNotification()
    {
        // Arrange
        var service = Services.GetRequiredService<IMariloNotificationService>();
        service.Show(new NotificationModel { Id = "n1", Message = "Dismiss me", Closeable = true, CloseAfterMs = 0 });

        var cut = Render<MariloSnackbarHost>();
        Assert.Single(cut.FindAll(".mar-snackbar__message"));

        // Act — click dismiss
        var dismissBtn = cut.Find(".mar-snackbar__dismiss");
        dismissBtn.Click();

        // Assert
        Assert.Empty(cut.FindAll(".mar-snackbar__message"));
    }

    [Fact]
    public void NotificationService_Show_AddsNotification()
    {
        var service = new MariloNotificationService();

        service.Show(new NotificationModel { Id = "test1", Message = "Test" });

        var notifications = service.GetNotifications();
        Assert.Single(notifications);
        Assert.Equal("test1", notifications[0].Id);
    }

    [Fact]
    public void NotificationService_Hide_RemovesNotification()
    {
        var service = new MariloNotificationService();
        service.Show(new NotificationModel { Id = "test1", Message = "Test" });
        service.Show(new NotificationModel { Id = "test2", Message = "Test 2" });

        service.Hide("test1");

        var notifications = service.GetNotifications();
        Assert.Single(notifications);
        Assert.Equal("test2", notifications[0].Id);
    }

    [Fact]
    public void NotificationService_HideAll_ClearsList()
    {
        var service = new MariloNotificationService();
        service.Show(new NotificationModel { Id = "test1", Message = "Test" });
        service.Show(new NotificationModel { Id = "test2", Message = "Test 2" });

        service.HideAll();

        Assert.Empty(service.GetNotifications());
    }

    [Fact]
    public void NotificationService_ShowToast_CreatesNotification()
    {
        var service = new MariloNotificationService();

        service.ShowToast("Hello", ToastSeverity.Warning, 5000);

        var notifications = service.GetNotifications();
        Assert.Single(notifications);
        Assert.Equal("Hello", notifications[0].Message);
        Assert.Equal(ToastSeverity.Warning, notifications[0].Severity);
        Assert.Equal(5000, notifications[0].CloseAfterMs);
    }

    [Fact]
    public void NotificationService_OnChange_FiresOnShow()
    {
        var service = new MariloNotificationService();
        var fired = false;
        service.OnChange += () => fired = true;

        service.Show(new NotificationModel { Message = "Test" });

        Assert.True(fired);
    }
}
