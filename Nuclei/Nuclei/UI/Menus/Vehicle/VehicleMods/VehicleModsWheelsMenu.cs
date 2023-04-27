using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GTA;
using LemonUI.Menus;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.UI.Menus.Base.ItemFactory;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsWheelsMenu : VehicleModsMenuBase
{
    public VehicleModsWheelsMenu(Enum @enum) : base(@enum)
    {
        Width = 600;
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.PropertyChanged -= OnPropertyChanged;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Service.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Service.CurrentVehicle == null) return;
        if (e.PropertyName == nameof(Service.WheelType))
        {
            var filteredMods = Service.VehicleMods.Where(vehicleMod => vehicleMod.Count > 0 &&
                                                                       vehicleMod.Type is VehicleModType.FrontWheel
                                                                           or VehicleModType.RearWheel);

            // Clear the filtered mods and get the new ones, then add them to the menu replacing the old ones
            foreach (var vehicleMod in filteredMods)
            {
                // find the associated listitem and update its corresponding values
                var oldListItem = Items.OfType<NativeListItem<string>>()
                    .FirstOrDefault(nativeListItem =>
                        nativeListItem.Title == vehicleMod.Type.GetLocalizedDisplayNameFromHash());
                if (oldListItem == null) continue;

                // Create a new list item
                var newListItem = _itemFactoryService.CreateNativeListItem(
                    vehicleMod.Type.GetLocalizedDisplayNameFromHash(), "",
                    null, Service,
                    (value, index) => { vehicleMod.Index = index; },
                    Enumerable.Range(0, vehicleMod.Count + 1).Select(index =>
                    {
                        if (index == -1) return $"Stock {0} / {vehicleMod.Count}";
                        vehicleMod.Index = index;
                        string localizedName;
                        if (index == vehicleMod.Count)
                            localizedName = $"{vehicleMod.LocalizedName} {0} / {vehicleMod.Count}";
                        else
                            localizedName = vehicleMod.LocalizedName + $" {index + 1} / {vehicleMod.Count}";
                        return localizedName;
                    }).ToArray());

                // Replace the old list item with the new one
                var index = Items.IndexOf(oldListItem);
                Items[index] = newListItem;
                if (newListItem.Items.Count > 0)
                {
                    newListItem.SelectedIndex++;
                    newListItem.SelectedIndex--;
                }
            }
        }
    }

    protected override void PreModTypeMods()
    {
        WheelTypes();
    }

    protected override void PostModTypeMods()
    {
        RimColors();
        AddHeader("Tires");
        CustomTires();
        TireSmokeColor();
    }

    private void CustomTires()
    {
        var checkBoxCustomTires = AddCheckbox(VehicleModsItemTitles.CustomTires, () => Service.CustomTires, Service,
            @checked => { Service.CustomTires = @checked; });
        checkBoxCustomTires.Checked = Service.CustomTires;
    }

    private void TireSmokeColor()
    {
        var listItemTireSmokeColor = AddListItem(VehicleModsItemTitles.TireSmokeColor,
            () => (int)Service.TireSmokeColor,
            Service,
            (value, index) => { Service.TireSmokeColor = (TireSmokeColor)index; },
            Enumerable.Range(0, Service.TireSmokeColorDictionary.Keys.Count)
                .Select(index =>
                    $"{((TireSmokeColor)index).GetLocalizedDisplayNameFromHash()} {index} / {Service.TireSmokeColorDictionary.Count - 1}")
                .ToArray());
        listItemTireSmokeColor.SetSelectedIndexSafe((int)Service.TireSmokeColor);
    }

    private void RimColors()
    {
        var listItemRimColor = AddListItem(VehicleModsItemTitles.RimColor,
            () => (int)Service.RimColor,
            Service,
            (value, index) =>
            {
                Service.RimColor =
                    (VehicleColor)index;
            },
            Enumerable.Range(0, Enum.GetValues(typeof(VehicleColor)).Length)
                .Select(index =>
                    $"{((VehicleColor)index).GetLocalizedDisplayNameFromHash()} {index} / {Enum.GetValues(typeof(VehicleColor)).Length - 1}")
                .ToArray());
        listItemRimColor.SetSelectedIndexSafe((int)Service.RimColor);
    }

    private void WheelTypes()
    {
        var listItemWheelType = AddListItem(VehicleModsItemTitles.WheelType, () => (int)Service.WheelType,
            Service,
            (value, index) => { Service.WheelType = (VehicleWheelType)index; },
            typeof(VehicleWheelType).ToDisplayNameArray());
        listItemWheelType.SetSelectedIndexSafe((int)Service.WheelType);
    }

    protected override IEnumerable<VehicleMod> GetValidMods()
    {
        return Service.VehicleMods.Where(vehicleMod =>
            vehicleMod.Type is VehicleModType.FrontWheel or VehicleModType.RearWheel);
    }
}