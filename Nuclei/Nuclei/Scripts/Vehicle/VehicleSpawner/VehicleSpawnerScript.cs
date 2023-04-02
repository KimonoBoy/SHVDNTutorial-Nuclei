using GTA;
using Nuclei.Services.Vehicle.VehicleSpawner;
using System;
using System.Windows.Forms;
using GTA.UI;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;

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
        const int maxAttempts = 3;
        int currentAttempt = 1;

        while (currentAttempt <= maxAttempts)
        {
            try
            {
                var vehicleModel = CreateVehicleModel(vehicleHash);
                var vehicle = CreateAndPositionVehicle(vehicleModel, vehicleHash);

                if (vehicle != null)
                {
                    // Places the Vehicle on all wheels
                    vehicle.PlaceOnGround();
                    break; // Exit the loop since the vehicle was successfully spawned
                }
            }
            catch (VehicleException vehicleSpawnerException)
            {
                currentAttempt++;

                if (currentAttempt == maxAttempts)
                {
                    ExceptionService.Instance.RaiseError($"{vehicleSpawnerException}");
                    // Logging will be implemented later.
                    break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.Instance.RaiseError($"Something went wrong:\n\n{ex.Message}\n\nSee log for more info!");
                // Logging will be implemented later.
                break;
            }
        }
    }

    private Model CreateVehicleModel(VehicleHash vehicleHash)
    {
        // Create a model from the VehicleHash.
        var vehicleModel = new Model(vehicleHash);

        // Ensure the model IsValid and in CdImage
        if (vehicleModel is { IsValid: true, IsInCdImage: true })
        {
            // Request the associated asset with a 1-second timeout.
            if (!vehicleModel.Request(1000))
            {
                throw new VehicleModelRequestTimedOutException($"Loading of vehicle model timed out: {vehicleHash}");
            }
        }
        else
        {
            throw new VehicleModelNotFoundException($"Invalid Vehicle Model: {vehicleHash}");
        }

        return vehicleModel;
    }

    private GTA.Vehicle CreateAndPositionVehicle(Model vehicleModel, VehicleHash vehicleHash)
    {
        // Create the Vehicle from the model, position it 5.0f in front of the player, and set its heading.
        var vehicle = World.CreateVehicle(vehicleModel,
            Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f,
            Game.Player.Character.Heading + 90);

        // Remove the model from resources.
        vehicleModel.MarkAsNoLongerNeeded();

        // Ensure the vehicle is actually spawned
        if (vehicle == null || !vehicle.Exists())
        {
            throw new VehicleSpawnFailedException($"Failed to spawn the actual vehicle object: {vehicleHash}");
        }

        return vehicle;
    }
}