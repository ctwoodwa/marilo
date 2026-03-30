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
            "/api/Marilo.Components.Buttons.MariloButton.html", FullSubPages),
        new("Button Group", "button-group", "Buttons", "buttons",
            "Button groups combine related buttons into a single visual unit, making it clear that the actions are related.",
            "/api/Marilo.Components.Buttons.MariloButtonGroup.html", FullSubPages),
        new("Chip", "chip", "Buttons", "buttons",
            "Chips represent complex entities in small blocks, such as a contact, tag, or filter selection.",
            "/api/Marilo.Components.Buttons.MariloChip.html", FullSubPages),
        new("FAB", "fab", "Buttons", "buttons",
            "The Floating Action Button (FAB) represents the primary action on a screen.",
            "/api/Marilo.Components.Buttons.MariloFab.html", FullSubPages),
        new("Toggle Button", "toggle-button", "Buttons", "buttons",
            "Toggle buttons allow users to switch a setting between two states.",
            "/api/Marilo.Components.Buttons.MariloToggleButton.html", FullSubPages),
        new("Segmented Control", "segmented-control", "Buttons", "buttons",
            "Segmented controls let users select one option from a set of mutually exclusive choices.",
            "/api/Marilo.Components.Buttons.MariloSegmentedControl.html", FullSubPages),

        // Forms
        new("Text Field", "text-field", "Forms", "forms",
            "The text field lets users enter and edit text. It supports prefix and suffix adornments for enhanced interactivity.",
            "/api/Marilo.Components.Forms.Inputs.MariloTextField.html", FullSubPages),
        new("Text Area", "text-area", "Forms", "forms",
            "The text area allows users to enter multi-line text.",
            "/api/Marilo.Components.Forms.Inputs.MariloTextArea.html", FullSubPages),
        new("Select", "select", "Forms", "forms",
            "The select component lets users choose a single value from a dropdown list.",
            "/api/Marilo.Components.Forms.Inputs.MariloSelect.html", FullSubPages),
        new("Checkbox", "checkbox", "Forms", "forms",
            "Checkboxes let users select one or more items from a list, or toggle an option on or off.",
            "/api/Marilo.Components.Forms.Inputs.MariloCheckbox.html", FullSubPages),
        new("Switch", "switch", "Forms", "forms",
            "The switch toggles the state of a single setting on or off.",
            "/api/Marilo.Components.Forms.Inputs.MariloSwitch.html", FullSubPages),
        new("Slider", "slider", "Forms", "forms",
            "The slider lets users select a value from a continuous range by moving a thumb along a track.",
            "/api/Marilo.Components.Forms.Inputs.MariloSlider.html", FullSubPages),
        new("Search Box", "search-box", "Forms", "forms",
            "The search box provides a text input optimized for search queries, with a built-in search icon and clear button.",
            "/api/Marilo.Components.Forms.Inputs.MariloSearchBox.html", FullSubPages),
        new("Rating", "rating", "Forms", "forms",
            "The rating component lets users provide a star-based evaluation.",
            "/api/Marilo.Components.Forms.Inputs.MariloRating.html", FullSubPages),
        new("Date Picker", "date-picker", "Forms", "forms",
            "The date picker lets users select a date from a calendar or by typing.",
            "/api/Marilo.Components.Forms.Inputs.MariloDatePicker.html", FullSubPages),
        new("Color Picker", "color-picker", "Forms", "forms",
            "The color picker lets users select a color value.",
            "/api/Marilo.Components.Forms.Inputs.MariloColorPicker.html", FullSubPages),

        // Data Display
        new("Card", "card", "Data Display", "data-display",
            "Cards contain content and actions about a single subject, grouping related information together.",
            "/api/Marilo.Components.DataDisplay.MariloCard.html", NoEventsSubPages),
        new("Avatar", "avatar", "Data Display", "data-display",
            "Avatars display images, icons, or initials representing people or other entities.",
            "/api/Marilo.Components.DataDisplay.MariloAvatar.html", NoEventsSubPages),
        new("Badge", "badge", "Data Display", "data-display",
            "Badges are small status descriptors for UI elements, showing counts or labels.",
            "/api/Marilo.Components.DataDisplay.MariloBadge.html", NoEventsSubPages),
        new("List", "list", "Data Display", "data-display",
            "Lists present content in a structured, scannable format.",
            "/api/Marilo.Components.DataDisplay.MariloList.html", NoEventsSubPages),
        new("Table", "table", "Data Display", "data-display",
            "Tables display structured data in rows and columns for comparison and analysis.",
            "/api/Marilo.Components.DataDisplay.MariloTable.html", NoEventsSubPages),
        new("Tooltip", "tooltip", "Data Display", "data-display",
            "Tooltips display informative text when users hover over, focus on, or tap an element.",
            "/api/Marilo.Components.DataDisplay.MariloTooltip.html", NoEventsSubPages),

        // Feedback
        new("Alert", "alert", "Feedback", "feedback",
            "Alerts display brief, important messages to attract the user's attention without interrupting their task.",
            "/api/Marilo.Components.Feedback.MariloAlert.html", NoEventsSubPages),
        new("Progress Bar", "progress-bar", "Feedback", "feedback",
            "Progress bars indicate the progress of an operation, showing how much has been completed.",
            "/api/Marilo.Components.Feedback.MariloProgressBar.html", NoEventsSubPages),
        new("Spinner", "spinner", "Feedback", "feedback",
            "Spinners indicate an ongoing process, such as loading data.",
            "/api/Marilo.Components.Feedback.MariloSpinner.html", MinimalSubPages),
        new("Skeleton", "skeleton", "Feedback", "feedback",
            "Skeletons provide a low-fidelity preview of content before it loads, reducing perceived wait time.",
            "/api/Marilo.Components.Feedback.MariloSkeleton.html", MinimalSubPages),
        new("Dialog", "dialog", "Feedback", "feedback",
            "Dialogs present content in a modal overlay that requires user attention or action.",
            "/api/Marilo.Components.Feedback.MariloDialog.html", FullSubPages),
        new("Confirm Dialog", "confirm-dialog", "Feedback", "feedback",
            "Confirm dialogs ask users to verify an action before proceeding, especially for destructive operations.",
            "/api/Marilo.Components.Feedback.MariloConfirmDialog.html", FullSubPages),

        // Layout
        new("Grid", "grid", "Layout", "layout",
            "The grid system provides a 12-column responsive layout for organizing content.",
            "/api/Marilo.Components.Layout.MariloGrid.html", NoEventsSubPages),
        new("Stack", "stack", "Layout", "layout",
            "Stacks arrange child elements in a horizontal or vertical line with configurable spacing and alignment.",
            "/api/Marilo.Components.Layout.MariloStack.html", NoEventsSubPages),
        new("Divider", "divider", "Layout", "layout",
            "Dividers separate content into clear groups.",
            "/api/Marilo.Components.Layout.MariloDivider.html", MinimalSubPages),
        new("Panel", "panel", "Layout", "layout",
            "Panels provide a contained surface with elevation for grouping related content.",
            "/api/Marilo.Components.Layout.MariloPanel.html", NoEventsSubPages),
        new("Container", "container", "Layout", "layout",
            "Containers center content and constrain its maximum width.",
            "/api/Marilo.Components.Layout.MariloContainer.html", NoEventsSubPages),
        new("Tabs", "tabs", "Layout", "layout",
            "Tabs organize content into separate views where only one view is visible at a time.",
            "/api/Marilo.Components.Layout.MariloTabs.html", FullSubPages),
        new("Accordion", "accordion", "Layout", "layout",
            "Accordions display collapsible content panels for presenting information in a limited space.",
            "/api/Marilo.Components.Layout.MariloAccordion.html", FullSubPages),
        new("Stepper", "stepper", "Layout", "layout",
            "Steppers display progress through a sequence of logical and numbered steps.",
            "/api/Marilo.Components.Layout.MariloStepper.html", FullSubPages),
        new("Drawer", "drawer", "Layout", "layout",
            "Drawers provide a slide-out panel for supplementary content or navigation.",
            "/api/Marilo.Components.Layout.MariloDrawer.html", FullSubPages),

        // Navigation
        new("Breadcrumb", "breadcrumb", "Navigation", "navigation",
            "Breadcrumbs show the current page's location within a navigational hierarchy.",
            "/api/Marilo.Components.Navigation.MariloBreadcrumb.html", NoEventsSubPages),
        new("Pagination", "pagination", "Navigation", "navigation",
            "Pagination lets users navigate through pages of content.",
            "/api/Marilo.Components.Navigation.MariloPagination.html", FullSubPages),
        new("Toolbar", "toolbar", "Navigation", "navigation",
            "Toolbars group related actions and controls in a horizontal bar.",
            "/api/Marilo.Components.Navigation.MariloToolbar.html", NoEventsSubPages),
        new("Tree View", "tree-view", "Navigation", "navigation",
            "Tree views display hierarchical data in an expandable tree structure.",
            "/api/Marilo.Components.Navigation.MariloTreeView.html", FullSubPages),
        new("Menu", "menu", "Navigation", "navigation",
            "Menus display a list of actions or options in a popup overlay.",
            "/api/Marilo.Components.Navigation.MariloMenu.html", FullSubPages),
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
