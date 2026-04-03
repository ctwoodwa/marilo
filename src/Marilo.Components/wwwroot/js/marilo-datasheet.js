// ESM module for MariloDataSheet — clipboard, focus, scroll, keyboard.
// Loaded lazily via import() in MariloDataSheet.Interop.cs.

const handlers = new Map();

/**
 * Copies text to the system clipboard.
 * @param {string} text - TSV or plain text to copy.
 */
export function copyToClipboard(text) {
    return navigator.clipboard.writeText(text);
}

/**
 * Reads text from the system clipboard.
 * @returns {Promise<string>} Clipboard text content.
 */
export async function readClipboard() {
    return await navigator.clipboard.readText();
}

/**
 * Scrolls a row with the given key into view.
 * @param {string} gridId - The grid element ID.
 * @param {string} rowKey - The data-row-key attribute value.
 */
export function scrollToRow(gridId, rowKey) {
    const grid = document.getElementById(gridId);
    if (!grid) return;
    const row = grid.querySelector(`[data-row-key="${rowKey}"]`);
    if (row) {
        row.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }
}

/**
 * Focuses a specific cell input element.
 * @param {string} gridId - The grid element ID.
 * @param {string} rowKey - The data-row-key attribute value.
 * @param {string} field - The data-field attribute value.
 */
export function focusCell(gridId, rowKey, field) {
    const grid = document.getElementById(gridId);
    if (!grid) return;
    const row = grid.querySelector(`[data-row-key="${rowKey}"]`);
    if (!row) return;
    const cell = row.querySelector(`[data-field="${field}"]`);
    if (!cell) return;
    const input = cell.querySelector('input, select, textarea');
    if (input) {
        input.focus();
    } else {
        cell.focus();
    }
}

/**
 * Registers a keydown handler on the grid element that dispatches to .NET.
 * @param {string} gridId - The grid element ID.
 * @param {object} dotNetRef - .NET object reference for callbacks.
 */
export function registerKeydownHandler(gridId, dotNetRef) {
    const grid = document.getElementById(gridId);
    if (!grid) return;

    const handler = async (e) => {
        const key = e.key;
        const ctrl = e.ctrlKey || e.metaKey;
        const shift = e.shiftKey;

        // Prevent default for grid-handled shortcuts
        if (ctrl && (key === 's' || key === 'z' || key === 'c' || key === 'v' || key === 'd')) {
            e.preventDefault();
        }
        if (['Tab', 'Enter', 'Escape', 'F2', 'Delete',
             'ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight'].includes(key)) {
            e.preventDefault();
        }

        // Handle Ctrl+C in JS (needs clipboard API access)
        if (ctrl && key === 'c') {
            const activeCell = grid.querySelector('.mar-datasheet__cell--active');
            if (activeCell) {
                const text = activeCell.textContent?.trim() || '';
                await copyToClipboard(text);
            }
            return;
        }

        // Handle Ctrl+V in JS (needs clipboard API access)
        if (ctrl && key === 'v') {
            try {
                const clipText = await readClipboard();
                if (clipText) {
                    await dotNetRef.invokeMethodAsync('PasteFromClipboard', clipText);
                }
            } catch (err) {
                console.warn('Clipboard read failed:', err);
            }
            return;
        }

        // All other keys dispatched to .NET
        try {
            await dotNetRef.invokeMethodAsync('HandleKeyDown', key, ctrl, shift);
        } catch (err) {
            // Component may have been disposed
        }
    };

    grid.addEventListener('keydown', handler);
    grid.setAttribute('tabindex', '0');
    handlers.set(gridId, handler);
}

/**
 * Removes the keydown handler from the grid element.
 * @param {string} gridId - The grid element ID.
 */
export function unregisterKeydownHandler(gridId) {
    const grid = document.getElementById(gridId);
    const handler = handlers.get(gridId);
    if (grid && handler) {
        grid.removeEventListener('keydown', handler);
    }
    handlers.delete(gridId);
}
