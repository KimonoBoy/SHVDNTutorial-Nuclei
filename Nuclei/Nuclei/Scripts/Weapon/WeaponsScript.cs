using System;
using GTA;
using GTA.Native;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;

namespace Nuclei.Scripts.Weapon;

public class WeaponsScript : GenericScriptBase<WeaponsService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.AllWeaponsRequested += OnAllWeaponsRequested;
        GameStateTimer.SubscribeToTimerElapsed(UpdateWeapons);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;

        UpdateFeature(Service.FireBullets.Value, ProcessFireBullets);
        UpdateFeature(Service.InfiniteAmmo.Value, ProcessInfiniteAmmo);
        UpdateFeature(Service.InfiniteAmmo.Value && Service.NoReload.Value, ProcessNoReload);
    }

    private void UpdateWeapons(object sender, EventArgs e)
    {
        if (Character == null) return;
    }

    private void ProcessInfiniteAmmo(bool infiniteAmmo)
    {
        if (!infiniteAmmo) return;
        if (!Character.IsReloading &&
            Character.Weapons.Current.Ammo != Character.Weapons.Current.AmmoInClip) return;
        if (Character.Weapons.Current.Ammo == Character.Weapons.Current.AmmoInClip &&
            Character.Weapons.Current.Ammo >= 10)
            return; // For minigun and other weapons with shared clipSize

        Character.Weapons.Current.Ammo = Character.Weapons.Current.MaxAmmo;
        Character.Weapons.Current.AmmoInClip = Character.Weapons.Current.MaxAmmoInClip;
    }

    private void ProcessNoReload(bool noReload)
    {
        if (!Character.IsShooting) return;
        if (noReload)
            Function.Call(Hash.REFILL_AMMO_INSTANTLY, Character);

        Character.Weapons.Current.InfiniteAmmoClip = noReload;
        Character.Weapons.Current.InfiniteAmmo = noReload;
    }

    private void ProcessFireBullets(bool isFireBullets)
    {
        if (!isFireBullets) return;
        if (!Character.IsShooting) return;

        Game.Player.SetFireAmmoThisFrame();
    }

    private void OnAllWeaponsRequested(object sender, EventArgs e)
    {
        GiveAllWeapons();
    }

    private void GiveAllWeapons()
    {
        foreach (WeaponHash weaponHash in Enum.GetValues(typeof(WeaponHash))) GiveWeapon(weaponHash);
    }

    private void GiveWeapon(WeaponHash weaponHash)
    {
        var weapon = Character.Weapons.Give(weaponHash, 0, false, true);
        Character.Weapons[weaponHash].Ammo = weapon.MaxAmmo;
    }
}