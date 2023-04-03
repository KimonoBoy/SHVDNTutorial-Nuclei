using System;
using GTA;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerMenu : MenuBase
{
    public VehicleSpawnerMenu(Enum @enum) : base(@enum)
    {
        GenerateVehicleClassMenus();
    }

    private void GenerateVehicleClassMenus()
    {
        foreach (VehicleClass vehicleClass in Enum.GetValues(typeof(VehicleClass)))
        {
            var vehicleClassMenu = new VehicleClassMenu(vehicleClass);
            var vehicleClassItem = AddMenu(vehicleClassMenu);
        }
    }
}