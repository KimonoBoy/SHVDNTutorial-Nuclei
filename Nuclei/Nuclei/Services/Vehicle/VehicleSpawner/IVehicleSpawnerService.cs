using System;
using GTA;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public interface IVehicleSpawnerService
{
    // Properties
    BindableProperty<bool> WarpInSpawned { get; }
    BindableProperty<bool> EnginesRunning { get; }
    BindableProperty<VehicleSeat> VehicleSeat { get; }

    // Events
    event EventHandler<VehicleHash> VehicleSpawned;

    // Methods
    void SpawnVehicle(VehicleHash vehicleHash);
}