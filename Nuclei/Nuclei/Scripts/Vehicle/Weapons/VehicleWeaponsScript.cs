using System;
using System.Linq;
using GTA;
using GTA.Math;
using Nuclei.Enums.Hotkey;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.VehicleWeapons;

namespace Nuclei.Scripts.Vehicle.Weapons;

public class VehicleWeaponsScript : GenericScript<VehicleWeaponsService>
{
    private const float MinProjectileDistance = 150.0f;
    private DateTime _lastShotTime = DateTime.UtcNow;
    private float _minShootInterval;

    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }

    private void ProcessFireRate()
    {
        if (Math.Abs(_minShootInterval - Service.FireRate * 25.0f) < 1.0f) return;

        _minShootInterval = Service.FireRate * 25.0f;
    }


    protected override void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        ProcessVehicleWeaponShoot(Service.VehicleWeapon);
        ProcessFireRate();
    }

    private void ProcessVehicleWeaponShoot(uint weaponHash)
    {
        if (!Service.HasVehicleWeapons) return;

        var shootWeaponKey =
            Service.Hotkeys.GetValue(SectionName.VehicleWeapon, VehicleWeaponsItemTitle.ShootVehicleWeapon);
        if (!Service.Hotkeys.IsKeyPressed(shootWeaponKey)) return;

        RemoveDistantProjectiles();

        if (!IsTimeToShoot()) return;
        _lastShotTime = DateTime.UtcNow;

        try
        {
            // Defines the target point for the projectile.
            Vector3? targetPoint = null;
            if (Service.PointAndShoot && Character.IsAiming)
                targetPoint = GetCrosshairAimPoint(MinProjectileDistance);

            // Get shooting points based on the VehicleWeaponAttachment value
            var shootingPoints = GetShootingPoints(Service.VehicleWeaponAttachment);
            foreach (var shootingPoint in shootingPoints) ShootBullet(weaponHash, shootingPoint, targetPoint);
        }
        catch (CustomExceptionBase vehicleWeaponsException)
        {
            ExceptionService.RaiseError(vehicleWeaponsException);
        }
        catch (Exception ex)
        {
            ExceptionService.RaiseError(ex);
        }
    }

    private Vector3 GetCrosshairAimPoint(float maxDistance)
    {
        var raycastResult = GTA.World.Raycast(
            GameplayCamera.Position,
            GameplayCamera.Position + GameplayCamera.Direction * maxDistance,
            IntersectFlags.Everything, Character);

        if (raycastResult.DidHit)
            return raycastResult.HitPosition;
        return GameplayCamera.Position + GameplayCamera.Direction * maxDistance;
    }

    private Vector3[] GetShootingPoints(VehicleWeaponAttachmentPoint attachmentPoint)
    {
        var (rearBottomLeft, frontTopRight) = CurrentVehicle.Model.Dimensions;
        var vehicleSize = frontTopRight - rearBottomLeft;
        var vehicleFrontOffset = CurrentVehicle.ForwardVector * vehicleSize.Y / 2;

        switch (attachmentPoint)
        {
            case VehicleWeaponAttachmentPoint.OneMiddle:
                return new[] { CurrentVehicle.Position + vehicleFrontOffset };
            case VehicleWeaponAttachmentPoint.OneOnEachSide:
            {
                var rightOffset = CurrentVehicle.RightVector * vehicleSize.X / 4;
                return new[]
                {
                    CurrentVehicle.Position + vehicleFrontOffset - rightOffset,
                    CurrentVehicle.Position + vehicleFrontOffset + rightOffset
                };
            }
            case VehicleWeaponAttachmentPoint.EachSideAndMiddle:
            {
                var rightOffset = CurrentVehicle.RightVector * vehicleSize.X / 4;
                return new[]
                {
                    CurrentVehicle.Position + vehicleFrontOffset,
                    CurrentVehicle.Position + vehicleFrontOffset - rightOffset,
                    CurrentVehicle.Position + vehicleFrontOffset + rightOffset
                };
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(attachmentPoint), "Invalid number of weapons");
        }
    }

    private bool IsTimeToShoot()
    {
        return (DateTime.UtcNow - _lastShotTime).TotalMilliseconds >= _minShootInterval;
    }

    private void ShootBullet(uint weaponHash, Vector3 shootingPoint, Vector3? targetPoint)
    {
        var weaponAsset = new WeaponAsset(weaponHash);

        if (!weaponAsset.Request(500))
            throw new VehicleWeaponRequestTimedOutException();

        if (weaponAsset is not { IsLoaded: true, IsValid: true })
            throw new VehicleWeaponNotFoundException();

        var targetPosition = targetPoint ?? shootingPoint + CurrentVehicle.ForwardVector * MinProjectileDistance;
        GTA.World.ShootBullet(
            shootingPoint,
            targetPosition,
            Character,
            weaponAsset,
            1000);

        weaponAsset.MarkAsNoLongerNeeded();
    }

    private void RemoveDistantProjectiles()
    {
        var projectilesFurtherThanMinProjectileDistance = GTA.World.GetAllProjectiles()
            .OrderByDescending(p => p.Position.DistanceTo(Character.Position));

        foreach (var projectile in projectilesFurtherThanMinProjectileDistance)
        {
            projectile.IsRecordingCollisions = true;
            if (projectile.HasCollided) projectile.Explode();
            if (projectile.IsAlive && projectile.Position.DistanceTo(Character.Position) >= MinProjectileDistance)
                projectile.Delete();
        }
    }
}