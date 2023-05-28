using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using Nuclei.Enums.Hotkey;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.Dtos;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.Services.Vehicle.VehicleSpawner;

namespace Nuclei.Scripts.Vehicle.Spawner;

public class VehicleSpawnerScript : GenericScript<VehicleSpawnerService>
{
    protected override void SubscribeToEvents()
    {
        Service.VehicleSpawned += OnVehicleSpawned;
        Service.CustomVehicleSpawned += OnCustomVehicleSpawned;
        KeyDown += OnKeyDown;
    }

    protected override void UnsubscribeOnExit()
    {
        Service.VehicleSpawned -= OnVehicleSpawned;
        Service.CustomVehicleSpawned -= OnCustomVehicleSpawned;
        KeyDown -= OnKeyDown;
    }

    protected override void OnTick(object sender, EventArgs e)
    {
    }

    private void OnVehicleSpawned(object sender, VehicleHash vehicleHash)
    {
        SpawnVehicle(vehicleHash);
    }

    private void OnCustomVehicleSpawned(object sender, CustomVehicleDto customVehicleDto)
    {
        SpawnVehicle(customVehicleDto);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        var updateCollectionKey = Service.Hotkeys.GetValue(SectionName.Menu, MenuTitle.UpdateCollection);
        if (Service.Hotkeys.IsKeyPressed(updateCollectionKey))
        {
            UpdateFavoriteVehicles();
            UpdateSavedVehicles();
        }

        SpawnVehicleOnKey(e.KeyCode);
    }

