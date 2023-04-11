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
        var checkBoxWarpInSpawned = AddCheckbox(VehicleSpawnerItemTitles.WarpInSpawned,
            _vehicleSpawnerService.WarpInSpawned,
            @checked => { _vehicleSpawnerService.WarpInSpawned.Value = @checked; });
    }

    private void SelectSeat()
    {
        var listItemSeat = AddListItem(VehicleSpawnerItemTitles.SelectSeat,
            (selected, index) => { _vehicleSpawnerService.VehicleSeat.Value = selected; },
            null, VehicleSeat.Driver,
            VehicleSeat.LeftRear, VehicleSeat.RightRear,
            VehicleSeat.RightFront);

        // Disable the seat selection item if the WarpInSpawned is not true.
        Shown += (sender, args) => { listItemSeat.Enabled = _vehicleSpawnerService.WarpInSpawned.Value; };
        _vehicleSpawnerService.WarpInSpawned.ValueChanged += (sender, args) => { listItemSeat.Enabled = args.Value; };

        _vehicleSpawnerService.VehicleSeat.ValueChanged += (sender, args) =>
        {
            listItemSeat.SelectedItem = args.Value;
        };
    }

    private void EnginesRunning()
    {
        var checkBoxEnginesRunning = AddCheckbox(VehicleSpawnerItemTitles.EnginesRunning,
            _vehicleSpawnerService.EnginesRunning,
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