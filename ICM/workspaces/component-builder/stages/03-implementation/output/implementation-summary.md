# Implementation Summary: MariloResizableContainer

## Files Created

| File | Purpose |
|------|---------|
| `src/Marilo.Core/Enums/ResizableContainerEnums.cs` | MariloResizeEdges (flags enum) and MariloResizeAxis enums |
| `src/Marilo.Core/Models/MariloResizeEventArgs.cs` | MariloResizeEventArgs and MariloObservedSizeChangedEventArgs models |
| `src/Marilo.Components/Layout/ResizableContainer/MariloResizableContainer.razor` | Razor markup with container, content, handles, ghost outline |
| `src/Marilo.Components/Layout/ResizableContainer/MariloResizableContainer.razor.cs` | Code-behind with parameters, events, JS interop, public methods |
| `src/Marilo.Components/wwwroot/js/resizable-container.js` | ESM JS module for pointer drag, ResizeObserver, keyboard, persistence |

## Files Modified

| File | Change |
|------|--------|
| `src/Marilo.Core/Contracts/IMariloCssProvider.cs` | Added 3 ResizableContainer methods |
| `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` | Implemented 3 ResizableContainer methods |
| `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` | Implemented 3 ResizableContainer methods |
| `samples/Marilo.Demo/Services/ProviderSwitcher.cs` | Added 3 ResizableContainer delegation methods |

## Implementation Notes

- Component inherits from MariloComponentBase and implements IAsyncDisposable
- JS interop uses ESM module import pattern
- Pointer events handled via setPointerCapture for reliable cross-browser drag
- ResizeObserver fires OnObservedSizeChanged for all size changes
- MariloResizeEdges is a [Flags] enum allowing bitwise combination
- Ghost outline rendered as positioned div only during drag
- Handle elements are button for native focus/keyboard support
- Size persistence uses localStorage with key prefix marilo-rc-
- All JSInvokable callbacks use InvokeAsync(StateHasChanged) for dispatcher safety
