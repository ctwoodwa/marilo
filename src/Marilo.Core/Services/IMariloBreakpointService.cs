using Marilo.Core.Enums;
using Marilo.Core.Models;

namespace Marilo.Core.Services;

public interface IMariloBreakpointService
{
    Breakpoint Current { get; }
    event EventHandler<BreakpointChangedEventArgs>? BreakpointChanged;
    Task InitializeAsync();
}
