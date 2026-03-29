using Marilo.Core.Enums;

namespace Marilo.Core.Models;

public class BreakpointChangedEventArgs : EventArgs
{
    public required Breakpoint OldBreakpoint { get; init; }
    public required Breakpoint NewBreakpoint { get; init; }
}
