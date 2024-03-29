﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.UI;

namespace Nuclei.Scripts.Weapon;

public class BlackHoleGunScript : WeaponScriptBase
{
    private readonly Dictionary<Prop, List<Entity>> _cachedObjects = new();
    private readonly Dictionary<Entity, long> _objectsInBlackHole = new();
    private Prop _activeBlackHole;
    private long _activeBlackHoleTimestamp;
    private Prop _blackHoleEntity;
    private int _blackHoleLifeSpan;
    private int _blackHolePower;
    private int _blackHoleRadius;
    private long _lastObjectCacheUpdateTime;
    private long _lastUpdateTime;
    private Model _model = new("prop_rock_4_a");


    protected override void OnTick(object sender, EventArgs e)
    {
        CacheServiceValues();
        ProcessBlackHoleGun();
    }

    private void CacheServiceValues()
    {
        _blackHoleLifeSpan = Service.BlackHoleLifeSpan + 1;
        _blackHoleRadius = (Service.BlackHoleRadius + 1) * 15;
        _blackHolePower = (Service.BlackHolePower + 1) * 50;
    }

    private bool ShouldUpdate()
    {
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (currentTime - _lastUpdateTime > 25)
        {
            _lastUpdateTime = currentTime;
            return true;
        }

        return false;
    }

    private bool ShouldUpdateObjectCache()
    {
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (currentTime - _lastObjectCacheUpdateTime > 100)
        {
            _lastObjectCacheUpdateTime = currentTime;
            return true;
        }

        return false;
    }

    private void UpdateObjectCache()
    {
        if (ShouldUpdateObjectCache() && _activeBlackHole != null)
            _cachedObjects[_activeBlackHole] =
                GTA.World.GetNearbyEntities(_activeBlackHole.Position, _blackHoleRadius).ToList();
    }

    private Prop CreateBlackHole(Vector3 position)
    {
        if (!_model.IsLoaded)
        {
            _model.Request(1000);
            if (!_model.IsInCdImage || !_model.IsValid)
            {
                Notification.Show("Model not valid");
                return null;
            }
        }

        while (!_model.IsLoaded) Wait(100);
        _blackHoleEntity = GTA.World.CreateProp(_model, position, false, false);
        _blackHoleEntity.IsVisible = false;
        _blackHoleEntity.IsCollisionProof = true;
        _blackHoleEntity.IsCollisionEnabled = false;

        return _blackHoleEntity;
    }

    private void ProcessBlackHoleGun()
    {
        if (!Service.BlackHoleGun) return;
        if (_blackHoleEntity != null)
            GTA.World.DrawMarker(MarkerType.DebugSphere, _blackHoleEntity.Position, _blackHoleEntity.ForwardVector,
                _blackHoleEntity.Rotation,
                new Vector3(_blackHoleRadius / 80.0f, _blackHoleRadius / 80.0f, _blackHoleRadius / 80.0f),
                Color.Black);

        if (Character.IsShooting)
        {
            // If a black hole is already active, delete it
            if (_activeBlackHole != null) _activeBlackHole.Delete();

            var aimedPosition = GetAimedPosition(50.0f);
            _activeBlackHole = CreateBlackHole(aimedPosition);
            _activeBlackHoleTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        if (ShouldUpdate())
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (_activeBlackHole != null && now - _activeBlackHoleTimestamp > (_blackHoleLifeSpan + 1) * 1000)
            {
                _activeBlackHole.Delete();
                _activeBlackHole = null;
            }

            UpdateObjectCache();

            if (_activeBlackHole != null)
                if (_cachedObjects.TryGetValue(_activeBlackHole, out var objectsInRange))
                    foreach (var obj in objectsInRange)
                    {
                        if (obj == Character || obj == _activeBlackHole) continue;
                        var distance = Vector3.Distance(obj.Position, _activeBlackHole.Position);

                        // Check if the object is within 3.0f units of the black hole
                        if (distance < 8.0f)
                        {
                            // Add the object to the dictionary and store the current time
                            if (!_objectsInBlackHole.ContainsKey(obj))
                            {
                                _objectsInBlackHole[obj] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            }
                            // Check if the object has spent enough time in the black hole, and if so, delete it
                            else if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _objectsInBlackHole[obj] >=
                                     1000.0f)
                            {
                                obj.Delete();
                                _objectsInBlackHole.Remove(obj);
                                _model.MarkAsNoLongerNeeded();
                                continue;
                            }
                        }
                        else
                        {
                            // Remove the object from the dictionary if it's no longer within the 3.0f units distance
                            _objectsInBlackHole.Remove(obj);
                        }

                        var force = _blackHolePower * (1 / (distance * distance));
                        var direction = (_activeBlackHole.Position - obj.Position).Normalized;

                        obj.ApplyForce(direction * force);
                    }
        }
    }
}