using System;
using System.Collections.Generic;
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
    }

    private void OnCurrentVehicleChanged(object sender, ValueEventArgs<GTA.Vehicle> currentVehicle)
    {
        if (currentVehicle.Value == null) return;

        InstallModKit();
        UpdateFeature(Service.ValidVehicleModTypes.Value, UpdateValidModTypes);
    }

    private void UpdateValidModTypes(List<VehicleModType> validModTypes)
    {
        validModTypes.Clear();

        foreach (VehicleModType vehicleModType in Enum.GetValues(typeof(VehicleModType)))
        {
            var vehicleMod = CurrentVehicle.Mods[vehicleModType];
            if (vehicleMod.Count < 1) continue;

            validModTypes.Add(vehicleModType);
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;
    }

    private void InstallModKit()
    {
        CurrentVehicle.Mods.InstallModKit();
    }
}