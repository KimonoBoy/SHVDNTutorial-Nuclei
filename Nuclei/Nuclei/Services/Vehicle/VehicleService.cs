﻿using System;
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
            OnPropertyChanged();
        }
    }

    public int SpeedBoost
    {
        get => _speedBoost;
        set
        {
            if (_speedBoost == value) return;
            _speedBoost = value;
            OnPropertyChanged();
        }
    }

    public bool HasSeatBelt
    {
        get => _hasSeatBelt;
        set
        {
            if (_hasSeatBelt == value) return;
            _hasSeatBelt = value;
            OnPropertyChanged();
        }
    }

    public bool DoorsAlwaysLocked
    {
        get => _doorsAlwaysLocked;
        set
        {
            if (_doorsAlwaysLocked == value) return;
            _doorsAlwaysLocked = value;
            OnPropertyChanged();
        }
    }

    public bool NeverFallOffBike
    {
        get => _neverFallOffBike;
        set
        {
            if (_neverFallOffBike == value) return;
            _neverFallOffBike = value;
            OnPropertyChanged();
        }
    }

    public bool IsIndestructible
    {
        get => _isIndestructible;
        set
        {
            if (_isIndestructible == value) return;
            _isIndestructible = value;
            OnPropertyChanged();
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