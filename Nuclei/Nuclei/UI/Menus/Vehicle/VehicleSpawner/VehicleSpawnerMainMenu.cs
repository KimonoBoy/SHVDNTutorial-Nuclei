using System;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerMainMenu : GenericMenuBase<VehicleSpawnerService>
{
    public VehicleSpawnerMainMenu(Enum @enum) : base(@enum)
    {
        WarpInSpawned();
        SelectSeat();
        EnginesRunning();
        AddFavoriteVehiclesMenu();
        AddSavedVehiclesMenu();
        AddHeader("Type");
        GenerateVehicleClassMenus();
    }

    private void AddSavedVehiclesMenu()
    {
        var savedVehiclesMenu = new VehicleSpawnerSavedVehiclesMenu(MenuTitles.SavedVehicles);
        AddMenu(savedVehiclesMenu);
    }

    private void AddFavoriteVehiclesMenu()
    {
        var favoriteVehiclesMenu = new VehicleSpawnerFavoritesMenu(MenuTitles.FavoriteVehicles);
        AddMenu(favoriteVehiclesMenu);
    }

    private void WarpInSpawned()
    {
        var checkBoxWarpInSpawned = AddCheckbox(VehicleSpawnerItemTitles.WarpInSpawned,
            Service.WarpInSpawned,
            @checked => { Service.WarpInSpawned.Value = @checked; });
    }

    private void SelectSeat()
    {
        var listItemSeat = AddListItem(VehicleSpawnerItemTitles.SelectSeat,
            (selected, index) => { Service.VehicleSeat.Value = selected; },
            null, VehicleSeat.Driver,
            VehicleSeat.LeftRear, VehicleSeat.RightRear,
            VehicleSeat.RightFront);

        // Disable the seat selection item if the WarpInSpawned is not true.
        Shown += (sender, args) => { listItemSeat.Enabled = Service.WarpInSpawned.Value; };
        Service.WarpInSpawned.ValueChanged += (sender, args) => { listItemSeat.Enabled = args.Value; };

        Service.VehicleSeat.ValueChanged += (sender, args) => { listItemSeat.SelectedItem = args.Value; };
    }

    private void EnginesRunning()
    {
        var checkBoxEnginesRunning = AddCheckbox(VehicleSpawnerItemTitles.EnginesRunning,
            Service.EnginesRunning,
            @checked => { Service.EnginesRunning.Value = @checked; });
    }

    private void GenerateVehicleClassMenus()
    {
        foreach (VehicleClass vehicleClass in Enum.GetValues(typeof(VehicleClass)))
        {
            var vehicleClassMenu = new VehicleSpawnerClassMenu(vehicleClass);
            var vehicleClassItem = AddMenu(vehicleClassMenu);
        }
    }
}