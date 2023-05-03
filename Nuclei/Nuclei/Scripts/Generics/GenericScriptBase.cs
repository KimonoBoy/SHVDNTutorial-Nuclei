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
    private readonly TService _defaultValuesService = new();

    private readonly CustomTimer _gameStateTimer = new(100);
    private readonly StorageService _storageService = StorageService.Instance;
    private Ped _character;
    private GTA.Vehicle _currentVehicle;
    private GTA.Weapon _currentWeapon;
    private bool _eventsSubscribed;
    private GTA.Vehicle _lastVehicle;

    protected GenericScriptBase()
    {
        if (_storageService.GetStorage().GetState().AutoLoad) Load();
        if (_storageService.GetStorage().GetState().AutoSave) _storageService.AutoSave = true;

        if (SubscribeToSharedEvents()) return;
    }

    protected TService Service => GenericService<TService>.Instance;

    protected ExceptionService ExceptionService => ExceptionService.Instance;

    protected GenericStateService<TService> State => GenericStateService<TService>.Instance;

    public GTA.Vehicle CurrentVehicle
    {
        get => _currentVehicle;
        private set
        {
            if (_currentVehicle == Game.Player.Character.CurrentVehicle) return;
            _currentVehicle = value;
        }
    }

    public Ped Character
    {
        get => _character;
        private set
        {
            if (_character == Game.Player.Character) return;
            _character = value;
        }
    }

    public GTA.Vehicle LastVehicle
    {
        get => _lastVehicle;
        private set
        {
            if (_lastVehicle == Game.Player.Character.LastVehicle) return;
            _lastVehicle = value;
        }
    }

    public GTA.Weapon CurrentWeapon
    {
        get => _currentWeapon;
        set
        {
            if (_currentWeapon == Game.Player.Character.Weapons.Current) return;
            _currentWeapon = value;
        }
    }

    public Entity CurrentEntity => CurrentVehicle ?? (Entity)Character;

    public void Dispose()
    {
        _gameStateTimer?.Stop();
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
        _gameStateTimer.TimerElapsed += ProcessGameStatesTimer;
        _gameStateTimer.TimerElapsed += UpdateServiceStatesTimer;

        _eventsSubscribed = true;

        SubscribeToEvents();

        return false;
    }

    private void OnTick(object sender, EventArgs e)
    {
        UpdateCurrentCharacter();
        UpdateCurrentVehicle();
        UpdateLastVehicle();
        UpdateCurrentWeapon();

        if (_storageService.AutoSave && Game.IsPaused)
            Save();
    }

    private void UpdateCurrentWeapon()
    {
        CurrentWeapon = Game.Player.Character.Weapons.Current;
        Service.CurrentWeapon = CurrentWeapon;
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

    protected abstract void SubscribeToEvents();

    protected abstract void UnsubscribeOnExit();

    protected abstract void ProcessGameStatesTimer(object sender, EventArgs e);

    protected abstract void UpdateServiceStatesTimer(object sender, EventArgs e);

    private void UpdateLastVehicle()
    {
        if (CurrentVehicle == null || LastVehicle == CurrentVehicle) return;

        LastVehicle = CurrentVehicle;
    }

    private void UpdateCurrentCharacter()
    {
        Character = Game.Player.Character;
        Service.Character = Character;
    }

    private void UpdateCurrentVehicle()
    {
        CurrentVehicle =
            Game.Player.Character.IsInVehicle() ? Game.Player.Character.CurrentVehicle : null;
        Service.CurrentVehicle = CurrentVehicle;
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
}