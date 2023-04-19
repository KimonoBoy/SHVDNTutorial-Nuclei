using System;
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
            AddItem(modType.ToString());
    }
}