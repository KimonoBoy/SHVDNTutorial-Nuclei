using System;
using System.Drawing;
using GTA;
using Nuclei.Constants;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Vehicle.VehicleSpawner;

public class VehicleSpawnerScript : Script
{
    private readonly VehicleSpawnerService _vehicleSpawnerService = VehicleSpawnerService.Instance;
    private readonly Logger logger = new(Paths.LoggerPath);

    public VehicleSpawnerScript()
    {
        _vehicleSpawnerService.VehicleSpawned += OnVehicleSpawned;
        Tick += VehicleSpawnerScript_Tick;
    }

    private void VehicleSpawnerScript_Tick(object sender, EventArgs e)
    {
        Display.DrawTextElement($"VehicleSeat: {_vehicleSpawnerService.VehicleSeat.Value}", 100.0f, 150.0f,
            Color.AntiqueWhite);
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
        // Calculate the heading of the vehicle based on the player's heading.
        var heading = Game.Player.Character.Heading + 90;

        // If the player is spawning the vehicle, then set the heading to the player's heading.
        if (_vehicleSpawnerService.WarpInSpawned.Value)
            heading = Game.Player.Character.Heading;

        // Create the Vehicle from the model, position it 5.0f in front of the player, and set its heading.
        var vehicle = World.CreateVehicle(vehicleModel,
            Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f,
            heading);

        // Remove the model from resources.
        vehicleModel.MarkAsNoLongerNeeded();

        // Ensure the vehicle is actually spawned
        if (vehicle == null || !vehicle.Exists())
            throw new VehicleSpawnFailedException($"Failed to spawn the actual vehicle object: {vehicleHash}");

        // Places the Vehicle on all wheels
        vehicle.PlaceOnGround();

        if (_vehicleSpawnerService.EnginesRunning.Value)
            vehicle.IsEngineRunning = true;

        if (!_vehicleSpawnerService.WarpInSpawned.Value) return vehicle;

        Game.Player.Character.SetIntoVehicle(vehicle,
            vehicle.IsSeatFree(_vehicleSpawnerService.VehicleSeat.Value)
                ? _vehicleSpawnerService.VehicleSeat.Value
                : VehicleSeat.Driver);

        return vehicle;
    }
}