# WebForms Server Controls to Blazor Mapping

Maps ASP.NET WebForms server controls to their Blazor/Telerik equivalents. Use during Stage 04 (UI conversion) when the source project contains WebForms pages.

## Data Controls

| WebForms Control | Blazor / Telerik Equivalent | Conversion Notes |
|-----------------|---------------------------|------------------|
| `<asp:GridView>` | `<TelerikGrid>` | Replace `DataSource`/`DataBind()` with `Data="@list"` parameter. Move `RowDataBound` logic to `GridColumn` templates. |
| `<asp:DetailsView>` | `<TelerikForm>` (read-only) or custom layout | Extract field bindings to `FormItem` components |
| `<asp:FormView>` | `<TelerikForm>` with `EditTemplate` | Map `ItemTemplate`/`EditItemTemplate` to Telerik form modes |
| `<asp:Repeater>` | `@foreach` loop or `<TelerikListView>` | Replace `ItemTemplate` with Razor loop body |
| `<asp:DataList>` | `<TelerikListView>` | Template-based rendering |
| `<asp:ListView>` | `<TelerikListView>` or `<TelerikGrid>` | Choose based on layout needs |
| `<asp:ObjectDataSource>` | Injected service (`@inject IService`) | Replace declarative binding with DI |
| `<asp:SqlDataSource>` | DAB REST endpoint or EF Core query | Never use inline SQL in Blazor |
| `<asp:EntityDataSource>` | EF Core `DbContext` via DI | Replace with repository/service pattern |

## Input Controls

| WebForms Control | Blazor / Telerik Equivalent | Conversion Notes |
|-----------------|---------------------------|------------------|
| `<asp:TextBox>` | `<TelerikTextBox>` with `@bind-Value` | Map `TextChanged` to `ValueChanged` |
| `<asp:TextBox TextMode="MultiLine">` | `<TelerikTextArea>` | Map `Rows`/`Columns` to CSS sizing |
| `<asp:TextBox TextMode="Password">` | `<TelerikTextBox Password="true">` | |
| `<asp:DropDownList>` | `<TelerikDropDownList>` | Map `SelectedIndexChanged` to `ValueChanged`. Replace `Items.Add()` with bound `Data` parameter. |
| `<asp:ListBox>` | `<TelerikMultiSelect>` or `<TelerikListBox>` | Depends on single vs. multi-select |
| `<asp:CheckBox>` | `<TelerikCheckBox>` with `@bind-Value` | Map `CheckedChanged` to `ValueChanged` |
| `<asp:CheckBoxList>` | `@foreach` with `<TelerikCheckBox>` | Bind to `List<bool>` or model flags |
| `<asp:RadioButton>` | `<TelerikRadioGroup>` | Group radios into single component |
| `<asp:RadioButtonList>` | `<TelerikRadioGroup>` | Map `SelectedValue` to `@bind-Value` |
| `<asp:HiddenField>` | Private field in `@code { }` | No hidden field needed in Blazor |
| `<asp:FileUpload>` | `<TelerikUpload>` | Async upload with progress |
| `<asp:Calendar>` | `<TelerikDatePicker>` | Single date selection |

## Button and Command Controls

| WebForms Control | Blazor / Telerik Equivalent | Conversion Notes |
|-----------------|---------------------------|------------------|
| `<asp:Button>` | `<TelerikButton>` with `OnClick` | Map `Click` event to async method |
| `<asp:LinkButton>` | `<TelerikButton>` or `<a>` with `@onclick` | Replace postback with Blazor event |
| `<asp:ImageButton>` | `<TelerikButton Icon="...">` | Use Telerik icon or image |
| `<asp:CommandField>` (in GridView) | `<GridCommandButton>` in `<GridCommandColumn>` | Built-in edit/delete/save/cancel |

## Navigation Controls

| WebForms Control | Blazor / Telerik Equivalent | Conversion Notes |
|-----------------|---------------------------|------------------|
| `<asp:Menu>` | `<TelerikMenu>` | Map `MenuItem` hierarchy to `MenuItems` data |
| `<asp:TreeView>` | `<TelerikTreeView>` | Map `TreeNode` to `TreeViewItem` |
| `<asp:SiteMapPath>` | `<TelerikBreadcrumb>` | Map sitemap to breadcrumb items |
| `<asp:HyperLink>` | `<NavLink>` or `<a href>` | Use `NavigationManager` for programmatic nav |
| `<asp:Wizard>` | `<TelerikWizard>` | Map `WizardStep` to `WizardStep` components |
| `Response.Redirect()` | `NavigationManager.NavigateTo()` | Inject `NavigationManager` |

## Layout Controls

| WebForms Control | Blazor / Telerik Equivalent | Conversion Notes |
|-----------------|---------------------------|------------------|
| `<asp:Content>` / `<asp:ContentPlaceHolder>` | `@Body` in layout, `@page` in component | Master/content becomes layout/page |
| `<asp:Panel>` | `<div>` or conditional `@if` block | Map `Visible` to `@if (show)` |
| `<asp:PlaceHolder>` | `RenderFragment` or conditional block | Dynamic content insertion |
| `<asp:MultiView>` / `<asp:View>` | `<TelerikTabStrip>` or `@switch` | Map `ActiveViewIndex` to active tab |
| `<asp:UpdatePanel>` | Not needed (Blazor re-renders automatically) | Remove; Blazor handles partial updates |
| `<asp:UpdateProgress>` | `<TelerikLoader>` | Show during async operations |
| `<asp:ScriptManager>` | Not needed | Remove entirely; Blazor manages its own JS |

## Validation Controls

| WebForms Control | Blazor / Telerik Equivalent | Conversion Notes |
|-----------------|---------------------------|------------------|
| `<asp:RequiredFieldValidator>` | `[Required]` DataAnnotation + `<TelerikValidationMessage>` | On model property |
| `<asp:RangeValidator>` | `[Range(min, max)]` DataAnnotation | |
| `<asp:RegularExpressionValidator>` | `[RegularExpression("pattern")]` | |
| `<asp:CompareValidator>` | `[Compare("OtherProperty")]` | |
| `<asp:CustomValidator>` | Custom `ValidationAttribute` or `EditContext.OnValidationRequested` | |
| `<asp:ValidationSummary>` | `<TelerikValidationSummary>` | Top-of-form error list |

## Postback and Event Model

| WebForms Pattern | Blazor Equivalent | Notes |
|-----------------|-------------------|-------|
| `AutoPostBack="true"` | `ValueChanged` or `@bind` | Blazor re-renders automatically on state change |
| `IsPostBack` check | `OnInitializedAsync` vs. `OnParametersSetAsync` | Use lifecycle methods |
| `__doPostBack()` JS call | `@onclick` or `EventCallback` | No postback mechanism in Blazor |
| `Page_Load` | `OnInitializedAsync()` | Component lifecycle |
| `Page_PreRender` | `OnAfterRenderAsync(firstRender)` | After-render lifecycle |
| `ViewState["key"]` storage | Private field in `@code { }` | Component state is in-memory |
| `EnableViewState="false"` | Not applicable | No ViewState in Blazor |
