using System;
using GTA;
using GTA.Math;
using GTA.Native;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;

namespace Nuclei.Scripts.Weapon;

public class WeaponsScript : GenericScriptBase<WeaponsService>
{
    private DateTime _teleportGunLastShot = DateTime.UtcNow;

    protected override void UpdateServiceStatesTimer(object sender, EventArgs e)
    {
    }

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.AllWeaponsRequested += OnAllWeaponsRequested;
    }

    protected override void UnsubscribeOnExit()
    {
        Tick -= OnTick;
        Service.AllWeaponsRequested -= OnAllWeaponsRequested;
    }

    protected override void ProcessGameStatesTimer(object sender, EventArgs e)
    {
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;
        Character.Draw3DRectangleAroundObject();
        ProcessFireBullets();
        ProcessInfiniteAmmo();
        ProcessNoReload();
        ProcessExplosiveBullets();
        ProcessLevitationGun();
        ProcessTeleportGun();
        ProcessShootVehicles();
    }

    private void ProcessShootVehicles()
    {
        // if (!Character.IsAiming || !Character.IsShooting) return;
        // var model = new Model(VehicleHash.Adder);
        // model.Request(1000);
        // if (!model.IsLoaded) return;
        // Vector3? targetLocation;
        // var vehicle = World.CreateVehicle(model,
        //     Character.Position + Character.ForwardVector * 2.0f,
        //     Character.Heading);
        // var crosshairCoords = World.GetCrosshairCoordinates(IntersectFlags.Everything);
        //
        // if (crosshairCoords.DidHit)
        //     targetLocation = crosshairCoords.HitPosition;
        // else
        //     targetLocation = GameplayCamera.Position + GameplayCamera.Direction;
        //
        // vehicle.ApplyForce(targetLocation.Value + Character.ForwardVector * 1000.0f);
    }

    private void ProcessTeleportGun()
    {
        if (!Service.TeleportGun || !Character.IsAiming || !Character.IsShooting) return;

        if ((DateTime.UtcNow - _teleportGunLastShot).TotalMilliseconds < 500) return;
        _teleportGunLastShot = DateTime.UtcNow;

        Vector3? targetLocation;
        var crosshairCoords = World.GetCrosshairCoordinates(IntersectFlags.Everything);

        if (crosshairCoords.DidHit)
            targetLocation = crosshairCoords.HitPosition;
        else
            targetLocation = GameplayCamera.Position + GameplayCamera.Direction * 200.0f;
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