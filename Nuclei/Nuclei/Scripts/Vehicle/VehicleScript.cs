using System;
using GTA;
using GTA.Native;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle;

namespace Nuclei.Scripts.Vehicle;

public class VehicleScript : GenericScriptBase<VehicleService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.RepairRequested += OnRepairRequested;
        Service.VehicleFlipRequested += OnVehicleFlipRequested;
        GameStateTimer.SubscribeToTimerElapsed(UpdateVehicle);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (CurrentVehicle == null) return;

        UpdateFeature(Service.CanDriveUnderWater.Value, ProcessDriveUnderWater);
    }

    private void OnRepairRequested(object sender, EventArgs e)
    {
        Repair();
    }

    private void OnVehicleFlipRequested(object sender, EventArgs e)
    {
        FlipVehicle();
    }

    private void UpdateVehicle(object sender, EventArgs e)
    {
        UpdateFeature(Service.DoorsAlwaysLocked.Value, UpdateLockDoors);

        if (CurrentVehicle == null) return;

        UpdateFeature(Service.IsIndestructible.Value, UpdateIndestructible);
        UpdateFeature(Service.SpeedBoost.Value, ProcessSpeedBoost);
        UpdateFeature(Service.HasSeatBelt.Value, UpdateSeatBelt);
        UpdateFeature(Service.NeverFallOffBike.Value, UpdateNeverFallOffBike);
    }

    private void UpdateLockDoors(bool lockDoors)
    {
        if (LastVehicle == null) return;

        if (LastVehicle == CurrentVehicle && lockDoors)
            LastVehicle.LockStatus = VehicleLockStatus.CannotEnter;
        else
            LastVehicle.LockStatus = VehicleLockStatus.Unlocked;
    }

    private void ProcessDriveUnderWater(bool driveUnderWater)
    {
        if (!CurrentVehicle.IsEngineRunning && driveUnderWater)
            CurrentVehicle.IsEngineRunning = true;
    }

    private void ProcessSpeedBoost(int speedValue)
    {
        if (!Game.IsControlPressed(Control.Sprint) || speedValue <= 0) return;

        CurrentVehicle.Speed += speedValue / 2.0f;
        CurrentVehicle.ForwardSpeed = CurrentVehicle.Speed + (1 + speedValue / 1.5f);
        ;
    }

    private void UpdateIndestructible(bool indestructible)
    {
        if (CurrentVehicle.IsInvincible != indestructible)
        {
            CurrentVehicle.IsInvincible = indestructible;
            CurrentVehicle.CanBeVisiblyDamaged = !indestructible;
        }
    }

    private void UpdateSeatBelt(bool seatBelt)
    {
        if (Character.GetConfigFlag(FliesThroughWindscreen) == !seatBelt) return;

        Character.SetConfigFlag(FliesThroughWindscreen, !seatBelt);
    }

    private void UpdateNeverFallOffBike(bool neverFallOffBike)
    {
        var canBeKnockedOffBike = Function.Call<bool>(Hash.CAN_KNOCK_PED_OFF_VEHICLE, Character);

        if (!canBeKnockedOffBike && neverFallOffBike) return;

        Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Character, neverFallOffBike ? 1 : 0);
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