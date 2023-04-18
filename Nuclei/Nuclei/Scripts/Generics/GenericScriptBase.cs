using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception;
using Nuclei.Services.Generics;
using Nuclei.Services.Settings.Storage;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Generics;

public abstract class GenericScriptBase<TService> : Script where TService : GenericService<TService>, new()
{
    /// <summary>
    ///     Ped flag for seat belt.
    /// </summary>
    protected const int FliesThroughWindscreen = 32;

    /// <summary>
    ///     Ped flag for drowning in water.
    /// </summary>
    protected const int DrownsInWater = 3;

    private static bool _eventsSubscribed;

    /// <summary>
    ///     A timer that can be subscribed to, that updates the game state every 100ms.
    ///     This is way more efficient than subscribing to the Tick event, but scripts that require
    ///     updates every tick should still subscribe to the Tick event.
    /// </summary>
    protected static readonly CustomTimer GameStateTimer = new(100);

    private readonly TService _defaultValuesService = new();
    private readonly StorageService _storageService = StorageService.Instance;

    protected GenericScriptBase()
    {
        if (_storageService.GetStateService().GetState().AutoLoad.Value) Load();
        if (_storageService.GetStateService().GetState().AutoSave.Value) _storageService.AutoSave.Value = true;

        if (SubscribeToSharedEvents()) return;
    }

    /// <summary>
    ///     The service associated with the current script class.
    /// </summary>
    protected TService Service => GenericService<TService>.Instance;

    /// <summary>
    ///     The exception service. Used to log exceptions and raise and throw errors.
    /// </summary>
    protected ExceptionService ExceptionService => ExceptionService.Instance;

    /// <summary>
    ///     The state service. Used to save and load the state of the script to JSON files.
    /// </summary>
    protected GenericStateService<TService> State => GenericStateService<TService>.Instance;

    /// <summary>
    ///     The vehicle the character is currently in.
    /// </summary>
    protected static GTA.Vehicle CurrentVehicle { get; private set; } = Game.Player.Character.CurrentVehicle;

    /// <summary>
    ///     The character the player is currently controlling.
    /// </summary>
    protected static Ped Character { get; private set; } = Game.Player.Character;

    /// <summary>
    ///     The last vehicle the player was in.
    /// </summary>
    protected static GTA.Vehicle LastVehicle { get; private set; }

    /// <summary>
    ///     The current entity the player is controlling, returns either the character or the vehicle the character is in.
    /// </summary>
    protected static Entity CurrentEntity => CurrentVehicle ?? (Entity)Character;

    /// <summary>
    ///     Subscribe to the events that are shared between all scripts.
    /// </summary>
    /// <returns></returns>
    private bool SubscribeToSharedEvents()
    {
        // Ensures that the events are only subscribed once.
        if (_eventsSubscribed) return true;

        // Subscribe the GameStateUpdater event handler only if it hasn't been subscribed before.
        GameStateTimer.SubscribeToTimerElapsed(GameStateUpdater);

        KeyDown += OnKeyDown;
        Tick += OnTick;
        Aborted += OnAborted;

        _storageService.SaveRequested += OnSaveRequested;
        _storageService.LoadRequested += OnLoadRequested;
        _storageService.RestoreDefaultsRequested += OnRestoreDefaultsRequested;

        _eventsSubscribed = true;

        SubscribeToEvents();

        return false;
    }

    private void GameStateUpdater(object sender, EventArgs e)
    {
        SetCharacter();
        SetCurrentVehicle();
        SetLastVehicle();
    }

    /// <summary>
    ///     Set the last vehicle the player was in.
    /// </summary>
    private void SetLastVehicle()
    {
        if (CurrentVehicle != null)
            LastVehicle = CurrentVehicle;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (_storageService.AutoSave.Value && Game.IsPaused)
            Save();
    }

    /// <summary>
    ///     Set the current character if the player has changed.
    /// </summary>
    private void SetCharacter()
    {
        if (Character != Game.Player.Character)
        {
            Character = Game.Player.Character;
            Display.Notify("Character Change Registered", "Applying Settings");
        }
    }

    /// <summary>
    ///     Set the current vehicle if the player is in a vehicle.
    /// </summary>
    private void SetCurrentVehicle()
    {
        CurrentVehicle = Game.Player.Character.IsInVehicle() ? Game.Player.Character.CurrentVehicle : null;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.S && e.Control && e.Shift)
            Save();
        else if (e.KeyCode == Keys.L && e.Control && e.Shift)
            Load();
    }

    private void OnAborted(object sender, EventArgs e)
    {
        if (_storageService.AutoSave.Value) Save();
        GameStateTimer?.Stop();
    }

    private void OnRestoreDefaultsRequested(object sender, EventArgs e)
    {
        RestoreDefaults();
    }

    private void RestoreDefaults()
    {
        Service.SetState(_defaultValuesService);
        State.SetState(_defaultValuesService);
    }

    private void OnLoadRequested(object sender, EventArgs e)
    {
        Load();
    }

    private void OnSaveRequested(object sender, EventArgs e)
    {
        Save();
    }

    protected void Save()
    {
        State.SetState(Service);
        State.SaveState();
        Display.Notify("All Settings Saved", "Successfully");
    }

    protected void Load()
    {
        var loadedStorageService = State.LoadState();
        if (loadedStorageService != null) Service.SetState(loadedStorageService);
        Display.Notify("All Settings Loaded", "Successfully");
    }

    /// <summary>
    ///     Updates the feature if the service state is different from the game state.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="value">The value from the service state.</param>
    /// <param name="action">The action to perform depending on the value.</param>
    protected void UpdateFeature<T>(T value, Action<T> action)
    {
        action(value);
    }

    /// <summary>
    ///     Event subscription placed in this method ensures that the events subscribed to in the derived classes are only
    ///     subscribed once.
    /// </summary>
    protected virtual void SubscribeToEvents()
    {
    }
}