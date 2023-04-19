using System;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Weapon;

public class WeaponsService : GenericService<WeaponsService>
{
    public BindableProperty<bool> InfiniteAmmo { get; set; } = new();
    public BindableProperty<bool> NoReload { get; set; } = new();
    public BindableProperty<bool> FireBullets { get; set; } = new();
    public BindableProperty<bool> ExplosiveBullets { get; set; } = new();
    public BindableProperty<bool> OneHitKill { get; set; } = new();
    public BindableProperty<int> Accuracy { get; set; } = new();
    public BindableProperty<bool> GravityGun { get; set; } = new();
    public BindableProperty<bool> TeleportGun { get; set; } = new();
    public BindableProperty<bool> LevitationGun { get; set; } = new();
    public BindableProperty<bool> ShootVehicles { get; set; } = new();
    public BindableProperty<bool> ShootPeds { get; set; } = new();
    public BindableProperty<bool> ShootObjects { get; set; } = new();
    public BindableProperty<bool> AimBot { get; set; } = new();

    public event EventHandler AllWeaponsRequested;

    public void RequestAllWeapons()
    {
        AllWeaponsRequested?.Invoke(this, EventArgs.Empty);
    }
}