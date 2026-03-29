namespace Marilo.Core.Services;

public interface IMariloOverlayService
{
    event EventHandler? OverlayRequested;
    event EventHandler? OverlayDismissed;
    void RequestOverlay();
    void DismissOverlay();
}
