using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Generics;
using Nuclei.Services.Player;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Settings;

public class SaveAndLoadMenu : MenuBase
{
    /*
     * Settings Service and Script needs to be implemented.
     *
     * This allows us to save and load more efficiently.
     */

    private readonly PlayerService _playerService = PlayerService.Instance;

    private readonly GenericStateService<PlayerService> _playerServiceState =
        GenericStateService<PlayerService>.Instance;

    private readonly VehicleSpawnerService _vehicleSpawnerService = VehicleSpawnerService.Instance;

    private readonly GenericStateService<VehicleSpawnerService> _vehicleSpawnerStateService =
        GenericStateService<VehicleSpawnerService>.Instance;

    public SaveAndLoadMenu(Enum @enum) : base(@enum)
    {
        AutoSave();
        AutoLoad();
        Save();
        Load();
    }

    private void Load()
    {
        var itemLoad = AddItem(SettingsTitles.Load, () =>
        {
            // Load the state from the file.
            var loadedPlayerService = _playerServiceState.LoadState();
            var loadedVehicleSpawnerService = _vehicleSpawnerStateService.LoadState();

            // Set the _playerService current state to the loaded state.
            if (loadedPlayerService != null) _playerService.SetState(loadedPlayerService);
            if (loadedVehicleSpawnerService != null) loadedVehicleSpawnerService.SetState(loadedVehicleSpawnerService);
        });
        itemLoad.AltTitle = "CTRL + SHIFT + L";
    }

    private void Save()
    {
        var itemSave = AddItem(SettingsTitles.Save, () =>
        {
            // Update the current state
            _playerServiceState.SetState(_playerService);
            _vehicleSpawnerStateService.SetState(_vehicleSpawnerService);

            // Save the updated state
            _playerServiceState.SaveState();
            _vehicleSpawnerStateService.SaveState();
        });
        itemSave.AltTitle = "CTRL + SHIFT + S";
    }

    private void AutoLoad()
    {
        var checkBoxAutoLoad = AddCheckbox(SettingsTitles.AutoLoad, action: @checked => { });
    }

    private void AutoSave()
    {
        var checkBoxAutoSave = AddCheckbox(SettingsTitles.AutoSave, action: @checked => { });
    }
}