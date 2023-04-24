using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Text;
using Control = GTA.Control;

namespace Nuclei.Scripts.Vehicle.VehicleSpawner;

public class VehicleSpawnerScript : GenericScriptBase<VehicleSpawnerService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.VehicleSpawned += OnVehicleSpawned;
        Service.CustomVehicleSpawned += OnCustomVehicleSpawned;
        KeyDown += OnKeyDown;
    }

    public override void UnsubscribeOnExit()
    {
        Tick -= OnTick;
        Service.VehicleSpawned -= OnVehicleSpawned;
        Service.CustomVehicleSpawned -= OnCustomVehicleSpawned;
        KeyDown -= OnKeyDown;
    }

    private void OnTick(object sender, EventArgs e)
    {
        Display.DrawTextElement(Service.VehicleSeat.ToString(), 100.0f, 100.0f, Color.AliceBlue);
    }

    private void OnCustomVehicleSpawned(object sender, CustomVehicleDto customVehicleDto)
    {
        var vehicle = SpawnVehicle(customVehicleDto.VehicleHash);
        vehicle.Mods.InstallModKit();
        vehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = true;
        vehicle.Mods.WheelType = customVehicleDto.WheelType;
        vehicle.Mods.RimColor = customVehicleDto.RimColor;
        vehicle.Mods.WindowTint = customVehicleDto.WindowTint;

        foreach (var customVehicleMod in customVehicleDto.VehicleMods)
            vehicle.Mods[customVehicleMod.VehicleModType].Index = customVehicleMod.ModIndex;

        vehicle.Mods[VehicleModType.FrontWheel].Variation = customVehicleDto.CustomTires;
        vehicle.Mods[VehicleModType.RearWheel].Variation = customVehicleDto.CustomTires;
        vehicle.Mods.TireSmokeColor = customVehicleDto.TireSmokeColor;

        vehicle.Mods.LicensePlate = customVehicleDto.LicensePlate;
        vehicle.Mods.LicensePlateStyle = customVehicleDto.LicensePlateStyle;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (Game.IsControlPressed(Control.Jump))
        {
            var vehicleDisplayName = Service.CurrentVehicleHash.GetLocalizedDisplayNameFromHash();
            if (Service.FavoriteVehicles.Contains(vehicleDisplayName.GetHashFromDisplayName<VehicleHash>()))
                Service.FavoriteVehicles.Remove(Service.CurrentVehicleHash);
            else
                Service.FavoriteVehicles.Add(Service.CurrentVehicleHash);

            var customVehicle =
                Service.CustomVehicles.FirstOrDefault(
                    x => x.VehicleHash == Service.CurrentVehicleHash);
            if (Service.CustomVehicles.Contains(customVehicle))
                Service.CustomVehicles.Remove(customVehicle);
        }

        if (e.KeyCode == Keys.NumPad1)
            SpawnVehicle(VehicleHash.Adder);
        else if (e.KeyCode == Keys.NumPad2)
            SpawnVehicle(VehicleHash.DeathBike);
        else if (e.KeyCode == Keys.NumPad3)
            SpawnVehicle(VehicleHash.SultanRS);
    }

    // Handles VehicleSpawned event by spawning the corresponding vehicleDto
    private void OnVehicleSpawned(object sender, VehicleHash vehicleHash)
    {
        SpawnVehicle(vehicleHash);
    }

    /// <summary>
    ///     Spawns a vehicleDto with the given VehicleHash at the player's current position.
    /// </summary>
    /// <param name="vehicleHash">The VehicleHash of the vehicleDto to be spawned.</param>
    private GTA.Vehicle SpawnVehicle(VehicleHash vehicleHash)
    {
        const int maxAttempts = 3;
        var currentAttempt = 1;

        // Attempt to spawn the vehicleDto up to a maximum number of attempts.
        while (currentAttempt <= maxAttempts)
            try
            {
                var vehicleModel = CreateVehicleModel(vehicleHash);
                var vehicle = CreateAndPositionVehicle(vehicleModel, vehicleHash);
                return vehicle;
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

        return null;
    }

    /// <summary>
    ///     Creates a Model object from the given VehicleHash and validates its existence in the game files.
    /// </summary>
    /// <param name="vehicleHash">The VehicleHash of the vehicleDto model to be created.</param>
    /// <returns>A validated Model object corresponding to the given VehicleHash.</returns>
    /// <exception cref="VehicleModelNotFoundException">Thrown when the model is invalid.</exception>
    /// <exception cref="VehicleModelRequestTimedOutException">
    ///     Thrown when the model fails to load within the specified
    ///     timeout.
    /// </exception>
    private Model CreateVehicleModel(VehicleHash vehicleHash)
    {
        // Create a Model object from the vehicleHash.
        var vehicleModel =
            new Model(vehicleHash); // Extremely important to create the Model object in order to validate it for garages.

        // Validate the model and check if it exists in the game files.
        if (vehicleModel is { IsValid: true, IsInCdImage: true })
        {
            // Request the model and wait up to 1000 ms for it to load.
            if (!vehicleModel.Request(1000))
                throw new VehicleModelRequestTimedOutException($"Loading of vehicleDto model timed out: {vehicleHash}");
        }
        else
        {
            throw new VehicleModelNotFoundException($"Invalid Vehicle Model: {vehicleHash}");
        }

        return vehicleModel;
    }

    /// <summary>
    ///     Creates and positions a vehicleDto using the provided Model object.
    /// </summary>
    /// <param name="vehicleModel">The Model object of the vehicleDto to be created.</param>
    /// <param name="vehicleHash">The vehicleHash</param>
    /// <returns>The spawned vehicleDto.</returns>
    /// <exception cref="VehicleSpawnFailedException">
    ///     Thrown when the vehicleDto object is not created successfully or does not
    ///     exist.
    /// </exception>
    private GTA.Vehicle CreateAndPositionVehicle(Model vehicleModel, VehicleHash vehicleHash)
    {
        // Calculate the heading of the vehicleDto based on the player's heading.
        var heading = GetVehicleHeading();

        // Create the Vehicle from the model, position it in front of the player, and set its heading.
        var vehicle = World.CreateVehicle(vehicleModel, GetVehiclePosition(), heading);

        // Release the vehicleDto model resources.
        vehicleModel.MarkAsNoLongerNeeded();

        // Set the vehicleDto as persistent so it doesn't despawn.
        vehicle.IsPersistent = true;

        if (!vehicle.Exists())
            throw new VehicleSpawnFailedException($"Failed to spawn the actual vehicleDto object: {vehicleHash}");

        // Set the vehicleDto's properties and place the player inside if necessary.
        InitializeVehicle(vehicle);

        return vehicle;
    }

    /// <summary>
    ///     Calculates the vehicleDto heading based on the player's heading and the WarpInSpawned setting.
    /// </summary>
    /// <returns>A float value representing the vehicleDto heading.</returns>
    private float GetVehicleHeading()
    {
        if (!Service.WarpInSpawned) return Game.Player.Character.Heading + 90.0f;

        return Game.Player.Character.Heading;
    }

    /// <summary>
    ///     Calculates the vehicleDto position in front of the player.
    /// </summary>
    /// <returns>A Vector3 value representing the vehicleDto position.</returns>
    private Vector3 GetVehiclePosition()
    {
        return Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f;
    }

    /// <summary>
    ///     Sets the vehicleDto's properties and places the player inside if WarpInSpawned is enabled.
    /// </summary>
    /// <param name="vehicle">The GTA.Vehicle object to initialize.</param>
    private void InitializeVehicle(GTA.Vehicle vehicle)
    {
        vehicle.PlaceOnGround();

        if (Service.EnginesRunning) vehicle.IsEngineRunning = true;

        vehicle.PreviouslyOwnedByPlayer = true;

        if (!Service.WarpInSpawned) return;

        var preferredSeat = Service.VehicleSeat;

        Game.Player.Character.SetIntoVehicle(vehicle,
            vehicle.IsSeatFree(preferredSeat) ? preferredSeat : VehicleSeat.Driver);
    }
}