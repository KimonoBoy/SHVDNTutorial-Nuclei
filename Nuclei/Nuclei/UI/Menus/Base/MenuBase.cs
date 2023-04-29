using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Scaleform;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Observable;
using Nuclei.UI.Menus.Base.ItemFactory;
using Nuclei.UI.Menus.Base.ItemFactory.CustomItems;
using Font = GTA.UI.Font;

namespace Nuclei.UI.Menus.Base;

public abstract class MenuBase : NativeMenu
{
    public static ObjectPool Pool = new();

    private readonly ExceptionService _exceptionService = ExceptionService.Instance;

    protected readonly ItemFactoryService _itemFactoryService = new();

    private readonly Dictionary<string, MenuBase> subMenus = new();

    private bool _isMovingUp;

    protected MenuBase(string subtitle, string description) : base("Nuclei", subtitle, description)
    {
        InitializeMenu();
    }

    protected MenuBase(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
    }

    public static MenuBase LatestMenu { get; set; }

    private void InitializeMenu()
    {
        SubtitleFont = Font.Pricedown;
        Banner.Color = Color.Black;
        MaxItems = 12;
        AddButtons();

        Shown += OnShown;
        SelectedIndexChanged += OnSelectedIndexChanged;

        _exceptionService.ErrorOccurred += OnErrorOccurred;

        // Check if the menu with the same subtitle is already in the Pool
        var existingMenu = Pool.OfType<MenuBase>().FirstOrDefault(menu => menu.Subtitle == Subtitle);

        if (existingMenu == null)
        {
            // If the menu does not exist, add it to the Pool
            Pool.Add(this);
        }
        else
        {
            // If the menu already exists, remove the current instance and use the existing one
            Pool.Remove(this);
            Subtitle = existingMenu.Subtitle;
            Description = existingMenu.Description;
        }
    }

    protected virtual void AddButtons()
    {
        var instructionalButtonHotKey = new InstructionalButton("Change Hotkey", Control.ReplayStartStopRecording);
        Buttons.Add(instructionalButtonHotKey);
    }

