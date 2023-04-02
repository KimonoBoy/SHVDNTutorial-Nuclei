using GTA;
using Nuclei.Services.Vehicle.VehicleSpawner;
using System;
using Nuclei.Services.Exception;

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
        try
        {
            SpawnVehicle(vehicleHash);
        }
        catch (Exception ex)
        {
            ExceptionService.Instance.RaiseError($"Failed to Spawn Vehicle:\n\n{ex.Message}");
        }
        
    }

    /// <summary>
    ///     Spawns a Vehicle from the given VehicleHash.
    /// </summary>
    /// <param name="vehicleHash">The vehicle to be spawned.</param>
    private void SpawnVehicle(VehicleHash vehicleHash)
    {
        // Create a model from the VehicleHash.
        var vehicleModel = new Model(vehicleHash);

        // Ensure the model IsValid
        if (vehicleModel is { IsValid: true, IsInCdImage: true })
        {
            // Request the associated asset with a 1-second timeout.
            if (!vehicleModel.Request(1000))
            {
                throw new Exception($"Loading of vehicle model timed out: {vehicleHash}");
            }
        }
        else
        {
            throw new Exception($"Invalid Vehicle Model: {vehicleHash}");
        }

        // 1st parameter - Create the Vehicle from the model.
        // 2nd parameter - Position the Vehicle 5.0f in front of the player.
        // 3rd parameter - Set the Vehicle's heading to Character.Heading + 90 degrees. (Making the Driver seat be right in front of the Player)
        var vehicle = World.CreateVehicle(vehicleModel,
            Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f,
            Game.Player.Character.Heading + 90);

        // Ensure the vehicle is actually spawned
        if (vehicle != null && vehicle.Exists())
        {
            // Places the Vehicle on all wheels
            vehicle.PlaceOnGround();
        }
        else
        {
            throw new Exception($"Failed to Spawn the actual Vehicle object: {vehicleHash}");
        }

        // Remove the model from resources.
        vehicleModel.MarkAsNoLongerNeeded();
    }
}