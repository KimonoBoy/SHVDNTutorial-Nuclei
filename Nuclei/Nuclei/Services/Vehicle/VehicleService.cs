using System;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle;

public class VehicleService : GenericService<VehicleService>, IVehicleService
{
    public BindableProperty<bool> DriveUnderWater { get; set; } = new();
    public BindableProperty<int> SpeedBoost { get; set; } = new();
    public BindableProperty<bool> SeatBelt { get; set; } = new();
    public BindableProperty<bool> LockDoors { get; set; } = new();
    public BindableProperty<bool> NeverFallOffBike { get; set; } = new();
    public BindableProperty<bool> Indestructible { get; set; } = new();

    public event EventHandler RepairRequested;

    public event EventHandler VehicleFlipRequested;

    public void RequestRepair()
    {
        RepairRequested?.Invoke(this, EventArgs.Empty);
    }

    public void RequestVehicleFlip()
    {
        VehicleFlipRequested?.Invoke(this, EventArgs.Empty);
    }
}