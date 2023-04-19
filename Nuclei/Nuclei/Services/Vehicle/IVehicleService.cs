using System;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Vehicle;

public interface IVehicleService
{
    // Properties
    BindableProperty<bool> IsIndestructible { get; }
    BindableProperty<int> SpeedBoost { get; }
    BindableProperty<bool> HasSeatBelt { get; }
    BindableProperty<bool> DoorsAlwaysLocked { get; }
    BindableProperty<bool> NeverFallOffBike { get; }
    BindableProperty<bool> CanDriveUnderWater { get; }

    // Events
    event EventHandler RepairRequested;
    event EventHandler VehicleFlipRequested;

    // Methods
    void RequestRepair();
    void RequestVehicleFlip();
}