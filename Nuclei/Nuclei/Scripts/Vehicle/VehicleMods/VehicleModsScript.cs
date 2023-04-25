using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using GTA;
using Nuclei.Enums.Vehicle;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleMods;

namespace Nuclei.Scripts.Vehicle.VehicleMods;

public class VehicleModsScript : GenericScriptBase<VehicleModsService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.LicensePlateInputRequested += OnLicensePlateInputRequested;
        Service.RandomizeModsRequested += OnRandomizeModsRequested;
        Service.PropertyChanged += OnPropertyChanged;
    }

    public override void UnsubscribeOnExit()
    {
        Tick -= OnTick;
        Service.LicensePlateInputRequested -= OnLicensePlateInputRequested;
        Service.RandomizeModsRequested -= OnRandomizeModsRequested;
        Service.PropertyChanged -= OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Service.CurrentVehicle == null) return;

        if (e.PropertyName == nameof(Service.CurrentVehicle))
        {
            InstallModKit();
            UpdateFeature(() => Service.ValidVehicleModTypes, UpdateValidModTypes);
            UpdateFeature(() => Service.ValidWheelTypes, UpdateValidWheelTypes);
            UpdateFeature(() => Service.LicensePlate, UpdateLicensePlate);
            UpdateFeature(() => Service.LicensePlateStyle, UpdateLicensePlateStyle);
            UpdateFeature(() => Service.CurrentWheelType, UpdateWheelType);
            UpdateFeature(() => Service.CurrentRimColor, UpdateRimColor);
            UpdateFeature(() => Service.CurrentCustomTires, UpdateCustomTires);
            UpdateFeature(() => Service.CurrentTireSmokeColor, UpdateTireSmokeColor);
            UpdateFeature(() => Service.CurrentNeonLightsLayout, UpdateNeonLightsLayout);
            // UpdateFeature(() => Service.CurrentWindowTint, UpdateWindowTint);
        }

        if (e.PropertyName == nameof(Service.LicensePlate))
        {
            if (Service.LicensePlate == CurrentVehicle.Mods.LicensePlate) return;
            CurrentVehicle.Mods.LicensePlate = Service.LicensePlate;
        }

        if (e.PropertyName == nameof(Service.LicensePlateStyle))
        {
            if (Service.LicensePlateStyle == CurrentVehicle.Mods.LicensePlateStyle) return;

            CurrentVehicle.Mods.LicensePlateStyle = Service.LicensePlateStyle;
        }

        if (e.PropertyName == nameof(Service.CurrentWheelType))
        {
            if (Service.CurrentWheelType == CurrentVehicle.Mods.WheelType) return;

            CurrentVehicle.Mods.WheelType = Service.CurrentWheelType;
        }

        if (e.PropertyName == nameof(Service.CurrentRimColor))
        {
            if (Service.CurrentRimColor == CurrentVehicle.Mods.RimColor) return;
            CurrentVehicle.Mods.RimColor = Service.CurrentRimColor;
        }

        if (e.PropertyName == nameof(Service.CurrentCustomTires))
        {
            if (Service.CurrentCustomTires == CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation) return;
            CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation = Service.CurrentCustomTires;
            CurrentVehicle.Mods[VehicleModType.RearWheel].Variation = Service.CurrentCustomTires;
        }

        if (e.PropertyName == nameof(Service.CurrentTireSmokeColor))
        {
            if (Service.CurrentTireSmokeColor == CurrentVehicle.Mods.TireSmokeColor) return;
            CurrentVehicle.Mods.TireSmokeColor = Service.CurrentTireSmokeColor;
        }

        if (e.PropertyName == nameof(Service.CurrentWindowTint))
        {
            if (Service.CurrentWindowTint == CurrentVehicle.Mods.WindowTint) return;
            CurrentVehicle.Mods.WindowTint = Service.CurrentWindowTint;
        }

        if (e.PropertyName == nameof(Service.XenonHeadLights))
        {
            if (Service.XenonHeadLights ==
                CurrentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled) return;
            CurrentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = Service.XenonHeadLights;
        }

        if (e.PropertyName == nameof(Service.CurrentNeonLightsLayout))
        {
            if (GetNeonLightsLayout() != NeonLightsLayout.Off)
                CurrentVehicle.Mods.NeonLightsColor = Color.White;
            switch (Service.CurrentNeonLightsLayout)
            {
                case NeonLightsLayout.Off:
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                    break;
                case NeonLightsLayout.Front:
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                    break;
                case NeonLightsLayout.Back:
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                    break;
                case NeonLightsLayout.FrontAndBack:
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                    break;
                case NeonLightsLayout.Sides:
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, false);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
                    break;
                case NeonLightsLayout.FrontBackAndSides:
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
                    CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    // private void UpdateWindowTint(VehicleWindowTint vehicleWindowTint)
    // {
    //     if (vehicleWindowTint == CurrentVehicle.Mods.WindowTint) return;
    //
    //     Service.CurrentWindowTint = CurrentVehicle.Mods.WindowTint;
    // }

    private void OnRandomizeModsRequested(object sender, ObservableCollection<VehicleModType> modsToRandomize)
    {
        if (CurrentVehicle == null) return;

        try
        {
            Random r = new();
            if (!Service.ValidWheelTypes.Contains(VehicleWheelType.BikeWheels))
            {
                var randomWheelType = r.Next(0, CurrentVehicle.Mods.AllowedWheelTypes.Length);
                Service.CurrentWheelType = (VehicleWheelType)randomWheelType;
            }
            else
            {
                Service.CurrentWheelType = VehicleWheelType.BikeWheels;
            }

            var randomRimColor = r.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);
            Service.CurrentRimColor = (VehicleColor)randomRimColor;

            var randomizeVehicleWindowTint = r.Next(0, Enum.GetValues(typeof(VehicleWindowTint)).Length);
            Service.CurrentWindowTint = (VehicleWindowTint)randomizeVehicleWindowTint;

            var randomLicensePlateStyle = r.Next(0, Enum.GetValues(typeof(LicensePlateStyle)).Length);
            Service.LicensePlateStyle = (LicensePlateStyle)randomLicensePlateStyle;

            foreach (var vehicleModType in modsToRandomize)
            {
                var currentMod = CurrentVehicle.Mods[vehicleModType];
                var randomMod = r.Next(-1, currentMod.Count);
                if (currentMod.Index == randomMod) continue;
                currentMod.Index = randomMod;
                Yield();
            }

            var randomizeTireSmoke = r.Next(0, Service.TireSmokeColorDictionary.Count);
            Service.CurrentTireSmokeColor = Service.TireSmokeColorDictionary.ElementAt(randomizeTireSmoke).Value;

            var randomizeCustomTires = r.Next(0, 2);
            Service.CurrentCustomTires = randomizeCustomTires != 0;
        }
        catch (Exception e)
        {
            // Need feedback on this.
            ExceptionService.RaiseError(e);
        }
    }

    private void OnLicensePlateInputRequested(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;
        var licensePlateInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 8);

        if (CurrentVehicle.Mods.LicensePlate == licensePlateInput) return;

        CurrentVehicle.Mods.LicensePlate = licensePlateInput;
    }

    private void UpdateTireSmokeColor(Color tireSmokeColor)
    {
        if (tireSmokeColor == CurrentVehicle.Mods.TireSmokeColor) return;

        Service.CurrentTireSmokeColor = CurrentVehicle.Mods.TireSmokeColor;
    }

    private void UpdateValidWheelTypes(ObservableCollection<VehicleWheelType> validWheelTypes)
    {
        validWheelTypes.Clear();

        foreach (var vehicleWheelType in CurrentVehicle.Mods.AllowedWheelTypes) validWheelTypes.Add(vehicleWheelType);
    }

    private void UpdateLicensePlateStyle(LicensePlateStyle licensePlateStyle)
    {
        if (licensePlateStyle == CurrentVehicle.Mods.LicensePlateStyle) return;

        Service.LicensePlateStyle = CurrentVehicle.Mods.LicensePlateStyle;
    }

    private void UpdateValidModTypes(ObservableCollection<VehicleModType> validModTypes)
    {
        validModTypes.Clear();

        foreach (VehicleModType vehicleModType in Enum.GetValues(typeof(VehicleModType)))
        {
            var vehicleMod = CurrentVehicle.Mods[vehicleModType];
            if (vehicleMod.Count > 0) validModTypes.Add(vehicleModType);
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;
        UpdateFeature(() => Service.LicensePlate, UpdateLicensePlate);
        UpdateFeature(() => Service.LicensePlateStyle, UpdateLicensePlateStyle);
        UpdateFeature(() => Service.CurrentWheelType, UpdateWheelType);
        UpdateFeature(() => Service.CurrentRimColor, UpdateRimColor);
        UpdateFeature(() => Service.CurrentCustomTires, UpdateCustomTires);
    }

    private NeonLightsLayout GetNeonLightsLayout()
    {
        if (Enum.GetValues(typeof(VehicleNeonLight)).Cast<VehicleNeonLight>()
            .All(n => Service.CurrentVehicle.Mods.HasNeonLight(n)))
            Service.CurrentNeonLightsLayout = NeonLightsLayout.FrontBackAndSides;
        else if (Service.CurrentVehicle.Mods.HasNeonLight(VehicleNeonLight.Back) &&
                 Service.CurrentVehicle.Mods.HasNeonLight(VehicleNeonLight.Front))
            Service.CurrentNeonLightsLayout = NeonLightsLayout.FrontAndBack;
        else if (Service.CurrentVehicle.Mods.HasNeonLight(VehicleNeonLight.Left) &&
                 Service.CurrentVehicle.Mods.HasNeonLight(VehicleNeonLight.Right))
            Service.CurrentNeonLightsLayout = NeonLightsLayout.Sides;
        else if (Service.CurrentVehicle.Mods.HasNeonLight(VehicleNeonLight.Front))
            Service.CurrentNeonLightsLayout = NeonLightsLayout.Front;
        else if (Service.CurrentVehicle.Mods.HasNeonLight(VehicleNeonLight.Back))
            Service.CurrentNeonLightsLayout = NeonLightsLayout.Back;
        else
            Service.CurrentNeonLightsLayout = NeonLightsLayout.Off;

        return Service.CurrentNeonLightsLayout;
    }

    private void UpdateNeonLightsLayout(NeonLightsLayout obj)
    {
        if (Service.CurrentNeonLightsLayout != GetNeonLightsLayout())
            Service.CurrentNeonLightsLayout = GetNeonLightsLayout();
    }

    private void UpdateCustomTires(bool customTires)
    {
        if (customTires == CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation) return;

        Service.CurrentCustomTires = CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation;
    }

    private void UpdateRimColor(VehicleColor currentRimColor)
    {
        if (currentRimColor == CurrentVehicle.Mods.RimColor) return;

        Service.CurrentRimColor = CurrentVehicle.Mods.RimColor;
    }

    private void UpdateWheelType(VehicleWheelType currentWheelType)
    {
        if (currentWheelType == CurrentVehicle.Mods.WheelType) return;

        Service.CurrentWheelType = CurrentVehicle.Mods.WheelType;
    }

    private void UpdateLicensePlate(string licensePlate)
    {
        if (licensePlate == CurrentVehicle.Mods.LicensePlate) return;

        Service.LicensePlate = CurrentVehicle.Mods.LicensePlate;
    }

    private void InstallModKit()
    {
        CurrentVehicle.Mods.InstallModKit();
        CurrentVehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = true;
    }
}