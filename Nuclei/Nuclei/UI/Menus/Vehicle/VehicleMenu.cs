using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Vehicle;
using Nuclei.UI.Menus.Base;
using Nuclei.UI.Menus.Vehicle.VehicleMods;
using Nuclei.UI.Menus.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Vehicle.VehicleWeapons;

namespace Nuclei.UI.Menus.Vehicle;

public class VehicleMenu : GenericMenuBase<VehicleService>
{
    public VehicleMenu(Enum @enum) : base(@enum)
    {
        AddVehicleSpawnerMenu();
        AddVehicleWeaponsMenu();
        AddVehicleModsMenu();

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

    private void AddVehicleModsMenu()
    {
        var vehicleModsMenu = new VehicleModsMenu(MenuTitles.VehicleMods);
        var vehicleModItem = AddMenu(vehicleModsMenu);
        Shown += (sender, args) =>
        {
            UpdateAltTitleOnDisable(vehicleModItem, Service.CurrentVehicle.Value != null,
                Service.CurrentVehicle.Value?.LocalizedName + " MENU",
                "No Vehicle");
        };
        Service.CurrentVehicle.ValueChanged += (sender, vehicle) =>
        {
            UpdateAltTitleOnDisable(vehicleModItem, vehicle.Value != null, vehicle.Value?.LocalizedName + " MENU",
                "No Vehicle");
        };
    }

    private void AddVehicleWeaponsMenu()
    {
        var vehicleWeaponsMenu = new VehicleWeaponsMenu(MenuTitles.VehicleWeapons);
        AddMenu(vehicleWeaponsMenu);
    }

    private void LockDoors()
    {
        var checkBoxLockDoors = AddCheckbox(VehicleItemTitles.LockDoors, Service.DoorsAlwaysLocked,
            @checked => { Service.DoorsAlwaysLocked.Value = @checked; });
    }

    private void NeverFallOffBike()
    {
        var checkBoxNeverFallOffBike = AddCheckbox(VehicleItemTitles.NeverFallOffBike, Service.NeverFallOffBike,
            @checked => { Service.NeverFallOffBike.Value = @checked; });
    }

    private void DriveUnderWater()
    {
        var checkBoxDriveUnderWater = AddCheckbox(VehicleItemTitles.DriveUnderWater, Service.CanDriveUnderWater,
            @checked => { Service.CanDriveUnderWater.Value = @checked; });
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
        var checkBoxSeatBelt = AddCheckbox(VehicleItemTitles.SeatBelt, Service.HasSeatBelt,
            @checked => { Service.HasSeatBelt.Value = @checked; });
    }

    private void RepairVehicle()
    {
        var itemRepairVehicle = AddItem(VehicleItemTitles.RepairVehicle, () => { Service.RequestRepair(); });
    }

    private void Indestructible()
    {
        var checkBoxIndestructible = AddCheckbox(VehicleItemTitles.Indestructible, Service.IsIndestructible,
            @checked => { Service.IsIndestructible.Value = @checked; });
    }
}