using System;
using System.Collections.ObjectModel;
using GTA;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public interface IVehicleSpawnerService
{
    // Properties
    BindableProperty<bool> WarpInSpawned { get; }
    BindableProperty<bool> EnginesRunning { get; }
    BindableProperty<VehicleSeat> VehicleSeat { get; }
    BindableProperty<ObservableCollection<VehicleHash>> FavoriteVehicles { get; }
    BindableProperty<ObservableCollection<CustomVehicle>> CustomVehicles { get; }

    // Events
    event EventHandler<VehicleHash> VehicleSpawned;

    // Methods
    void SpawnVehicle(VehicleHash vehicleHash);
}