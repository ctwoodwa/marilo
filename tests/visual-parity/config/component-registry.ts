/**
 * Component registry for visual parity tests.
 *
 * Maps CDW component names to their demo routes, root CSS selectors,
 * and reference strategy. This is the single source of truth for the
 * Playwright harness — keep it aligned with CDW 03-visual-parity
 * capture matrices manually.
 *
 * When adding a new component:
 * 1. Add an entry here with the demo route and best-known selector.
 * 2. Create a spec file under specs/<component>.spec.ts.
 * 3. Confirm the CDW workspace has 03-visual-parity stage files.
 */

export interface ComponentEntry {
  /** Lowercase slug used in snapshot naming (e.g., "datagrid") */
  slug: string;
  /** Demo page route relative to base URL (e.g., "/components/DataGrid/overview") */
  route: string;
  /** CSS selector for the component's outermost container on the demo page */
  rootSelector: string;
  /** Additional selectors for specific sub-regions */
  selectors?: Record<string, string>;
  /** Telerik parity or internal Marilo baseline */
  referenceStrategy: 'telerik' | 'internal';
}

export const COMPONENTS: Record<string, ComponentEntry> = {
  datagrid: {
    slug: 'datagrid',
    route: '/components/DataGrid/overview',
    rootSelector: '.mar-datagrid',
    selectors: {
      header: '.mar-datagrid-header',
      row: '.mar-datagrid-row',
      pager: '.mar-datagrid-pager',
      filterRow: '.mar-datagrid-filter-row',
      filterCell: '.mar-datagrid-filter-cell',
      toolbar: '.mar-datagrid-toolbar',
      groupHeader: '.mar-datagrid-group-header',
    },
    referenceStrategy: 'telerik',
  },

  treeview: {
    slug: 'treeview',
    route: '/components/TreeView/overview',
    rootSelector: '.mar-treeview',
    selectors: {
      item: '.mar-treeview-item',
      // TODO: Verify exact CSS classes for expanded/selected/checkbox states
    },
    referenceStrategy: 'telerik',
  },

  scheduler: {
    slug: 'scheduler',
    route: '/components/scheduler/overview',
    rootSelector: '.mar-scheduler',
    selectors: {
      // TODO: Verify exact CSS classes once Scheduler SCSS is more mature
    },
    referenceStrategy: 'telerik',
  },

  'allocation-scheduler': {
    slug: 'allocation-scheduler',
    route: '/components/AllocationScheduler/overview',
    rootSelector: '.mar-allocation-scheduler',
    referenceStrategy: 'telerik',
  },

  chart: {
    slug: 'chart',
    route: '/components/Chart/overview',
    rootSelector: '.mar-chart',
    referenceStrategy: 'telerik',
  },

  gantt: {
    slug: 'gantt',
    route: '/components/Gantt/overview',
    rootSelector: '.mar-gantt',
    referenceStrategy: 'telerik',
  },

  pivotgrid: {
    slug: 'pivotgrid',
    route: '/components/PivotGrid/overview',
    rootSelector: '.mar-pivotgrid',
    referenceStrategy: 'telerik',
  },

  treelist: {
    slug: 'treelist',
    route: '/components/TreeList/overview',
    rootSelector: '.mar-treelist',
    referenceStrategy: 'telerik',
  },

  editor: {
    slug: 'editor',
    route: '/components/Editor/overview',
    rootSelector: '.mar-editor',
    referenceStrategy: 'telerik',
  },

  filemanager: {
    slug: 'filemanager',
    route: '/components/FileManager/overview',
    rootSelector: '.mar-filemanager',
    referenceStrategy: 'telerik',
  },

  splitter: {
    slug: 'splitter',
    route: '/components/Splitter/overview',
    rootSelector: '.mar-splitter',
    referenceStrategy: 'telerik',
  },

  map: {
    slug: 'map',
    route: '/components/Map/overview',
    rootSelector: '.mar-map',
    referenceStrategy: 'telerik',
  },

  wizard: {
    slug: 'wizard',
    route: '/components/Wizard/overview',
    rootSelector: '.mar-wizard',
    referenceStrategy: 'telerik',
  },

  datasheet: {
    slug: 'datasheet',
    route: '/components/DataSheet/overview',
    rootSelector: '.mar-datasheet',
    referenceStrategy: 'internal',
  },

  diagram: {
    slug: 'diagram',
    route: '/components/Diagram/overview',
    rootSelector: '.mar-diagram',
    referenceStrategy: 'internal',
  },

  dockmanager: {
    slug: 'dockmanager',
    route: '/components/DockManager/overview',
    rootSelector: '.mar-dockmanager',
    referenceStrategy: 'internal',
  },

  'resizable-container': {
    slug: 'resizable-container',
    route: '/components/ResizableContainer/overview',
    rootSelector: '.mar-resizable-container',
    referenceStrategy: 'internal',
  },
};
