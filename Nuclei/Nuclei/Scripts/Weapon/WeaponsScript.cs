using System;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;

namespace Nuclei.Scripts.Weapon;

public class WeaponsScript : GenericScriptBase<WeaponsService>
{
    private Entity _grabbedEntity;
    private float _grabbedEntityDistance;
    private Vector3 _previousCameraDirection;

    private DateTime _previousCameraDirectionTime;
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
        ProcessFireBullets();
        ProcessInfiniteAmmo();
        ProcessNoReload();
        ProcessExplosiveBullets();
        ProcessLevitationGun();
        ProcessGravityGun();
        ProcessTeleportGun();
    }

    private void ProcessGravityGun()
    {
        if (!Service.GravityGun || !Character.IsAiming) return;

        if (_grabbedEntity == null)
        {
            var targetedEntity = Game.Player.TargetedEntity;
            if (targetedEntity == null) return;

            if (targetedEntity is Ped ped) targetedEntity = ped.IsInVehicle() ? ped.CurrentVehicle : ped;
            if (targetedEntity is Prop prop) targetedEntity = prop.IsAttached() ? prop.AttachedEntity : prop;

            if (Game.IsKeyPressed(Keys.J))
            {
                _grabbedEntity = targetedEntity;
                _grabbedEntityDistance = Vector3.Distance(Character.Position, _grabbedEntity.Position);
                _previousCameraDirection = GameplayCamera.Direction;
            }
        }
        else
        {
            _grabbedEntity.Draw3DRectangleAroundObject();

            var cameraPosition = GameplayCamera.Position;
            var cameraDirection = GameplayCamera.Direction;

            var raycastHitPlane = RaycastPlaneIntersection(cameraPosition, cameraDirection, Character.Position,
                Vector3.WorldUp, _grabbedEntityDistance);
            var targetPosition = raycastHitPlane ?? cameraPosition + cameraDirection * _grabbedEntityDistance;

            var min = new Vector3(-0.5f, -0.5f, 0f);
            var max = new Vector3(0.5f, 0.5f, 1f);
            var boundingBoxCorners = _grabbedEntity.GetBoundingBoxCorners(min, max);
            var entityHeight = Math.Abs(boundingBoxCorners[4].Z - boundingBoxCorners[0].Z);
            var verticalOffset = entityHeight * 1.0f;

            targetPosition.Z -= verticalOffset;
            _grabbedEntity.Position = targetPosition;

            if (!Game.IsKeyPressed(Keys.J))
            {
                var releaseVelocity = (GameplayCamera.Direction - _previousCameraDirection) * 5000.0f;
                _grabbedEntity.ApplyForce(releaseVelocity);
                _grabbedEntity = null;
            }
            else
            {
                _previousCameraDirection = GameplayCamera.Direction;
            }
        }
    }

    private Vector3? RaycastPlaneIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 planePoint,
        Vector3 planeNormal, float maxDistance)
    {
        var denom = Vector3.Dot(rayDirection, planeNormal);

        if (Math.Abs(denom) > 0.0001f)
        {
            var t = Vector3.Dot(planePoint - rayOrigin, planeNormal) / denom;

            if (t >= 0 && t <= maxDistance) return rayOrigin + rayDirection * t;
        }

        return null;
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