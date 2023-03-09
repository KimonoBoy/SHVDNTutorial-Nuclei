using System;
using Nuclei.Enums;
using Nuclei.UI.Menus.Abstracts;
using Nuclei.UI.Vehicle.VehicleSpawner;

namespace Nuclei.UI.Vehicle;

public class VehicleMenu : MenuBase
{
    public VehicleMenu(Enum @enum) : base(@enum)
    {
        AddVehicleSpawnerMenu();
    }

    private void AddVehicleSpawnerMenu()
    {
        var vehicleSpawnerMenu = new VehicleSpawnerMenu(MenuTitles.SpawnVehicle);
        AddMenu(vehicleSpawnerMenu);
    }
}