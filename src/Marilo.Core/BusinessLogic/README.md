# Marilo.Core.BusinessLogic — Building Blocks

This folder contains seven reusable, **component-agnostic** building blocks
that power all Marilo complex components: `GanttChart`, `AllocationScheduler`,
`DataGrid`, `DataSheet`, and `Wizard`.

> **No Blazor references.** Everything here is pure .NET and fully
> unit-testable without spinning up a Blazor host.

---

## Building Blocks

| Block | File(s) | Purpose |
|---|---|---|
| `PropertyInfo<T>` | `PropertyInfo.cs` | Compile-time property token — name, CLR type, default, initial access |
| `FieldManager` | `FieldManager.cs` | Per-instance value store, dirty tracking, snapshot |
| `BusinessObjectBase<T>` | `BusinessObjectBase.cs` | Composes all blocks; exposes the 8 accessor methods, `IsDirty`, `BrokenRules`, `CanUndo/Redo` |
| `BusinessRuleEngine` | `BusinessRuleEngine.cs` | Registers `IBusinessRule` instances to property tokens; runs them on `SetProperty` |
| `AuthorizationEngine` | `Authorization/AuthorizationEngine.cs` | Most-restrictive-wins `IAuthorizationRule` composition |
| `UndoStack` | `UndoStack.cs` | Bounded snapshot stack — `Push/Undo/Redo/Discard` |
| Rule primitives | `Rules/*.cs` | `LambdaRule`, `CrossFieldLambdaRule`, `RequiredWhenRule` — no custom class needed |

---

## Folder Layout

```
Marilo.Core/
└── BusinessLogic/
    ├── README.md
    ├── PropertyInfo.cs
    ├── FieldManager.cs
    ├── BusinessObjectBase.cs
    ├── BusinessRuleEngine.cs
    ├── UndoStack.cs
    ├── Authorization/
    │   ├── IAuthorizationRule.cs
    │   └── AuthorizationEngine.cs
    ├── Rules/
    │   ├── IBusinessRule.cs
    │   ├── BrokenRule.cs
    │   ├── LambdaRule.cs
    │   ├── CrossFieldLambdaRule.cs
    │   └── RequiredWhenRule.cs
    └── Enums/
        └── BusinessLogicEnums.cs
```

---

## Relationship to `Marilo.Core.Base`

`Base/` holds Blazor component infrastructure (`MariloComponentBase`,
`CssClassBuilder`, `StyleBuilder`). `BusinessLogic/` is parallel to it —
no Blazor dependency flows in either direction.

A component **inherits** `MariloComponentBase` for rendering and **owns** a
business object that extends `BusinessObjectBase<T>` as a field. Never inherit
both.

```
MariloComponentBase  ←  Blazor component lifecycle
BusinessObjectBase<T> ← Domain rules, dirty tracking, undo
```

---

## Property Pipeline (SetProperty)

```
SetProperty<T>(property, value)
  │
  ├─ 1. AuthorizationEngine.CanWrite?  ──No──► UnauthorizedAccessException
  │
  ├─ 2. UndoStack.Push(Fields.GetSnapshot())
  │
  ├─ 3. FieldManager.Write(property, value)
  │        └─ Value unchanged? ──Yes──► pop snapshot, return false
  │
  ├─ 4. OnPropertyChanged(property.Name)  ← INotifyPropertyChanged
  │
  └─ 5. return true
```

`BusinessRuleEngine.CheckRules()` is **lazy** — called on demand by the
component (e.g., before save, on blur) rather than on every keystroke.

---

## Scenario Planning Enums

`BusinessLogicEnums.cs` includes `ScenarioStatus` and `AllocationSetType`
for components that support scenario planning (see `AllocationScheduler`):

| Enum | Values |
|---|---|
| `ScenarioStatus` | Draft → Shared → Approved → Promoted / Rejected |
| `AllocationSetType` | Baseline, Scenario |
| `AccessMode` | None, ReadOnly, ReadWrite |
| `AuthorizationAction` | Read, Write |
