using System;
using GTA.Native;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleMods;

namespace Nuclei.Scripts.Vehicle.VehicleMods;

public class VehicleModsScript : GenericScriptBase<VehicleModsService>
{
    private bool IsModKitInstalled => Function.Call<int>(Hash.GET_VEHICLE_MOD_KIT, CurrentVehicle) == 0;

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        UpdateFeature(Service.IsModKitInstalled.Value, UpdateModKit);
    }

    private void UpdateModKit(bool isModKitInstalled)
    {
        if (isModKitInstalled && !IsModKitInstalled)
            CurrentVehicle.Mods.InstallModKit();
    }
}