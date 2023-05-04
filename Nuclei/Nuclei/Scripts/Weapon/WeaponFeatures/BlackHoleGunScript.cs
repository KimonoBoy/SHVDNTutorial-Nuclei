using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.UI;

namespace Nuclei.Scripts.Weapon.WeaponFeatures;

public class BlackHoleGunScript : WeaponScriptBase
{
    private readonly List<Tuple<Prop, long>> _activeBlackHoles = new();
    private readonly int _maxBlackHoles = 3;
    private Prop _blackHoleEntity;

    protected override void OnTick(object sender, EventArgs e)
    {
        ProcessBlackHoleGun();
    }

    private Prop CreateBlackHole(Vector3 position)
    {
        var model = new Model("prop_rock_4_a");
        model.Request(1000);
        if (!model.IsInCdImage || !model.IsValid)
        {
            Notification.Show("Model not valid");
            return null;
        }

        while (!model.IsLoaded) Wait(100);
        // Create an invisible black hole entity
        _blackHoleEntity = World.CreateProp(model, position, false, false);
        _blackHoleEntity.IsVisible = false;
        _blackHoleEntity.IsCollisionProof = true;
        _blackHoleEntity.IsCollisionEnabled = false;

        return _blackHoleEntity;
    }

    private void ProcessBlackHoleGun()
    {
        if (!Service.BlackHoleGun) return;

        if (Character.IsShooting)
        {
            // If there are already MaxBlackHoles active, remove the oldest one
            if (_activeBlackHoles.Count >= _maxBlackHoles)
            {
                _activeBlackHoles[0].Item1.Delete();
                _activeBlackHoles.RemoveAt(0);
            }

            var aimedPosition = GetAimedPosition(50.0f);
            var newBlackHole = CreateBlackHole(aimedPosition);
            var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _activeBlackHoles.Add(Tuple.Create(newBlackHole, currentTimestamp));
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Remove black holes that have exceeded their duration
        _activeBlackHoles.RemoveAll(bh => now - bh.Item2 > (Service.BlackHoleLifeSpan + 1) * 1000);


        // Iterate through all active black holes and apply forces to nearby objects
        foreach (var blackHoleEntry in _activeBlackHoles)
        {
            var blackHole = blackHoleEntry.Item1;
            var blackHoleRadius = (Service.BlackHoleRadius + 1) * 15.0f;
            var blackHoleForceMultiplier = (Service.BlackHolePower + 1) * 50.0f;
            var objectsInRange = World.GetNearbyEntities(blackHole.Position, blackHoleRadius);
            foreach (var obj in objectsInRange)
            {
                if (obj == Character) continue;
                var distance = Vector3.Distance(obj.Position, blackHole.Position);
                var force = blackHoleForceMultiplier * (1 / (distance * distance));
                var direction = (blackHole.Position - obj.Position).Normalized;

                obj.ApplyForce(direction * force);
            }
        }
    }
}