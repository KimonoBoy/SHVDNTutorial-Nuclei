using System;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle;

public class VehicleService : GenericService<VehicleService>
{
    private bool _canDriveUnderWater;
    private bool _doorsAlwaysLocked;
    private bool _hasSeatBelt;
    private bool _isIndestructible;
    private bool _neverFallOffBike;
    private int _speedBoost;

    public bool CanDriveUnderWater
    {
        get => _canDriveUnderWater;
        set
        {
            if (_canDriveUnderWater == value) return;
            _canDriveUnderWater = value;
            OnPropertyChanged(nameof(_canDriveUnderWater));
        }
    }

    public int SpeedBoost
    {
        get => _speedBoost;
        set
        {
            if (_speedBoost == value) return;
            _speedBoost = value;
            OnPropertyChanged(nameof(_speedBoost));
        }
    }

    public bool HasSeatBelt
    {
        get => _hasSeatBelt;
        set
        {
            if (_hasSeatBelt == value) return;
            _hasSeatBelt = value;
            OnPropertyChanged(nameof(_hasSeatBelt));
        }
    }

    public bool DoorsAlwaysLocked
    {
        get => _doorsAlwaysLocked;
        set
        {
            if (_doorsAlwaysLocked == value) return;
            _doorsAlwaysLocked = value;
            OnPropertyChanged(nameof(_doorsAlwaysLocked));
        }
    }

    public bool NeverFallOffBike
    {
        get => _neverFallOffBike;
        set
        {
            if (_neverFallOffBike == value) return;
            _neverFallOffBike = value;
            OnPropertyChanged(nameof(_neverFallOffBike));
        }
    }

    public bool IsIndestructible
    {
        get => _isIndestructible;
        set
        {
            if (_isIndestructible == value) return;
            _isIndestructible = value;
            OnPropertyChanged(nameof(_isIndestructible));
        }
    }

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