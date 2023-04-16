using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Vehicle;
using Nuclei.UI.Menus.Abstracts;
using Nuclei.UI.Menus.Vehicle.VehicleSpawner;

namespace Nuclei.UI.Menus.Vehicle;

public class VehicleMenu : GenericMenuBase<VehicleService>
{
    public VehicleMenu(Enum @enum) : base(@enum)
    {
        AddVehicleSpawnerMenu();

        AddHeader("Basics");
        RepairVehicle();
        Indestructible();
        SpeedBoost();

        AddHeader("Utilities");
        FlipVehicle();
        LockDoors();
        SeatBelt();
        NeverFallOffBike();
        DriveUnderWater();
    }

    private void LockDoors()
    {
        var checkBoxLockDoors = AddCheckbox(VehicleItemTitles.LockDoors, Service.LockDoors,
            @checked => { Service.LockDoors.Value = @checked; });
    }

    private void NeverFallOffBike()
    {
        var checkBoxNeverFallOffBike = AddCheckbox(VehicleItemTitles.NeverFallOffBike, Service.NeverFallOffBike,
            @checked => { Service.NeverFallOffBike.Value = @checked; });
    }

    private void DriveUnderWater()
    {
        var checkBoxDriveUnderWater = AddCheckbox(VehicleItemTitles.DriveUnderWater, Service.DriveUnderWater,
            @checked => { Service.DriveUnderWater.Value = @checked; });
    }

    private void AddVehicleSpawnerMenu()
    {
        var vehicleSpawnerMenu = new VehicleSpawnerMainMenu(MenuTitles.SpawnVehicle);
        AddMenu(vehicleSpawnerMenu);
    }

    private void FlipVehicle()
    {
        var itemFlipVehicle = AddItem(VehicleItemTitles.FlipVehicle, () => { Service.RequestVehicleFlip(); });
    }

    private void SpeedBoost()
    {
        var sliderItemSpeedBoost = AddSliderItem(VehicleItemTitles.SpeedBoost, Service.SpeedBoost,
            speedBoostValue => { Service.SpeedBoost.Value = speedBoostValue; }, 0, 5);
    }

    private void SeatBelt()
    {
        var checkBoxSeatBelt = AddCheckbox(VehicleItemTitles.SeatBelt, Service.SeatBelt,
            @checked => { Service.SeatBelt.Value = @checked; });
    }

    private void RepairVehicle()
    {
        var itemRepairVehicle = AddItem(VehicleItemTitles.RepairVehicle, () => { Service.RequestRepair(); });
    }

    private void Indestructible()
    {
        var checkBoxIndestructible = AddCheckbox(VehicleItemTitles.Indestructible, Service.Indestructible,
            @checked => { Service.Indestructible.Value = @checked; });
    }
}