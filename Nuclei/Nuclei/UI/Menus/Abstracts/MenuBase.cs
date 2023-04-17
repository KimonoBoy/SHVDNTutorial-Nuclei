using System;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Scaleform;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.UI.Items;
using Nuclei.UI.Text;
using Font = GTA.UI.Font;

namespace Nuclei.UI.Menus.Abstracts;

public abstract class MenuBase : NativeMenu
{
    /// <summary>
    ///     The pool of menus.
    /// </summary>
    public static ObjectPool Pool = new();

    /// <summary>
    ///     This service helps with handling exceptions.
    /// </summary>
    private readonly ExceptionService _exceptionService = ExceptionService.Instance;

    /// <summary>
    ///     Whether the user is navigating up (true) or down (false).
    /// </summary>
    private bool _isMovingUp;

    /// <summary>
    ///     Creates a new menu.
    /// </summary>
    /// <param name="subtitle">The subtitle to display below the header.</param>
    /// <param name="description">The description of the menu.</param>
    protected MenuBase(string subtitle, string description) : base("Nuclei", subtitle, description)
    {
        SubtitleFont = Font.Pricedown;
        Banner.Color = Color.Black;
        MaxItems = 12;

        Shown += OnShown;
        SelectedIndexChanged += OnSelectedIndexChanged;

        AddButtons();

        _exceptionService.ErrorOccurred += OnErrorOccurred;

        Pool.Add(this);
    }

    /// <summary>
    ///     Creates a new menu.
    /// </summary>
    /// <param name="enum">The Enum to get the Sub Title and Description from.</param>
    protected MenuBase(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
    }

    /// <summary>
    ///     The latest active menu. This is used to determine which menu to return to when closing a menu.
    /// </summary>
    public static MenuBase LatestMenu { get; set; }

    protected virtual void AddButtons()
    {
        var instructionalButtonHotKey = new InstructionalButton("Change Hotkey", Control.ReplayStartStopRecording);
        Buttons.Add(instructionalButtonHotKey);
    }

