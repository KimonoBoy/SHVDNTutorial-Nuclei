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
    }

    private void AddVehicleSpawnerMenu()
    {
        var vehicleSpawnerMenu = new VehicleSpawnerMenu(MenuTitles.SpawnVehicle);
        AddMenu(vehicleSpawnerMenu);
    }

    private void RepairVehicle()
    {
        var itemRepairVehicle = AddItem(VehicleItemTitles.RepairVehicle, () => { Service.Repair(); });
    }

    private void Indestructible()
    {
        var checkBoxIndestructible = AddCheckbox(VehicleItemTitles.Indestructible, Service.Indestructible,
            @checked => { Service.Indestructible.Value = @checked; });
    }
}