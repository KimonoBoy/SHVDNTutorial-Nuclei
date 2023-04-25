using System;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Settings;

public class StorageService : GenericService<StorageService>
{
    private bool _autoLoad;
    private bool _autoSave;

    public bool AutoSave
    {
        get => _autoSave;
        set
        {
            if (_autoSave != value)
            {
                _autoSave = value;
                OnPropertyChanged(nameof(_autoSave));
            }
        }
    }

    public bool AutoLoad
    {
        get => _autoLoad;
        set
        {
            if (_autoLoad != value)
            {
                _autoLoad = value;
                OnPropertyChanged(nameof(_autoLoad));
            }
        }
    }

    public event EventHandler SaveRequested;

    public event EventHandler LoadRequested;

    public event EventHandler RestoreDefaultsRequested;

    public void Save()
    {
        SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    public void Load()
    {
        LoadRequested?.Invoke(this, EventArgs.Empty);
    }

    public void RestoreDefaults()
    {
        RestoreDefaultsRequested?.Invoke(this, EventArgs.Empty);
    }
}