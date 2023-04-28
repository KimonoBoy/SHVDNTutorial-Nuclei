using System;
using System.ComponentModel;
using LemonUI.Menus;
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
        Shown += OnShown;
        Closed += OnClosed;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.PropertyChanged -= OnPropertyChanged;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Service.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.CurrentVehicle))
        {
            var item = GetItem<NativeSubmenuItem>(MenuTitle.VehicleMods);
            if (item == null) return;
            item.Enabled = Service.CurrentVehicle != null;
            UpdateAltTitleOnCondition(item, Service.CurrentVehicle != null,
                Service.CurrentVehicle?.LocalizedName + " MENU", "No Vehicle");
        }
    }

    private void AddVehicleModsMenu()
    {
        var vehicleModsMenu = new VehicleModsMenu(MenuTitle.VehicleMods);
        var vehicleModItem = AddMenu(vehicleModsMenu);
        Shown += (sender, args) =>
        {
            UpdateAltTitleOnCondition(vehicleModItem, Service.CurrentVehicle != null,
                Service.CurrentVehicle?.LocalizedName + " MENU",
                "No Vehicle");
        };
    }

    private void AddVehicleWeaponsMenu()
    {
        var vehicleWeaponsMenu = new VehicleWeaponsMenu(MenuTitle.VehicleWeapons);
        AddMenu(vehicleWeaponsMenu);
    }

    private void LockDoors()
    {
        var checkBoxLockDoors = AddCheckbox(VehicleItemTitle.LockDoors, () => Service.DoorsAlwaysLocked, Service,
            @checked => { Service.DoorsAlwaysLocked = @checked; });
    }

    private void NeverFallOffBike()
    {
        var checkBoxNeverFallOffBike = AddCheckbox(VehicleItemTitle.NeverFallOffBike, () => Service.NeverFallOffBike,
            Service, @checked => { Service.NeverFallOffBike = @checked; });
    }

    private void DriveUnderWater()
    {
        var checkBoxDriveUnderWater = AddCheckbox(VehicleItemTitle.DriveUnderWater, () => Service.CanDriveUnderWater,
            Service, @checked => { Service.CanDriveUnderWater = @checked; });
    }

    private void AddVehicleSpawnerMenu()
    {
        var vehicleSpawnerMenu = new VehicleSpawnerMainMenu(MenuTitle.SpawnVehicle);
        AddMenu(vehicleSpawnerMenu);
    }

    private void FlipVehicle()
    {
        var itemFlipVehicle = AddItem(VehicleItemTitle.FlipVehicle, () => { Service.RequestVehicleFlip(); });
    }

    private void SpeedBoost()
    {
        var sliderItemSpeedBoost = AddSliderItem(VehicleItemTitle.SpeedBoost, () => Service.SpeedBoost, Service,
            speedBoostValue => { Service.SpeedBoost = speedBoostValue; }, 0, 5);
    }

    private void SeatBelt()
    {
        var checkBoxSeatBelt = AddCheckbox(VehicleItemTitle.SeatBelt, () => Service.HasSeatBelt, Service,
            @checked => { Service.HasSeatBelt = @checked; });
    }

    private void RepairVehicle()
    {
        var itemRepairVehicle = AddItem(VehicleItemTitle.RepairVehicle, () => { Service.RequestRepair(); });
    }

    private void Indestructible()
    {
        var checkBoxIndestructible = AddCheckbox(VehicleItemTitle.Indestructible, () => Service.IsIndestructible,
            Service, @checked => { Service.IsIndestructible = @checked; });
    }
}