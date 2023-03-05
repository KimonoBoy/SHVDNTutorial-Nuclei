using System;
using LemonUI;
using LemonUI.Menus;

namespace Nuclei;

public abstract class MenuBase : NativeMenu
{
    public static ObjectPool MenuPool = new();

    protected MenuBase(string subtitle, string description) : base("Nuclei", subtitle, description)
    {
        MenuPool.Add(this);
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
        item.Activated += (sender, args) =>
        {
            if (action != null)
                action();
        };
        Add(item);
        return item;
    }
}