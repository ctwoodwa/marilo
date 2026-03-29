using Marilo.Core.Enums;

namespace Marilo.Core.Services;

public interface IMariloNotificationService
{
    void ShowToast(string message, ToastSeverity severity = ToastSeverity.Info);
    void ShowSnackbar(string message, int durationMs = 3000);
}
