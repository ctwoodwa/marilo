# MariloPopup — Component Specification

## Overview
Lightweight anchor-positioned popup for filter menus, column choosers, and popup edit forms.
Intermediate between MariloPopover (tooltip-like) and MariloDialog (full-screen modal).

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| IsOpen | bool | false | Controls visibility |
| IsOpenChanged | EventCallback<bool> | - | Two-way binding callback |
| AnchorId | string? | null | ID of the anchor element for positioning |
| Placement | PopupPlacement | Bottom | Preferred position relative to anchor |
| Offset | int | 4 | Pixel offset from anchor |
| ChildContent | RenderFragment | - | Popup content |
| Class | string? | null | Additional CSS classes (via MariloComponentBase) |
| OnOutsideClick | EventCallback | - | Fires when clicking outside the popup |
| FocusTrap | bool | false | Whether to trap focus inside |
| CloseOnEscape | bool | true | Close when Escape is pressed |

## PopupPlacement Enum
Top, Bottom, Left, Right, Auto

## Behavior
- Opens when IsOpen = true
- Closes on Escape key (when CloseOnEscape = true)
- Closes on outside click (fires OnOutsideClick, then sets IsOpen = false)
- Focus management: moves focus into popup on open, returns on close
- Anchor positioning: stub uses absolute CSS (no JS scroll tracking)
- Full anchor tracking (Floating UI) deferred to Pass 4

## Accessibility
- role="dialog" with aria-modal when FocusTrap is true
- role="listbox" or no role when FocusTrap is false
- Focus returned to trigger element on close

## CSS Classes
- `mar-popup` — root container
- `mar-popup--open` — when visible
- `mar-popup--{placement}` — position modifier
- Provider integration: IPopupClass on IMariloProvider (deferred)

@[template](/_contentTemplates/common/parameters-table-styles.md#table-layout)
## Relationship to Existing Components
- MariloPopover: tooltip-like, uses show/hide methods. MariloPopup is parameter-driven.
- MariloDrawer: sidebar panel. MariloPopup is anchor-relative.
- MariloDialog: full-screen modal. MariloPopup is lightweight inline.
