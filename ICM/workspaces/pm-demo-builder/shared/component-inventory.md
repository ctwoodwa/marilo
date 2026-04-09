# Marilo Component Inventory

Existing components in `src/Marilo.Components/` available for use in PM demo pages. Checked against repo as of 2026-04-09.

## Form Inputs (`Forms/Inputs/`)

| Component | File | Notes |
|---|---|---|
| MariloTextBox | `MariloTextBox.razor` | Text input with Value/ValueChanged |
| MariloTextArea | `MariloTextArea.razor` | Multi-line text |
| MariloSelect | `MariloSelect.razor` | Dropdown select |
| MariloCheckbox | `MariloCheckbox.razor` | Checkbox with Value/ValueChanged |
| MariloSwitch | `MariloSwitch.razor` | Toggle switch |
| MariloSlider | `MariloSlider.razor` | Range slider |
| MariloRangeSlider | `MariloRangeSlider.razor` | Dual-handle range |
| MariloSignature | `MariloSignature.razor` | Signature capture |
| MariloFileManager | `MariloFileManager.razor` | File management |

## Form Containers (`Forms/Containers/`)

| Component | Purpose |
|---|---|
| MariloForm | EditContext wrapper |
| MariloField | Input + label + validation wrapper |
| MariloLabel | Standalone label |
| MariloValidation | Validation host |
| MariloValidationMessage | Per-field validation display |
| MariloValidationSummary | Summary validation display |
| MariloValidationTooltip | Tooltip validation |

## Buttons (`Buttons/`)

| Component | Purpose |
|---|---|
| MariloButton | Standard button |
| MariloIconButton | Icon-only button |
| MariloButtonGroup | Button group container |
| MariloChip | Tag/chip element |
| MariloChipSet | Chip multi-select container |
| MariloSegmentedControl | Segmented toggle (use for theme/density pickers) |
| MariloSplitButton | Split button with dropdown |
| MariloToggleButton | Toggle button |
| MariloFab | Floating action button |

## Data Display (`DataDisplay/`)

| Component | Purpose |
|---|---|
| MariloCard / CardHeader / CardBody / CardActions | Card layout |
| MariloAvatar | User avatar |
| MariloBadge | Badge indicator |
| MariloList / MariloListItem | List display |
| MariloTimeline / MariloTimelineItem | Activity timeline |
| MariloTooltip | Tooltip |
| MariloPopover | Popover content |
| MariloImage | Image display |
| MariloCarousel | Image carousel |
| MariloGantt | Gantt chart |
| MariloScheduler | Calendar scheduler |
| MariloLinearGauge | Gauge display |
| MariloQRCode | QR code generator |
| MariloHighlighter | Text highlighter |
| MariloTypography | Typography component |

## Data Grid (`DataGrid/`)

| Component | Purpose |
|---|---|
| MariloGridToolbar | Grid toolbar |
| MariloGridCommandButton | Grid command button |
| (MariloDataGrid) | Full data grid — check gap analysis status |

## Layout (`Layout/`)

| Component | Purpose |
|---|---|
| MariloAccordion / AccordionItem | Collapsible sections |
| MariloDrawer | Slide-over drawer |
| MariloPanel | Content panel |
| MariloTabStrip / TabStripTab | Tab navigation |
| MariloStepper / MariloStep | Step wizard |
| MariloStack | Flex stack |
| MariloContainer / MariloRow / MariloColumn | Grid layout |
| MariloGridLayout / GridLayoutItem | CSS grid |
| MariloDivider | Divider line |
| MariloTileLayout | Tile grid |
| MariloAppBar | Top app bar |
| MariloAnimationContainer | Animation wrapper |

## Feedback (`Feedback/`)

| Component | Purpose |
|---|---|
| MariloAlert / MariloAlertStrip | Alert messages |
| MariloDialog / MariloConfirmDialog | Modal dialogs |
| MariloToast / MariloDataToast | Toast notifications |
| MariloSnackbar / MariloSnackbarHost | Snackbar notifications (already mounted) |
| MariloCallout | Callout messages |
| MariloDataBanner | Data banner |
| MariloProgressBar / MariloProgressCircle | Progress indicators |
| MariloSkeleton | Loading skeleton |
| MariloSpinner | Loading spinner |

## Navigation (`Navigation/`)

| Component | Purpose |
|---|---|
| MariloNavBar / NavMenu / NavItem | Navigation |
| MariloToolbar / ToolbarButton / ToolbarGroup / ToolbarSeparator / ToolbarToggleButton | Toolbar |
| MariloBreadcrumb / BreadcrumbItem | Breadcrumb navigation |
| MariloMenuItem / MariloMenuDivider | Menu items |
| MariloPagination | Pagination |
| MariloTimeRangeSelector | Time range picker (verify fit for quiet-hours) |
| MariloEnvironmentBadge | Environment indicator |

## Shell (`Marilo.Components.Shell/AppShell/`)

| Component | Purpose |
|---|---|
| MariloAppShell | Full app shell with sidebar |
| MariloAppShellNavGroup | Sidebar nav group |
| MariloAppShellNavLink | Sidebar nav link |
| MariloAppShellSlideOver | Slide-over panel |
| MariloUserMenu | User menu popup |
| MariloNotificationBell | Notification bell popup |

## Known Gaps (not yet created)

| Need | Notes |
|---|---|
| MariloKeyRecorder | For keyboard shortcut recording — build when Shortcuts page is in scope |
| DynamicForm | Schema-driven form renderer — build as PM demo component, not upstream |
