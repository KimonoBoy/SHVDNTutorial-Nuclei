using System;
using GTA;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class VehicleSpawnerService : IVehicleSpawnerService
{
    public static VehicleSpawnerService Instance = new();
    public BindableProperty<bool> EnginesRunning { get; } = new();
    public BindableProperty<bool> WarpInSpawned { get; } = new();
    public BindableProperty<VehicleSeat> VehicleSeat { get; } = new();

    public event EventHandler<VehicleHash> VehicleSpawned;

    public void SpawnVehicle(VehicleHash vehicleHash)
    {
        VehicleSpawned?.Invoke(this, vehicleHash);
    }
}