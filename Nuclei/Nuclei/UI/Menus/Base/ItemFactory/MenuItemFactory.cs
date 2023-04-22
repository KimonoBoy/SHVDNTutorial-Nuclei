using System;
using GTA.UI;
using LemonUI.Menus;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.UI.Menus.Base.ItemFactory;

public class MenuItemFactory
{
    public NativeItem CreateNativeItem(string title, string description = "", Action action = null,
        string altTitle = "")
    {
        var item = new NativeItem(title, description, altTitle);
        item.AltTitleFont = Font.ChaletComprimeCologne;
        item.Activated += (sender, args) => { action?.Invoke(); };
        return item;
    }

    public NativeCheckboxItem CreateNativeCheckboxItem(string title, string description = "",
        BindableProperty<bool> bindableProperty = null, Action<bool> action = null)
    {
        NativeCheckboxItem checkBoxItem;
        if (bindableProperty != null)
        {
            checkBoxItem = new NativeCheckboxItem(title, description, bindableProperty.Value);
            bindableProperty.ValueChanged += (_, args) => checkBoxItem.Checked = args.Value;
        }
        else
        {
            checkBoxItem = new NativeCheckboxItem(title, description, false);
        }

        checkBoxItem.CheckboxChanged += (sender, args) => { action?.Invoke(checkBoxItem.Checked); };
        return checkBoxItem;
    }

    public NativeSliderItem CreateNativeSliderItem(string title, string description = "",
        BindableProperty<int> bindableProperty = null, Action<int> action = null, int value = 0, int maxValue = 10)
    {
        var nativeSliderItem = new NativeSliderItem(title, description, maxValue, value);

        if (bindableProperty != null)
            bindableProperty.ValueChanged += (sender, args) => { nativeSliderItem.Value = args.Value; };

        nativeSliderItem.ValueChanged += (sender, args) => { action?.Invoke(nativeSliderItem.Value); };
        return nativeSliderItem;
    }

    public NativeListItem<T> CreateNativeListItem<T>(string title, string description = "",
        Action<T, int> itemChangedAction = null, Action<T, int> itemActivatedAction = null, params T[] items)
    {
        var item = new NativeListItem<T>(title, description, items);

        if (itemChangedAction != null)
            item.ItemChanged += (sender, args) => { itemChangedAction?.Invoke(item.SelectedItem, item.SelectedIndex); };

        if (itemActivatedAction != null)
            item.Activated += (sender, args) => { itemActivatedAction?.Invoke(item.SelectedItem, item.SelectedIndex); };

        return item;
    }
}