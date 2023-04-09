using System;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerMenu : MenuBase
{
    private readonly VehicleSpawnerService _vehicleSpawnerService = VehicleSpawnerService.Instance;

    public VehicleSpawnerMenu(Enum @enum) : base(@enum)
    {
        WarpInSpawned();
        SelectSeat();
        EnginesRunning();
        GenerateVehicleClassMenus();
    }

    private void WarpInSpawned()
    {
        var checkBoxWarpInSpawned = AddCheckbox(VehicleSpawnerItemTitles.WarpInSpawned, false,
            @checked => { _vehicleSpawnerService.WarpInSpawned.Value = @checked; });
    }

    private void SelectSeat()
    {
        // Default
        _vehicleSpawnerService.VehicleSeat.Value = VehicleSeat.Driver;

        var listItemSeat = AddListItem(VehicleSpawnerItemTitles.SelectSeat,
            (selected, index) => { _vehicleSpawnerService.VehicleSeat.Value = selected; },
            ListItemEventType.ItemChanged, VehicleSeat.Driver, VehicleSeat.LeftRear, VehicleSeat.RightRear,
            VehicleSeat.RightFront);

        Shown += (sender, args) => { listItemSeat.Enabled = _vehicleSpawnerService.WarpInSpawned.Value; };

        _vehicleSpawnerService.WarpInSpawned.ValueChanged += (_, args) => { listItemSeat.Enabled = args.Value; };
    }

    private void EnginesRunning()
    {
        var checkBoxEnginesRunning = AddCheckbox(VehicleSpawnerItemTitles.EnginesRunning, false,
            @checked => { _vehicleSpawnerService.EnginesRunning.Value = @checked; });
    }

    private void GenerateVehicleClassMenus()
    {
        foreach (VehicleClass vehicleClass in Enum.GetValues(typeof(VehicleClass)))
        {
            var vehicleClassMenu = new VehicleClassMenu(vehicleClass);
            var vehicleClassItem = AddMenu(vehicleClassMenu);
        }
    }
}