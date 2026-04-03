# Component Requirements: MariloDataSheet

## Primary Use Cases
1. Bulk-edit a list of domain records (e.g. positions, allocations, transactions) without opening per-row modals — Excel feel inside a governed Blazor app.
2. Paste a range of values copied from Excel directly into the grid (TSV clipboard format).
3. Queue multiple cell edits, review dirty state, then commit all at once via Save All.
4. Validate each cell against per-column rules before allowing Save All to proceed.

## User Interactions
- Click a cell to activate it; type immediately to enter edit mode
- Tab / Shift+Tab to move right/left; Enter to commit and move down
- Escape cancels the current cell edit and restores the previous value
- Arrow keys navigate non-editing cells; F2 enters edit on focused cell
- Ctrl+S triggers Save All; Ctrl+Z undoes last cell change
- Ctrl+C copies selected range as TSV; Ctrl+V pastes TSV from clipboard
- Delete clears selected cells; Ctrl+D fills selected range down from anchor
- Multi-select rows via checkbox column; bulk-delete and bulk-reset via action bar

## Column Types Required
- Text, Number, Date, Select (enum/option list), Checkbox, Computed (display-only)

## Dirty State
- Every changed cell is individually tracked (row key + field name)
- Dirty cells show a left-accent indicator using the warning token
- The toolbar shows "N unsaved changes" badge
- Save All collects only dirty rows and fires OnSaveAll callback
- Reset reverts all dirty rows to their original values

## Validation
- Per-column Validate func returning null (pass) or error message
- Required flag: fires a "required" error if cell is empty on Save All
- Validation runs on commit of each cell AND on Save All
- Invalid cells show a red background and tooltip with the error message
- Save All is blocked until all validation errors are cleared

## Accessibility
- role="grid", role="row", role="gridcell"
- aria-readonly on computed cells; aria-invalid on invalid cells
- aria-live="polite" region for save success/failure announcements
- Full keyboard navigation; visible focus ring on active cell

## Performance
- Virtualized rows using Blazor's Virtualize component for datasets > 200 rows
- Dirty state tracked in a dictionary keyed by the consumer-supplied key selector

## Integration
- Inherits MariloComponentBase (CssProvider, CombineClasses, Class, Style, AdditionalAttributes)
- Column children register via CascadingValue pattern
- CSS classes via IMariloCssProvider (never hardcoded)
- JS interop via IJSRuntime — lazy-loaded ESM module
