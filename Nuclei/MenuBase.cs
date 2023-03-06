using System;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;

namespace Nuclei;

public abstract class MenuBase : NativeMenu
{
    /// <summary>
    ///     The pool of menus.
    /// </summary>
    public static ObjectPool Pool = new();

    /// <summary>
    ///     Creates a new menu.
    /// </summary>
    /// <param name="subtitle">The subtitle to display below the header.</param>
    /// <param name="description">The description of the menu.</param>
    protected MenuBase(string subtitle, string description) : base("Nuclei", subtitle, description)
    {
        Pool.Add(this);
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
}