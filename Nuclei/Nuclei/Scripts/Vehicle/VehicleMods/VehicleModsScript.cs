using System;
using System.Drawing;
using GTA.Native;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Vehicle.VehicleMods;

public class VehicleModsScript : GenericScriptBase<VehicleModsService>
{
    private bool IsModKitInstalled;

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.CurrentVehicle.ValueChanged += OnCurrentVehicleChanged;
    }

    private void OnCurrentVehicleChanged(object sender, ValueEventArgs<GTA.Vehicle> currentVehicle)
    {
        if (currentVehicle.Value == null) return;

        UpdateFeature(Service.IsModKitInstalled.Value, UpdateModKit);
        Display.Notify("Changed...");
    }

    private void OnTick(object sender, EventArgs e)
    {
        Display.DrawTextElement(IsModKitInstalled.ToString(), 100.0f, 100.0f, Color.AntiqueWhite);

        if (CurrentVehicle != null)
            IsModKitInstalled = Function.Call<int>(Hash.GET_VEHICLE_MOD_KIT, CurrentVehicle) == 0;
        else
            IsModKitInstalled = false;

        if (CurrentVehicle == null) return;
    }

    private void UpdateModKit(bool isModKitInstalled)
    {
        if (isModKitInstalled && !IsModKitInstalled)
            CurrentVehicle?.Mods.InstallModKit();
    }
}