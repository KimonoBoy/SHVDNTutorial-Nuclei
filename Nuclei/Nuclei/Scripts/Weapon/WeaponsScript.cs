using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;
using Control = GTA.Control;

namespace Nuclei.Scripts.Weapon;

public class WeaponsScript : GenericScriptBase<WeaponsService>
{
    private const float BlackHoleRadius = 100.0f;
    private const float BlackHoleForceMultiplier = 1000.0f;
    private const string BlackHoleParticleAsset = "core";
    private const string BlackHoleParticleEffect = "ent_amb_foundry_molten_puddle";
    private readonly List<Tuple<Vector3, long>> _cameraDirectionsTimestamps = new();
    private Prop _blackHoleEntity;
    private int _blackHoleParticle;

    private Entity _grabbedEntity;
    private float _grabbedEntityDistance;
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
        ProcessBlackHoleGun();
    }

    private void CreateBlackHole(Vector3 position)
    {
        var model = new Model("prop_rock_4_a");
        model.Request(1000);
        if (!model.IsInCdImage || !model.IsValid)
        {
            Notification.Show("Model not valid");
            return;
        }

        while (!model.IsLoaded) Wait(100);
        // Create an invisible black hole entity
        _blackHoleEntity = World.CreateProp(model, position, false, false);
        _blackHoleEntity.IsVisible = false;
    }

    private void ProcessBlackHoleGun()
    {
        if (!Service.BlackHoleGun) return;

        if (_blackHoleEntity == null)
        {
            if (Game.IsKeyPressed(Keys.H))
            {
                Vector3 aimedPosition;

                var crosshairCoords = World.GetCrosshairCoordinates(IntersectFlags.Everything);

                if (crosshairCoords.DidHit)
                    aimedPosition = crosshairCoords.HitPosition;
                else
                    aimedPosition = GameplayCamera.Position + GameplayCamera.Direction * 200.0f;

                CreateBlackHole(aimedPosition);
            }
        }
        else
        {
            var objectsInRange = World.GetNearbyEntities(_blackHoleEntity.Position, BlackHoleRadius);
            foreach (var obj in objectsInRange)
            {
                if (obj == Character) continue;
                var distance = Vector3.Distance(obj.Position, _blackHoleEntity.Position);
                var force = BlackHoleForceMultiplier * (1 / (distance * distance));
                var direction = (_blackHoleEntity.Position - obj.Position).Normalized;

                obj.ApplyForce(direction * force);
            }

            if (!Game.IsKeyPressed(Keys.H))
            {
                _blackHoleEntity.Delete();
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, _blackHoleParticle, false);
                _blackHoleEntity = null;
            }
        }
    }

    private void ProcessGravityGun()
    {
        if (!Service.GravityGun || !Character.IsAiming) return;

        if (_grabbedEntity == null)
        {
            var targetedEntity = Game.Player.TargetedEntity;
            if (targetedEntity == null) return;
            if (targetedEntity is Ped ped) targetedEntity = ped.IsInVehicle() ? ped.CurrentVehicle : ped;
            if (Game.IsKeyPressed(Keys.J))
            {
                _grabbedEntity = targetedEntity;
                _grabbedEntityDistance = Vector3.Distance(Character.Position, _grabbedEntity.Position);
            }
        }
        else
        {
            HandleGrabbedEntity();
            AdjustGrabbedEntityDistance();
        }
    }

    private void HandleGrabbedEntity()
    {
        if (_grabbedEntity is Ped ped)
        {
            ped.Task.ClearAllImmediately();
            ped.Task.LookAt(Character);
        }

        _grabbedEntity.Draw3DRectangleAroundObject();
        var targetPosition = CalculateTargetPosition();
        _grabbedEntity.Position = targetPosition;

        if (!Game.IsKeyPressed(Keys.J))
        {
            var throwVelocity = (Service.ThrowVelocity + 1) * 25.0f;
            ReleaseGrabbedEntity(throwVelocity);
        }

        _cameraDirectionsTimestamps.Add(new Tuple<Vector3, long>(GameplayCamera.Direction,
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
    }

    private Vector3 CalculateTargetPosition()
    {
        var cameraPosition = GameplayCamera.Position;
        var cameraDirection = GameplayCamera.Direction;
        var targetPosition = cameraPosition + cameraDirection * _grabbedEntityDistance;

        var min = new Vector3(-0.5f, -0.5f, 0f);
        var max = new Vector3(0.5f, 0.5f, 1f);
        var boundingBoxCorners = _grabbedEntity.GetBoundingBoxCorners(min, max);
        var entityHeight = Math.Abs(boundingBoxCorners[4].Z - boundingBoxCorners[0].Z);
        var verticalOffset = entityHeight * 1.0f;

        targetPosition.Z -= verticalOffset;
        return targetPosition;
    }

    private void ReleaseGrabbedEntity(float throwVelocity)
    {
        var accumulatedVelocity = CalculateAccumulatedVelocity(150);
        var releaseVelocity = accumulatedVelocity * throwVelocity;
        if (_grabbedEntity is Ped grabbedPed) grabbedPed.Ragdoll(-1, RagdollType.NarrowLegs);

        _grabbedEntity.ApplyForce(releaseVelocity);
        _grabbedEntity = null;
    }

    private void AdjustGrabbedEntityDistance()
    {
        if (Game.IsControlJustPressed(Control.CursorScrollUp))
            _grabbedEntityDistance += 10.0f;
        else if (Game.IsControlJustPressed(Control.CursorScrollDown))
            _grabbedEntityDistance -= 10.0f;
    }

    private Vector3 CalculateAccumulatedVelocity(long durationMs)
    {
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var accumulatedVelocity = new Vector3();

        _cameraDirectionsTimestamps.RemoveAll(timestampedDirection =>
            currentTime - timestampedDirection.Item2 > durationMs);

        for (var i = 1; i < _cameraDirectionsTimestamps.Count; i++)
            accumulatedVelocity += _cameraDirectionsTimestamps[i].Item1 - _cameraDirectionsTimestamps[i - 1].Item1;

        return accumulatedVelocity;
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