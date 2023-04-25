using System;
using GTA.UI;
using LemonUI.Menus;
using Nuclei.Services.Observable;

namespace Nuclei.UI.Menus.Base.ItemFactory;

public class ItemFactoryService
{
    public NativeItem CreateNativeItem(string title, string description = "",
        string altTitle = "", Action action = null)
    {
        var item = new NativeItem(title, description, altTitle);
        item.AltTitleFont = Font.ChaletComprimeCologne;
        item.Activated += (sender, args) => { action?.Invoke(); };
        return item;
    }

    public NativeCheckboxItem CreateNativeCheckboxItem(string title, string description = "",
        Func<bool> getProperty = null, ObservableService propertyChangedSource = null, Action<bool> action = null)
    {
        var checkBoxItem = new NativeCheckboxItem(title, description);

        if (getProperty != null && action != null && propertyChangedSource != null)
        {
            checkBoxItem.Checked = getProperty.Invoke();

            propertyChangedSource.PropertyChanged += (sender, args) => { checkBoxItem.Checked = getProperty.Invoke(); };
        }

        checkBoxItem.CheckboxChanged += (sender, args) => { action?.Invoke(checkBoxItem.Checked); };

        return checkBoxItem;
    }

    public NativeSliderItem CreateNativeSliderItem(string title, string description = "",
        Func<int> getProperty = null,
        ObservableService propertyChangedSource = null, int value = 0, int maxValue = 10, Action<int> action = null)
    {
        var nativeSliderItem = new NativeSliderItem(title, description, maxValue, value);
        if (getProperty != null && action != null && propertyChangedSource != null)
        {
            nativeSliderItem.Value = getProperty();
            propertyChangedSource.PropertyChanged += (sender, args) => { nativeSliderItem.Value = getProperty(); };
        }

        nativeSliderItem.ValueChanged += (sender, args) => { action?.Invoke(nativeSliderItem.Value); };

        return nativeSliderItem;
    }

    public NativeListItem<T> CreateNativeListItem<T>(string title, string description = "",
        Func<int> getProperty = null, ObservableService propertyChangedSource = null,
        Action<T, int> itemChangedAction = null,
        params T[] items)
    {
        var item = new NativeListItem<T>(title, description, items);

        if (getProperty != null && propertyChangedSource != null)
            propertyChangedSource.PropertyChanged += (sender, args) =>
            {
                var newIndex = getProperty();
                if (newIndex >= 0 && newIndex < item.Items.Count) item.SelectedIndex = newIndex;
            };

        item.ItemChanged += (sender, args) => { itemChangedAction?.Invoke(args.Object, args.Index); };

        return item;
    }

    public NativeListItem<T> CreateNativeActivateListItem<T>(string title, string description = "",
        Func<int> getProperty = null,
        ObservableService propertyChangedSource = null,
        Action<T, int> itemActivatedAction = null, params T[] items)
    {
        var item = new NativeListItem<T>(title, description, items);

        if (getProperty != null && propertyChangedSource != null)
            propertyChangedSource.PropertyChanged += (sender, args) =>
            {
                var newIndex = getProperty();
                if (newIndex >= 0 && newIndex < item.Items.Count) item.SelectedIndex = newIndex;
            };

        item.Activated += (sender, args) => { itemActivatedAction?.Invoke(item.SelectedItem, item.SelectedIndex); };

        return item;
    }
}