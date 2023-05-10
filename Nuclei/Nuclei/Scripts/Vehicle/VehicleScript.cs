using System;
using GTA;
using GTA.Math;
using GTA.Native;
using Nuclei.Enums.Hotkey;
using Nuclei.Enums.UI;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle;

namespace Nuclei.Scripts.Vehicle;

public class VehicleScript : GenericScript<VehicleService>
{
    private const int FliesThroughWindscreen = 32;
    private DateTime _speedBoostTimer = DateTime.UtcNow;

    protected override void SubscribeToEvents()
    {
        Service.RepairRequested += OnRepairRequested;
        Service.VehicleFlipRequested += OnVehicleFlipRequested;
    }

    protected override void UnsubscribeOnExit()
    {
        Service.RepairRequested -= OnRepairRequested;
        Service.VehicleFlipRequested -= OnVehicleFlipRequested;
    }

    protected override void OnTick(object sender, EventArgs e)
    {
        ProcessLockDoors();

        if (CurrentVehicle == null) return;

        ProcessIndestructible();
        ProcessSeatBelt();
        ProcessNeverFallOffBike();
        ProcessDriveUnderWater();
        ProcessSpeedBoost();
    }

    private void OnRepairRequested(object sender, EventArgs e)
    {
        Repair();
    }

    private void OnVehicleFlipRequested(object sender, EventArgs e)
    {
        FlipVehicle();
    }

    private void ProcessLockDoors()
    {
        /*
         * We'll implement this later.
         *
         * Need to think of a good way to make this less intrusive for the player.
         */
        if (!Service.DoorsAlwaysLocked) return;
        if (CurrentVehicle != null)
        {
            if (CurrentVehicle.LockStatus == VehicleLockStatus.CannotEnter) return;
            CurrentVehicle.LockStatus = VehicleLockStatus.CannotEnter;
        }

        if (Character.VehicleTryingToEnter == null) return;

        Character.VehicleTryingToEnter.LockStatus = VehicleLockStatus.Unlocked;
    }

    private void ProcessDriveUnderWater()
    {
        if (!CurrentVehicle.IsEngineRunning && CurrentVehicle.IsInWater && Service.CanDriveUnderWater)
            CurrentVehicle.IsEngineRunning = true;
    }

    private void ProcessSpeedBoost()
    {
        if (Service.SpeedBoost <= 0) return;
        if (CurrentVehicle.Velocity.Length() <= 0.1f) return;
        var speedBoostKey = Hotkeys.GetValue(SectionName.Vehicle, VehicleItemTitle.SpeedBoost);
        if (!Hotkeys.IsKeyPressed(speedBoostKey)) return;

        if ((DateTime.UtcNow - _speedBoostTimer).TotalMilliseconds < 100) return;

        _speedBoostTimer = DateTime.UtcNow;
        CurrentVehicle.Speed += Service.SpeedBoost;
        CurrentVehicle.ApplyForce(Vector3.WorldUp * -2.0f);
    }

    private void ProcessIndestructible()
    {
        if (CurrentVehicle.IsInvincible == Service.IsIndestructible) return;

        CurrentVehicle.Repair();
        CurrentVehicle.IsInvincible = Service.IsIndestructible;
        CurrentVehicle.CanBeVisiblyDamaged = !Service.IsIndestructible;
        CurrentVehicle.CanTiresBurst = !Service.IsIndestructible;
        CurrentVehicle.CanEngineDegrade = !Service.IsIndestructible;
        CurrentVehicle.CanWheelsBreak = !Service.IsIndestructible;
        Function.Call(Hash.SET_DONT_PROCESS_VEHICLE_GLASS, CurrentVehicle, Service.IsIndestructible);
    }

    private void ProcessSeatBelt()
    {
        if (Character.GetConfigFlag(FliesThroughWindscreen) == !Service.HasSeatBelt) return;

        Character.SetConfigFlag(FliesThroughWindscreen, !Service.HasSeatBelt);
    }

    private void ProcessNeverFallOffBike()
    {
        var canBeKnockedOffBike = Function.Call<bool>(Hash.CAN_KNOCK_PED_OFF_VEHICLE, Character);
        if (canBeKnockedOffBike != Service.NeverFallOffBike) return;
        Character.CanBeKnockedOffBike = !Service.NeverFallOffBike;
    }

    private void FlipVehicle()
    {
        CurrentVehicle?.PlaceOnGround();
    }

    private void Repair()
    {
        CurrentVehicle?.Repair();
    }
}

// private void ProcessTest()
// {
//     if (!Game.IsKeyPressed(Keys.NumPad9)) return;
//
//     var nearestProp = World.GetClosestProp(Character.Position, 10.0f);
//     if (nearestProp == null) return;
//     var vehicle = World.CreateVehicle(new Model(VehicleHash.Formula),
//         Character.Position,
//         Character.Heading);
//     nearestProp.AttachTo(vehicle);
//     vehicle.IsVisible = false;
//     nearestProp.IsVisible = true;
//     Character.SetIntoVehicle(vehicle, VehicleSeat.Driver);
//     vehicle.PlaceOnGround();
// }