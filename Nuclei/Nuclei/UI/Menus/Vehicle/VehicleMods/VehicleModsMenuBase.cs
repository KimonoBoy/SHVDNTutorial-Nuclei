using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public abstract class VehicleModsMenuBase : GenericMenuBase<VehicleModsService>
{
    protected VehicleModsMenuBase(Enum @enum) : base(@enum)
    {
        Width = 550;
        Shown += OnShown;
    }

    private void OnShown(object sender, EventArgs e)
    {
        if (Service.CurrentVehicle.Value == null)
        {
            Back();
            return;
        }

        UpdateMenuItems();
        Service.CurrentVehicle.ValueChanged += OnVehicleChanged;
    }

    private void OnVehicleChanged(object sender, ValueEventArgs<GTA.Vehicle> currentVehicle)
    {
        if (!Visible) return;

        if (currentVehicle.Value == null)
        {
            Back();
            return;
        }

        UpdateMenuItems();
    }

    protected abstract IEnumerable<VehicleModType> GetValidModTypes();

    protected virtual void UpdateMenuItems()
    {
        GenerateModsMenu();
    }

    private void GenerateModsMenu()
    {
        foreach (var modType in GetValidModTypes())
        {
            var currentMod = Service.CurrentVehicle.Value.Mods[modType];
            var currentIndex = currentMod.Index;
            var listItem = AddListItem(modType.GetLocalizedDisplayNameFromHash(), "",
                (value, index) => { currentMod.Index = index == currentMod.Count ? -1 : index; },
                null,
                Enumerable.Range(0, currentMod.Count + 1).ToList().Select(i =>
                {
                    if (i == currentMod.Count) return "Stock" + $" {0}/{currentMod.Count}";
                    currentMod.Index = i;
                    var localizedString = currentMod.LocalizedName + $" {i + 1}/{currentMod.Count}";
                    return localizedString;
                }).ToArray());

            listItem.SelectedIndex = currentIndex == -1 ? currentMod.Count : currentIndex;
        }
    }
}