# Gap Inventory: MariloEditor

> Imported: 2026-04-03
> Analysis mode: Reconstructed (code exists before gap analysis)
> Total gaps: ~12 (3 Critical, 5 High, 4 Medium)

---

## Component Inventory

| Attribute | Value |
|-----------|-------|
| **Source files** | `MariloEditor.razor` (670 lines) |
| **Code-behind partials** | None |
| **Public parameters** | 18 (Value, ValueChanged, ReadOnly, Disabled, EditMode, EditModeChanged, Tools, ToolbarTemplate, ChildContent, Placeholder, Height, Width, DebounceDelay, AriaLabelledBy, AriaDescribedBy, OnChange, OnSelectionChange, OnCommand) |
| **Tests** | Yes -- `EditorTests.cs` (94 lines, 7 test methods) |
| **Demos** | No demo pages found |
| **Spec** | `docs/component-specs/editor/overview.md` |

---

## Gap Summary

The spec describes a full WYSIWYG editor backed by ProseMirror with schema/plugin customization, table and image resizing, import/export, adaptive toolbar, programmatic command execution via ExecuteAsync(), validation support, large content handling, and security (HTML sanitization). The implementation has a solid foundation: contenteditable WYSIWYG, source and preview modes, toolbar with tools, debounced value binding, and basic HTML sanitization. Gaps focus on ProseMirror integration, advanced features, and missing demos.

### GAP-EDITOR-001: Missing ExecuteAsync() public method

**Area:** MariloEditor
**Severity:** Critical
**Theme:** missing-public-method
**Source:** editor/overview.md -- Editor Reference and Methods

**Target behavior:** `ExecuteAsync(HtmlCommandArgs)` method for programmatic command execution (insert HTML, apply formatting).
**Current behavior:** Internal `ExecuteToolAsync()` exists for toolbar buttons; no public ExecuteAsync API visible.
**Impact:** Consumers cannot programmatically insert content or execute formatting commands.
**Recommended direction:** Expose public ExecuteAsync method that delegates to JS interop.
**Status:** Open

---

### GAP-EDITOR-002: Missing Adaptive parameter

**Area:** MariloEditor
**Severity:** High
**Theme:** missing-parameter
**Source:** editor/overview.md -- Parameters table

**Target behavior:** `Adaptive` (bool) parameter makes toolbar responsive, hiding overflow items in a popup.
**Current behavior:** No Adaptive parameter implemented.
**Impact:** Toolbar overflows on narrow containers without graceful handling.
**Recommended direction:** Add Adaptive parameter with resize observer and overflow popup.
**Status:** Open

---

### GAP-EDITOR-003: Missing ProseMirror schema/plugin customization

**Area:** MariloEditor
**Severity:** Critical
**Theme:** missing-extensibility
**Source:** editor/overview.md -- ProseMirror Schema and Plugins section

**Target behavior:** Consumers can customize the ProseMirror schema and plugins for custom node/mark types.
**Current behavior:** No ProseMirror integration visible; editor uses contenteditable with execCommand-style approach.
**Impact:** Cannot extend editor with custom block types, marks, or plugins. Core architectural gap.
**Recommended direction:** Evaluate ProseMirror JS integration or document the alternative approach used.
**Status:** Open

---

### GAP-EDITOR-004: Missing table and image resize support

**Area:** MariloEditor
**Severity:** High
**Theme:** missing-feature
**Source:** editor/overview.md -- Resizing section

**Target behavior:** Tables (columns/rows) and images are resizable via drag handles in the content area.
**Current behavior:** No resize handle implementation visible in the 670-line source.
**Impact:** Tables and images cannot be visually resized by end users.
**Recommended direction:** Implement via JS interop resize observers on table/image elements.
**Status:** Open

---

### GAP-EDITOR-005: Missing import/export functionality

**Area:** MariloEditor
**Severity:** High
**Theme:** missing-feature
**Source:** editor/overview.md -- Next Steps references "Import and Export Data"

**Target behavior:** Import from and export to various formats (e.g., Markdown, DOCX).
**Current behavior:** Only raw HTML value binding.
**Impact:** Cannot round-trip content in non-HTML formats.
**Recommended direction:** Add ImportAsync/ExportAsync methods with format parameter.
**Status:** Open

