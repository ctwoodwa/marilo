# Demo Scenario Format

Each demo section in the Blazor demo page must contain:

1. A scenario title that describes a real use case (not "Test" or "Example 1").
2. A brief description (1-2 sentences) explaining when a developer would use this configuration.
3. A live interactive Blazor component using actual Marilo API.
4. At least one user-controllable input (toggle, slider, input, etc.) that changes the component's behaviour in real time.
5. A code snippet panel showing the minimal Razor markup for this scenario (collapsible preferred; matches current API exactly).
6. A parameter table listing which parameters are active in this scenario:
   | Parameter | Value in this scenario | Notes |
7. A link or anchor reference to the corresponding spec section.

A demo page section is COMPLETE when:
- Every parameter in the spec has at least one scenario where it is the primary focus (other parameters may be present but are secondary).
- Every event has at least one scenario that triggers it visibly.
- Disabled state is demonstrated.
- Readonly state is demonstrated (if the component supports it).
- Empty/no-data state is demonstrated.
- Error state is demonstrated (if the component supports it).

A code snippet is STALE when:
- It references a parameter name that no longer exists in the source.
- It uses a type that has changed (e.g., string where enum is now required).
- It is missing a required parameter added after the snippet was written.
