# Test Summary: MariloResizableContainer

## Test Files

| File | Tests |
|------|-------|
| `tests/Marilo.Tests.Unit/Layout/MariloResizableContainerTests.cs` | 27 |

## Results

Passed: 27, Failed: 0, Skipped: 0

## Test Coverage

### Render tests
- Default rendering with container class, content class, and handle
- Default bottom-right handle rendered
- Child content rendering

### Sizing parameters
- Width and Height applied as inline style
- MinWidth and MinHeight applied
- MaxWidth and MaxHeight applied

### Handle visibility
- ShowHandle=false hides handle
- Enabled=false hides handle and applies disabled class

### Edge configuration
- ResizeEdges.Right renders right handle only
- ResizeEdges.Bottom renders bottom handle
- ResizeEdges.None renders no handles

### Accessibility
- Handle has default aria-label "Resize"
- Custom HandleAriaLabel applied
- Handle is a button element
- Handle has tabindex="0"

### Custom classes
- Custom Class applied to root
- Custom HandleClass applied to handle

### FluentUI CSS Provider contract
- ContainerClass default, resizing, disabled states
- ContentClass returns expected string
- HandleClass BottomRight, active, focused states

### Bootstrap CSS Provider
- Not tested in unit project (no project reference); verified structurally identical
