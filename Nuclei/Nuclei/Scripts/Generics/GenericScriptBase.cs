using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Services.Exception;
using Nuclei.Services.Generics;
using Nuclei.Services.Settings;

namespace Nuclei.Scripts.Generics;

public abstract class GenericScriptBase<TService> : Script where TService : GenericService<TService>, new()
{
    private readonly TService _defaultValuesService = new();
    private readonly StorageService _storageService = StorageService.Instance;

    protected GenericScriptBase()
    {
        if (_storageService.CurrentState().GetState().AutoLoad.Value) Load();
        if (_storageService.CurrentState().GetState().AutoSave.Value) _storageService.AutoSave.Value = true;

        KeyDown += OnKeyDown;
        Tick += OnTick;
        Aborted += OnAborted;

        _storageService.SaveRequested += OnSaveRequested;
        _storageService.LoadRequested += OnLoadRequested;
        _storageService.RestoreDefaultsRequested += OnRestoreDefaultsRequested;
    }

    protected TService Service => GenericService<TService>.Instance;
    protected ExceptionService ExceptionService => ExceptionService.Instance;
    protected GenericStateService<TService> State => GenericStateService<TService>.Instance;

    private void OnTick(object sender, EventArgs e)
    {
        if (_storageService.AutoSave.Value && Game.IsPaused) Save();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.S && e.Control && e.Shift) Save();
        else if (e.KeyCode == Keys.L && e.Control && e.Shift) Load();
    }

    private void OnAborted(object sender, EventArgs e)
    {
        if (_storageService.AutoSave.Value) Save();
    }

    private void OnRestoreDefaultsRequested(object sender, EventArgs e)
    {
        RestoreDefaults();
    }

    private void RestoreDefaults()
    {
        Service.SetState(_defaultValuesService);
        State.SetState(_defaultValuesService);
        Save();
    }

    private void OnLoadRequested(object sender, EventArgs e)
    {
        Load();
    }

    private void OnSaveRequested(object sender, EventArgs e)
    {
        Save();
    }

    private void Save()
    {
        State.SetState(Service);
        State.SaveState();
    }

    private void Load()
    {
        var loadedStorageService = State.LoadState();
        if (loadedStorageService != null) Service.SetState(loadedStorageService);
    }
}