using System;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle;

namespace Nuclei.Scripts.Vehicle;

public class VehicleScript : GenericScriptBase<VehicleService>
{
    public VehicleScript()
    {
        Service.RepairRequested += OnRepairRequested;
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        Indestructible();
    }

    private void Indestructible()
    {
        if (CurrentVehicle == null) return;
        if (CurrentVehicle.IsInvincible != Service.Indestructible.Value)
            CurrentVehicle.IsInvincible = Service.Indestructible.Value;
    }

    private void OnRepairRequested(object sender, EventArgs e)
    {
        CurrentVehicle?.Repair();
    }
}