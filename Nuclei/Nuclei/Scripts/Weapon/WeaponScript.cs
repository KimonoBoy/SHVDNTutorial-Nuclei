﻿using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Nuclei.Scripts.Weapon;

public class WeaponScript : WeaponScriptBase
{
    private DateTime _teleportGunLastShot = DateTime.UtcNow;

    protected override void SubscribeToEvents()
    {
        Service.AllWeaponsRequested += OnAllWeaponsRequested;
    }

    protected override void UnsubscribeOnExit()
    {
        Service.AllWeaponsRequested -= OnAllWeaponsRequested;
    }

    protected override void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;
        ProcessFireBullets();
        ProcessInfiniteAmmo();
        ProcessNoReload();
        ProcessExplosiveBullets();
        ProcessLevitationGun();
        ProcessTeleportGun();
    }

    private void ProcessTeleportGun()
    {
        if (!Service.TeleportGun || !Character.IsAiming || !Character.IsShooting) return;

        if ((DateTime.UtcNow - _teleportGunLastShot).TotalMilliseconds < 500) return;
        _teleportGunLastShot = DateTime.UtcNow;

        Vector3? targetLocation = GetAimedPosition(200.0f);
        Wait(100);

        CurrentEntity.Position = targetLocation.Value;
    }

    private void ProcessLevitationGun()
    {
        if (!Service.LevitationGun) return;

        var targetedEntity = Game.Player.TargetedEntity;

        if (targetedEntity == null || !targetedEntity.HasBeenDamagedBy(Character)) return;

        if (targetedEntity is Ped ped) targetedEntity = ped.IsInVehicle() ? ped.CurrentVehicle : ped;

        Function.Call(Hash.SET_VEHICLE_GRAVITY, targetedEntity, false);
        targetedEntity.HasGravity = false;
        targetedEntity.ApplyForce(Vector3.WorldUp * 0.2f);
    }

    private void ProcessExplosiveBullets()
    {
        if (Service.ExplosiveBullets)
            Game.Player.SetExplosiveAmmoThisFrame();
    }

    private void ProcessInfiniteAmmo()
    {
        if (!Service.InfiniteAmmo || (!Character.IsReloading &&
                                      Character.Weapons.Current.Ammo != Character.Weapons.Current.AmmoInClip)) return;
        if (Character.Weapons.Current.Ammo == Character.Weapons.Current.AmmoInClip &&
            Character.Weapons.Current.Ammo >= 10)
            return;

        Character.Weapons.Current.Ammo = Character.Weapons.Current.MaxAmmo;
        Character.Weapons.Current.AmmoInClip = Character.Weapons.Current.MaxAmmoInClip;
    }

    private void ProcessNoReload()
    {
        if (!Character.IsShooting) return;

        var infiniteAmmoNoReload = Service.NoReload && Service.InfiniteAmmo;
        if (infiniteAmmoNoReload)
            Function.Call(Hash.REFILL_AMMO_INSTANTLY, Character);

        Character.Weapons.Current.InfiniteAmmoClip = infiniteAmmoNoReload;
        Character.Weapons.Current.InfiniteAmmo = infiniteAmmoNoReload;
    }

    private void ProcessFireBullets()
    {
        if (Service.FireBullets)
            Game.Player.SetFireAmmoThisFrame();
    }

    private void OnAllWeaponsRequested(object sender, EventArgs e)
    {
        GiveAllWeapons();
    }

    private void GiveAllWeapons()
    {
        foreach (WeaponHash weaponHash in Enum.GetValues(typeof(WeaponHash)))
            GiveWeapon(weaponHash);
    }

    private void GiveWeapon(WeaponHash weaponHash)
    {
        var weapon = Character.Weapons.Give(weaponHash, 0, false, true);
        Character.Weapons[weaponHash].Ammo = weapon.MaxAmmo;
    }
}