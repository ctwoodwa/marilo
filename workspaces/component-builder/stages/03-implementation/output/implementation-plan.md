# Implementation Plan: MariloDataSheet

## Files to Create

### Core (Marilo.Core)
- [x] src/Marilo.Core/Enums/DataSheetColumnType.cs
- [x] src/Marilo.Core/Enums/CellState.cs
- [x] src/Marilo.Core/Models/DataSheet/DataSheetSaveArgs.cs
- [x] src/Marilo.Core/Models/DataSheet/DataSheetRowChangedArgs.cs
- [x] src/Marilo.Core/Models/DataSheet/DataSheetValidateArgs.cs
- [x] src/Marilo.Core/Models/DataSheet/DataSheetValidationError.cs
- [x] src/Marilo.Core/Models/DataSheet/DataSheetSelectOption.cs
- [x] src/Marilo.Core/Models/DataSheet/DataSheetCellContext.cs
- [x] src/Marilo.Core/Helpers/GridReflectionHelper.cs

### Contracts & Providers
- [x] src/Marilo.Core/Contracts/IMariloCssProvider.cs (add 7 methods)
- [x] src/Marilo.Providers.FluentUI/FluentUICssProvider.cs (implement 7 methods)
- [x] src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs (implement 7 methods)

### Component (Marilo.Components)
- [x] src/Marilo.Components/DataGrid/MariloDataSheet.razor
- [x] src/Marilo.Components/DataGrid/MariloDataSheet.razor.cs
- [x] src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs
- [x] src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs
- [x] src/Marilo.Components/DataGrid/MariloDataSheet.Interop.cs
- [x] src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs
- [x] src/Marilo.Components/DataGrid/MariloDataSheetColumn.razor

### JS Interop
- [x] src/Marilo.Components/wwwroot/js/marilo-datasheet.js

### Demo & Docs
- [x] samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor
- [x] docs/components/MariloDataSheet.md

### Tests
- [x] tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs
