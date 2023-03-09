using System;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Vehicle.VehicleSpawner;

public class VehicleClassMenu : MenuBase
{
    private readonly VehicleClass _vehicleClass;

    public VehicleClassMenu(Enum @enum) : base(@enum)
    {
        _vehicleClass = (VehicleClass)@enum;
        AddVehicles();
    }

    private void AddVehicles()
    {
        foreach (var vehicleHash in GTA.Vehicle.GetAllModelsOfClass(_vehicleClass).OrderBy(v => v.ToPrettyString()))
        {
            var itemSpawnVehicle = AddItem(vehicleHash, () =>
            {
                // Call the service here and spawn vehicle.
            });
        }
    }
}