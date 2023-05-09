using System;
using System.Collections.Generic;
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

    protected override void UnsubscribeOnExit()
    {
        Service.LicensePlateInputRequested -= OnLicensePlateInputRequested;
        Service.RandomizeAllModsRequested -= OnRandomizeAllModsRequested;
        Service.PropertyChanged -= OnPropertyChanged;
    }

    protected override void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        ProcessCustomTires();
        ProcessRainbowMode();
    }

    private void OnRandomizeAllModsRequested(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        RandomizeVehicleMods();
    }

    private void OnLicensePlateInputRequested(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        var licensePlateInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 8);
        Service.LicensePlate = licensePlateInput;
    }

    private new void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (CurrentVehicle == null) return;

        var propertyActions = new Dictionary<string, Action>
        {
            [nameof(Service.CurrentVehicle)] = UpdateMods,
            [nameof(Service.WheelType)] = () => CurrentVehicle.Mods.WheelType = Service.WheelType,
            [nameof(Service.RimColor)] = () => CurrentVehicle.Mods.RimColor = Service.RimColor,
            [nameof(Service.TireSmokeColor)] = () => CurrentVehicle.Mods.TireSmokeColor = Service
                .TireSmokeColorDictionary
                .FirstOrDefault(tireSmokeColor => tireSmokeColor.Key == Service.TireSmokeColor).Value,
            [nameof(Service.CustomTires)] = () =>
            {
                CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation = Service.CustomTires;
                if (CurrentVehicle.Mods[VehicleModType.RearWheel].Count > 0)
                    CurrentVehicle.Mods[VehicleModType.RearWheel].Variation = Service.CustomTires;
            },
            [nameof(Service.LicensePlate)] = () => CurrentVehicle.Mods.LicensePlate = Service.LicensePlate,
            [nameof(Service.LicensePlateStyle)] =
                () => CurrentVehicle.Mods.LicensePlateStyle = Service.LicensePlateStyle,
            [nameof(Service.PrimaryColor)] = () => CurrentVehicle.Mods.PrimaryColor = Service.PrimaryColor,
            [nameof(Service.SecondaryColor)] = () => CurrentVehicle.Mods.SecondaryColor = Service.SecondaryColor,
            [nameof(Service.PearlescentColor)] = () => CurrentVehicle.Mods.PearlescentColor = Service.PearlescentColor,
            [nameof(Service.WindowTint)] = () => CurrentVehicle.Mods.WindowTint = Service.WindowTint,
            [nameof(Service.XenonHeadLights)] = () =>
                CurrentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = Service.XenonHeadLights,
            [nameof(Service.NeonLightsLayout)] = () =>
            {
                bool front = false, back = false, left = false, right = false;
                switch (Service.NeonLightsLayout)
                {
                    case NeonLightsLayout.Front:
                        front = true;
                        break;
                    case NeonLightsLayout.Back:
                        back = true;
                        break;
                    case NeonLightsLayout.FrontAndBack:
                        front = true;
                        back = true;
                        break;
                    case NeonLightsLayout.Sides:
                        left = true;
                        right = true;
                        break;
                    case NeonLightsLayout.FrontBackAndSides:
                        front = true;
                        back = true;
                        left = true;
                        right = true;
                        break;
                    case NeonLightsLayout.Off:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, front);
                CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, back);
                CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, left);
                CurrentVehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, right);
            },
            [nameof(Service.NeonLightsColor)] = () => CurrentVehicle.Mods.NeonLightsColor = Service
                .NeonLightsColorDictionary
                .FirstOrDefault(color => color.Key == Service.NeonLightsColor).Value,
            [nameof(Service.Turbo)] = () => CurrentVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = Service.Turbo
        };

        if (propertyActions.TryGetValue(e.PropertyName, out var propertyAction)) propertyAction();
    }

    private void RandomizeVehicleMods()
    {
        Service.WheelType = GetRandomEnumValue<VehicleWheelType>();

        foreach (var vehicleMod in Service.VehicleMods)
            vehicleMod.Index = _random.Next(0, vehicleMod.Count + 1);

        Service.RimColor = GetRandomEnumValue<VehicleColor>();
        Service.TireSmokeColor = GetRandomEnumValue<TireSmokeColor>();
        Service.CustomTires = GetRandomBool();
        Service.LicensePlateStyle = GetRandomEnumValue<LicensePlateStyle>();
        Service.PrimaryColor = GetRandomEnumValue<VehicleColor>();
        Service.SecondaryColor = GetRandomEnumValue<VehicleColor>();
        Service.WindowTint = GetRandomEnumValue<VehicleWindowTint>();
        Service.PearlescentColor = GetRandomEnumValue<VehicleColor>();
        Service.XenonHeadLights = GetRandomBool();
        Service.NeonLightsLayout = GetRandomEnumValue<NeonLightsLayout>();
        Service.NeonLightsColor = GetRandomEnumValue<NeonLightsColor>();
        Service.Turbo = GetRandomBool();
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

    private void InstallModKits()
    {
        if (CurrentVehicle == null) return;
        CurrentVehicle.Mods.InstallModKit();
        CurrentVehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = true;
    }

    private void UpdateWheelType()
    {
        if (CurrentVehicle == null) return;

        Service.WheelType = CurrentVehicle.Mods.WheelType;
    }

    private void UpdateModTypes()
    {
        if (CurrentVehicle == null) return;

        foreach (var vehicleMod in CurrentVehicle.Mods.ToArray()) Service.VehicleMods.Add(vehicleMod);
    }

    private void UpdateTurbo()
    {
        if (CurrentVehicle == null) return;

        Service.Turbo = CurrentVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled;
    }

    private void UpdatePrimaryColor()
    {
        if (CurrentVehicle == null) return;

        Service.PrimaryColor = CurrentVehicle.Mods.PrimaryColor;
    }

    private void UpdateSecondaryColor()
    {
        if (CurrentVehicle == null) return;

        Service.SecondaryColor = CurrentVehicle.Mods.SecondaryColor;
    }

    private void UpdatePearlescentColor()
    {
        if (CurrentVehicle == null) return;

        Service.PearlescentColor = CurrentVehicle.Mods.PearlescentColor;
    }

    private void UpdateWindowTint()
    {
        if (CurrentVehicle == null) return;

        Service.WindowTint = CurrentVehicle.Mods.WindowTint;
    }

    private void UpdateRimColor()
    {
        if (CurrentVehicle == null) return;

        Service.RimColor = CurrentVehicle.Mods.RimColor;
    }

    private void UpdateTireSmokeColor()
    {
        if (CurrentVehicle == null) return;

        Service.TireSmokeColor = Service.TireSmokeColorDictionary
            .FirstOrDefault(tireSmokeColor => tireSmokeColor.Value == CurrentVehicle.Mods.TireSmokeColor).Key;
    }

    private void UpdateCustomTires()
    {
        if (CurrentVehicle == null) return;

        Service.CustomTires = CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation;
    }

    private void UpdateXenonHeadLights()
    {
        if (CurrentVehicle == null) return;

        Service.XenonHeadLights = CurrentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
    }

    private void UpdateNeonLightsLayout()
    {
        if (CurrentVehicle == null) return;

        var isFrontOn = CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
        var isBackOn = CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
        var isLeftOn = CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
        var isRightOn = CurrentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right);

        if (isFrontOn && isBackOn && isLeftOn && isRightOn)
            Service.NeonLightsLayout = NeonLightsLayout.FrontBackAndSides;
        else if (isFrontOn && isBackOn)
            Service.NeonLightsLayout = NeonLightsLayout.FrontAndBack;
        else if (isLeftOn && isRightOn)
            Service.NeonLightsLayout = NeonLightsLayout.Sides;
        else if (isFrontOn)
            Service.NeonLightsLayout = NeonLightsLayout.Front;
        else if (isBackOn)
            Service.NeonLightsLayout = NeonLightsLayout.Back;
        else
            Service.NeonLightsLayout = NeonLightsLayout.Off;
    }

    private void UpdateNeonLightsColor()
    {
        if (CurrentVehicle == null) return;
        Service.NeonLightsColor = Service.NeonLightsColorDictionary
            .FirstOrDefault(color => color.Value == CurrentVehicle.Mods.NeonLightsColor).Key;
    }

    private void UpdateLicensePlate()
    {
        if (CurrentVehicle == null) return;
        Service.LicensePlate = CurrentVehicle.Mods.LicensePlate;
    }

    private void UpdateLicensePlateStyle()
    {
        if (CurrentVehicle == null) return;

        Service.LicensePlateStyle = CurrentVehicle.Mods.LicensePlateStyle;
    }

    private void ProcessCustomTires()
    {
        if (CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation == Service.CustomTires) return;

        CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation = Service.CustomTires;
        if (CurrentVehicle.Mods[VehicleModType.RearWheel].Count > 0)
            CurrentVehicle.Mods[VehicleModType.RearWheel].Variation = Service.CustomTires;
    }

    private void ProcessRainbowMode()
    {
        if (!Service.RainbowMode) return;
        if ((DateTime.UtcNow - _rainBowModeInterval).TotalMilliseconds <= 50) return;

        CurrentVehicle.Mods.PrimaryColor = GetRandomEnumValue<VehicleColor>();
        CurrentVehicle.Mods.SecondaryColor = GetRandomEnumValue<VehicleColor>();
        CurrentVehicle.Mods.PearlescentColor = GetRandomEnumValue<VehicleColor>();

        _rainBowModeInterval = DateTime.UtcNow;
    }

    private T GetRandomEnumValue<T>()
    {
        return (T)Enum.GetValues(typeof(T)).GetValue(_random.Next(0, Enum.GetValues(typeof(T)).Length));
    }

    private bool GetRandomBool()
    {
        return _random.Next(0, 2) == 1;
    }
}