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

        try
        {
            // Get shooting points based on the VehicleWeaponAttachment value
            var shootingPoints = GetShootingPoints(Service.VehicleWeaponAttachment.Value);
            foreach (var shootingPoint in shootingPoints) ShootBullet(weaponHash, shootingPoint);
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
        switch (attachmentPoint)
        {
            case VehicleWeaponAttachmentPoint.OneMiddle:
                return new[] { CurrentVehicle.Position + CurrentVehicle.ForwardVector * 5.0f };
            case VehicleWeaponAttachmentPoint.OneOnEachSide:
                return new[]
                {
                    CurrentVehicle.Position + CurrentVehicle.ForwardVector * 5.0f + CurrentVehicle.RightVector * -1.5f,
                    CurrentVehicle.Position + CurrentVehicle.ForwardVector * 5.0f + CurrentVehicle.RightVector * 1.5f
                };
            case VehicleWeaponAttachmentPoint.EachSideAndMiddle:
                return new[]
                {
                    CurrentVehicle.Position + CurrentVehicle.ForwardVector * 5.0f,
                    CurrentVehicle.Position + CurrentVehicle.ForwardVector * 5.0f + CurrentVehicle.RightVector * -1.5f,
                    CurrentVehicle.Position + CurrentVehicle.ForwardVector * 5.0f + CurrentVehicle.RightVector * 1.5f
                };
            default:
                throw new ArgumentOutOfRangeException(nameof(attachmentPoint), "Invalid number of weapons");
        }
    }

    private bool IsTimeToShoot()
    {
        return (DateTime.UtcNow - _lastShotTime).TotalMilliseconds >= _minBulletInterval;
    }

    private void ShootBullet(uint weaponHash, Vector3 shootingPoint)
    {
        var weaponAsset = new WeaponAsset(weaponHash);

        if (!weaponAsset.Request(500))
            throw new VehicleWeaponRequestTimedOutException();

        if (weaponAsset is not { IsLoaded: true, IsValid: true })
            throw new VehicleWeaponNotFoundException();

        World.ShootBullet(
            shootingPoint,
            shootingPoint + CurrentVehicle.ForwardVector * MinBulletDistance, Character,
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