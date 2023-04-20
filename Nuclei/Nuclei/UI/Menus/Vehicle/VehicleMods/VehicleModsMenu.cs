using System;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsMenu : GenericMenuBase<VehicleModsService>
{
    /// <summary>
    ///     Used to store the selected index of the menu.
    ///     This is used to prevent the menu from resetting to the first item when the menu is closed and reopened unless a new
    ///     vehicle is selected.
    /// </summary>
    private static int _selectedIndex;

    public VehicleModsMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        _selectedIndex = SelectedIndex;
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

        _selectedIndex = 0;
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
                (value, index) => { currentMod.Index = index == currentMod.Count ? -1 : index; },
                null,
                Enumerable.Range(0, currentMod.Count + 1).ToList().Select(i =>
                {
                    if (i == currentMod.Count) return "Stock";
                    currentMod.Index = i;
                    var localizedString = currentMod.LocalizedName;
                    return localizedString;
                }).ToArray());

            listItem.SelectedIndex = currentIndex == -1 ? currentMod.Count : currentIndex;
        }

        var itemLicensePlate = AddItem("License Plate", "", () =>
        {
            var userInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 8);
            Service.CurrentVehicle.Value.Mods.LicensePlate = userInput;
        });

        SelectedIndex = _selectedIndex;
    }
}