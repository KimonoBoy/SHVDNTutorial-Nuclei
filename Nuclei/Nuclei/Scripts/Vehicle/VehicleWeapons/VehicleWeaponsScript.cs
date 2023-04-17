using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleWeapons;

namespace Nuclei.Scripts.Vehicle.VehicleWeapons;

public class VehicleWeaponsScript : GenericScriptBase<VehicleWeaponsService>
{
    private const int MinBulletInterval = 20;
    private const float MinBulletDistance = 100.0f;
    private DateTime _lastShotTime = DateTime.UtcNow;

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
    }

    private void ProcessVehicleWeaponShoot(uint weaponHash)
    {
        if (!Service.HasVehicleWeapons.Value || !Game.IsKeyPressed(Keys.T)) return;
        RemoveDistantProjectiles();

        if (!IsTimeToShoot()) return;
        _lastShotTime = DateTime.UtcNow;
        ShootBullet(weaponHash);
    }

    private bool IsTimeToShoot()
    {
        return (DateTime.UtcNow - _lastShotTime).TotalMilliseconds >= MinBulletInterval;
    }

    private void ShootBullet(uint weaponHash)
    {
        var weaponAsset = new WeaponAsset(weaponHash);
        weaponAsset.Request(1000);

        if (!weaponAsset.IsLoaded) return;

        World.ShootBullet(
            CurrentVehicle.Position + CurrentVehicle.ForwardVector * 5.0f,
            CurrentVehicle.Position + CurrentVehicle.ForwardVector * 100.0f, Character,
            weaponAsset,
            1000);

        weaponAsset.MarkAsNoLongerNeeded();
    }

    private void RemoveDistantProjectiles()
    {
        var allProjectilesInWorld = World.GetAllProjectiles()
            .Where(p => p.Position.DistanceTo(Character.Position) >= MinBulletDistance);

        foreach (var projectile in allProjectilesInWorld)
        {
            projectile.Explode();

            if (projectile.Exists()) projectile.Delete();
        }
    }
}