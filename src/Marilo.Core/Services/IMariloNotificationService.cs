using Marilo.Core.Enums;
using Marilo.Core.Models;

namespace Marilo.Core.Services;

public interface IMariloNotificationService
{
    /// <summary>Shows a notification model directly.</summary>
    void Show(NotificationModel notification);

    /// <summary>Creates and shows a toast-style notification.</summary>
    void ShowToast(string message, ToastSeverity severity = ToastSeverity.Info, int durationMs = 3000);

    /// <summary>Creates and shows a snackbar-style notification.</summary>
    void ShowSnackbar(string message, int durationMs = 3000);

    /// <summary>Hides (removes) a notification by its ID.</summary>
    void Hide(string id);

    /// <summary>Hides all active notifications.</summary>
    void HideAll();

    /// <summary>Returns the current list of active notifications.</summary>
    IReadOnlyList<NotificationModel> GetNotifications();

    /// <summary>Fires whenever the notification list changes.</summary>
    event Action? OnChange;
}
