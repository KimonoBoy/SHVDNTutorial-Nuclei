using System;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Vehicle.VehicleSpawner;

public class VehicleClassMenu : MenuBase
{
    private readonly VehicleClass _vehicleClass;
    private readonly VehicleSpawnerService _vehicleSpawnerService = VehicleSpawnerService.Instance;

    public VehicleClassMenu(Enum @enum) : base(@enum)
    {
        _vehicleClass = (VehicleClass)@enum;
        AddVehicles();
    }

    private void AddVehicles()
    {
        foreach (var vehicleHash in GTA.Vehicle.GetAllModelsOfClass(_vehicleClass).OrderBy(v => v.ToPrettyString()))
        {
            var itemSpawnVehicle = AddItem(vehicleHash, () => { _vehicleSpawnerService.SpawnVehicle(vehicleHash); });
            itemSpawnVehicle.Description = $"Spawn {vehicleHash.ToPrettyString()}";
        }
    }
}