using System;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Weapon;

public class WeaponsService : GenericService<WeaponsService>
{
    private int _accuracy;
    private bool _aimBot;
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

    public bool InfiniteAmmo
    {
        get => _infiniteAmmo;
        set
        {
            if (_infiniteAmmo == value) return;
            _infiniteAmmo = value;
            OnPropertyChanged(nameof(_infiniteAmmo));
        }
    }

    public bool NoReload
    {
        get => _noReload;
        set
        {
            if (_noReload == value) return;
            _noReload = value;
            OnPropertyChanged(nameof(_noReload));
        }
    }

    public bool FireBullets
    {
        get => _fireBullets;
        set
        {
            if (_fireBullets == value) return;
            _fireBullets = value;
            OnPropertyChanged(nameof(_fireBullets));
        }
    }

    public bool ExplosiveBullets
    {
        get => _explosiveBullets;
        set
        {
            if (_explosiveBullets == value) return;
            _explosiveBullets = value;
            OnPropertyChanged(nameof(_explosiveBullets));
        }
    }

    public bool OneHitKill
    {
        get => _oneHitKill;
        set
        {
            if (_oneHitKill == value) return;
            _oneHitKill = value;
            OnPropertyChanged(nameof(_oneHitKill));
        }
    }

    public int Accuracy
    {
        get => _accuracy;
        set
        {
            if (_accuracy == value) return;
            _accuracy = value;
            OnPropertyChanged(nameof(_accuracy));
        }
    }

    public bool GravityGun
    {
        get => _gravityGun;
        set
        {
            if (_gravityGun == value) return;
            _gravityGun = value;
            OnPropertyChanged(nameof(_gravityGun));
        }
    }

    public bool TeleportGun
    {
        get => _teleportGun;
        set
        {
            if (_teleportGun == value) return;
            _teleportGun = value;
            OnPropertyChanged(nameof(_teleportGun));
        }
    }

    public bool LevitationGun
    {
        get => _levitationGun;
        set
        {
            if (_levitationGun == value) return;
            _levitationGun = value;
            OnPropertyChanged(nameof(_levitationGun));
        }
    }

    public bool ShootVehicles
    {
        get => _shootVehicles;
        set
        {
            if (_shootVehicles == value) return;
            _shootVehicles = value;
            OnPropertyChanged(nameof(_shootVehicles));
        }
    }

    public bool ShootPeds
    {
        get => _shootPeds;
        set
        {
            if (_shootPeds == value) return;
            _shootPeds = value;
            OnPropertyChanged(nameof(_shootPeds));
        }
    }

    public bool ShootObjects
    {
        get => _shootObjects;
        set
        {
            if (_shootObjects == value) return;
            _shootObjects = value;
            OnPropertyChanged(nameof(_shootObjects));
        }
    }

    public bool AimBot
    {
        get => _aimBot;
        set
        {
            if (_aimBot == value) return;
            _aimBot = value;
            OnPropertyChanged(nameof(_aimBot));
        }
    }

    public event EventHandler AllWeaponsRequested;

    public void RequestAllWeapons()
    {
        AllWeaponsRequested?.Invoke(this, EventArgs.Empty);
    }
}