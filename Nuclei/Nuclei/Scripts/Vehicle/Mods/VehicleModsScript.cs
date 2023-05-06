﻿using System;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Enums.Vehicle;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleMods;

namespace Nuclei.Scripts.Vehicle.Mods;

public class VehicleModsScript : GenericScript<VehicleModsService>
{
    private readonly Random _random = new();
    private DateTime _rainBowModeInterval = DateTime.UtcNow;

    protected override void SubscribeToEvents()
    {
        Service.PropertyChanged += OnPropertyChanged;
        Service.LicensePlateInputRequested += OnLicensePlateInputRequested;
        Service.RandomizeAllModsRequested += OnRandomizeAllModsRequested;
    }

    protected override void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        ProcessCustomTires();
        ProcessRainbowMode();
    }

    private void ProcessRainbowMode()
    {
        if (!Service.RainbowMode) return;
        if ((DateTime.UtcNow - _rainBowModeInterval).TotalMilliseconds <= 50) return;

        CurrentVehicle.Mods.PrimaryColor = (VehicleColor)_random.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);
        CurrentVehicle.Mods.SecondaryColor = (VehicleColor)_random.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);
        CurrentVehicle.Mods.PearlescentColor =
            (VehicleColor)_random.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);

        _rainBowModeInterval = DateTime.UtcNow;
    }

    private void OnRandomizeAllModsRequested(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        Service.WheelType = (VehicleWheelType)_random.Next(0, Enum.GetValues(typeof(VehicleWheelType)).Length);

        foreach (var vehicleMod in Service.VehicleMods)
            vehicleMod.Index = _random.Next(0, vehicleMod.Count + 1);

        Service.RimColor =
            (VehicleColor)_random.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);

        Service.TireSmokeColor = (TireSmokeColor)_random.Next(0, Service.TireSmokeColorDictionary.Values.Count + 1);

        Service.CustomTires = _random.Next(0, 2) == 1;

        Service.LicensePlateStyle =
            (LicensePlateStyle)_random.Next(0, Enum.GetValues(typeof(LicensePlateStyle)).Length);

        Service.PrimaryColor = (VehicleColor)_random.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);
        Service.SecondaryColor = (VehicleColor)_random.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);

        Service.WindowTint = (VehicleWindowTint)_random.Next(0, Enum.GetValues(typeof(VehicleWindowTint)).Length);

        Service.PearlescentColor = (VehicleColor)_random.Next(0, Enum.GetValues(typeof(VehicleColor)).Length);

        Service.XenonHeadLights = _random.Next(0, 2) == 1;

        Service.NeonLightsLayout =
            (NeonLightsLayout)_random.Next(0, Enum.GetValues(typeof(NeonLightsLayout)).Length);

        Service.NeonLightsColor = (NeonLightsColor)_random.Next(0, Enum.GetValues(typeof(NeonLightsColor)).Length);

        Service.Turbo = _random.Next(0, 2) == 1;
    }

    private void OnLicensePlateInputRequested(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        var licensePlateInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 8);
        Service.LicensePlate = licensePlateInput;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (CurrentVehicle == null) return;

        switch (e.PropertyName)
        {
            case nameof(Service.CurrentVehicle):
                UpdateMods();
                break;
            case nameof(Service.WheelType):
                CurrentVehicle.Mods.WheelType = Service.WheelType;
                break;
            case nameof(Service.RimColor):
                CurrentVehicle.Mods.RimColor = Service.RimColor;
                break;
            case nameof(Service.TireSmokeColor):
                CurrentVehicle.Mods.TireSmokeColor = Service.TireSmokeColorDictionary
                    .FirstOrDefault(tireSmokeColor => tireSmokeColor.Key == Service.TireSmokeColor).Value;
                break;
            case nameof(Service.CustomTires):
                CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation = Service.CustomTires;
                CurrentVehicle.Mods[VehicleModType.RearWheel].Variation = Service.CustomTires;
                break;
            case nameof(Service.LicensePlate):
                CurrentVehicle.Mods.LicensePlate = Service.LicensePlate;
                break;
            case nameof(Service.LicensePlateStyle):
                CurrentVehicle.Mods.LicensePlateStyle = Service.LicensePlateStyle;
                break;
            case nameof(Service.PrimaryColor):
                CurrentVehicle.Mods.PrimaryColor = Service.PrimaryColor;
                break;
            case nameof(Service.SecondaryColor):
                CurrentVehicle.Mods.SecondaryColor = Service.SecondaryColor;
                break;
            case nameof(Service.PearlescentColor):
                CurrentVehicle.Mods.PearlescentColor = Service.PearlescentColor;
                break;
            case nameof(Service.WindowTint):
                CurrentVehicle.Mods.WindowTint = Service.WindowTint;
                break;
            case nameof(Service.XenonHeadLights):
                CurrentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = Service.XenonHeadLights;
                break;
            case nameof(Service.NeonLightsLayout):
                switch (Service.NeonLightsLayout)
                {
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
                    case NeonLightsLayout.Off:
                        CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, false);
                        CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, false);
                        CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, false);
                        CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, false);
                        break;
                }

                break;
            case nameof(Service.NeonLightsColor):
                CurrentVehicle.Mods.NeonLightsColor = Service.NeonLightsColorDictionary
                    .FirstOrDefault(color => color.Key == Service.NeonLightsColor).Value;
                break;
            case nameof(Service.Turbo):
                CurrentVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = Service.Turbo;
                break;
        }
    }

    protected override void UnsubscribeOnExit()
    {
        Service.PropertyChanged -= OnPropertyChanged;
    }


    private void ProcessCustomTires()
    {
        if (CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation != Service.CustomTires)
        {
            CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation = Service.CustomTires;
            CurrentVehicle.Mods[VehicleModType.RearWheel].Variation = Service.CustomTires;
        }
    }


    private void UpdateMods()
    {
        if (CurrentVehicle == null) return;

        Service.VehicleMods.Clear();
        InstallModKits();
        UpdateWheelType();
        UpdateModTypes();
        UpdateTurbo();
        UpdatePrimaryColor();
        UpdateSecondaryColor();
        UpdatePearlescentColor();
        UpdateWindowTint();
        UpdateRimColor();
        UpdateTireSmokeColor();
        UpdateCustomTires();
        UpdateXenonHeadLights();
        UpdateNeonLightsLayout();
        UpdateNeonLightsColor();
        UpdateLicensePlate();
        UpdateLicensePlateStyle();
    }

    private void UpdateTurbo()
    {
        if (CurrentVehicle == null) return;

        Service.Turbo = CurrentVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled;
    }

    private void UpdateNeonLightsColor()
    {
        if (CurrentVehicle == null) return;


        Service.NeonLightsColor = Service.NeonLightsColorDictionary
            .FirstOrDefault(color => color.Value == CurrentVehicle.Mods.NeonLightsColor).Key;
    }

    private void UpdateNeonLightsLayout()
    {
        if (CurrentVehicle == null) return;


        if (CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front) &&
            CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back) &&
            CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left) &&
            CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right))
            Service.NeonLightsLayout = NeonLightsLayout.FrontBackAndSides;
        else if (CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front) &&
                 CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back))
            Service.NeonLightsLayout = NeonLightsLayout.FrontAndBack;
        else if (CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left) &&
                 CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right))
            Service.NeonLightsLayout = NeonLightsLayout.Sides;
        else if (CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front))
            Service.NeonLightsLayout = NeonLightsLayout.Front;
        else if (CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back))
            Service.NeonLightsLayout = NeonLightsLayout.Back;
        else
            Service.NeonLightsLayout = NeonLightsLayout.Off;
    }

    private void UpdatePearlescentColor()
    {
        if (CurrentVehicle == null) return;

        Service.PearlescentColor = CurrentVehicle.Mods.PearlescentColor;
    }

    private void UpdateXenonHeadLights()
    {
        if (CurrentVehicle == null) return;

        Service.XenonHeadLights = CurrentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
    }

    private void UpdateWindowTint()
    {
        if (CurrentVehicle == null) return;

        Service.WindowTint = CurrentVehicle.Mods.WindowTint;
    }

    private void UpdateSecondaryColor()
    {
        if (CurrentVehicle == null) return;

        Service.SecondaryColor = CurrentVehicle.Mods.SecondaryColor;
    }

    private void UpdatePrimaryColor()
    {
        if (CurrentVehicle == null) return;

        Service.PrimaryColor = CurrentVehicle.Mods.PrimaryColor;
    }

    private void UpdateLicensePlateStyle()
    {
        if (CurrentVehicle == null) return;
        Service.LicensePlateStyle = CurrentVehicle.Mods.LicensePlateStyle;
    }

    private void UpdateLicensePlate()
    {
        if (CurrentVehicle == null) return;

        Service.LicensePlate = CurrentVehicle.Mods.LicensePlate;
    }

    private void UpdateCustomTires()
    {
        if (CurrentVehicle == null) return;

        Service.CustomTires = CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation;
    }

    private void UpdateTireSmokeColor()
    {
        if (CurrentVehicle == null) return;

        Service.TireSmokeColor = Service.TireSmokeColorDictionary
            .FirstOrDefault(tireSmokeColor => tireSmokeColor.Value == CurrentVehicle.Mods.TireSmokeColor).Key;
    }

    private void UpdateWheelType()
    {
        if (CurrentVehicle == null) return;

        Service.WheelType = CurrentVehicle.Mods.WheelType;
    }

    private void UpdateRimColor()
    {
        if (CurrentVehicle == null) return;

        Service.RimColor = CurrentVehicle.Mods.RimColor;
    }

    private void UpdateModTypes()
    {
        if (CurrentVehicle == null) return;

        foreach (var vehicleMod in CurrentVehicle.Mods.ToArray()) Service.VehicleMods.Add(vehicleMod);
    }

    private void InstallModKits()
    {
        if (CurrentVehicle == null) return;
        CurrentVehicle.Mods.InstallModKit();
        CurrentVehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = true;
    }
}