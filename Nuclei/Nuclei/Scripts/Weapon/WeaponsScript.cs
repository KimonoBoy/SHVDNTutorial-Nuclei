using System;
using GTA;
using GTA.Math;
using GTA.Native;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;

namespace Nuclei.Scripts.Weapon;

public class WeaponsScript : GenericScriptBase<WeaponsService>
{
    private DateTime _teleportGunLastShot = DateTime.UtcNow;

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.AllWeaponsRequested += OnAllWeaponsRequested;
        GameStateTimer.SubscribeToTimerElapsed(UpdateWeapons);
    }

    public override void UnsubscribeOnExit()
    {
        Tick -= OnTick;
        Service.AllWeaponsRequested -= OnAllWeaponsRequested;
        GameStateTimer.UnsubscribeFromTimerElapsed(UpdateWeapons);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;

        void ProcessFeature(Func<bool> isEnabled, Action<bool> process)
        {
            var enabled = isEnabled();
            if (enabled)
                process(enabled);
        }

        ProcessFeature(() => Service.FireBullets, ProcessFireBullets);
        ProcessFeature(() => Service.InfiniteAmmo, ProcessInfiniteAmmo);
        ProcessFeature(() => Service.NoReload, ProcessNoReload);
        ProcessFeature(() => Service.ExplosiveBullets, ProcessExplosiveBullets);
        ProcessFeature(() => Service.LevitationGun, ProcessLevitationGun);
        ProcessFeature(() => Service.TeleportGun, ProcessTeleportGun);
    }

    private void ProcessTeleportGun(bool teleportGun)
    {
        if (!teleportGun || !Character.IsAiming || !Character.IsShooting) return;

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

    private void ProcessLevitationGun(bool levitationGun)
    {
        if (!levitationGun) return;

        var targetedEntity = Game.Player.TargetedEntity;

        if (targetedEntity == null || !targetedEntity.HasBeenDamagedBy(Character)) return;

        if (targetedEntity is Ped ped) targetedEntity = ped.IsInVehicle() ? ped.CurrentVehicle : ped;

        Function.Call(Hash.SET_VEHICLE_GRAVITY, targetedEntity, false);
        targetedEntity.HasGravity = false;
        targetedEntity.ApplyForce(Vector3.WorldUp * 0.2f);
    }

    private void ProcessExplosiveBullets(bool explosiveBullets)
    {
        if (explosiveBullets)
            Game.Player.SetExplosiveAmmoThisFrame();
    }

    private void UpdateWeapons(object sender, EventArgs e)
    {
        if (Character == null) return;
    }

    private void ProcessInfiniteAmmo(bool infiniteAmmo)
    {
        if (!infiniteAmmo || (!Character.IsReloading &&
                              Character.Weapons.Current.Ammo != Character.Weapons.Current.AmmoInClip)) return;
        if (Character.Weapons.Current.Ammo == Character.Weapons.Current.AmmoInClip &&
            Character.Weapons.Current.Ammo >= 10)
            return;

        Character.Weapons.Current.Ammo = Character.Weapons.Current.MaxAmmo;
        Character.Weapons.Current.AmmoInClip = Character.Weapons.Current.MaxAmmoInClip;
    }

    private void ProcessNoReload(bool noReload)
    {
        if (!Character.IsShooting) return;

        var infiniteAmmoNoReload = noReload && Service.InfiniteAmmo;
        if (infiniteAmmoNoReload)
            Function.Call(Hash.REFILL_AMMO_INSTANTLY, Character);

        Character.Weapons.Current.InfiniteAmmoClip = infiniteAmmoNoReload;
        Character.Weapons.Current.InfiniteAmmo = infiniteAmmoNoReload;
    }

    private void ProcessFireBullets(bool isFireBullets)
    {
        if (isFireBullets)
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