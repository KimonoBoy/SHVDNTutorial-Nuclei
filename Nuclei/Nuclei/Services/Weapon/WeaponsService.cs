﻿using System;
using GTA;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Weapon;

public class WeaponsService : GenericService<WeaponsService>
{
    private int _accuracy;
    private bool _aimBot;
    private bool _blackHoleGun;
    private int _blackHoleLifeSpan = 10;
    private int _blackHolePower = 10;
    private int _blackHoleRadius = 10;
    private VehicleHash _currentVehicleGun;
    private bool _explosiveBullets;
    private bool _fireBullets;
    private bool _gravityGun;
    private bool _infiniteAmmo;
    private bool _levitationGun;
    private bool _noReload;
    private bool _oneHitKill;
    private bool _shootObjects;
    private bool _shootPeds;
    private bool _shootVehicles;
    private bool _teleportGun;
    private int _throwVelocity;

    public bool InfiniteAmmo
    {
        get => _infiniteAmmo;
        set
        {
            if (_infiniteAmmo == value) return;
            _infiniteAmmo = value;
            OnPropertyChanged();
        }
    }

    public bool NoReload
    {
        get => _noReload;
        set
        {
            if (_noReload == value) return;
            _noReload = value;
            OnPropertyChanged();
        }
    }

    public bool FireBullets
    {
        get => _fireBullets;
        set
        {
            if (_fireBullets == value) return;
            _fireBullets = value;
            OnPropertyChanged();
        }
    }

    public bool ExplosiveBullets
    {
        get => _explosiveBullets;
        set
        {
            if (_explosiveBullets == value) return;
            _explosiveBullets = value;
            OnPropertyChanged();
        }
    }

    public bool OneHitKill
    {
        get => _oneHitKill;
        set
        {
            if (_oneHitKill == value) return;
            _oneHitKill = value;
            OnPropertyChanged();
        }
    }

    public int Accuracy
    {
        get => _accuracy;
        set
        {
            if (_accuracy == value) return;
            _accuracy = value;
            OnPropertyChanged();
        }
    }

    public bool GravityGun
    {
        get => _gravityGun;
        set
        {
            if (_gravityGun == value) return;
            _gravityGun = value;
            OnPropertyChanged();
        }
    }

    public int ThrowVelocity
    {
        get => _throwVelocity;
        set
        {
            if (value == _throwVelocity) return;
            _throwVelocity = value;
            OnPropertyChanged();
        }
    }

    public bool TeleportGun
    {
        get => _teleportGun;
        set
        {
            if (_teleportGun == value) return;
            _teleportGun = value;
            OnPropertyChanged();
        }
    }

    public bool LevitationGun
    {
        get => _levitationGun;
        set
        {
            if (_levitationGun == value) return;
            _levitationGun = value;
            OnPropertyChanged();
        }
    }

    public bool ShootVehicles
    {
        get => _shootVehicles;
        set
        {
            if (_shootVehicles == value) return;
            _shootVehicles = value;
            OnPropertyChanged();
        }
    }

    public bool ShootPeds
    {
        get => _shootPeds;
        set
        {
            if (_shootPeds == value) return;
            _shootPeds = value;
            OnPropertyChanged();
        }
    }

    public bool ShootObjects
    {
        get => _shootObjects;
        set
        {
            if (_shootObjects == value) return;
            _shootObjects = value;
            OnPropertyChanged();
        }
    }

    public bool AimBot
    {
        get => _aimBot;
        set
        {
            if (_aimBot == value) return;
            _aimBot = value;
            OnPropertyChanged();
        }
    }

    public VehicleHash CurrentVehicleGun
    {
        get => _currentVehicleGun;
        set
        {
            if (value == _currentVehicleGun) return;
            _currentVehicleGun = value;
            OnPropertyChanged();
        }
    }

    public bool BlackHoleGun
    {
        get => _blackHoleGun;
        set
        {
            if (value == _blackHoleGun) return;
            _blackHoleGun = value;
            OnPropertyChanged();
        }
    }

    public int BlackHoleLifeSpan
    {
        get => _blackHoleLifeSpan;
        set
        {
            if (value == _blackHoleLifeSpan) return;
            _blackHoleLifeSpan = value;
            OnPropertyChanged();
        }
    }

    public int BlackHoleRadius
    {
        get => _blackHoleRadius;
        set
        {
            if (value == _blackHoleRadius) return;
            _blackHoleRadius = value;
            OnPropertyChanged();
        }
    }

    public int BlackHolePower
    {
        get => _blackHolePower;
        set
        {
            if (value == _blackHolePower) return;
            _blackHolePower = value;
            OnPropertyChanged();
        }
    }

    public event EventHandler AllWeaponsRequested;

    public void RequestAllWeapons()
    {
        AllWeaponsRequested?.Invoke(this, EventArgs.Empty);
    }
}