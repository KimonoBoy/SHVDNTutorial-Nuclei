using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using Nuclei.Helpers.ExtensionMethods;
using Control = GTA.Control;

namespace Nuclei.Scripts.Weapon;

public class GravityGunScript : WeaponScriptBase
{
    private readonly List<Tuple<Vector3, long>> _cameraDirectionsTimestamps = new();
    private Entity _grabbedEntity;
    private float _grabbedEntityDistance;

    protected override void OnTick(object sender, EventArgs e)
    {
        ProcessGravityGun();
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
}