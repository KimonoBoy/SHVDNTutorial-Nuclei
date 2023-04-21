using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleMods;

namespace Nuclei.Scripts.Vehicle.VehicleMods;

public class VehicleModsScript : GenericScriptBase<VehicleModsService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.CurrentVehicle.ValueChanged += OnCurrentVehicleChanged;
        Service.LicensePlateInputRequested += OnLicensePlateInputRequested;
        Service.LicensePlateStyle.ValueChanged += OnLicensePlateStyleChanged;
        Service.RandomizeModsRequested += OnRandomizeModsRequested;
        Service.CurrentWheelType.ValueChanged += OnCurrentWheelTypeChanged;
    }

    private void OnCurrentWheelTypeChanged(object sender, ValueEventArgs<VehicleWheelType> currentWheelType)
    {
        if (CurrentVehicle == null) return;

        CurrentVehicle.Mods.WheelType = currentWheelType.Value;
    }

    private void OnRandomizeModsRequested(object sender, List<VehicleModType> modsToRandomize)
    {
        if (CurrentVehicle == null) return;

        Random r = new();
        var randomWheelType = r.Next(0, Service.ValidWheelTypes.Value.Count);
        Service.CurrentWheelType.Value = (VehicleWheelType)randomWheelType;
        foreach (var vehicleModType in modsToRandomize)
        {
            var currentMod = CurrentVehicle.Mods[vehicleModType];
            var randomMod = r.Next(-1, currentMod.Count);
            if (currentMod.Index == randomMod) continue;
            currentMod.Index = randomMod;
        }
    }

    private void OnLicensePlateStyleChanged(object sender, ValueEventArgs<LicensePlateStyle> licensePlateStyle)
    {
        if (CurrentVehicle == null) return;

        if (licensePlateStyle.Value == CurrentVehicle.Mods.LicensePlateStyle) return;

        CurrentVehicle.Mods.LicensePlateStyle = licensePlateStyle.Value;
    }

    private void OnLicensePlateInputRequested(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;
        var licensePlateInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 8);

        if (CurrentVehicle.Mods.LicensePlate == licensePlateInput) return;

        CurrentVehicle.Mods.LicensePlate = licensePlateInput;
    }

    private void OnCurrentVehicleChanged(object sender, ValueEventArgs<GTA.Vehicle> currentVehicle)
    {
        if (currentVehicle.Value == null) return;

        InstallModKit();
        UpdateFeature(Service.ValidVehicleModTypes.Value, UpdateValidModTypes);
        UpdateFeature(Service.ValidWheelTypes.Value, UpdateValidWheelTypes);
    }

    private void UpdateValidWheelTypes(List<VehicleWheelType> validWheelTypes)
    {
        validWheelTypes.Clear();

        validWheelTypes.AddRange(CurrentVehicle.Mods.AllowedWheelTypes);
    }

    private void UpdateLicensePlateStyle(LicensePlateStyle licensePlateStyle)
    {
        if (licensePlateStyle == CurrentVehicle.Mods.LicensePlateStyle) return;

        Service.LicensePlateStyle.Value = CurrentVehicle.Mods.LicensePlateStyle;
    }

    private void UpdateValidModTypes(List<VehicleModType> validModTypes)
    {
        validModTypes.Clear();

        validModTypes.AddRange(from VehicleModType vehicleModType in Enum.GetValues(typeof(VehicleModType))
            let vehicleMod = CurrentVehicle.Mods[vehicleModType]
            where vehicleMod.Count > 0
            select vehicleModType);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        UpdateFeature(Service.LicensePlate.Value, UpdateLicensePlate);
        UpdateFeature(Service.LicensePlateStyle.Value, UpdateLicensePlateStyle);
        UpdateFeature(Service.CurrentWheelType.Value, UpdateCurrentWheelType);
    }

    private void UpdateCurrentWheelType(VehicleWheelType currentWheelType)
    {
        if (CurrentVehicle.Mods.WheelType == currentWheelType) return;

        Service.CurrentWheelType.Value = CurrentVehicle.Mods.WheelType;
    }

    private void UpdateLicensePlate(string licensePlate)
    {
        if (Service.LicensePlate.Value == CurrentVehicle.Mods.LicensePlate) return;

        Service.LicensePlate.Value = CurrentVehicle.Mods.LicensePlate;
    }

    private void InstallModKit()
    {
        CurrentVehicle.Mods.InstallModKit();
    }
}