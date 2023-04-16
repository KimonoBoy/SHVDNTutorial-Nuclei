using System;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Vehicle;

public interface IVehicleService
{
    // Properties
    BindableProperty<bool> Indestructible { get; }
    BindableProperty<int> SpeedBoost { get; }
    BindableProperty<bool> SeatBelt { get; }
    BindableProperty<bool> LockDoors { get; }
    BindableProperty<bool> NeverFallOffBike { get; }
    BindableProperty<bool> DriveUnderWater { get; }

    // Events
    event EventHandler RepairRequested;
    event EventHandler VehicleFlipRequested;

    // Methods
    void RequestRepair();
    void RequestVehicleFlip();
}