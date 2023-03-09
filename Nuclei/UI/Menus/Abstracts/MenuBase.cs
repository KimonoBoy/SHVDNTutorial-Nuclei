using System;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Abstracts;

public abstract class MenuBase : NativeMenu
{
    /// <summary>
    ///     The pool of menus.
    /// </summary>
    public static ObjectPool Pool = new();

    /// <summary>
    ///     The latest active menu. This is used to determine which menu to return to when closing a menu.
    /// </summary>
    public static MenuBase LatestMenu { get; set; }

    /// <summary>
    ///     Creates a new menu.
    /// </summary>
    /// <param name="subtitle">The subtitle to display below the header.</param>
    /// <param name="description">The description of the menu.</param>
    protected MenuBase(string subtitle, string description) : base("Nuclei", subtitle, description)
    {
        Shown += OnShown;

        Pool.Add(this);
    }

    /// <summary>
    ///     Creates a new menu.
    /// </summary>
    /// <param name="enum">The Enum to get the Sub Title and Description from.</param>
    protected MenuBase(Enum @enum) : base("Nuclei", @enum.ToPrettyString(), @enum.GetDescription())
    {
        Shown += OnShown;

        Pool.Add(this);
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
    /// <returns>The item.</returns>
    protected NativeItem AddItem(string title, string description = "", Action action = null)
    {
        var item = new NativeItem(title, description);

        // anonymous method to handle the event
        item.Activated += (sender, args) => { action?.Invoke(); };

        Add(item);
        return item;
    }

    /// <summary>
    ///     Adds a new item to the menu.
    /// </summary>
    /// <param name="enum">The Enum to get the Title and the Description from.</param>
    /// <param name="action">The action to perform when activated.</param>
    /// <returns>The item.</returns>
    protected NativeItem AddItem(Enum @enum, Action action = null)
    {
        return AddItem(@enum.ToPrettyString(), @enum.GetDescription(), action);
    }

    /// <summary>
    ///     Adds a new checkbox item to the menu.
    /// </summary>
    /// <param name="title">The 'title' of the item.</param>
    /// <param name="description">The description when the item is selected.</param>
    /// <param name="defaultValue">The default value of the checkbox Checked state.</param>
    /// <param name="action">The action to perform when the checkbox Checked state changes.</param>
    /// <returns>The checkbox item.</returns>
    protected NativeCheckboxItem AddCheckbox(string title, string description = "", bool defaultValue = false,
        Action<bool> action = null)
    {
        var item = new NativeCheckboxItem(title, description, defaultValue);

        // anonymous method to handle the event
        item.CheckboxChanged += (sender, args) => { action?.Invoke(item.Checked); };

        Add(item);
        return item;
    }

    /// <summary>
    ///     Adds a new checkbox item to the menu.
    /// </summary>
    /// <param name="enum">The enum to get the Title and the Description from.</param>
    /// <param name="defaultValue">The default checked state of the checkbox.</param>
    /// <param name="action">The action to perform when the checkbox Checked state changes.</param>
    /// <returns>The checkbox item.</returns>
    protected NativeCheckboxItem AddCheckbox(Enum @enum, bool defaultValue = false, Action<bool> action = null)
    {
        return AddCheckbox(@enum.ToPrettyString(), @enum.GetDescription(), defaultValue, action);
    }

    /// <summary>
    ///     Adds a new list item to the menu.
    /// </summary>
    /// <typeparam name="T">The object type of the values.</typeparam>
    /// <param name="title">The 'title' of the item.</param>
    /// <param name="description">The description when the item is selected.</param>
    /// <param name="action">The action to perform when the selected item of the list changes.</param>
    /// <param name="items">The items array.</param>
    /// <returns>The list item.</returns>
    protected NativeListItem<T> AddListItem<T>(string title, string description = "", Action<T, int> action = null,
        params T[] items)
    {
        var item = new NativeListItem<T>(title, description, items);
        item.ItemChanged += (sender, args) => { action?.Invoke(item.SelectedItem, item.SelectedIndex); };
        Add(item);
        return item;
    }

    /// <summary>
    ///     Adds a new list item to the menu.
    /// </summary>
    /// <typeparam name="T">The object type of the values.</typeparam>
    /// <param name="enum">The enum to get the Title and the Description from.</param>
    /// <param name="action">The action to perform when the selected item of the list changes.</param>
    /// <param name="items">The items array.</param>
    /// <returns>The list item.</returns>
    protected NativeListItem<T> AddListItem<T>(Enum @enum, Action<T, int> action = null, params T[] items)
    {
        return AddListItem(@enum.ToPrettyString(), @enum.GetDescription(), action, items);
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

    private void OnShown(object sender, EventArgs e)
    {
        LatestMenu = this;
    }
}