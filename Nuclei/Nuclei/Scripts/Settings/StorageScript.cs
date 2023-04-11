using GTA;

namespace Nuclei.Scripts.Settings;

public class StorageScript : Script
{
    // private readonly PlayerService _defaultPlayerService = new();
    // private readonly StorageService _defaultStorageService = new();
    // private readonly VehicleSpawnerService _defaultVehicleSpawnerService = new();
    // private readonly PlayerService _playerService = PlayerService.Instance;
    //
    //
    // private readonly GenericStateService<PlayerService> _playerStateService =
    //     GenericStateService<PlayerService>.Instance;
    //
    // private readonly StorageService _storageService = StorageService.Instance;
    //
    // private readonly GenericStateService<StorageService> _storageStateService =
    //     GenericStateService<StorageService>.Instance;
    //
    // private readonly VehicleSpawnerService _vehicleSpawnerService = VehicleSpawnerService.Instance;
    //
    // private readonly GenericStateService<VehicleSpawnerService> _vehicleSpawnerStateService =
    //     GenericStateService<VehicleSpawnerService>.Instance;
    //
    // public StorageScript()
    // {
    //     if (_storageStateService.GetState().AutoLoad.Value)
    //         Load();
    //
    //     if (_storageStateService.GetState().AutoSave.Value)
    //         _storageService.AutoSave.Value = true;
    //
    //     SubScribeToEvents();
    // }
    //
    // private void SubScribeToEvents()
    // {
    //     KeyDown += OnKeyDown;
    //     Tick += OnTick;
    //     _storageService.SaveRequested += OnSaveRequested;
    //     _storageService.LoadRequested += OnLoadRequested;
    //     _storageService.RestoreDefaultsRequested += OnRestoreDefaultsRequested;
    //     Aborted += OnAborted;
    // }
    //
    // private void OnRestoreDefaultsRequested(object sender, EventArgs e)
    // {
    //     _playerService.SetState(_defaultPlayerService);
    //     _vehicleSpawnerService.SetState(_defaultVehicleSpawnerService);
    //     _storageService.SetState(_defaultStorageService);
    //     Save();
    // }
    //
    // private void OnKeyDown(object sender, KeyEventArgs e)
    // {
    //     if (e.KeyCode == Keys.S && e.Control && e.Shift)
    //         Save();
    //     else if (e.KeyCode == Keys.L && e.Control && e.Shift)
    //         Load();
    // }
    //
    // private void OnTick(object sender, EventArgs e)
    // {
    //     // The IsPaused needs to be handled differently at a later time.
    //     if (_storageService.AutoSave.Value && Game.IsPaused)
    //         Save();
    // }
    //
    // private void OnAborted(object sender, EventArgs e)
    // {
    //     if (_storageService.AutoSave.Value)
    //         Save();
    // }
    //
    // private void OnLoadRequested(object sender, EventArgs e)
    // {
    //     Load();
    // }
    //
    // private void OnSaveRequested(object sender, EventArgs e)
    // {
    //     Save();
    // }
    //
    // private void Save()
    // {
    //     // Update the current state
    //     _storageStateService.SetState(_storageService);
    //     _playerStateService.SetState(_playerService);
    //     _vehicleSpawnerStateService.SetState(_vehicleSpawnerService);
    //
    //     // Save the updated state
    //     _storageStateService.SaveState();
    //     _playerStateService.SaveState();
    //     _vehicleSpawnerStateService.SaveState();
    //
    //     Display.Notify("Save", "Settings saved.");
    // }
    //
    // private void Load()
    // {
    //     // Load the state from the file.
    //     var loadedStorageService = _storageStateService.LoadState();
    //     var loadedPlayerService = _playerStateService.LoadState();
    //     var loadedVehicleSpawnerService = _vehicleSpawnerStateService.LoadState();
    //
    //     // Set the _playerService current state to the loaded state.
    //     if (loadedStorageService != null) _storageService.SetState(loadedStorageService);
    //     if (loadedPlayerService != null) _playerService.SetState(loadedPlayerService);
    //     if (loadedVehicleSpawnerService != null) _vehicleSpawnerService.SetState(loadedVehicleSpawnerService);
    //
    //     Display.Notify("Load", "All settings loaded successfully.");
    // }
}