using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception;
using Nuclei.Services.Generics;
using Nuclei.Services.Settings;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Generics;

public abstract class GenericScriptBase<TService> : Script, IDisposable where TService : GenericService<TService>, new()
{
    private static bool _eventsSubscribed;

    protected static readonly CustomTimer GameStateTimer = new(100);
    private static Ped _character;
    private static GTA.Vehicle _currentVehicle;
    private static GTA.Vehicle _lastVehicle;

    private readonly TService _defaultValuesService = new();
    private readonly StorageService _storageService = StorageService.Instance;

    protected GenericScriptBase()
    {
        if (_storageService.GetStateService().GetState().AutoLoad) Load();
        if (_storageService.GetStateService().GetState().AutoSave) _storageService.AutoSave = true;

        if (SubscribeToSharedEvents()) return;
    }

    protected TService Service => GenericService<TService>.Instance;

    protected ExceptionService ExceptionService => ExceptionService.Instance;

    protected GenericStateService<TService> State => GenericStateService<TService>.Instance;

    public static GTA.Vehicle CurrentVehicle
    {
        get => _currentVehicle;
        private set
        {
            if (_currentVehicle == Game.Player.Character.CurrentVehicle) return;
            _currentVehicle = value;
        }
    }

    public static Ped Character
    {
        get => _character;
        private set
        {
            if (_character == Game.Player.Character) return;
            _character = value;
        }
    }

    public static GTA.Vehicle LastVehicle
    {
        get => _lastVehicle;
        private set
        {
            if (_lastVehicle == Game.Player.Character.LastVehicle) return;
            _lastVehicle = value;
        }
    }

    public static Entity CurrentEntity => CurrentVehicle ?? (Entity)Character;

    public void Dispose()
    {
        GameStateTimer?.Stop();
        KeyDown -= OnKeyDown;
        Tick -= OnTick;

        _storageService.SaveRequested -= OnSaveRequested;
        _storageService.LoadRequested -= OnLoadRequested;
        _storageService.RestoreDefaultsRequested -= OnRestoreDefaultsRequested;

        UnsubscribeOnExit();
    }

    private bool SubscribeToSharedEvents()
    {
        // Ensures that the events are only subscribed once.
        if (_eventsSubscribed) return true;
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

    private void UpdateLastVehicle()
    {
        if (CurrentVehicle == null || LastVehicle == CurrentVehicle) return;

        LastVehicle = CurrentVehicle;
    }

    private void OnTick(object sender, EventArgs e)
    {
        UpdateCurrentCharacter();
        UpdateCurrentVehicle();
        UpdateLastVehicle();

        if (_storageService.AutoSave && Game.IsPaused)
            Save();
    }

    private void UpdateCurrentCharacter()
    {
        if (Character == Game.Player.Character) return;
        Character = Game.Player.Character;
        Service.Character = Character;
        Display.Notify("Character Change Registered", "Applying Settings");
    }

    private void UpdateCurrentVehicle()
    {
        if (CurrentVehicle == Game.Player.Character.CurrentVehicle) return;

        CurrentVehicle =
            Game.Player.Character.IsInVehicle() ? Game.Player.Character.CurrentVehicle : null;
        Service.CurrentVehicle = CurrentVehicle;
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
        if (_storageService.AutoSave) Save();
        Dispose();
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

    protected void UpdateFeature<T>(Func<T> getProperty, Action<T> action)
    {
        action(getProperty());
    }

    protected virtual void SubscribeToEvents()
    {
    }

    public abstract void UnsubscribeOnExit();
}