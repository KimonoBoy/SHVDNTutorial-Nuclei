using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.UI.Menus.Base;
using Nuclei.UI.Menus.Base.ItemFactory;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public abstract class VehicleModsMenuBase : GenericMenu<VehicleModsService>
{
    protected VehicleModsMenuBase(Enum @enum) : base(@enum)
    {
        Width = 620;
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
        if (Service.CurrentVehicle == null)
        {
            NavigateToMenu(MenuTitle.Vehicle);
            return;
        }

        GenerateMenu();
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.CurrentVehicle))
        {
            if (!Visible) return;

            if (Service.CurrentVehicle == null)
            {
                NavigateToMenu(MenuTitle.Vehicle);
                return;
            }

            GenerateMenu();
        }
    }

    protected abstract IEnumerable<VehicleMod> GetValidMods();

    protected virtual void PreModTypeMods()
    {
    }

    protected virtual void PostModTypeMods()
    {
    }

    protected void GenerateMenu()
    {
        if (Service.CurrentVehicle == null) return;

        Clear();
        PreModTypeMods();
        foreach (var vehicleMod in GetValidMods())
        {
            var currentIndex = vehicleMod.Index;
            if (vehicleMod.Count <= 0) continue;
            var listItemTest = AddListItem(vehicleMod.Type.GetLocalizedDisplayNameFromHash(), "",
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

            listItemTest.SetSelectedIndexSafe(currentIndex == -1 ? vehicleMod.Count : currentIndex);
            if (!listItemTest.Any()) return;
            if (listItemTest.SelectedIndex == 0 && listItemTest.Items.Count > 0)
            {
                listItemTest.SelectedIndex++;
                listItemTest.SelectedIndex--;
            }
        }

        PostModTypeMods();
    }
}