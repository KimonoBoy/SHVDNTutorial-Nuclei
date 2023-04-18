using System;
using System.Collections.ObjectModel;
using GTA;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class VehicleSpawnerService : GenericService<VehicleSpawnerService>, IVehicleSpawnerService
{
    /// <summary>
    ///     A property that contains the current selected vehicle hash.
    /// </summary>
    public BindableProperty<VehicleHash> CurrentVehicleHash { get; set; } = new();

    /// <summary>
    ///     A property that indicates whether the spawned vehicle should have its engines running.
    /// </summary>
    public BindableProperty<bool> EnginesRunning { get; set; } = new();

    /// <summary>
    ///     A property that indicates whether the spawned vehicle should be warped into.
    /// </summary>
    public BindableProperty<bool> WarpInSpawned { get; set; } = new();

    /// <summary>
    ///     A property that indicates which seat the player should be placed at when spawning a vehicle.
    /// </summary>
    public BindableProperty<VehicleSeat> VehicleSeat { get; set; } =
        new(GTA.VehicleSeat.Driver);

    /// <summary>
    ///     A property that contains the list of favorite vehicles.
    /// </summary>
    public BindableProperty<ObservableCollection<VehicleHash>> FavoriteVehicles { get; set; } =
        new(new ObservableCollection<VehicleHash>());


    /// <summary>
    ///     An event that is raised when a vehicle is spawned.
    /// </summary>
    public event EventHandler<VehicleHash> VehicleSpawned;

    /// <summary>
    ///     Spawns a vehicle with the given VehicleHash.
    /// </summary>
    /// <param name="vehicleHash"></param>
    public void SpawnVehicle(VehicleHash vehicleHash)
    {
        VehicleSpawned?.Invoke(this, vehicleHash);
    }
}