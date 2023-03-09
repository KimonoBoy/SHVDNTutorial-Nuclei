using System;
using GTA;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class VehicleSpawnerService : IVehicleSpawnerService
{
    public static VehicleSpawnerService Instance = new();
    public event EventHandler<VehicleHash> VehicleSpawned;

    public void SpawnVehicle(VehicleHash vehicleHash)
    {
        VehicleSpawned?.Invoke(this, vehicleHash);
    }
}