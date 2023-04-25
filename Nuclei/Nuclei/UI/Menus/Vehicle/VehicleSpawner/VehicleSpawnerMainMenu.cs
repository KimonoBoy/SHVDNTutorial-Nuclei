using System;
using System.ComponentModel;
using GTA;
using LemonUI.Menus;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerMainMenu : GenericMenuBase<VehicleSpawnerService>
{
    public VehicleSpawnerMainMenu(Enum @enum) : base(@enum)
    {
        Service.PropertyChanged += OnPropertyChanged;
        WarpInSpawned();
        SelectSeat();
        EnginesRunning();
        AddFavoriteVehiclesMenu();
        AddSavedVehiclesMenu();
        AddHeader("Type");
        GenerateVehicleClassMenus();
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.WarpInSpawned))
        {
            var item = GetItem<NativeListItem<string>>(VehicleSpawnerItemTitles.SelectSeat);
            item.Enabled = Service.WarpInSpawned;
            if (item.Enabled)
                item.SelectedItem = Service.VehicleSeat.GetLocalizedDisplayNameFromHash();
        }
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
            () => Service.WarpInSpawned, Service, @checked => { Service.WarpInSpawned = @checked; });
    }

    private void SelectSeat()
    {
        var listItemSeat = AddListItem(VehicleSpawnerItemTitles.SelectSeat,
            () => (int)Service.VehicleSeat, Service,
            (selected, index) => { Service.VehicleSeat = (VehicleSeat)index; },
            VehicleSeat.Driver.GetLocalizedDisplayNameFromHash(),
            VehicleSeat.Passenger.GetLocalizedDisplayNameFromHash(),
            VehicleSeat.LeftRear.GetLocalizedDisplayNameFromHash(),
            VehicleSeat.RightRear.GetLocalizedDisplayNameFromHash());

        // Disable the seat selection item if the WarpInSpawned is not true.
        Shown += (sender, args) => { listItemSeat.Enabled = Service.WarpInSpawned; };
    }

    private void EnginesRunning()
    {
        var checkBoxEnginesRunning = AddCheckbox(VehicleSpawnerItemTitles.EnginesRunning,
            () => Service.EnginesRunning, Service, @checked => { Service.EnginesRunning = @checked; });
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