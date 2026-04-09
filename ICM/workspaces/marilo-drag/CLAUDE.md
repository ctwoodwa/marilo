# marilo-drag — JS Drag Interop Module

**Status:** IN PROGRESS — design complete, implementation pending

Shared TypeScript/JS interop module for drag operations across Marilo components.

## Consumers
- MariloGantt (timeline bar drag-move, resize, column reorder, column resize)
- MariloWindow (drag-move, resize — already has custom interop)
- MariloSplitter (pane resize — already has custom interop)
- MariloDataGrid (column reorder, column resize — already has custom interop)

## Goal
Consolidate drag interop into a single shared module with consistent API, 
replacing per-component ad-hoc implementations.
