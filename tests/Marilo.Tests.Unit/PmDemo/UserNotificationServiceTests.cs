using System.Linq;
using System.Threading.Tasks;
using Marilo.Core.Models;
using Marilo.PmDemo.Client.Notifications;
using Xunit;

namespace Marilo.Tests.Unit.PmDemo;

public class UserNotificationServiceTests
{
    private static InMemoryUserNotificationService NewService(IUserNotificationToastForwarder? forwarder = null)
    {
        var svc = new InMemoryUserNotificationService(forwarder);
        // Reset the seed so each test starts from a known empty state.
        svc.ReplaceAllAsync(System.Array.Empty<UserNotification>()).GetAwaiter().GetResult();
        return svc;
    }

    [Fact]
    public async Task CreateAsync_inserts_and_raises_change()
    {
        var svc = NewService();
        var raised = 0;
        svc.Changed += () => raised++;

        await svc.CreateAsync(new UserNotification { Message = "hello" });

        Assert.Single(svc.All);
        Assert.Equal(1, svc.UnreadCount);
        Assert.Equal(1, raised);
    }

    [Fact]
    public async Task CreateAsync_with_correlation_key_dedupes_and_refreshes()
    {
        var svc = NewService();

        await svc.CreateAsync(new UserNotification
        {
            Message = "first",
            CorrelationKey = "risk:R-1:severity",
        });
        await svc.MarkAllReadAsync();

        await svc.CreateAsync(new UserNotification
        {
            Message = "second",
            CorrelationKey = "risk:R-1:severity",
        });

        Assert.Single(svc.All);
        var only = svc.All[0];
        Assert.Equal("second", only.Message);
        Assert.False(only.IsRead); // refresh resets read state
        Assert.Equal(1, svc.UnreadCount);
    }

    [Fact]
    public async Task MarkRead_only_affects_target_and_lowers_unread_count()
    {
        var svc = NewService();
        var a = await svc.CreateAsync(new UserNotification { Message = "a" });
        var b = await svc.CreateAsync(new UserNotification { Message = "b" });

        await svc.MarkReadAsync(a.Id);

        Assert.True(svc.All.Single(n => n.Id == a.Id).IsRead);
        Assert.False(svc.All.Single(n => n.Id == b.Id).IsRead);
        Assert.Equal(1, svc.UnreadCount);
    }

    [Fact]
    public async Task MarkAllRead_clears_unread_count()
    {
        var svc = NewService();
        await svc.CreateAsync(new UserNotification { Message = "a" });
        await svc.CreateAsync(new UserNotification { Message = "b" });
        await svc.CreateAsync(new UserNotification { Message = "c" });

        await svc.MarkAllReadAsync();

        Assert.Equal(0, svc.UnreadCount);
        Assert.All(svc.All, n => Assert.True(n.IsRead));
    }

    [Fact]
    public async Task DeleteAllRead_removes_only_read_items()
    {
        var svc = NewService();
        var a = await svc.CreateAsync(new UserNotification { Message = "keep" });
        var b = await svc.CreateAsync(new UserNotification { Message = "drop" });
        await svc.MarkReadAsync(b.Id);

        await svc.DeleteAllReadAsync();

        Assert.Single(svc.All);
        Assert.Equal(a.Id, svc.All[0].Id);
    }

    [Fact]
    public async Task ToastForwarder_called_only_when_delivery_includes_toast()
    {
        var spy = new SpyForwarder();
        var svc = NewService(spy);

        await svc.CreateAsync(new UserNotification
        {
            Message = "feed only",
            Delivery = NotificationDelivery.FeedOnly,
        });
        Assert.Empty(spy.Forwarded);

        await svc.CreateAsync(new UserNotification
        {
            Message = "feed and toast",
            Delivery = NotificationDelivery.FeedAndToast,
            Importance = NotificationImportance.High,
        });
        Assert.Single(spy.Forwarded);
        Assert.Equal("feed and toast", spy.Forwarded[0].Message);
    }

    [Fact]
    public async Task FeedProjection_carries_id_and_read_state()
    {
        var svc = NewService();
        var n = await svc.CreateAsync(new UserNotification
        {
            Title = "Risk R-1",
            Message = "Severity raised",
            Source = NotificationSource.Risks,
        });

        var item = NotificationFeedProjection.ToFeedItem(n);

        Assert.Equal(n.Id, item.Id);
        Assert.Equal("Risk R-1", item.Title);
        Assert.False(item.IsRead);
        Assert.NotNull(item.Icon); // Risks source maps to an SVG glyph
    }

    [Fact]
    public async Task ToastForwarder_maps_critical_to_persistent_error_toast()
    {
        var spy = new SpyForwarder();
        var svc = NewService(spy);

        await svc.CreateAsync(new UserNotification
        {
            Message = "outage",
            Delivery = NotificationDelivery.FeedAndToast,
            Importance = NotificationImportance.Critical,
        });

        Assert.Single(spy.Forwarded);
        Assert.Equal(Marilo.Core.Enums.ToastSeverity.Error, spy.LastToast!.Severity);
        Assert.Equal(0, spy.LastToast!.CloseAfterMs); // critical = no auto-close
    }

    private sealed class SpyForwarder : IUserNotificationToastForwarder
    {
        public System.Collections.Generic.List<UserNotification> Forwarded { get; } = new();
        public NotificationModel? LastToast { get; private set; }

        public void Forward(UserNotification notification)
        {
            Forwarded.Add(notification);
            // Mirror the production adapter's mapping so the test exercises the same shape.
            var sev = notification.Importance switch
            {
                NotificationImportance.High     => Marilo.Core.Enums.ToastSeverity.Warning,
                NotificationImportance.Critical => Marilo.Core.Enums.ToastSeverity.Error,
                _                               => Marilo.Core.Enums.ToastSeverity.Info,
            };
            LastToast = new NotificationModel
            {
                Id = notification.Id,
                Message = notification.Message,
                Severity = sev,
                CloseAfterMs = notification.Importance == NotificationImportance.Critical ? 0 : 5000,
                Closeable = true,
            };
        }
    }
}