    private void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        SkipHeader();
    }

    /// <summary>
    ///     Header Items are meant to only categorize items, not be selectable.
    ///     So when a header item is selected, we skip it, selecting the next item (depending on user-input) instead.
    /// </summary>
    private void SkipHeader()
    {
        // If the menu contains 0 items or the selected item is not a header item, return.
        if (Items.Count < 1 || SelectedItem is not NativeHeaderItem) return;

        // If the menu contains only header items, return. (This is to prevent an infinite loop)
        if (Items.All(i => i is NativeHeaderItem)) return;

        // Set the state of the menus latest navigation to up or down.
        if (Game.IsControlPressed(Control.PhoneUp))
            _isMovingUp = true;
        else if (Game.IsControlPressed(Control.PhoneDown))
            _isMovingUp = false;

        // Get the index of the next item.
        var nextIndex = _isMovingUp ? SelectedIndex - 1 : SelectedIndex + 1;
        if (nextIndex >= 0 && nextIndex < Items.Count)
            SelectedIndex = nextIndex;
        else
            SelectedIndex = nextIndex < 0 ? Items.Count - 1 : 0;
    }

    /// <summary>
    ///     Toggles the visibility of the menu.
    /// </summary>
    public void Toggle()
    {
        Visible = !Visible;
    }

    /// <summary>
    ///     Adds a new item to the menu.
    /// </summary>
    /// <param name="title">The 'title' of the item.</param>
    /// <param name="description">The description when the item is selected.</param>
    /// <param name="action">The action to perform when activated.</param>
    /// <param name="hotkey">The hotkey to activate this item.</param>
    /// <returns>The item.</returns>
    protected NativeItem AddItem(string title, string description = "", Action action = null, string hotkey = "")
    {
        var item = new NativeItem(title, description, hotkey);
        item.AltTitleFont = Font.ChaletComprimeCologne;

        // anonymous method to handle the event
        item.Activated += (sender, args) =>
        {
            action?.Invoke();
            Display.Notify(title, "Activated");
        };

        Add(item);
        return item;
    }

    /// <summary>
    ///     Adds a new item to the menu.
    /// </summary>
    /// <param name="enum">The Enum to get the Title and the Description from.</param>
    /// <param name="action">The action to perform when activated.</param>
    /// <param name="hotkey">The hotkey to activate this item.</param>
    /// <returns>The item.</returns>
    protected NativeItem AddItem(Enum @enum, Action action = null, string hotkey = "")
    {
        return AddItem(@enum.ToPrettyString(), @enum.GetDescription(), action, hotkey);
    }

    /// <summary>
    ///     Adds a new checkbox item to the menu.
    /// </summary>
    /// <param name="title">The 'title' of the item.</param>
    /// <param name="description">The description when the item is selected.</param>
    /// <param name="bindableProperty">The value to bind this item to.</param>
    /// <param name="action">The action to perform when the checkbox Checked state changes.</param>
    /// <returns>The checkbox item.</returns>
    protected NativeCheckboxItem AddCheckbox(string title, string description = "",
        BindableProperty<bool> bindableProperty = null,
        Action<bool> action = null)
    {
        NativeCheckboxItem checkBoxItem;
        if (bindableProperty != null)
        {
            checkBoxItem = new NativeCheckboxItem(title, description, bindableProperty.Value);
            bindableProperty.ValueChanged += (sender, args) => checkBoxItem.Checked = args.Value;
        }
        else
        {
            checkBoxItem = new NativeCheckboxItem(title, description, false);
        }

        // anonymous method to handle the event
        checkBoxItem.CheckboxChanged += (sender, args) =>
        {
            action?.Invoke(checkBoxItem.Checked);
            Display.Notify(title, checkBoxItem.Checked ? "Activated" : "Deactivated", checkBoxItem.Checked);
        };

        Add(checkBoxItem);
        return checkBoxItem;
    }

    /// <summary>
    ///     Adds a new checkbox item to the menu.
    /// </summary>
    /// <param name="enum">The enum to get the Title and the Description from.</param>
    /// <param name="bindableProperty">The value to bind this item to.</param>
    /// <param name="action">The action to perform when the checkbox Checked state changes.</param>
    /// <returns>The checkbox item.</returns>
    protected NativeCheckboxItem AddCheckbox(Enum @enum, BindableProperty<bool> bindableProperty = null,
        Action<bool> action = null)
    {
        return AddCheckbox(@enum.ToPrettyString(), @enum.GetDescription(), bindableProperty, action);
    }

    /// <summary>
    ///     Adds a new slider item to the control and returns the slider item instance.
    /// </summary>
    /// <param name="title">The title of the slider item.</param>
    /// <param name="description">The description of the slider item.</param>
    /// <param name="bindableProperty">The data binding for the slider item's value.</param>
    /// <param name="action">The delegate that will be invoked whenever the value of the slider item changes.</param>
    /// <param name="value">Current Value</param>
    /// <param name="maxValue">Max Value</param>
    /// <returns>The slider item instance.</returns>
    protected NativeSliderItem AddSliderItem(string title, string description = "",
        BindableProperty<int> bindableProperty = null, Action<int> action = null, int value = 0, int maxValue = 10)
    {
        var nativeSliderItem = new NativeSliderItem(title, description, maxValue, value);

        if (bindableProperty != null)
            bindableProperty.ValueChanged += (sender, args) => { nativeSliderItem.Value = args.Value; };


        nativeSliderItem.ValueChanged += (sender, args) => { action?.Invoke(nativeSliderItem.Value); };

        Add(nativeSliderItem);
        return nativeSliderItem;
    }

    /// <summary>
    ///     Adds a new slider item to the control using an enum value and returns the slider item instance.
    /// </summary>
    /// <param name="enum">The enum value representing the title and description of the slider item.</param>
    /// <param name="bindableProperty">The data binding for the slider item's value.</param>
    /// <param name="action">The delegate that will be invoked whenever the value of the slider item changes.</param>
    /// <param name="value">Current Value.</param>
    /// <param name="maxValue">Max Value.</param>
    /// <returns>The slider item instance.</returns>
    protected NativeSliderItem AddSliderItem(Enum @enum,
        BindableProperty<int> bindableProperty = null,
        Action<int> action = null, int value = 0, int maxValue = 10)
    {
        return AddSliderItem(@enum.ToPrettyString(), @enum.GetDescription(), bindableProperty, action, value, maxValue);
    }

    /// <summary>
    ///     Adds a new list item to the menu with a given title, description, action, event type, and items.
    /// </summary>
    /// <typeparam name="T">The object type of the values.</typeparam>
    /// <param name="title">The 'title' of the item.</param>
    /// <param name="description">The description when the item is selected (optional).</param>
    /// <param name="itemChangedAction">The action to perform when the item in the list changes. (Null to do nothing)</param>
    /// <param name="itemActivatedAction">The action to perform when the item in the list is activated. (Null to do nothing)</param>
    /// <param name="items">The items array.</param>
    /// <returns>The list item.</returns>
    protected NativeListItem<T> AddListItem<T>(string title, string description = "",
        Action<T, int> itemChangedAction = null,
        Action<T, int> itemActivatedAction = null,
        params T[] items)
    {
        var item = new NativeListItem<T>(title, description, items);

        if (itemChangedAction != null)
            item.ItemChanged += (sender, args) => { itemChangedAction?.Invoke(item.SelectedItem, item.SelectedIndex); };

        if (itemActivatedAction != null)
            item.Activated += (sender, args) => { itemActivatedAction?.Invoke(item.SelectedItem, item.SelectedIndex); };

        Add(item);
        return item;
    }


    /// <summary>
    ///     Adds a new list item to the menu with a given enum, action, event type, and items.
    /// </summary>
    /// <typeparam name="T">The object type of the values.</typeparam>
    /// <param name="enum">The enum to get the Title and the Description from.</param>
    /// <param name="itemChangedAction">The action to perform when the item in the list changes. (Null to do nothing)</param>
    /// <param name="itemActivatedAction">The action to perform when the item in the list is activated. (Null to do nothing)</param>
    /// <param name="items">The items array.</param>
    /// <returns>The list item.</returns>
    protected NativeListItem<T> AddListItem<T>(Enum @enum,
        Action<T, int> itemChangedAction = null,
        Action<T, int> itemActivatedAction = null,
        params T[] items)
    {
        return AddListItem(@enum.ToPrettyString(), @enum.GetDescription(), itemChangedAction, itemActivatedAction,
            items);
    }

    /// <summary>
    ///     Adds a new submenu and associated item to the menu.
    /// </summary>
    /// <param name="subMenu">The Sub Menu to add.</param>
    /// <returns>The item associated with the sub menu.</returns>
    protected NativeSubmenuItem AddMenu(MenuBase subMenu)
    {
        var subMenuItem = AddSubMenu(subMenu);
        subMenuItem.AltTitle = "Menu";
        subMenuItem.AltTitleFont = Font.ChaletComprimeCologne;
        return subMenuItem;
    }

    /// <summary>
    ///     Adds a new NativeHeaderItem it to the menu.
    /// </summary>
    /// <param name="title">The title of the header item.</param>
    /// <returns>The header item.</returns>
    protected NativeHeaderItem AddHeader(string title)
    {
        var headerItem = new NativeHeaderItem(title);
        Add(headerItem);
        return headerItem;
    }

    /// <summary>
    ///     Performs a few tasks when the Menu is shown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnShown(object sender, EventArgs e)
    {
        LatestMenu = this;
        SkipHeader();
    }

    /// <summary>
    ///     Displays a Notification when an error is raised.
    /// </summary>
    /// <param name="sender">The caller.</param>
    /// <param name="exception">The exception that was thrown.</param>
    private void OnErrorOccurred(object sender, CustomExceptionBase exception)
    {
        Notification.Show($"~r~~h~{exception.Prefix}~h~:\n\n~w~{exception.Message}");
    }
}