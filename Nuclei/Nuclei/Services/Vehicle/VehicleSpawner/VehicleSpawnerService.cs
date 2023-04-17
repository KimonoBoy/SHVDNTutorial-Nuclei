using System;
using System.Collections.ObjectModel;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class VehicleSpawnerService : GenericService<VehicleSpawnerService>
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
    ///     Gets the VehicleHash from a display name string.
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public VehicleHash GetVehicleHashFromDisplayName(string displayName)
    {
        return Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().ToList().Find(vH =>
            vH.ToPrettyString() == displayName || displayName == Game.GetLocalizedString(vH.ToString()));
    }

    /// <summary>
    ///     Gets the localized dispaly name from a VehicleHash.
    /// </summary>
    /// <param name="vehicleHash"></param>
    /// <returns></returns>
    public string GetVehicleDisplayName(VehicleHash vehicleHash)
    {
        var localizedString = Game.GetLocalizedString(vehicleHash.ToString());

        if (string.IsNullOrEmpty(localizedString)) return vehicleHash.ToPrettyString();

        return localizedString;
    }


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