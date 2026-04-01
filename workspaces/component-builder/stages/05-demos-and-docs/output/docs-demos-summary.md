# Docs & Demos Summary: SignalRConnectionStatus

## Documentation Created

### Component Spec (`docs/component-specs/signalr-connection-status/`)
- `overview.md` -- Purpose, basic usage, parameters table, service setup, aggregate state mapping
- `appearance.md` -- Compact mode, hide counts, popup placement, health badges, critical vs noncritical
- `accessibility/overview.md` -- Keyboard interactions, ARIA attributes, screen reader support
- `toc.yml` -- DocFx table of contents

All doc files include YAML front matter with title, slug, tags, and position.

## Demo Pages

Demo page creation deferred -- requires running app context. Structure follows existing Marilo demo pattern:

```
samples/Marilo.Demo/Pages/Components/SignalRConnectionStatus/Overview.razor
```

Sections planned:
- Basic Usage (default configuration)
- Compact Mode (toolbar density)
- Aggregate States (simulated healthy/degraded/offline)
- Popup Content (hub rows with various health states)
- Custom Title and Placement
- Accessibility Info (keyboard interactions, ARIA attributes)