---

### GAP-EDITOR-006: No demo pages

**Area:** MariloEditor
**Severity:** Critical
**Theme:** missing-demos
**Source:** samples/Marilo.Demo/Pages/Components/Editor/ (directory absent)

**Target behavior:** Demo pages showing WYSIWYG editing, source mode, custom tools, programmatic insertion.
**Current behavior:** No demo directory or pages exist.
**Impact:** No way to preview or validate editor functionality.
**Recommended direction:** Create demo pages for core editor scenarios.
**Status:** Open

---

### GAP-EDITOR-007: Missing validation integration

**Area:** MariloEditor
**Severity:** High
**Theme:** missing-validation
**Source:** editor/overview.md -- Validation section

**Target behavior:** Supports Data Annotation validation with DebounceDelay-timed updates.
**Current behavior:** DebounceDelay parameter exists but EditContext/validation integration unclear.
**Impact:** Cannot validate editor content within forms.
**Recommended direction:** Integrate with EditContext via InputBase or manual field notification.
**Status:** Open

---

### GAP-EDITOR-008: Missing security/sanitization documentation alignment

**Area:** MariloEditor
**Severity:** High
**Theme:** security
**Source:** editor/overview.md -- Security section (empty in spec)

**Target behavior:** Documented sanitization rules for pasted/imported HTML content.
**Current behavior:** `SanitizeHtml()` method exists in source but behavior undocumented; EditorPasteSettings registered.
**Impact:** Security posture unclear; consumers cannot evaluate XSS protection.
**Recommended direction:** Document sanitization rules; verify paste settings are configurable.
**Status:** Open

---

### GAP-EDITOR-009: Missing custom tool creation API

**Area:** MariloEditor
**Severity:** Medium
**Theme:** missing-extensibility
**Source:** editor/overview.md -- Next Steps references "Create Custom Tools"

**Target behavior:** API for defining custom editor tools beyond built-in set.
**Current behavior:** ToolbarTemplate allows custom rendering; EditorTool enum for built-in tools.
**Impact:** Partial coverage; ToolbarTemplate workaround exists but typed custom tool API missing.
**Recommended direction:** Add EditorCustomTool component or extend EditorTool.
**Status:** Open

---

### GAP-EDITOR-010: Missing edit mode documentation for Div mode

**Area:** MariloEditor
**Severity:** Medium
**Theme:** missing-feature
**Source:** editor/overview.md -- Next Steps references "Editor Edit Modes"

**Target behavior:** Multiple edit modes beyond Edit/Preview/Source (spec implies additional modes).
**Current behavior:** EditorEditMode enum has Edit, Preview, Source. Possibly complete.
**Impact:** Low if three modes cover the spec; needs verification.
**Recommended direction:** Verify enum completeness against full edit modes spec.
**Status:** Open

---

### GAP-EDITOR-011: Low test coverage relative to complexity

**Area:** MariloEditor
**Severity:** Medium
**Theme:** low-test-coverage
**Source:** EditorTests.cs (7 tests for 670 lines)

**Target behavior:** Tests covering modes, toolbar, value binding, sanitization, events.
**Current behavior:** 7 tests; coverage ratio ~1 test per 96 lines.
**Impact:** Regressions undetected in complex editing scenarios.
**Recommended direction:** Expand to cover edit modes, tool execution, debounce behavior.
**Status:** Open

---

### GAP-EDITOR-012: Missing large content handling

**Area:** MariloEditor
**Severity:** Medium
**Theme:** missing-feature
**Source:** editor/overview.md -- Large Content Support section (empty in spec)

**Target behavior:** Performance optimizations for editing large HTML documents.
**Current behavior:** No virtualization or lazy loading visible in source.
**Impact:** Performance may degrade with large content.
**Recommended direction:** Evaluate and document content size limits; add virtualization if needed.
**Status:** Open

---

## Severity Breakdown

| Severity | Count |
|----------|-------|
| Critical | 3 |
| High | 5 |
| Medium | 4 |
| Low | 0 |
| **Total** | **12** |
