using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using Nuclei.Enums.Vehicle;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Vehicle.VehicleWeapons;

namespace Nuclei.Scripts.Vehicle.VehicleWeapons;

public class VehicleWeaponsScript : GenericScriptBase<VehicleWeaponsService>
{
    private const float MinBulletDistance = 120.0f;
    private DateTime _lastShotTime = DateTime.UtcNow;
    private int _minBulletInterval;

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        GameStateTimer.SubscribeToTimerElapsed(UpdateVehicleWeapon);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        ProcessVehicleWeaponShoot(Service.VehicleWeapon.Value);
    }

    private void UpdateVehicleWeapon(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        UpdateFeature(Service.FireRate.Value, UpdateFireRate);
    }

    private void UpdateFireRate(int fireRate)
    {
        if (!Service.HasVehicleWeapons.Value) return;
        if (_minBulletInterval == fireRate) return;

        _minBulletInterval = fireRate * 25;
    }

    private void ProcessVehicleWeaponShoot(uint weaponHash)
    {
        if (!Service.HasVehicleWeapons.Value || !Game.IsKeyPressed(Keys.T)) return;
        RemoveDistantProjectiles();

        if (!IsTimeToShoot()) return;
        _lastShotTime = DateTime.UtcNow;

        Vector3? targetPoint = null;
        if (Service.PointAndShoot.Value)
        {
            var targetDistance = 150.0f; // You can adjust this value as needed
            targetPoint = GameplayCamera.Position + GameplayCamera.Direction * targetDistance;
        }

        try
        {
            // Get shooting points based on the VehicleWeaponAttachment value
            var shootingPoints = GetShootingPoints(Service.VehicleWeaponAttachment.Value);
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

    private Vector3[] GetShootingPoints(VehicleWeaponAttachmentPoint attachmentPoint)
    {
        var dimensions = CurrentVehicle.Model.Dimensions;
        var vehicleSize = dimensions.frontTopRight - dimensions.rearBottomLeft;
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
        return (DateTime.UtcNow - _lastShotTime).TotalMilliseconds >= _minBulletInterval;
    }

    private void ShootBullet(uint weaponHash, Vector3 shootingPoint, Vector3? targetPoint)
    {
        var weaponAsset = new WeaponAsset(weaponHash);

        if (!weaponAsset.Request(500))
            throw new VehicleWeaponRequestTimedOutException();

        if (weaponAsset is not { IsLoaded: true, IsValid: true })
            throw new VehicleWeaponNotFoundException();

        var targetPosition = targetPoint ?? shootingPoint + CurrentVehicle.ForwardVector * MinBulletDistance;

        World.ShootBullet(
            shootingPoint,
            targetPosition,
            Character,
            weaponAsset,
            1000);

        weaponAsset.MarkAsNoLongerNeeded();
    }

    private void RemoveDistantProjectiles()
    {
        var allProjectilesInWorld = World.GetAllProjectiles()
            .Where(p => p.Position.DistanceTo(Character.Position) >= MinBulletDistance);

        foreach (var projectile in allProjectilesInWorld)
            if (projectile.Exists())
                projectile.Delete();
    }
}