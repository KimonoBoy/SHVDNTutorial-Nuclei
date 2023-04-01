using GTA;
using GTA.UI;
using Nuclei.Services.Vehicle.VehicleSpawner;
using System;

namespace Nuclei.Scripts.Vehicle.VehicleSpawner;

public class VehicleSpawnerScript : Script
{
    private readonly VehicleSpawnerService _vehicleSpawnerService = VehicleSpawnerService.Instance;

    public VehicleSpawnerScript()
    {
        _vehicleSpawnerService.VehicleSpawned += OnVehicleSpawned;
    }

    private void OnVehicleSpawned(object sender, VehicleHash vehicleHash)
    {
        SpawnVehicle(vehicleHash);
    }

    /// <summary>
    ///     Spawns a Vehicle from the given VehicleHash.
    /// </summary>
    /// <param name="vehicleHash">The vehicle to be spawned.</param>
    private void SpawnVehicle(VehicleHash vehicleHash)
    {
        // Create a model from the VehicleHash and then request the associated asset.
        var vehicleModel = new Model(vehicleHash);
        vehicleModel.Request();
        
        // Ensure the model IsValid
        if (vehicleModel.IsValid && vehicleModel.IsInCdImage) 
        {
            // Wait for the model to load.
            while (!vehicleModel.IsLoaded) Yield();
        } else
        {
            Notification.Show($"Invalid Vehicle Model: {vehicleHash}");
            return;
        }

        // 1st parameter - Create the Vehicle from the model.
        // 2nd parameter - Position the Vehicle 5.0f in front of the player.
        // 3rd parameter - Set the Vehicle's heading to Character.Heading + 90 degrees. (Making the Driver seat be right infront of Player)
        var vehicle = World.CreateVehicle(vehicleModel,
            Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f,
            Game.Player.Character.Heading + 90);

        // Places the Vehicle on all wheels
        vehicle.PlaceOnGround();

        // Remove the model from resources.
        vehicleModel.MarkAsNoLongerNeeded();
    }
}