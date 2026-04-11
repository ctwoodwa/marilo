# MariloGantt -- Stage 01 Spec Review: Gap List

**Audit date:** 2026-04-10 (re-verified 2026-04-11: source inventory, spec paths, and shared-primitive scan unchanged)
**Source files:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs` (main component)
- `src/Marilo.Components/DataDisplay/MariloGantt.razor` (markup)
- `src/Marilo.Components/DataDisplay/GanttColumn.razor`
- `src/Marilo.Components/DataDisplay/GanttCommandColumn.razor`
- `src/Marilo.Components/DataDisplay/GanttViewBase.cs`
- `src/Marilo.Components/DataDisplay/GanttDayView.cs`
- `src/Marilo.Components/DataDisplay/GanttWeekView.cs`
- `src/Marilo.Components/DataDisplay/GanttMonthView.cs`
- `src/Marilo.Components/DataDisplay/GanttYearView.cs`
- `src/Marilo.Components/DataDisplay/GanttState.cs` (enums + state classes)
- `src/Marilo.Components/DataDisplay/GanttEventArgs.cs`
- `src/Marilo.Components/DataDisplay/GanttDependencyEventArgs.cs`
- `src/Marilo.Components/DataDisplay/GanttDependency.cs`
- `src/Marilo.Components/DataDisplay/GanttDependencyType.cs`
- `src/Marilo.Components/DataDisplay/GanttView.cs`
- `src/Marilo.Components/DataDisplay/MariloGanttDependencies.razor`

**Spec files:** `docs/component-specs/gantt/` (44 markdown files)

**Source parameter count:** 60 (MariloGantt: 30, GanttColumn: 13, GanttCommandColumn: 3, GanttViewBase: 6, MariloGanttDependencies: 6, GanttState: ~10 properties)
**Spec parameter count:** 78 (including parameters referenced in spec but not yet implemented)
**Total gaps:** 33

| Gap type | Count |
|----------|-------|
| Undocumented | 5 |
| Spec-ahead | 22 |
| Mismatch | 6 |

---

## Source Inventory

### MariloGantt<TItem> Parameters

| # | Parameter | Type | Default | Documented in Spec? |
|---|-----------|------|---------|---------------------|
| 1 | `Data` | `IEnumerable<TItem>` | `Empty<TItem>()` | Yes |
| 2 | `IdField` | `string` | `"Id"` | Yes |
| 3 | `ParentIdField` | `string` | `"ParentId"` | Yes |
| 4 | `TitleField` | `string` | `"Title"` | Yes |
| 5 | `StartField` | `string` | `"Start"` | Yes |
| 6 | `EndField` | `string` | `"End"` | Yes |
| 7 | `PercentCompleteField` | `string` | `"PercentComplete"` | Yes |
| 8 | `DependsOnField` | `string` | `"DependsOn"` | Yes |
| 9 | `ItemsField` | `string?` | `null` | Yes |
| 10 | `HasChildrenField` | `string?` | `null` | Yes |
| 11 | `Sortable` | `bool` | `true` | Yes |
| 12 | `FilterMode` | `GanttFilterMode` | `FilterRow` | Yes |
| 13 | `FilterPopupMode` | `GanttFilterPopupMode` | `Drawer` | No (UNDOCUMENTED) |
| 14 | `FilterRowDebounceDelay` | `int` | `0` | No (UNDOCUMENTED) |
| 15 | `Width` | `string?` | `null` | Yes |
| 16 | `Height` | `string?` | `null` | Yes |
| 17 | `TaskListWidth` | `int` | `250` | Yes |
| 18 | `DayWidth` | `int` | `30` | Yes (legacy) |
| 19 | `RowHeight` | `int` | `36` | Yes |
| 20 | `TaskTemplate` | `RenderFragment<TItem>?` | `null` | Yes |
| 21 | `TooltipTemplate` | `RenderFragment<TItem>?` | `null` | Yes |
| 22 | `OnTaskClick` | `EventCallback<TItem>` | - | Yes |
| 23 | `OnTaskEdit` | `EventCallback<GanttEditEventArgs>` | - | Yes |
| 24 | `TreeListEditMode` | `GanttTreeListEditMode` | `Inline` | Yes |
| 25 | `NewRowPosition` | `GanttNewRowPosition` | `Top` | Yes |
| 26 | `OnCreate` | `EventCallback<GanttCreateEventArgs>` | - | Yes |
| 27 | `OnUpdate` | `EventCallback<GanttUpdateEventArgs>` | - | Yes |
| 28 | `OnDelete` | `EventCallback<GanttDeleteEventArgs>` | - | Yes |
| 29 | `OnExpand` | `EventCallback<GanttExpandEventArgs>` | - | Yes |
| 30 | `OnCollapse` | `EventCallback<GanttCollapseEventArgs>` | - | Yes |
| 31 | `ShowColumnChooser` | `bool` | `false` | No (UNDOCUMENTED) |
| 32 | `GanttToolBarTemplate` | `RenderFragment?` | `null` | Yes |
| 33 | `GanttColumns` | `RenderFragment?` | `null` | Yes |
| 34 | `GanttViews` | `RenderFragment?` | `null` | Yes |
| 35 | `GanttDependenciesSlot` | `RenderFragment?` | `null` | No (UNDOCUMENTED) |
| 36 | `View` | `GanttView` | `Week` | Yes |
| 37 | `ViewChanged` | `EventCallback<GanttView>` | - | Yes |
| 38 | `OnStateInit` | `EventCallback<GanttStateEventArgs<TItem>>` | - | Yes |
| 39 | `OnStateChanged` | `EventCallback<GanttStateEventArgs<TItem>>` | - | Yes |

### GanttColumn<TItem> Parameters

| # | Parameter | Type | Default | Documented in Spec? |
|---|-----------|------|---------|---------------------|
| 1 | `Field` | `string` | `""` | Yes |
| 2 | `Title` | `string?` | `null` | Yes |
| 3 | `Width` | `string?` | `null` | Yes |
| 4 | `Expandable` | `bool` | `false` | Yes |
| 5 | `Visible` | `bool` | `true` | Yes |
| 6 | `DisplayFormat` | `string?` | `null` | Yes |
| 7 | `TextAlign` | `string?` | `null` | Yes (MISMATCH: spec says enum) |
| 8 | `HeaderClass` | `string?` | `null` | Yes |
| 9 | `Editable` | `bool` | `true` | Yes |
| 10 | `EditorType` | `GanttEditorType?` | `null` | Yes (MISMATCH: enum name differs) |
| 11 | `Sortable` | `bool` | `true` | Yes |
| 12 | `Filterable` | `bool` | `true` | Yes |
| 13 | `FilterType` | `GanttColumnFilterType` | `Text` | No (UNDOCUMENTED) |
| 14 | `Template` | `RenderFragment<TItem>?` | `null` | Yes |
| 15 | `HeaderTemplate` | `RenderFragment?` | `null` | Yes |

### GanttCommandColumn<TItem> Parameters

| # | Parameter | Type | Default | Documented in Spec? |
|---|-----------|------|---------|---------------------|
| 1 | `Width` | `string?` | `null` | Yes |
| 2 | `Title` | `string?` | `null` | Yes |
| 3 | `ShowAddButton` | `bool` | `false` | Partial (spec uses child GanttCommandButton) |

### GanttViewBase Parameters (inherited by Day/Week/Month/YearView)

| # | Parameter | Type | Default | Documented in Spec? |
|---|-----------|------|---------|---------------------|
| 1 | `SlotWidth` | `double` | varies by view | Yes (MISMATCH: spec defaults differ) |
| 2 | `RangeStart` | `DateTime?` | `null` | Yes |
| 3 | `RangeEnd` | `DateTime?` | `null` | Yes |
| 4 | `MainHeaderTemplate` | `RenderFragment<DateTime>?` | `null` | Yes (via spec names) |
| 5 | `SubHeaderTemplate` | `RenderFragment<DateTime>?` | `null` | Yes (via spec names) |
| 6 | `MainHeaderDateFormat` | `string?` | `null` | Yes (via spec names) |
| 7 | `SubHeaderDateFormat` | `string?` | `null` | Yes (via spec names) |

### MariloGanttDependencies<TItem> Parameters

| # | Parameter | Type | Default | Documented in Spec? |
|---|-----------|------|---------|---------------------|
| 1 | `Data` | `IEnumerable<GanttDependency>` | `Empty<>()` | Yes |
| 2 | `IdField` | `string` | `"Id"` | Yes |
| 3 | `PredecessorIdField` | `string` | `"PredecessorId"` | Yes |
| 4 | `SuccessorIdField` | `string` | `"SuccessorId"` | Yes |
| 5 | `TypeField` | `string` | `"Type"` | Yes |
| 6 | `OnCreate` | `EventCallback<GanttDependencyCreateEventArgs>` | - | Yes |
| 7 | `OnDelete` | `EventCallback<GanttDependencyDeleteEventArgs>` | - | Yes |

---

## Gap Records

### A. Undocumented (in source, not in spec)

#### GAP-GANTT-U01: FilterPopupMode parameter
| | |
|---|---|
| **ID** | GAP-GANTT-U01 |
| **Type** | Undocumented |
| **Component** | MariloGantt |
| **Parameter** | `FilterPopupMode` |
| **Source type** | `GanttFilterPopupMode` (enum: Drawer, Popup) |
| **Source default** | `Drawer` |
| **Spec** | Not mentioned anywhere |
| **Priority** | P2 |
| **Notes** | Controls whether checkbox filters display in a drawer or anchor-positioned popup. Should be documented in the filtering overview or checkboxlist spec. |

#### GAP-GANTT-U02: FilterRowDebounceDelay parameter
| | |
|---|---|
| **ID** | GAP-GANTT-U02 |
| **Type** | Undocumented |
| **Component** | MariloGantt |
| **Parameter** | `FilterRowDebounceDelay` |
| **Source type** | `int` |
| **Source default** | `0` |
| **Spec** | Not mentioned anywhere |
| **Priority** | P2 |
| **Notes** | Debounce delay in milliseconds for filter row inputs. 0 = immediate. Should be documented in filter-row spec. |

#### GAP-GANTT-U03: ShowColumnChooser parameter
| | |
|---|---|
| **ID** | GAP-GANTT-U03 |
| **Type** | Undocumented |
| **Component** | MariloGantt |
| **Parameter** | `ShowColumnChooser` |
| **Source type** | `bool` |
| **Source default** | `false` |
| **Spec** | Not mentioned on MariloGantt (spec mentions column chooser only through `ShowColumnMenu` and `GanttColumnMenuSettings`) |
| **Priority** | P2 |
| **Notes** | Source has a standalone `ShowColumnChooser` toolbar toggle. Spec expects it via `ShowColumnMenu`. These are different features. |

#### GAP-GANTT-U04: GanttDependenciesSlot parameter
| | |
|---|---|
| **ID** | GAP-GANTT-U04 |
| **Type** | Undocumented |
| **Component** | MariloGantt |
| **Parameter** | `GanttDependenciesSlot` |
| **Source type** | `RenderFragment?` |
| **Source default** | `null` |
| **Spec** | Spec uses `<GanttDependenciesSettings>` wrapper tag instead |
| **Priority** | P2 |
| **Notes** | Source uses `GanttDependenciesSlot` as the child content slot. Spec expects `<GanttDependenciesSettings>` wrapper. |

#### GAP-GANTT-U05: GanttColumn.FilterType parameter
| | |
|---|---|
| **ID** | GAP-GANTT-U05 |
| **Type** | Undocumented |
| **Component** | GanttColumn |
| **Parameter** | `FilterType` |
| **Source type** | `GanttColumnFilterType` (enum: Text, CheckboxList) |
| **Source default** | `Text` |
| **Spec** | Not documented; spec uses `FilterMenuType` on the column instead |
| **Priority** | P2 |
| **Notes** | Source uses `FilterType` per-column. Spec refers to `FilterMenuType` per-column. Name mismatch. |

---

### B. Spec-Ahead (in spec, not in source)

#### GAP-GANTT-S01: SortMode parameter
| | |
|---|---|
| **ID** | GAP-GANTT-S01 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt |
| **Parameter** | `SortMode` |
| **Spec type** | `SortMode` enum (Single, Multiple) |
| **Spec location** | `gantt-tree/data-binding/hierarchical-data.md` code example |
| **Priority** | P3 |
| **Notes** | Spec code example uses `SortMode="@SortMode.Multiple"`. Source only supports single-column sort. Sorting spec notes multi-column is planned. |

#### GAP-GANTT-S02: ShowColumnMenu parameter (MariloGantt level)
| | |
|---|---|
| **ID** | GAP-GANTT-S02 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt |
| **Parameter** | `ShowColumnMenu` |
| **Spec type** | `bool` |
| **Spec location** | `gantt-tree/columns/menu.md` |
| **Priority** | P3 |
| **Notes** | Not implemented in source. Source has `ShowColumnChooser` but not `ShowColumnMenu`. |

#### GAP-GANTT-S03: ShowColumnMenu parameter (GanttColumn level)
| | |
|---|---|
| **ID** | GAP-GANTT-S03 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `ShowColumnMenu` |
| **Spec type** | `bool` |
| **Spec location** | `gantt-tree/columns/menu.md` |
| **Priority** | P3 |
| **Notes** | Per-column menu toggle. Not implemented in source. |

#### GAP-GANTT-S04: ColumnReorderable parameter
| | |
|---|---|
| **ID** | GAP-GANTT-S04 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt |
| **Parameter** | `ColumnReorderable` |
| **Spec type** | `bool` |
| **Spec location** | `gantt-tree/columns/reorder.md`, `gantt-tree/columns/menu.md` |
| **Priority** | P3 |
| **Notes** | Not implemented. Column reorder is spec-only. |

#### GAP-GANTT-S05: Reorderable parameter (GanttColumn level)
| | |
|---|---|
| **ID** | GAP-GANTT-S05 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `Reorderable` |
| **Spec type** | `bool` |
| **Spec location** | `gantt-tree/columns/reorder.md` |
| **Priority** | P3 |
| **Notes** | Per-column reorder toggle. Not implemented. |

#### GAP-GANTT-S06: Resizable parameter (MariloGantt level / ColumnResizable)
| | |
|---|---|
| **ID** | GAP-GANTT-S06 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt |
| **Parameter** | `Resizable` / `ColumnResizable` |
| **Spec type** | `bool` |
| **Spec location** | `gantt-tree/columns/resize.md`, `gantt-tree/columns/menu.md` |
| **Priority** | P3 |
| **Notes** | Column resizing not implemented. |

#### GAP-GANTT-S07: Resizable parameter (GanttColumn level)
| | |
|---|---|
| **ID** | GAP-GANTT-S07 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `Resizable` |
| **Spec type** | `bool` |
| **Spec location** | `gantt-tree/columns/resize.md` |
| **Priority** | P3 |
| **Notes** | Per-column resize toggle. Not implemented. |

#### GAP-GANTT-S08: MinResizableWidth parameter
| | |
|---|---|
| **ID** | GAP-GANTT-S08 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `MinResizableWidth` |
| **Spec type** | `int` (default 30) |
| **Spec location** | `gantt-tree/columns/bound.md` |
| **Priority** | P3 |
| **Notes** | Minimum column width during resize. Not implemented. |

#### GAP-GANTT-S09: MaxResizableWidth parameter
| | |
|---|---|
| **ID** | GAP-GANTT-S09 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `MaxResizableWidth` |
| **Spec type** | `int` |
| **Spec location** | `gantt-tree/columns/bound.md` |
| **Priority** | P3 |
| **Notes** | Maximum column width during resize. Not implemented. |

#### GAP-GANTT-S10: FilterMenuType parameter (MariloGantt level)
| | |
|---|---|
| **ID** | GAP-GANTT-S10 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt |
| **Parameter** | `FilterMenuType` |
| **Spec type** | `FilterMenuType` enum (Menu, CheckBoxList) |
| **Spec location** | `gantt-tree/filter/checkboxlist.md`, `dependencies/databind.md` |
| **Priority** | P2 |
| **Notes** | Spec uses `FilterMenuType` at the Gantt level to switch between menu and checkbox-list filter modes. Source uses `GanttColumn.FilterType` per-column instead. |

#### GAP-GANTT-S11: FilterMenuType parameter (GanttColumn level)
| | |
|---|---|
| **ID** | GAP-GANTT-S11 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `FilterMenuType` |
| **Spec type** | `FilterMenuType` enum |
| **Spec location** | `gantt-tree/filter/checkboxlist.md` |
| **Priority** | P2 |
| **Notes** | Spec allows per-column override of `FilterMenuType`. Source has `FilterType` (name mismatch -- see U05). |

#### GAP-GANTT-S12: FilterEditorType parameter
| | |
|---|---|
| **ID** | GAP-GANTT-S12 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `FilterEditorType` |
| **Spec type** | `GanttTreeListFilterEditorType` enum (DatePicker, DateTimePicker) |
| **Spec location** | `gantt-tree/filter/overview.md` |
| **Priority** | P3 |
| **Notes** | Customize filter editor type for date columns. Not implemented. |

#### GAP-GANTT-S13: TaskListWidthChanged event
| | |
|---|---|
| **ID** | GAP-GANTT-S13 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt |
| **Parameter** | `TaskListWidthChanged` |
| **Spec type** | Event (type not specified) |
| **Spec location** | `events.md` |
| **Priority** | P3 |
| **Notes** | Fires when user resizes the TreeList pane. Not implemented (splitter resize not implemented). |

#### GAP-GANTT-S14: GanttCommandButton component
| | |
|---|---|
| **ID** | GAP-GANTT-S14 |
| **Type** | Spec-ahead |
| **Component** | GanttCommandButton |
| **Spec location** | `gantt-tree/columns/command.md`, `events.md` |
| **Priority** | P2 |
| **Notes** | The spec references `GanttCommandButton` with `Command`, `ShowInEdit`, `ChildContent`, `OnClick`, `Icon`, etc. Source has `GanttCommandColumn` with simple `ShowAddButton` bool instead. No `GanttCommandButton` component exists. |

#### GAP-GANTT-S15: GanttSettings / GanttColumnMenuSettings components
| | |
|---|---|
| **ID** | GAP-GANTT-S15 |
| **Type** | Spec-ahead |
| **Component** | GanttSettings, GanttColumnMenuSettings, GanttColumnMenuChooser |
| **Spec location** | `gantt-tree/templates/column-chooser.md` |
| **Priority** | P3 |
| **Notes** | Spec defines nested settings components for column menu customization. Not implemented. |

#### GAP-GANTT-S16: GanttDependenciesSettings wrapper tag
| | |
|---|---|
| **ID** | GAP-GANTT-S16 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt / MariloGanttDependencies |
| **Spec location** | `dependencies/overview.md`, `dependencies/databind.md` |
| **Priority** | P2 |
| **Notes** | Spec expects dependencies declared inside `<GanttDependenciesSettings>` wrapper. Source uses `GanttDependenciesSlot` RenderFragment directly. Structural mismatch. |

#### GAP-GANTT-S17: RangeSnapTo parameter
| | |
|---|---|
| **ID** | GAP-GANTT-S17 |
| **Type** | Spec-ahead |
| **Component** | MariloGantt |
| **Parameter** | `RangeSnapTo` |
| **Spec type** | `GanttRangeSnapTo` enum (MajorSlot, MinorSlot) |
| **Spec location** | `timeline/zooming.md` |
| **Priority** | P3 |
| **Notes** | Controls timeline range snapping behavior. Not implemented. |

#### GAP-GANTT-S18: EditorTemplate on GanttColumn
| | |
|---|---|
| **ID** | GAP-GANTT-S18 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `EditorTemplate` |
| **Spec type** | `RenderFragment<TItem>?` |
| **Spec location** | `gantt-tree/templates/editor.md` |
| **Priority** | P2 |
| **Notes** | Custom editor template for columns. Not implemented. Source uses auto-detected input types. |

#### GAP-GANTT-S19: FormTemplate / GanttPopupEditFormSettings
| | |
|---|---|
| **ID** | GAP-GANTT-S19 |
| **Type** | Spec-ahead |
| **Component** | GanttPopupEditFormSettings |
| **Parameter** | `FormTemplate` |
| **Spec type** | `RenderFragment<object>?` |
| **Spec location** | `gantt-tree/templates/popup-form-template.md` |
| **Priority** | P3 |
| **Notes** | Custom popup edit form template. Not implemented; source uses generated popup form. |

#### GAP-GANTT-S20: GanttColumn.Id parameter
| | |
|---|---|
| **ID** | GAP-GANTT-S20 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `Id` |
| **Spec type** | `string` |
| **Spec location** | `gantt-tree/templates/column-chooser.md` |
| **Priority** | P3 |
| **Notes** | Column identifier for column chooser template `context.Columns`. Not implemented. |

#### GAP-GANTT-S21: FieldType parameter on GanttColumn
| | |
|---|---|
| **ID** | GAP-GANTT-S21 |
| **Type** | Spec-ahead |
| **Component** | GanttColumn |
| **Parameter** | `FieldType` |
| **Spec type** | `Type` |
| **Spec location** | `gantt-tree/data-binding/overview.md` notes section |
| **Priority** | P3 |
| **Notes** | Required when data is `IEnumerable<object>`. Not implemented. |

#### GAP-GANTT-S22: View-specific header date format/template parameters
| | |
|---|---|
| **ID** | GAP-GANTT-S22 |
| **Type** | Spec-ahead |
| **Component** | GanttDayView, GanttWeekView, GanttMonthView, GanttYearView |
| **Parameters** | `DayHeaderDateFormat`, `TimeHeaderDateFormat`, `WeekHeaderDateFormat`, `MonthHeaderDateFormat`, `YearHeaderDateFormat`, `DayHeaderTemplate`, `TimeHeaderTemplate`, `WeekHeaderTemplate`, `MonthHeaderTemplate`, `YearHeaderTemplate` |
| **Spec location** | `timeline/date-format.md`, `timeline/templates/dateheader.md` |
| **Priority** | P2 |
| **Notes** | Spec defines view-specific named template and format parameters (e.g., `DayHeaderTemplate` on GanttDayView). Source has generic `MainHeaderTemplate`/`SubHeaderTemplate`/`MainHeaderDateFormat`/`SubHeaderDateFormat` on GanttViewBase instead. The names differ. Source approach is more unified; spec uses view-specific names for documentation clarity. |

---

### C. Mismatch (both exist but differ)

#### GAP-GANTT-M01: GanttColumn.TextAlign type
| | |
|---|---|
| **ID** | GAP-GANTT-M01 |
| **Type** | Mismatch |
| **Component** | GanttColumn |
| **Parameter** | `TextAlign` |
| **Source type** | `string?` (accepts "left", "center", "right") |
| **Spec type** | `ColumnTextAlign` enum (Left, Center, Right) |
| **Priority** | P2 |
| **Notes** | Source uses raw string. Spec references `ColumnTextAlign` enum (used as `TextAlign="@ColumnTextAlign.Right"` in examples). Either create the enum or update the spec. |

#### GAP-GANTT-M02: GanttEditorType enum name
| | |
|---|---|
| **ID** | GAP-GANTT-M02 |
| **Type** | Mismatch |
| **Component** | GanttColumn |
| **Parameter** | `EditorType` |
| **Source enum** | `GanttEditorType` (TextBox, TextArea, CheckBox, DatePicker, NumericTextBox) |
| **Spec enum** | `GanttTreeListEditorType` (TextArea, TextBox, CheckBox, Switch, DatePicker, DateTimePicker, TimePicker) |
| **Priority** | P2 |
| **Notes** | Source enum is named `GanttEditorType`; spec says `GanttTreeListEditorType`. Source is missing Switch, DateTimePicker, TimePicker members. Source has NumericTextBox not in spec. |

#### GAP-GANTT-M03: GanttTreeListEditMode.None
| | |
|---|---|
| **ID** | GAP-GANTT-M03 |
| **Type** | Mismatch |
| **Component** | MariloGantt |
| **Parameter** | `TreeListEditMode` |
| **Source enum** | `GanttTreeListEditMode` (Inline, Incell, Popup) |
| **Spec enum** | includes `None` member in addition to Inline, Incell, Popup |
| **Priority** | P2 |
| **Notes** | Spec editing overview lists `None` as an option to disable editing. Source enum lacks `None`. |

#### GAP-GANTT-M04: View SlotWidth defaults
| | |
|---|---|
| **ID** | GAP-GANTT-M04 |
| **Type** | Mismatch |
| **Component** | GanttDayView, GanttWeekView, GanttMonthView, GanttYearView |
| **Parameter** | `SlotWidth` |
| **Source defaults** | Day=40, Week=100, Month=100, Year=30 |
| **Spec defaults** | Day=40, Week=40, Month=60, Year=80 |
| **Priority** | P1 |
| **Notes** | WeekView and MonthView source defaults (100, 100) don't match spec (40, 60). YearView source (30) doesn't match spec (80). This affects out-of-box rendering and any tutorial code. |

#### GAP-GANTT-M05: Dependency field name (spec PredecessorField vs source PredecessorIdField)
| | |
|---|---|
| **ID** | GAP-GANTT-M05 |
| **Type** | Mismatch |
| **Component** | MariloGanttDependencies |
| **Parameter** | `PredecessorIdField` / `SuccessorIdField` |
| **Source names** | `PredecessorIdField`, `SuccessorIdField` |
| **Spec names** | `PredecessorField`, `SuccessorField` (in dependencies features table in databind.md) |
| **Priority** | P2 |
| **Notes** | Spec feature table says `PredecessorField` and `SuccessorField`. Spec code example correctly uses `PredecessorIdField` and `SuccessorIdField`. The feature table is inconsistent with both the code example and source. |

#### GAP-GANTT-M06: GanttUpdateEventArgs.ParentItem
| | |
|---|---|
| **ID** | GAP-GANTT-M06 |
| **Type** | Mismatch |
| **Component** | GanttUpdateEventArgs |
| **Parameter** | `ParentItem` |
| **Source** | Not present on GanttUpdateEventArgs (only has `Item`) |
| **Spec** | Editing overview says `OnUpdate` args expose `ParentItem` |
| **Priority** | P2 |
| **Notes** | Spec says GanttUpdateEventArgs has `ParentItem`. Source only has `Item`. |

---

## Priority Summary

| Priority | Count | Description |
|----------|-------|-------------|
| **P1** | 1 | Blocking: SlotWidth defaults mismatch (M04) |
| **P2** | 14 | This phase: undocumented features (U01-U05), key mismatches (M01-M03, M05-M06), missing EditorTemplate (S18), command button gap (S14), dependency structure (S16), filter type naming (S10-S11), header template naming (S22) |
| **P3** | 18 | Next phase: column reorder/resize (S04-S09), column menu (S02-S03, S15), sort mode (S01), zoom (S17), popup form template (S19), column Id (S20), FieldType (S21), TaskListWidthChanged (S13), FilterEditorType (S12) |
