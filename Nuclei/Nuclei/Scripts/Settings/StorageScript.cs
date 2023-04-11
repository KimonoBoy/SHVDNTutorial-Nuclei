using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Services.Generics;
using Nuclei.Services.Player;
using Nuclei.Services.Settings;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Settings;

public class StorageScript : Script
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    private readonly GenericStateService<PlayerService> _playerStateService =
        GenericStateService<PlayerService>.Instance;

    private readonly StorageService _storageService = StorageService.Instance;

    private readonly GenericStateService<StorageService> _storageStateService =
        GenericStateService<StorageService>.Instance;

    private readonly VehicleSpawnerService _vehicleSpawnerService = VehicleSpawnerService.Instance;

    private readonly GenericStateService<VehicleSpawnerService> _vehicleSpawnerStateService =
        GenericStateService<VehicleSpawnerService>.Instance;

    public StorageScript()
    {
        if (_storageStateService.GetState().AutoLoad.Value)
            Load();

        if (_storageStateService.GetState().AutoSave.Value)
            _storageService.AutoSave.Value = true;

        KeyDown += OnKeyDown;
        Tick += OnTick;
        _storageService.SaveRequested += OnSaveRequested;
        _storageService.LoadRequested += OnLoadRequested;

        Aborted += OnAborted;
    }

    private void OnTick(object sender, EventArgs e)
    {
        // The IsPaused needs to be handled differently at a later time.
        if (_storageService.AutoSave.Value && Game.IsPaused)
            Save();
    }

    private void OnAborted(object sender, EventArgs e)
    {
        if (_storageService.AutoSave.Value)
            Save();
    }

    private void OnLoadRequested(object sender, EventArgs e)
    {
        Load();
    }

    private void Load()
    {
        // Load the state from the file.
        var loadedStorageService = _storageStateService.LoadState();
        var loadedPlayerService = _playerStateService.LoadState();
        var loadedVehicleSpawnerService = _vehicleSpawnerStateService.LoadState();

        // Set the _playerService current state to the loaded state.
        if (loadedStorageService != null) _storageService.SetState(loadedStorageService);
        if (loadedPlayerService != null) _playerService.SetState(loadedPlayerService);
        if (loadedVehicleSpawnerService != null) _vehicleSpawnerService.SetState(loadedVehicleSpawnerService);

        Display.Notify("Load", "All Settings Loaded Successfully.");
    }

    private void OnSaveRequested(object sender, EventArgs e)
    {
        Save();
    }

    private void Save()
    {
        // Update the current state
        _storageStateService.SetState(_storageService);
        _playerStateService.SetState(_playerService);
        _vehicleSpawnerStateService.SetState(_vehicleSpawnerService);

        // Save the updated state
        _storageStateService.SaveState();
        _playerStateService.SaveState();
        _vehicleSpawnerStateService.SaveState();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.S && e.Control && e.Shift)
            _storageService.Save();
        else if (e.KeyCode == Keys.L && e.Control && e.Shift)
            _storageService.Load();
    }
}