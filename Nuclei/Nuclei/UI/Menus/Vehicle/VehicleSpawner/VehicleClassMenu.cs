using System;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleClassMenu : GenericMenuBase<VehicleSpawnerService>
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
            var itemSpawnVehicle = AddItem(vehicleHash, () => { Service.SpawnVehicle(vehicleHash); });
            itemSpawnVehicle.Description = $"Spawn {vehicleHash.ToPrettyString()}";
        }
    }
}