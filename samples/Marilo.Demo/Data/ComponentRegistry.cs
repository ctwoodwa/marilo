namespace Marilo.Demo.Data;

public static class ComponentRegistry
{
    public record ComponentInfo(
        string Name,
        string Slug,
        string Category,
        string CategorySlug,
        string Description,
        string ApiPath,
        string[] SubPages
    );

    public record CategoryInfo(string Name, string Slug);

    private static readonly string[] FullSubPages = ["overview", "appearance", "events", "accessibility"];
    private static readonly string[] NoEventsSubPages = ["overview", "appearance", "accessibility"];
    private static readonly string[] MinimalSubPages = ["overview", "appearance"];

    public static readonly ComponentInfo[] All =
    [
        // Buttons
        new("Button", "Button", "Buttons", "buttons",
            "Buttons allow users to take actions with a single tap. They communicate actions that users can take and are typically placed in dialogs, forms, toolbars, and inline.",
            "/articles/components/button/overview.html", FullSubPages),
        new("ButtonGroup", "ButtonGroup", "Buttons", "buttons",
            "Button groups combine related buttons into a single visual unit, making it clear that the actions are related.",
            "/articles/components/buttongroup/overview.html", FullSubPages),
        new("Chip", "Chip", "Buttons", "buttons",
            "Chips represent complex entities in small blocks, such as a contact, tag, or filter selection.",
            "/articles/components/chip/overview.html", FullSubPages),
        new("Fab", "Fab", "Buttons", "buttons",
            "The Floating Action Button (FAB) represents the primary action on a screen.",
            "/articles/components/floatingactionbutton/overview.html", FullSubPages),
        new("ToggleButton", "ToggleButton", "Buttons", "buttons",
            "Toggle buttons allow users to switch a setting between two states.",
            "/articles/components/togglebutton/overview.html", FullSubPages),
        new("SegmentedControl", "SegmentedControl", "Buttons", "buttons",
            "Segmented controls let users select one option from a set of mutually exclusive choices.",
            "/articles/components/segmented-control/overview.html", FullSubPages),

        // Forms
        new("TextField", "TextField", "Forms", "forms",
            "The text field lets users enter and edit text. It supports prefix and suffix adornments for enhanced interactivity.",
            "/articles/components/textbox/overview.html", FullSubPages),
        new("TextArea", "TextArea", "Forms", "forms",
            "The text area allows users to enter multi-line text.",
            "/articles/components/textarea/overview.html", FullSubPages),
        new("Select", "Select", "Forms", "forms",
            "The select component lets users choose a single value from a dropdown list.",
            "/articles/components/dropdownlist/overview.html", FullSubPages),
        new("Checkbox", "Checkbox", "Forms", "forms",
            "Checkboxes let users select one or more items from a list, or toggle an option on or off.",
            "/articles/components/checkbox/overview.html", FullSubPages),
        new("Switch", "Switch", "Forms", "forms",
            "The switch toggles the state of a single setting on or off.",
            "/articles/components/switch/overview.html", FullSubPages),
        new("Slider", "Slider", "Forms", "forms",
            "The slider lets users select a value from a continuous range by moving a thumb along a track.",
            "/articles/components/slider/overview.html", FullSubPages),
        new("SearchBox", "SearchBox", "Forms", "forms",
            "The search box provides a text input optimized for search queries, with a built-in search icon and clear button.",
            "/articles/components/search-box/overview.html", FullSubPages),
        new("Rating", "Rating", "Forms", "forms",
            "The rating component lets users provide a star-based evaluation.",
            "/articles/components/rating/overview.html", FullSubPages),
        new("DatePicker", "DatePicker", "Forms", "forms",
            "The date picker lets users select a date from a calendar or by typing.",
            "/articles/components/datepicker/overview.html", FullSubPages),
        new("ColorPicker", "ColorPicker", "Forms", "forms",
            "The color picker lets users select a color value.",
            "/articles/components/colorpicker/overview.html", FullSubPages),
        new("ComboBox", "ComboBox", "Forms", "forms",
            "The combo box combines a text input with a dropdown list, allowing users to type to filter or select from options.",
            "/articles/components/combobox/overview.html", FullSubPages),
        new("DropDownList", "DropDownList", "Forms", "forms",
            "The dropdown list lets users select a single item from a predefined list with support for data binding and templates.",
            "/articles/components/dropdownlist/overview.html", FullSubPages),
        new("MultiSelect", "MultiSelect", "Forms", "forms",
            "The multi-select lets users choose multiple items from a dropdown list.",
            "/articles/components/multiselect/overview.html", FullSubPages),
        new("Autocomplete", "Autocomplete", "Forms", "forms",
            "The autocomplete provides suggestions as the user types, helping them quickly find and select values.",
            "/articles/components/autocomplete/overview.html", FullSubPages),
        new("Upload", "Upload", "Forms", "forms",
            "The upload component lets users select and upload files.",
            "/articles/components/upload/overview.html", FullSubPages),

        // Data Display
        new("Card", "Card", "Data Display", "data-display",
            "Cards contain content and actions about a single subject, grouping related information together.",
            "/articles/components/card/overview.html", NoEventsSubPages),
        new("Avatar", "Avatar", "Data Display", "data-display",
            "Avatars display images, icons, or initials representing people or other entities.",
            "/articles/components/avatar/overview.html", NoEventsSubPages),
        new("Badge", "Badge", "Data Display", "data-display",
            "Badges are small status descriptors for UI elements, showing counts or labels.",
            "/articles/components/badge/overview.html", NoEventsSubPages),
        new("List", "List", "Data Display", "data-display",
            "Lists present content in a structured, scannable format.",
            "/articles/components/listview/overview.html", NoEventsSubPages),
        new("DataGrid", "DataGrid", "Data Display", "data-display",
            "The data grid displays structured data in rows and columns with sorting, filtering, paging, and selection.",
            "/articles/components/grid/overview.html", FullSubPages),
        new("ListView", "ListView", "Data Display", "data-display",
            "The list view displays a collection of items with support for templates, selection, and virtualization.",
            "/articles/components/listview/overview.html", FullSubPages),
        new("Tooltip", "Tooltip", "Data Display", "data-display",
            "Tooltips display informative text when users hover over, focus on, or tap an element.",
            "/articles/components/tooltip/overview.html", NoEventsSubPages),
        new("Chart", "Chart", "Data Display", "data-display",
            "Charts visualize data using various series types including line, bar, area, and pie.",
            "/articles/components/chart/overview.html", FullSubPages),

        // Feedback
        new("Alert", "Alert", "Feedback", "feedback",
            "Alerts display brief, important messages to attract the user's attention without interrupting their task.",
            "/articles/components/notification/overview.html", NoEventsSubPages),
        new("ProgressBar", "ProgressBar", "Feedback", "feedback",
            "Progress bars indicate the progress of an operation, showing how much has been completed.",
            "/articles/components/progressbar/overview.html", NoEventsSubPages),
        new("Spinner", "Spinner", "Feedback", "feedback",
            "Spinners indicate an ongoing process, such as loading data.",
            "/articles/components/loader/overview.html", MinimalSubPages),
        new("Skeleton", "Skeleton", "Feedback", "feedback",
            "Skeletons provide a low-fidelity preview of content before it loads, reducing perceived wait time.",
            "/articles/components/skeleton/overview.html", MinimalSubPages),
        new("Dialog", "Dialog", "Feedback", "feedback",
            "Dialogs present content in a modal overlay that requires user attention or action.",
            "/articles/components/dialog/overview.html", FullSubPages),
        new("ConfirmDialog", "ConfirmDialog", "Feedback", "feedback",
            "Confirm dialogs ask users to verify an action before proceeding, especially for destructive operations.",
            "/articles/components/dialog/overview.html", FullSubPages),
        new("SnackbarHost", "SnackbarHost", "Feedback", "feedback",
            "The snackbar host manages and displays transient notification messages at the bottom of the screen.",
            "/articles/components/snackbar/overview.html", FullSubPages),

        // Layout
        new("Grid", "Grid", "Layout", "layout",
            "The grid system provides a 12-column responsive layout for organizing content.",
            "/articles/components/gridlayout/overview.html", NoEventsSubPages),
        new("Stack", "Stack", "Layout", "layout",
            "Stacks arrange child elements in a horizontal or vertical line with configurable spacing and alignment.",
            "/articles/components/stacklayout/overview.html", NoEventsSubPages),
        new("Divider", "Divider", "Layout", "layout",
            "Dividers separate content into clear groups.",
            "/articles/components/divider/overview.html", MinimalSubPages),
        new("Panel", "Panel", "Layout", "layout",
            "Panels provide a contained surface with elevation for grouping related content.",
            "/articles/components/panel/overview.html", NoEventsSubPages),
        new("Container", "Container", "Layout", "layout",
            "Containers center content and constrain its maximum width.",
            "/articles/components/container/overview.html", NoEventsSubPages),
        new("TabStrip", "TabStrip", "Layout", "layout",
            "The TabStrip organizes content into separate views where only one view is visible at a time.",
            "/articles/components/tabstrip/overview.html", FullSubPages),
        new("Accordion", "Accordion", "Layout", "layout",
            "Accordions display collapsible content panels for presenting information in a limited space.",
            "/articles/components/panelbar/overview.html", FullSubPages),
        new("Stepper", "Stepper", "Layout", "layout",
            "Steppers display progress through a sequence of logical and numbered steps.",
            "/articles/components/stepper/overview.html", FullSubPages),
        new("Drawer", "Drawer", "Layout", "layout",
            "Drawers provide a slide-out panel for supplementary content or navigation.",
            "/articles/components/drawer/overview.html", FullSubPages),

        // Navigation
        new("Breadcrumb", "Breadcrumb", "Navigation", "navigation",
            "Breadcrumbs show the current page's location within a navigational hierarchy.",
            "/articles/components/breadcrumb/overview.html", NoEventsSubPages),
        new("Pagination", "Pagination", "Navigation", "navigation",
            "Pagination lets users navigate through pages of content.",
            "/articles/components/pager/overview.html", FullSubPages),
        new("Toolbar", "Toolbar", "Navigation", "navigation",
            "Toolbars group related actions and controls in a horizontal bar.",
            "/articles/components/toolbar/overview.html", NoEventsSubPages),
        new("TreeView", "TreeView", "Navigation", "navigation",
            "Tree views display hierarchical data in an expandable tree structure.",
            "/articles/components/treeview/overview.html", FullSubPages),
        new("Menu", "Menu", "Navigation", "navigation",
            "Menus display a list of actions or options in a popup overlay.",
            "/articles/components/menu/overview.html", FullSubPages),

        // Overlays
        new("Window", "Window", "Overlays", "overlays",
            "Windows provide a draggable, resizable overlay for displaying content.",
            "/articles/components/window/overview.html", FullSubPages),

        // Editors
        new("Editor", "Editor", "Editors", "editors",
            "The editor provides a rich text editing experience with formatting tools.",
            "/articles/components/editor/overview.html", FullSubPages),
    ];

    private static readonly Dictionary<string, ComponentInfo> _bySlug =
        All.ToDictionary(c => c.Slug, StringComparer.OrdinalIgnoreCase);

    private static readonly CategoryInfo[] _categories =
    [
        new("Buttons", "buttons"),
        new("Forms", "forms"),
        new("Data Display", "data-display"),
        new("Feedback", "feedback"),
        new("Layout", "layout"),
        new("Navigation", "navigation"),
        new("Overlays", "overlays"),
        new("Editors", "editors"),
    ];

    public static ComponentInfo? GetBySlug(string slug) =>
        _bySlug.TryGetValue(slug, out var info) ? info : null;

    public static CategoryInfo[] GetCategories() => _categories;

    public static ComponentInfo[] GetByCategory(string categorySlug) =>
        All.Where(c => c.CategorySlug.Equals(categorySlug, StringComparison.OrdinalIgnoreCase)).ToArray();

    public static string FormatSubPageName(string subPage) => subPage switch
    {
        "overview" => "Overview",
        "appearance" => "Appearance",
        "events" => "Events",
        "accessibility" => "Accessibility",
        _ => subPage
    };
}
