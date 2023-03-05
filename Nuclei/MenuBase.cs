using System;
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
    /// <param name="text">The 'title' of the item.</param>
    /// <param name="description">The description when the item is selected.</param>
    /// <param name="action">The action to perform when activated.</param>
    /// <returns>The item.</returns>
    protected NativeItem AddItem(string text, string description = "", Action action = null)
    {
        var item = new NativeItem(text, description);

        // anonymous method to handle the event
        item.Activated += (sender, args) => { action?.Invoke(); };

        Add(item);
        return item;
    }

    /// <summary>
    ///     Adds a new checkbox item to the menu.
    /// </summary>
    /// <param name="text">The 'title' of the item.</param>
    /// <param name="description">The description when the item is selected.</param>
    /// <param name="defaultValue">The default value of the checkbox Checked state.</param>
    /// <param name="action">The action to perform when the checkbox Checked state changes.</param>
    /// <returns>The checkbox item.</returns>
    protected NativeCheckboxItem AddCheckbox(string text, string description = "", bool defaultValue = false,
        Action<bool> action = null)
    {
        var item = new NativeCheckboxItem(text, description, defaultValue);

        // anonymous method to handle the event
        item.CheckboxChanged += (sender, args) => { action?.Invoke(item.Checked); };

        Add(item);
        return item;
    }
}