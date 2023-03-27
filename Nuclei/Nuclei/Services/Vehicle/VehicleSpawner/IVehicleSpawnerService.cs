using System;
using GTA;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public interface IVehicleSpawnerService
{
    event EventHandler<VehicleHash> VehicleSpawned;
    void SpawnVehicle(VehicleHash vehicleHash);
}