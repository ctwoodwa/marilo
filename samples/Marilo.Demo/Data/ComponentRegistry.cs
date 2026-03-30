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
        new("Button", "button", "Buttons", "buttons",
            "Buttons allow users to take actions with a single tap. They communicate actions that users can take and are typically placed in dialogs, forms, toolbars, and inline.",
            "/articles/components/button/overview.html", FullSubPages),
        new("Button Group", "button-group", "Buttons", "buttons",
            "Button groups combine related buttons into a single visual unit, making it clear that the actions are related.",
            "/articles/components/buttongroup/overview.html", FullSubPages),
        new("Chip", "chip", "Buttons", "buttons",
            "Chips represent complex entities in small blocks, such as a contact, tag, or filter selection.",
            "/articles/components/chip/overview.html", FullSubPages),
        new("FAB", "fab", "Buttons", "buttons",
            "The Floating Action Button (FAB) represents the primary action on a screen.",
            "/articles/components/floatingactionbutton/overview.html", FullSubPages),
        new("Toggle Button", "toggle-button", "Buttons", "buttons",
            "Toggle buttons allow users to switch a setting between two states.",
            "/articles/components/togglebutton/overview.html", FullSubPages),
        new("Segmented Control", "segmented-control", "Buttons", "buttons",
            "Segmented controls let users select one option from a set of mutually exclusive choices.",
            "/articles/components/segmented-control/overview.html", FullSubPages),

        // Forms
        new("Text Field", "text-field", "Forms", "forms",
            "The text field lets users enter and edit text. It supports prefix and suffix adornments for enhanced interactivity.",
            "/articles/components/textbox/overview.html", FullSubPages),
        new("Text Area", "text-area", "Forms", "forms",
            "The text area allows users to enter multi-line text.",
            "/articles/components/textarea/overview.html", FullSubPages),
        new("Select", "select", "Forms", "forms",
            "The select component lets users choose a single value from a dropdown list.",
            "/articles/components/dropdownlist/overview.html", FullSubPages),
        new("Checkbox", "checkbox", "Forms", "forms",
            "Checkboxes let users select one or more items from a list, or toggle an option on or off.",
            "/articles/components/checkbox/overview.html", FullSubPages),
        new("Switch", "switch", "Forms", "forms",
            "The switch toggles the state of a single setting on or off.",
            "/articles/components/switch/overview.html", FullSubPages),
        new("Slider", "slider", "Forms", "forms",
            "The slider lets users select a value from a continuous range by moving a thumb along a track.",
            "/articles/components/slider/overview.html", FullSubPages),
        new("Search Box", "search-box", "Forms", "forms",
            "The search box provides a text input optimized for search queries, with a built-in search icon and clear button.",
            "/articles/components/search-box/overview.html", FullSubPages),
        new("Rating", "rating", "Forms", "forms",
            "The rating component lets users provide a star-based evaluation.",
            "/articles/components/rating/overview.html", FullSubPages),
        new("Date Picker", "date-picker", "Forms", "forms",
            "The date picker lets users select a date from a calendar or by typing.",
            "/articles/components/datepicker/overview.html", FullSubPages),
        new("Color Picker", "color-picker", "Forms", "forms",
            "The color picker lets users select a color value.",
            "/articles/components/colorpicker/overview.html", FullSubPages),

        // Data Display
        new("Card", "card", "Data Display", "data-display",
            "Cards contain content and actions about a single subject, grouping related information together.",
            "/articles/components/card/overview.html", NoEventsSubPages),
        new("Avatar", "avatar", "Data Display", "data-display",
            "Avatars display images, icons, or initials representing people or other entities.",
            "/articles/components/avatar/overview.html", NoEventsSubPages),
        new("Badge", "badge", "Data Display", "data-display",
            "Badges are small status descriptors for UI elements, showing counts or labels.",
            "/articles/components/badge/overview.html", NoEventsSubPages),
        new("List", "list", "Data Display", "data-display",
            "Lists present content in a structured, scannable format.",
            "/articles/components/listview/overview.html", NoEventsSubPages),
        new("Table", "table", "Data Display", "data-display",
            "Tables display structured data in rows and columns for comparison and analysis.",
            "/articles/components/grid/overview.html", NoEventsSubPages),
        new("Tooltip", "tooltip", "Data Display", "data-display",
            "Tooltips display informative text when users hover over, focus on, or tap an element.",
            "/articles/components/tooltip/overview.html", NoEventsSubPages),

        // Feedback
        new("Alert", "alert", "Feedback", "feedback",
            "Alerts display brief, important messages to attract the user's attention without interrupting their task.",
            "/articles/components/notification/overview.html", NoEventsSubPages),
        new("Progress Bar", "progress-bar", "Feedback", "feedback",
            "Progress bars indicate the progress of an operation, showing how much has been completed.",
            "/articles/components/progressbar/overview.html", NoEventsSubPages),
        new("Spinner", "spinner", "Feedback", "feedback",
            "Spinners indicate an ongoing process, such as loading data.",
            "/articles/components/loader/overview.html", MinimalSubPages),
        new("Skeleton", "skeleton", "Feedback", "feedback",
            "Skeletons provide a low-fidelity preview of content before it loads, reducing perceived wait time.",
            "/articles/components/skeleton/overview.html", MinimalSubPages),
        new("Dialog", "dialog", "Feedback", "feedback",
            "Dialogs present content in a modal overlay that requires user attention or action.",
            "/articles/components/dialog/overview.html", FullSubPages),
        new("Confirm Dialog", "confirm-dialog", "Feedback", "feedback",
            "Confirm dialogs ask users to verify an action before proceeding, especially for destructive operations.",
            "/articles/components/dialog/overview.html", FullSubPages),

        // Layout
        new("Grid", "grid", "Layout", "layout",
            "The grid system provides a 12-column responsive layout for organizing content.",
            "/articles/components/gridlayout/overview.html", NoEventsSubPages),
        new("Stack", "stack", "Layout", "layout",
            "Stacks arrange child elements in a horizontal or vertical line with configurable spacing and alignment.",
            "/articles/components/stacklayout/overview.html", NoEventsSubPages),
        new("Divider", "divider", "Layout", "layout",
            "Dividers separate content into clear groups.",
            "/articles/components/divider/overview.html", MinimalSubPages),
        new("Panel", "panel", "Layout", "layout",
            "Panels provide a contained surface with elevation for grouping related content.",
            "/articles/components/panel/overview.html", NoEventsSubPages),
        new("Container", "container", "Layout", "layout",
            "Containers center content and constrain its maximum width.",
            "/articles/components/container/overview.html", NoEventsSubPages),
        new("TabStrip", "tabs", "Layout", "layout",
            "The TabStrip organizes content into separate views where only one view is visible at a time.",
            "/articles/components/tabstrip/overview.html", FullSubPages),
        new("Accordion", "accordion", "Layout", "layout",
            "Accordions display collapsible content panels for presenting information in a limited space.",
            "/articles/components/panelbar/overview.html", FullSubPages),
        new("Stepper", "stepper", "Layout", "layout",
            "Steppers display progress through a sequence of logical and numbered steps.",
            "/articles/components/stepper/overview.html", FullSubPages),
        new("Drawer", "drawer", "Layout", "layout",
            "Drawers provide a slide-out panel for supplementary content or navigation.",
            "/articles/components/drawer/overview.html", FullSubPages),

        // Navigation
        new("Breadcrumb", "breadcrumb", "Navigation", "navigation",
            "Breadcrumbs show the current page's location within a navigational hierarchy.",
            "/articles/components/breadcrumb/overview.html", NoEventsSubPages),
        new("Pagination", "pagination", "Navigation", "navigation",
            "Pagination lets users navigate through pages of content.",
            "/articles/components/pager/overview.html", FullSubPages),
        new("Toolbar", "toolbar", "Navigation", "navigation",
            "Toolbars group related actions and controls in a horizontal bar.",
            "/articles/components/toolbar/overview.html", NoEventsSubPages),
        new("Tree View", "tree-view", "Navigation", "navigation",
            "Tree views display hierarchical data in an expandable tree structure.",
            "/articles/components/treeview/overview.html", FullSubPages),
        new("Menu", "menu", "Navigation", "navigation",
            "Menus display a list of actions or options in a popup overlay.",
            "/articles/components/menu/overview.html", FullSubPages),
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
