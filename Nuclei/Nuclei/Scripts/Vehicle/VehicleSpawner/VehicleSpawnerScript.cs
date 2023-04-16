using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Control = GTA.Control;

namespace Nuclei.Scripts.Vehicle.VehicleSpawner;

public class VehicleSpawnerScript : GenericScriptBase<VehicleSpawnerService>
{
    protected override void SubscribeToEvents()
    {
        Service.VehicleSpawned += OnVehicleSpawned;
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>()
                .Contains(Service.CurrentVehicleHash.Value) && Game.IsControlPressed(Control.Jump))
        {
            if (Service.FavoriteVehicles.Value.Contains(Service.CurrentVehicleHash.Value))
                Service.FavoriteVehicles.Value.Remove(Service.CurrentVehicleHash.Value);
            else
                Service.FavoriteVehicles.Value.Add(Service.CurrentVehicleHash.Value);
        }
    }

    // Handles VehicleSpawned event by spawning the corresponding vehicle
    private void OnVehicleSpawned(object sender, VehicleHash vehicleHash)
    {
        SpawnVehicle(vehicleHash);
    }

    /// <summary>
    ///     Spawns a vehicle with the given VehicleHash at the player's current position.
    /// </summary>
    /// <param name="vehicleHash">The VehicleHash of the vehicle to be spawned.</param>
    private void SpawnVehicle(VehicleHash vehicleHash)
    {
        const int maxAttempts = 3;
        var currentAttempt = 1;

        // Attempt to spawn the vehicle up to a maximum number of attempts.
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
                    ExceptionService.RaiseError(vehicleSpawnerException);
                    break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.RaiseError(ex);
                break;
            }
    }

    /// <summary>
    ///     Creates a Model object from the given VehicleHash and validates its existence in the game files.
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
        // Create a Model object from the vehicleHash.
        Model vehicleModel = new(vehicleHash);

        // Validate the model and check if it exists in the game files.
        if (vehicleModel is { IsValid: true, IsInCdImage: true })
        {
            // Request the model and wait up to 1000 ms for it to load.
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
        var heading = GetVehicleHeading();

        // Create the Vehicle from the model, position it in front of the player, and set its heading.
        var vehicle = World.CreateVehicle(vehicleModel, GetVehiclePosition(), heading);

        // Release the vehicle model resources.
        vehicleModel.MarkAsNoLongerNeeded();

        if (vehicle == null || !vehicle.Exists())
            throw new VehicleSpawnFailedException($"Failed to spawn the actual vehicle object: {vehicleHash}");

        // Set the vehicle's properties and place the player inside if necessary.
        InitializeVehicle(vehicle);

        return vehicle;
    }

    /// <summary>
    ///     Calculates the vehicle heading based on the player's heading and the WarpInSpawned setting.
    /// </summary>
    /// <returns>A float value representing the vehicle heading.</returns>
    private float GetVehicleHeading()
    {
        if (!Service.WarpInSpawned.Value) return Game.Player.Character.Heading + 90.0f;

        return Game.Player.Character.Heading;
    }

    /// <summary>
    ///     Calculates the vehicle position in front of the player.
    /// </summary>
    /// <returns>A Vector3 value representing the vehicle position.</returns>
    private Vector3 GetVehiclePosition()
    {
        return Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f;
    }

    /// <summary>
    ///     Sets the vehicle's properties and places the player inside if WarpInSpawned is enabled.
    /// </summary>
    /// <param name="vehicle">The GTA.Vehicle object to initialize.</param>
    private void InitializeVehicle(GTA.Vehicle vehicle)
    {
        vehicle.PlaceOnGround();

        if (Service.EnginesRunning.Value) vehicle.IsEngineRunning = true;

        if (!Service.WarpInSpawned.Value) return;

        var preferredSeat = Service.VehicleSeat.Value;

        Game.Player.Character.SetIntoVehicle(vehicle,
            vehicle.IsSeatFree(preferredSeat) ? preferredSeat : VehicleSeat.Driver);
    }
}