    /// <summary>
    ///     Spawns a vehicle based on the given <paramref name="vehicleHash" />.
    /// </summary>
    /// <param name="vehicleHash">The hash of the vehicle to spawn.</param>
    /// <returns>The spawned vehicle object.</returns>
    private GTA.Vehicle SpawnVehicle(VehicleHash vehicleHash)
    {
        const int maxAttempts = 3;
        var currentAttempt = 1;

        // Attempt to spawn the vehicle up to a maximum number of attempts.
        while (currentAttempt <= maxAttempts)
            try
            {
                // Load the vehicle model.
                var vehicleModel = LoadVehicleModel(vehicleHash);

                // Create and position the vehicle.
                var vehicle = CreateAndPositionVehicle(vehicleModel, vehicleHash);

                // Return the spawned vehicle.
                return vehicle;
            }
            catch (CustomExceptionBase vehicleSpawnerException)
            {
                currentAttempt++;

                // If the maximum number of attempts has been reached, raise an error.
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
    ///     Spawns a custom vehicle based on the given <paramref name="customVehicleDto" />.
    /// </summary>
    /// <param name="customVehicleDto">The custom vehicle data for the spawned vehicle.</param>
    private void SpawnVehicle(CustomVehicleDto customVehicleDto)
    {
        var vehicle = SpawnVehicle(customVehicleDto.VehicleHash);
        InstallModKits(vehicle);
        LoadMods(vehicle, customVehicleDto);
        LoadToggleMods(vehicle, customVehicleDto);
        LoadNeonLights(vehicle, customVehicleDto.NeonLightsLayout);
        LoadColors(vehicle, customVehicleDto);
    }

    /// <summary>
    ///     Spawns a vehicle based on the given <paramref name="keyCode" />.
    /// </summary>
    /// <param name="keyCode">The key code representing the vehicle to spawn.</param>
    private void SpawnVehicleOnKey(Keys keyCode)
    {
        VehicleHash vehicleHash;
        switch (keyCode)
        {
            case Keys.NumPad1:
                vehicleHash = VehicleHash.Adder;
                break;
            case Keys.NumPad2:
                vehicleHash = VehicleHash.DeathBike;
                break;
            case Keys.NumPad3:
                vehicleHash = VehicleHash.SultanRS;
                break;
            default:
                return;
        }

        SpawnVehicle(vehicleHash);
    }

    /// <summary>
    ///     Loads the model for the given <paramref name="vehicleHash" /> and validates it.
    /// </summary>
    /// <param name="vehicleHash">The hash of the vehicle to load.</param>
    /// <returns>The loaded model.</returns>
    private Model LoadVehicleModel(VehicleHash vehicleHash)
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
    ///     Creates and positions a vehicle based on the given <paramref name="vehicleModel" /> and
    ///     <paramref name="vehicleHash" />.
    /// </summary>
    /// <param name="vehicleModel">The model of the vehicle to create.</param>
    /// <param name="vehicleHash">The hash of the vehicle to create.</param>
    /// <returns>The created and positioned vehicle.</returns>
    private GTA.Vehicle CreateAndPositionVehicle(Model vehicleModel, VehicleHash vehicleHash)
    {
        // Calculate the heading of the vehicle based on the player's heading.
        var heading = GetVehicleHeading();

        // Create the Vehicle from the model, position it in front of the player, and set its heading.
        var vehicle = GTA.World.CreateVehicle(vehicleModel, GetVehiclePosition(), heading);

        // Release the vehicle model resources.
        vehicleModel.MarkAsNoLongerNeeded();

        if (!vehicle.Exists())
            throw new VehicleSpawnFailedException(
                $"Failed to spawn the actual vehicle object: {vehicleHash}");

        // Set the vehicle's properties and place the player inside if necessary.
        InitializeVehicle(vehicle);

        return vehicle;
    }

    /// <summary>
    ///     Returns the heading for a vehicle to be spawned based on the player's heading.
    /// </summary>
    /// <returns>The vehicle heading.</returns>
    private float GetVehicleHeading()
    {
        if (!Service.WarpInSpawned) return Game.Player.Character.Heading + 90.0f;

        return Game.Player.Character.Heading;
    }

    /// <summary>
    ///     Returns the position in front of the player to spawn a vehicle.
    /// </summary>
    /// <returns>The vehicle spawn position.</returns>
    private Vector3 GetVehiclePosition()
    {
        return Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f;
    }

    /// <summary>
    ///     Initializes the given <paramref name="vehicle" /> by setting its properties and placing the player inside.
    /// </summary>
    /// <param name="vehicle">The vehicle to initialize.</param>
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

    /// <summary>
    ///     Updates the list of saved vehicles to remove the current vehicle if it exists.
    /// </summary>
    private void UpdateSavedVehicles()
    {
        var customVehicleDto =
            Service.CustomVehicles.FirstOrDefault(
                customVehicleDto => customVehicleDto.VehicleHash == Service.CurrentVehicleHash);
        if (Service.CustomVehicles.Contains(customVehicleDto))
            Service.CustomVehicles.Remove(customVehicleDto);
    }

    /// <summary>
    ///     Updates the list of favorite vehicles to add or remove the current vehicle.
    /// </summary>
    private void UpdateFavoriteVehicles()
    {
        if (Service.FavoriteVehicles.Contains(Service.CurrentVehicleHash))
            Service.FavoriteVehicles.Remove(Service.CurrentVehicleHash);
        else
            Service.FavoriteVehicles.Add(Service.CurrentVehicleHash);
    }

    /// <summary>
    ///     Installs mod kits on the given <paramref name="vehicle" />.
    /// </summary>
    /// <param name="vehicle">The vehicle to install the mod kits on.</param>
    private static void InstallModKits(GTA.Vehicle vehicle)
    {
        vehicle.Mods.InstallModKit();
        vehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = true;
    }

    /// <summary>
    ///     Loads the mods for the given <paramref name="vehicle" /> based on the <paramref name="customVehicleDto" />.
    /// </summary>
    /// <param name="vehicle">The vehicle to load the mods on.</param>
    /// <param name="customVehicleDto">The custom vehicle data.</param>
    private static void LoadMods(GTA.Vehicle vehicle, CustomVehicleDto customVehicleDto)
    {
        vehicle.Mods.WheelType = customVehicleDto.WheelType;
        foreach (var customVehicleMod in customVehicleDto.VehicleMods)
            vehicle.Mods[customVehicleMod.VehicleModType].Index = customVehicleMod.ModIndex;
        vehicle.Mods.LicensePlate = customVehicleDto.LicensePlate;
        vehicle.Mods.LicensePlateStyle = customVehicleDto.LicensePlateStyle;
        vehicle.Mods[VehicleModType.FrontWheel].Variation = customVehicleDto.CustomTires;
        vehicle.Mods[VehicleModType.RearWheel].Variation = customVehicleDto.CustomTires;
    }

    /// <summary>
    ///     Loads the toggle mods for the given <paramref name="vehicle" /> based on the <paramref name="customVehicleDto" />.
    /// </summary>
    /// <param name="vehicle">The vehicle to load the toggle mods on.</param>
    /// <param name="customVehicleDto">The custom vehicle data.</param>
    private static void LoadToggleMods(GTA.Vehicle vehicle, CustomVehicleDto customVehicleDto)
    {
        vehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = customVehicleDto.XenonHeadLights;
        vehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = customVehicleDto.Turbo;
    }

    /// <summary>
    ///     Loads the neon lights for the given <paramref name="vehicle" /> based on the <paramref name="layout" />.
    /// </summary>
    /// <param name="vehicle">The vehicle to load the neon lights on.</param>
    /// <param name="layout">The neon lights layout to use.</param>
    private void LoadNeonLights(GTA.Vehicle vehicle, NeonLightsLayout layout)
    {
        bool front = false, back = false, left = false, right = false;

        switch (layout)
        {
            case NeonLightsLayout.Front:
                front = true;
                break;
            case NeonLightsLayout.Back:
                back = true;
                break;
            case NeonLightsLayout.FrontAndBack:
                front = back = true;
                break;
            case NeonLightsLayout.Sides:
                left = right = true;
                break;
            case NeonLightsLayout.FrontBackAndSides:
                front = back = left = right = true;
                break;
            case NeonLightsLayout.Off:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layout), layout, null);
        }

        vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, front);
        vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, back);
        vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, left);
        vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, right);
    }

    /// <summary>
    ///     Loads the colors for the given <paramref name="vehicle" /> based on the <paramref name="customVehicleDto" />.
    /// </summary>
    /// <param name="vehicle">The vehicle to load the colors on.</param>
    /// <param name="customVehicleDto">The custom vehicle data.</param>
    private static void LoadColors(GTA.Vehicle vehicle, CustomVehicleDto customVehicleDto)
    {
        vehicle.Mods.NeonLightsColor = VehicleModsService.Instance.NeonLightsColorDictionary
            .FirstOrDefault(neonLightsColor => neonLightsColor.Key == customVehicleDto.NeonLightsColor).Value;
        vehicle.Mods.WindowTint = customVehicleDto.WindowTint;
        vehicle.Mods.PrimaryColor = customVehicleDto.PrimaryColor;
        vehicle.Mods.SecondaryColor = customVehicleDto.SecondaryColor;
        vehicle.Mods.PearlescentColor = customVehicleDto.PearlescentColor;
        vehicle.Mods.RimColor = customVehicleDto.RimColor;
        vehicle.Mods.TireSmokeColor = VehicleModsService.Instance.TireSmokeColorDictionary
            .FirstOrDefault(color => color.Key == customVehicleDto.TireSmokeColor).Value;
        VehicleModsService.Instance.RainbowMode = customVehicleDto.RainbowMode;
    }
}