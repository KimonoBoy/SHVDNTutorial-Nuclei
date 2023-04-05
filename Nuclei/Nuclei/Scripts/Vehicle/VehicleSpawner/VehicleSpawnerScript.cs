using System;
using GTA;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.VehicleSpawner;

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
    ///     Attempts to spawn a vehicle with the given VehicleHash up to a maximum number of attempts.
    /// </summary>
    /// <param name="vehicleHash">The VehicleHash of the vehicle to be spawned.</param>
    private void SpawnVehicle(VehicleHash vehicleHash)
    {
        const int maxAttempts = 3;
        var currentAttempt = 1;

        while (currentAttempt <= maxAttempts)
            try
            {
                throw new ArgumentOutOfRangeException();
                var vehicleModel = CreateVehicleModel(vehicleHash);
                var vehicle = CreateAndPositionVehicle(vehicleModel, vehicleHash);

                break;
            }
            catch (CustomExceptionBase vehicleSpawnerException)
            {
                currentAttempt++;

                if (currentAttempt == maxAttempts)
                {
                    ExceptionService.Instance.RaiseError(vehicleSpawnerException);
                    break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.Instance.RaiseError(ex);
                break;
            }
    }

    /// <summary>
    ///     Creates a Model object from the given VehicleHash and validates its existence in the game files.
    ///     Throws a VehicleModelNotFoundException if the model is invalid.
    ///     Throws a VehicleModelRequestTimedOutException if the model fails to load within the specified timeout.
    /// </summary>
    /// <param name="vehicleHash">The VehicleHash of the vehicle model to be created.</param>
    /// <returns>A validated Model object corresponding to the given VehicleHash.</returns>
    /// <exception cref="VehicleModelNotFoundException">Thrown when the model is invalid.</exception>
    /// <exception cref="VehicleModelRequestTimedOutException">
    ///     Thrown when the model fails to load within the specified
    ///     timeout.
    /// </exception>
    private Model CreateVehicleModel(VehicleHash vehicleHash)
    {
        // Create a model from the VehicleHash.
        var vehicleModel = new Model(vehicleHash);

        // Ensure the model IsValid and in CdImage
        if (vehicleModel is { IsValid: true, IsInCdImage: true })
        {
            // Request the associated asset with a 1-second timeout.
            if (!vehicleModel.Request(1000))
                throw new VehicleModelRequestTimedOutException($"Loading of vehicle model timed out: {vehicleHash}");
        }
        else
        {
            throw new VehicleModelNotFoundException($"Invalid Vehicle Model: {vehicleHash}");
        }

        return vehicleModel;
    }

    /// <summary>
    ///     Creates and positions a vehicle using the provided Model object.
    ///     Throws a VehicleSpawnFailedException if the vehicle object is not created successfully or does not exist.
    /// </summary>
    /// <param name="vehicleModel">The Model object of the vehicle to be created.</param>
    /// <param name="vehicleHash">The vehicleHash</param>
    /// <returns>The spawned vehicle.</returns>
    /// <exception cref="VehicleSpawnFailedException">
    ///     Thrown when the vehicle object is not created successfully or does not
    ///     exist.
    /// </exception>
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
            throw new VehicleSpawnFailedException($"Failed to spawn the actual vehicle object: {vehicleHash}");

        // Places the Vehicle on all wheels
        vehicle.PlaceOnGround();

        return vehicle;
    }
}