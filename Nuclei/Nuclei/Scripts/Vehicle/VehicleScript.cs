using System;
using GTA;
using GTA.Math;
using GTA.Native;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle;

namespace Nuclei.Scripts.Vehicle;

public class VehicleScript : GenericScriptBase<VehicleService>
{
    private const int FliesThroughWindscreen = 32;
    private DateTime _speedBoostTimer = DateTime.UtcNow;

    protected override void UpdateServiceStatesTimer(object sender, EventArgs e)
    {
    }

    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.RepairRequested += OnRepairRequested;
        Service.VehicleFlipRequested += OnVehicleFlipRequested;
    }

    protected override void UnsubscribeOnExit()
    {
        Tick -= OnTick;
        Service.RepairRequested -= OnRepairRequested;
        Service.VehicleFlipRequested -= OnVehicleFlipRequested;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

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

    protected override void ProcessGameStatesTimer(object sender, EventArgs e)
    {
        ProcessLockDoors();

        if (CurrentVehicle == null) return;

        ProcessIndestructible();
        ProcessSeatBelt();
        ProcessNeverFallOffBike();
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
        if (!Game.IsControlPressed(Control.Sprint) || Service.SpeedBoost <= 0) return;

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

    private static void FlipVehicle()
    {
        CurrentVehicle?.PlaceOnGround();
    }

    private void Repair()
    {
        CurrentVehicle?.Repair();
    }
}