    private void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        SkipHeader();
    }

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

    public void Toggle()
    {
        Visible = !Visible;
    }

    protected NativeItem AddItem(string title, string description = "", Action action = null, string altTitle = "")
    {
        var item = _itemFactoryService.CreateNativeItem(title, description, altTitle, action);
        Add(item);
        return item;
    }

    protected NativeItem AddItem(Enum @enum, Action action = null, string altTitle = "")
    {
        return AddItem(@enum.ToPrettyString(), @enum.GetDescription(), action, altTitle);
    }

    protected NativeCheckboxItem AddCheckbox(string title, string description = "",
        Func<bool> getProperty = null,
        ObservableService propertyObserver = null,
        Action<bool> action = null)
    {
        var checkBoxItem =
            _itemFactoryService.CreateNativeCheckboxItem(title, description, getProperty, propertyObserver, action);
        Add(checkBoxItem);
        return checkBoxItem;
    }

    protected NativeCheckboxItem AddCheckbox(Enum @enum, Func<bool> getProperty = null,
        ObservableService propertyObserver = null,
        Action<bool> action = null)
    {
        return AddCheckbox(@enum.ToPrettyString(), @enum.GetDescription(), getProperty, propertyObserver, action);
    }

    protected NativeSliderItem AddSliderItem(string title, string description = "",
        Func<int> getProperty = null,
        ObservableService propertyObserver = null, Action<int> action = null, int value = 0, int maxValue = 10)
    {
        var nativeSliderItem = _itemFactoryService.CreateNativeSliderItem(title, description, getProperty,
            propertyObserver, value, maxValue, action);
        Add(nativeSliderItem);
        return nativeSliderItem;
    }

    protected NativeSliderItem AddSliderItem(Enum @enum,
        Func<int> getProperty = null,
        ObservableService propertyObserver = null,
        Action<int> action = null, int value = 0, int maxValue = 10)
    {
        return AddSliderItem(@enum.ToPrettyString(), @enum.GetDescription(), getProperty,
            propertyObserver, action, value, maxValue);
    }

    protected NativeListItem<T> AddListItem<T>(string title, string description,
        Func<int> getProperty,
        ObservableService propertyChangedSource,
        Action<T, int> itemChangedAction,
        params T[] items)
    {
        var listItem = _itemFactoryService.CreateNativeListItem(title, description,
            getProperty, propertyChangedSource, itemChangedAction, items);
        Add(listItem);
        return listItem;
    }

    protected NativeListItem<T> AddListItem<T>(Enum @enum,
        Func<int> getProperty,
        ObservableService propertyChangedSource,
        Action<T, int> itemChangedAction,
        params T[] items)
    {
        return AddListItem(@enum.ToPrettyString(), @enum.GetDescription(),
            getProperty, propertyChangedSource, itemChangedAction, items);
    }

    protected NativeListItem<T> AddActivateListItem<T>(string title, string description = "",
        Func<int> getProperty = null,
        ObservableService propertyChangedSource = null,
        Action<T, int> itemActivatedAction = null,
        params T[] items)
    {
        var listItem = _itemFactoryService.CreateNativeActivateListItem(title, description,
            getProperty, propertyChangedSource, itemActivatedAction, items);
        Add(listItem);
        return listItem;
    }

    protected NativeListItem<T> AddActivateListItem<T>(Enum @enum,
        Func<int> getProperty = null,
        ObservableService propertyChangedSource = null,
        Action<T, int> itemActivatedAction = null,
        params T[] items)
    {
        return AddActivateListItem(@enum.ToPrettyString(), @enum.GetDescription(),
            getProperty, propertyChangedSource, itemActivatedAction, items);
    }

    protected NativeSubmenuItem AddMenu(MenuBase subMenu)
    {
        if (subMenus.ContainsKey(subMenu.Subtitle))
            // If the submenu already exists, get a reference to it
            subMenu = subMenus[subMenu.Subtitle];
        else
            // If the submenu does not exist, add it to the dictionary
            subMenus.Add(subMenu.Subtitle, subMenu);

        var submenuItem = AddSubMenu(subMenu);
        submenuItem.AltTitle = "Menu";
        submenuItem.AltTitleFont = Font.ChaletComprimeCologne;
        return submenuItem;
    }

    protected NativeHeaderItem AddHeader(string title)
    {
        var headerItem = new NativeHeaderItem(title);
        Add(headerItem);
        return headerItem;
    }

    protected void UpdateAltTitleOnCondition(NativeSubmenuItem itemToUpdate, bool condition,
        string enabled, string disabled)
    {
        var altTitle = condition ? $"{enabled}" : $"{disabled}";
        itemToUpdate.Enabled = condition;
        itemToUpdate.AltTitle = altTitle;
    }

    private void OnShown(object sender, EventArgs e)
    {
        LatestMenu = this;
        SkipHeader();
    }

    private void OnErrorOccurred(object sender, CustomExceptionBase exception)
    {
        Notification.Show($"~r~~h~{exception.Prefix}~h~:\n\n~w~{exception.Message}");
    }

    protected NativeMenu NavigateToMenu(string subTitle)
    {
        var allMenusInPool = Pool.Where(menu => menu is NativeMenu).Cast<NativeMenu>().ToList();
        var menu = allMenusInPool.FirstOrDefault(menu => menu.Subtitle == subTitle);
        if (menu != null)
        {
            Visible = false;
            menu.Visible = true;
            return menu;
        }

        throw new NullReferenceException($"Menu with subtitle '{subTitle}' not found.");
    }

    protected NativeMenu NavigateToMenu(Enum @enum)
    {
        return NavigateToMenu(@enum.GetLocalizedDisplayNameFromHash());
    }

    protected T GetItem<T>(string title) where T : NativeItem
    {
        var item = Items.FirstOrDefault(item => item.Title == title);
        return item == null ? throw new NullReferenceException($"Item with title '{title}' not found.") : item as T;
    }

    protected T GetItem<T>(Enum @enum) where T : NativeItem
    {
        return GetItem<T>(@enum.GetLocalizedDisplayNameFromHash());
    }
}