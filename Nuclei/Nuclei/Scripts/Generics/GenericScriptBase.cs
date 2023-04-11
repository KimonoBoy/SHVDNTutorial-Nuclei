using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Services.Exception;
using Nuclei.Services.Generics;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Generics;

public class GenericScriptBase<TService> : Script where TService : GenericService<TService>, new()
{
    private readonly TService _defaultValuesService = new();

    protected GenericScriptBase()
    {
        if (State.GetState().Storage.AutoLoad.Value) Load();
        if (State.GetState().Storage.AutoSave.Value) Service.Storage.AutoSave.Value = true;

        KeyDown += OnKeyDown;
        Tick += OnTick;
        Aborted += OnAborted;

        Service.Storage.SaveRequested += OnSaveRequested;
        Service.Storage.LoadRequested += OnLoadRequested;
        Service.Storage.RestoreDefaultsRequested += OnRestoreDefaultsRequested;
    }

    protected TService Service => GenericService<TService>.Instance;
    protected ExceptionService ExceptionService => ExceptionService.Instance;
    protected GenericStateService<TService> State => GenericStateService<TService>.Instance;

    private void OnTick(object sender, EventArgs e)
    {
        if (Service.Storage.AutoSave.Value && Game.IsPaused) Save();
    }

    private void OnAborted(object sender, EventArgs e)
    {
        if (Service.Storage.AutoSave.Value) Save();
    }

    private void OnRestoreDefaultsRequested(object sender, EventArgs e)
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
        Display.Notify("Save", "Settings saved.");
    }

    private void Load()
    {
        var loadedStorageService = State.LoadState();
        if (loadedStorageService != null) Service.SetState(loadedStorageService);
        Display.Notify("Load", "All settings loaded successfully.");
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.S && e.Control && e.Shift) Save();
        else if (e.KeyCode == Keys.L && e.Control && e.Shift) Load();
    }
}