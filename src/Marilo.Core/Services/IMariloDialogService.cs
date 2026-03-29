namespace Marilo.Core.Services;

public interface IMariloDialogService
{
    Task<bool> ShowConfirmAsync(string title, string message);
    Task ShowAlertAsync(string title, string message);
}
