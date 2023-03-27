﻿using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.UI.Items;
using Control = GTA.Control;

namespace Nuclei.UI.Menus.Abstracts;

public abstract class MenuBase : NativeMenu
{
    private bool _isMovingUp;

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
        SubtitleFont = Font.Pricedown;

        Shown += OnShown;
        SelectedIndexChanged += OnSelectedIndexChanged;

        Pool.Add(this);
    }

    /// <summary>
    ///     Creates a new menu.
    /// </summary>
    /// <param name="enum">The Enum to get the Sub Title and Description from.</param>
    protected MenuBase(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
    }

    private void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        SkipHeader();
    }

    /// <summary>
    /// Header Items are meant to only categorize items, not be selectable.
    /// So when a header item is selected, we skip it, selecting the next item instead.
    /// </summary>
    private void SkipHeader()
    {
        // If the menu contains 0 items or the selected item is not a header item, return.
        if (Items.Count < 1 || SelectedItem is not NativeHeaderItem) return;

        // If the menu contains only header items, return. (This is to prevent an infinite loop)
        if (Items.All(i => i is NativeHeaderItem)) return;

        // Set the state of the menus latest navigation to up or down.
        if (Game.IsKeyPressed(Keys.Up))
            _isMovingUp = true;
        else if (Game.IsKeyPressed(Keys.Down))
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

    /// <summary>
    /// Creates a new NativeHeaderItem and adds it to the menu.
    /// </summary>
    /// <param name="title">The title of the header item.</param>
    /// <returns>The header item.</returns>
    protected NativeHeaderItem AddHeader(string title)
    {
        var headerItem = new NativeHeaderItem(title);
        Add(headerItem);
        return headerItem;
    }

    private void OnShown(object sender, EventArgs e)
    {
        LatestMenu = this;
        SkipHeader();
    }
}