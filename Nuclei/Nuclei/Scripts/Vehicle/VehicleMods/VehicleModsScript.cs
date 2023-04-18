using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleMods;

namespace Nuclei.Scripts.Vehicle.VehicleMods;

public class VehicleModsScript : GenericScriptBase<VehicleModsService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.InstallModKitRequested += OnInstallModKitRequested;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        if (Game.IsKeyPressed(Keys.Add))
            CurrentVehicle.Mods[VehicleModType.FrontWheel].Index++;
        if (Game.IsKeyPressed(Keys.Subtract))
            CurrentVehicle.Mods[VehicleModType.FrontWheel].Index--;
    }

    private void OnInstallModKitRequested(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        InstallModKit();
    }

    private void InstallModKit()
    {
        CurrentVehicle.Mods.InstallModKit();
    }
}