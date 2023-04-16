using System;
using System.Collections.ObjectModel;
using GTA;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public interface IVehicleSpawnerService
{
    // Properties
    BindableProperty<bool> WarpInSpawned { get; }
    BindableProperty<bool> EnginesRunning { get; }
    BindableProperty<VehicleSeat> VehicleSeat { get; }
    BindableProperty<ObservableCollection<VehicleHash>> FavoriteVehicles { get; }

    // Events
    event EventHandler<VehicleHash> VehicleSpawned;

    // Methods
    void SpawnVehicle(VehicleHash vehicleHash);
}