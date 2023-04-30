using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Control = GTA.Control;

namespace Nuclei.Scripts.Vehicle.VehicleSpawner;

public class VehicleSpawnerScript : GenericScriptBase<VehicleSpawnerService>
{
    protected override void UpdateServiceStatesTimer(object sender, EventArgs e)
    {
    }

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.VehicleSpawned += OnVehicleSpawned;
        Service.CustomVehicleSpawned += OnCustomVehicleSpawned;
        KeyDown += OnKeyDown;
    }

    protected override void UnsubscribeOnExit()
    {
        Tick -= OnTick;
        Service.VehicleSpawned -= OnVehicleSpawned;
        Service.CustomVehicleSpawned -= OnCustomVehicleSpawned;
        KeyDown -= OnKeyDown;
    }

    protected override void ProcessGameStatesTimer(object sender, EventArgs e)
    {
    }

    private void OnTick(object sender, EventArgs e)
    {
    }

    private void OnCustomVehicleSpawned(object sender, CustomVehicleDto customVehicleDto)
    {
        var vehicle = SpawnVehicle(customVehicleDto.VehicleHash);
        // Install modkit first, otherwise the game will crash
        vehicle.Mods.InstallModKit();
        vehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = true;

        // Wheels
        vehicle.Mods.WheelType = customVehicleDto.WheelType;
        vehicle.Mods[VehicleModType.FrontWheel].Variation = customVehicleDto.CustomTires;
        vehicle.Mods[VehicleModType.RearWheel].Variation = customVehicleDto.CustomTires;

        // Mod Types
        foreach (var customVehicleMod in customVehicleDto.VehicleMods)
            vehicle.Mods[customVehicleMod.VehicleModType].Index = customVehicleMod.ModIndex;

        // License Plate
        vehicle.Mods.LicensePlate = customVehicleDto.LicensePlate;
        vehicle.Mods.LicensePlateStyle = customVehicleDto.LicensePlateStyle;

        // Toggles
        vehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = customVehicleDto.XenonHeadLights;
        vehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = customVehicleDto.Turbo;
        VehicleModsService.Instance.RainbowMode = customVehicleDto.RainbowMode;

        switch (customVehicleDto.NeonLightsLayout)
        {
            case NeonLightsLayout.Front:
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, false);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                break;
            case NeonLightsLayout.Back:
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, false);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                break;
            case NeonLightsLayout.FrontAndBack:
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                break;
            case NeonLightsLayout.Sides:
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, false);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, false);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
                break;
            case NeonLightsLayout.FrontBackAndSides:
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
                vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Colors and Tints
        vehicle.Mods.NeonLightsColor = VehicleModsService.Instance.NeonLightsColorDictionary
            .FirstOrDefault(neonLightsColor => neonLightsColor.Key == customVehicleDto.NeonLightsColor).Value;
        vehicle.Mods.WindowTint = customVehicleDto.WindowTint;
        vehicle.Mods.PrimaryColor = customVehicleDto.PrimaryColor;
        vehicle.Mods.SecondaryColor = customVehicleDto.SecondaryColor;
        vehicle.Mods.PearlescentColor = customVehicleDto.PearlescentColor;
        vehicle.Mods.RimColor = customVehicleDto.RimColor;
        vehicle.Mods.TireSmokeColor = VehicleModsService.Instance.TireSmokeColorDictionary
            .FirstOrDefault(color => color.Key == customVehicleDto.TireSmokeColor).Value;
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

    // Handles VehicleSpawned event by spawning the corresponding vehicle
    private void OnVehicleSpawned(object sender, VehicleHash vehicleHash)
    {
        SpawnVehicle(vehicleHash);
    }

    /// <summary>
    ///     Spawns a vehicle with the given VehicleHash at the player's current position.
    /// </summary>
    /// <param name="vehicleHash">The VehicleHash of the vehicle to be spawned.</param>
    private GTA.Vehicle SpawnVehicle(VehicleHash vehicleHash)
    {
        const int maxAttempts = 3;
        var currentAttempt = 1;

        // Attempt to spawn the vehicle up to a maximum number of attempts.
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
        var vehicleModel =
            new Model(vehicleHash); // Extremely important to create the Model object in order to validate it for garages.

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

        // Set the vehicle as persistent so it doesn't despawn.
        // vehicle.IsPersistent = true;

        if (!vehicle.Exists())
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
        if (!Service.WarpInSpawned) return Game.Player.Character.Heading + 90.0f;

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

        if (Service.EnginesRunning) vehicle.IsEngineRunning = true;

        vehicle.PreviouslyOwnedByPlayer = true;

        if (!Service.WarpInSpawned) return;

        var preferredSeat = Service.VehicleSeat;

        Game.Player.Character.SetIntoVehicle(vehicle,
            vehicle.IsSeatFree(preferredSeat) ? preferredSeat : VehicleSeat.Driver);
    }
}