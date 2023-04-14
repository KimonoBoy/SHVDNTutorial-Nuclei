using System;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle;

namespace Nuclei.Scripts.Vehicle;

public class VehicleScript : GenericScriptBase<VehicleService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.RepairRequested += OnRepairRequested;
        GameStateTimer.SubscribeToTimerElapsed(UpdateVehicle);
    }

    private void UpdateVehicle(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        UpdateFeature(Service.Indestructible.Value, ProcessIndestructible);
    }

    private void OnTick(object sender, EventArgs e)
    {
    }

    private void ProcessIndestructible(bool indestructible)
    {
        if (CurrentVehicle.IsInvincible != indestructible)
            CurrentVehicle.IsInvincible = indestructible;
    }

    private void OnRepairRequested(object sender, EventArgs e)
    {
        CurrentVehicle?.Repair();
    }
}