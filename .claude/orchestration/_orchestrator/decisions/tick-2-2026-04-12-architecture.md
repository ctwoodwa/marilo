# Architecture Decisions — 2026-04-12

## PivotGrid API Shape
**Decision:** Use spec-defined child component tags (`<PivotGridRows>`, `<PivotGridColumn>`, `<PivotGridMeasure>`).
**Rationale:** User decision. Matches the established Marilo child-registration pattern (DataGrid → GridColumn, Wizard → WizardStep).
**Impact:** Source refactor from flat `List<PivotGridField>` to child-tag registration. Follows `IXxxSettingsSink` cascade pattern.

## DockManager Scope
**Decision:** Commit to the full spec. Full dock manager with floating panes, drag-and-drop, split/tab-group hierarchy.
**Rationale:** User decision. Enterprise component library warrants the full feature set.
**Impact:** Major implementation effort. Current skeleton covers ~10-15% of spec surface.

## Diagram v1 API Shape
**Decision:** Three sub-decisions:
- **A) Data shape:** Flat list parameters (`IReadOnlyList<DiagramShapeDescriptor>`, `IReadOnlyList<DiagramConnectionDescriptor>`), not declarative child tags.
- **B) Event naming:** `OnShapeClick` with `DiagramShapeClickEventArgs`, not `OnNodeClick`.
- **C) Model naming:** `DiagramShapeDescriptor` as primary type, not `DiagramNode`.
**Rationale:** User decision. Pragmatic v1 for single-developer library. Child tags deferred to future.
**Impact:** Refactor existing prototype from ad-hoc structures to descriptor-based API. Implementation task dispatched.
