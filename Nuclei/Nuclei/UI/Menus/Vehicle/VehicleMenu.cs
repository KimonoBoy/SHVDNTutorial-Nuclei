using System;
using Nuclei.Enums.UI;
using Nuclei.UI.Menus.Abstracts;
using Nuclei.UI.Menus.Vehicle.VehicleSpawner;

namespace Nuclei.UI.Menus.Vehicle;

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