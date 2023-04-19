using System;
using System.Linq;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsMenu : GenericMenuBase<VehicleModsService>
{
    public VehicleModsMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.CurrentVehicle.ValueChanged -= OnVehicleChanged;
    }

    private void OnShown(object sender, EventArgs e)
    {
        GenerateModsMenu();
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

        GenerateModsMenu();
    }

    private void GenerateModsMenu()
    {
        Clear();

        foreach (var modType in Service.ValidVehicleModTypes.Value)
        {
            var currentMod = Service.CurrentVehicle.Value.Mods[modType];
            var currentIndex = currentMod.Index;
            var listItem = AddListItem(modType.GetLocalizedDisplayNameFromHash(), "",
                (value, index) => { currentMod.Index = index; },
                null,
                Enumerable.Range(0, currentMod.Count + 1).ToList().Select(i =>
                {
                    currentMod.Index = i;
                    var localizedString = currentMod.LocalizedName;
                    return localizedString;
                }).ToArray());

            listItem.SelectedIndex = currentIndex == -1 ? currentMod.Count : currentIndex;
        }
    }
}