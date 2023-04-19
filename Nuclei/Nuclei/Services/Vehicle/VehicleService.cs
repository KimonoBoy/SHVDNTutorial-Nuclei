using System;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle;

public class VehicleService : GenericService<VehicleService>, IVehicleService
{
    private bool _isInVehicle;

    public bool IsInVehicle
    {
        get => _isInVehicle;
        set
        {
            _isInVehicle = value;
            IsInVehicleChanged?.Invoke(this, value);
        }
    }

    public BindableProperty<bool> CanDriveUnderWater { get; set; } = new();
    public BindableProperty<int> SpeedBoost { get; set; } = new();
    public BindableProperty<bool> HasSeatBelt { get; set; } = new();
    public BindableProperty<bool> DoorsAlwaysLocked { get; set; } = new();
    public BindableProperty<bool> NeverFallOffBike { get; set; } = new();
    public BindableProperty<bool> IsIndestructible { get; set; } = new();

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

    public event EventHandler<bool> IsInVehicleChanged;
}