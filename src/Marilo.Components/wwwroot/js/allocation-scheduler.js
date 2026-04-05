/**
 * MariloAllocationScheduler JS Interop
 * Handles drag-fill, cell selection, and keyboard traversal interactions
 * that require DOM-level event handling beyond Blazor's event system.
 */
export const AllocationSchedulerInterop = {

    /**
     * Initialize drag-fill behavior on the scheduler grid.
     * @param {HTMLElement} gridElement - The root .mar-allocation-scheduler element
     * @param {DotNetObjectReference} dotNetRef - Reference to the Blazor component
     */
    initDragFill: function (gridElement, dotNetRef) {
        if (!gridElement) return;

        let isDragging = false;
        let startCell = null;
        let dragCells = [];

        const getCellKey = (cell) => {
            return {
                resourceKey: cell.dataset.resourceKey,
                bucketStart: cell.dataset.bucketStart
            };
        };

        const handleMouseDown = (e) => {
            const cell = e.target.closest('[role="gridcell"][data-resource-key]');
            if (!cell || cell.getAttribute('aria-disabled') === 'true') return;
            if (cell.getAttribute('aria-readonly') === 'true') return;

            isDragging = true;
            startCell = cell;
            dragCells = [cell];
            cell.classList.add('mar-allocation-scheduler__cell--drag-target');
            e.preventDefault();
        };

        const handleMouseMove = (e) => {
            if (!isDragging) return;
            const cell = e.target.closest('[role="gridcell"][data-resource-key]');
            if (!cell || dragCells.includes(cell)) return;
            if (cell.getAttribute('aria-disabled') === 'true') return;

            dragCells.push(cell);
            cell.classList.add('mar-allocation-scheduler__cell--drag-target');
        };

        const handleMouseUp = async (e) => {
            if (!isDragging) return;
            isDragging = false;

            const cellKeys = dragCells.map(getCellKey);
            dragCells.forEach(c => c.classList.remove('mar-allocation-scheduler__cell--drag-target'));
            dragCells = [];

            if (cellKeys.length > 1) {
                await dotNetRef.invokeMethodAsync('OnDragFillCompleted', JSON.stringify(cellKeys));
            }
        };

        gridElement.addEventListener('mousedown', handleMouseDown);
        gridElement.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);

        // Store cleanup reference
        gridElement._allocationSchedulerCleanup = () => {
            gridElement.removeEventListener('mousedown', handleMouseDown);
            gridElement.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('mouseup', handleMouseUp);
        };
    },

    /**
     * Initialize keyboard navigation for the scheduler grid.
     * @param {HTMLElement} gridElement - The root .mar-allocation-scheduler element
     * @param {DotNetObjectReference} dotNetRef - Reference to the Blazor component
     */
    initKeyboardNav: function (gridElement, dotNetRef) {
        if (!gridElement) return;

        const handleKeyDown = async (e) => {
            const cell = e.target.closest('[role="gridcell"]');
            if (!cell) return;

            const allCells = Array.from(gridElement.querySelectorAll('[role="gridcell"][tabindex="0"]'));
            const currentIndex = allCells.indexOf(cell);
            if (currentIndex === -1) return;

            let nextIndex = currentIndex;

            switch (e.key) {
                case 'ArrowRight':
                case 'Tab':
                    if (!e.shiftKey) {
                        nextIndex = Math.min(currentIndex + 1, allCells.length - 1);
                        e.preventDefault();
                    }
                    break;
                case 'ArrowLeft':
                    nextIndex = Math.max(currentIndex - 1, 0);
                    e.preventDefault();
                    break;
                case 'ArrowDown':
                case 'Enter':
                    // Move to same column in next row (approximate by row length)
                    nextIndex = Math.min(currentIndex + getColumnsPerRow(gridElement), allCells.length - 1);
                    e.preventDefault();
                    break;
                case 'ArrowUp':
                    nextIndex = Math.max(currentIndex - getColumnsPerRow(gridElement), 0);
                    e.preventDefault();
                    break;
                case 'Escape':
                    cell.blur();
                    await dotNetRef.invokeMethodAsync('OnEscapePressed');
                    e.preventDefault();
                    break;
                case 'Delete':
                    await dotNetRef.invokeMethodAsync('OnDeletePressed', getCellKey(cell));
                    e.preventDefault();
                    break;
                default:
                    return;
            }

            if (nextIndex !== currentIndex && allCells[nextIndex]) {
                allCells[nextIndex].focus();
                const key = getCellKey(allCells[nextIndex]);
                if (key.resourceKey) {
                    await dotNetRef.invokeMethodAsync('OnCellFocused', JSON.stringify(key));
                }
            }
        };

        const getCellKey = (cell) => ({
            resourceKey: cell.dataset.resourceKey,
            bucketStart: cell.dataset.bucketStart
        });

        gridElement.addEventListener('keydown', handleKeyDown);

        const existingCleanup = gridElement._allocationSchedulerCleanup;
        gridElement._allocationSchedulerCleanup = () => {
            existingCleanup?.();
            gridElement.removeEventListener('keydown', handleKeyDown);
        };
    },

    /**
     * Initialize splitter drag-to-resize behavior.
     * The splitter position is always derived from column widths — the JS sends
     * the desired total left-pane width to .NET, which maps it onto columns.
     * The left pane width is set via inline style to match the column sum exactly.
     * @param {HTMLElement} gridElement - The root .mar-allocation-scheduler element
     * @param {DotNetObjectReference} dotNetRef - Reference to the Blazor component
     */
    initSplitter: function (gridElement, dotNetRef) {
        if (!gridElement) return;

        const splitter = gridElement.querySelector('[role="separator"]');
        if (!splitter) return;

        const resourcePanel = gridElement.querySelector('.mar-allocation-scheduler__resource-panel');
        const grid = gridElement.querySelector('.mar-allocation-scheduler__grid');
        if (!resourcePanel || !grid) return;

        let isDragging = false;
        let startX = 0;
        let startWidth = 0;

        const handleMouseDown = (e) => {
            const sep = e.target.closest('[role="separator"]');
            if (sep !== splitter) return;
            // If splitter is locked (all columns non-resizable), do nothing
            if (splitter.dataset.locked === 'true') return;

            isDragging = true;
            startX = e.clientX;
            startWidth = resourcePanel.getBoundingClientRect().width;
            splitter.classList.add('mar-allocation-scheduler__splitter--dragging');
            document.body.style.cursor = 'col-resize';
            document.body.style.userSelect = 'none';
            e.preventDefault();
        };

        const handleMouseMove = (e) => {
            if (!isDragging) return;
            const delta = e.clientX - startX;
            const newWidth = startWidth + delta;
            const containerWidth = grid.getBoundingClientRect().width;
            const splitterWidth = splitter.getBoundingClientRect().width || 8;
            const minLeft = parseFloat(gridElement.dataset.minLeftPane) || 40;
            const minRight = parseFloat(gridElement.dataset.minRightPane) || 300;
            const allowCollapse = gridElement.dataset.allowCollapse === 'true';

            if (allowCollapse && newWidth < minLeft * 0.5) {
                resourcePanel.style.width = '0px';
                return;
            }

            if (allowCollapse && newWidth > containerWidth - splitterWidth - minRight * 0.5) {
                resourcePanel.style.width = (containerWidth - splitterWidth) + 'px';
                return;
            }

            // Clamp and apply visual preview — .NET will map this to column widths on drag end
            const clamped = Math.max(minLeft, Math.min(newWidth, containerWidth - splitterWidth - minRight));
            resourcePanel.style.width = clamped + 'px';
        };

        const handleMouseUp = async (e) => {
            if (!isDragging) return;
            isDragging = false;
            splitter.classList.remove('mar-allocation-scheduler__splitter--dragging');
            document.body.style.cursor = '';
            document.body.style.userSelect = '';

            const containerWidth = grid.getBoundingClientRect().width;
            const splitterWidth = splitter.getBoundingClientRect().width || 8;
            const currentWidth = resourcePanel.getBoundingClientRect().width;
            const minLeft = parseFloat(gridElement.dataset.minLeftPane) || 40;
            const minRight = parseFloat(gridElement.dataset.minRightPane) || 300;
            const allowCollapse = gridElement.dataset.allowCollapse === 'true';

            if (allowCollapse && currentWidth < minLeft * 0.5) {
                await dotNetRef.invokeMethodAsync('OnSplitterDragEnd', 0);
                return;
            }

            if (allowCollapse && currentWidth > containerWidth - splitterWidth - minRight * 0.5) {
                await dotNetRef.invokeMethodAsync('OnSplitterCollapseRight');
                return;
            }

            // Send the target width to .NET — it maps the delta onto the last resizable column
            const clamped = Math.max(minLeft, Math.min(currentWidth, containerWidth - splitterWidth - minRight));
            await dotNetRef.invokeMethodAsync('OnSplitterDragEnd', clamped);
        };

        splitter.addEventListener('mousedown', handleMouseDown);
        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);

        const existingCleanup = gridElement._allocationSchedulerCleanup;
        gridElement._allocationSchedulerCleanup = () => {
            existingCleanup?.();
            splitter.removeEventListener('mousedown', handleMouseDown);
            document.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('mouseup', handleMouseUp);
        };
    },

    /**
     * Synchronize vertical scrolling between resource panel and timeline panel.
     * @param {HTMLElement} gridElement - The root .mar-allocation-scheduler element
     */
    initScrollSync: function (gridElement) {
        if (!gridElement) return;

        const resourcePanel = gridElement.querySelector('.mar-allocation-scheduler__resource-panel');
        const timelinePanel = gridElement.querySelector('.mar-allocation-scheduler__timeline-panel');
        if (!resourcePanel || !timelinePanel) return;

        let isSyncing = false;

        const syncFromTimeline = () => {
            if (isSyncing) return;
            isSyncing = true;
            resourcePanel.scrollTop = timelinePanel.scrollTop;
            isSyncing = false;
        };

        const syncFromResource = () => {
            if (isSyncing) return;
            isSyncing = true;
            timelinePanel.scrollTop = resourcePanel.scrollTop;
            isSyncing = false;
        };

        timelinePanel.addEventListener('scroll', syncFromTimeline);
        resourcePanel.addEventListener('scroll', syncFromResource);

        const existingCleanup = gridElement._allocationSchedulerCleanup;
        gridElement._allocationSchedulerCleanup = () => {
            existingCleanup?.();
            timelinePanel.removeEventListener('scroll', syncFromTimeline);
            resourcePanel.removeEventListener('scroll', syncFromResource);
        };
    },

    /**
     * Dispose all event listeners for the scheduler grid.
     * @param {HTMLElement} gridElement - The root .mar-allocation-scheduler element
     */
    dispose: function (gridElement) {
        gridElement?._allocationSchedulerCleanup?.();
    }
};

function getColumnsPerRow(gridElement) {
    const rows = gridElement.querySelectorAll('[role="row"]');
    if (rows.length < 2) return 1;
    return rows[1].querySelectorAll('[role="gridcell"][tabindex="0"]').length || 1;
}
