using System;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Settings.Storage;

public class StorageService : GenericService<StorageService>, IStorageService
{
    public BindableProperty<bool> AutoSave { get; set; } = new();

    public BindableProperty<bool> AutoLoad { get; set; } = new();

    public event EventHandler SaveRequested;

    public void Save()
    {
        SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler LoadRequested;

    public void Load()
    {
        LoadRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler RestoreDefaultsRequested;

    public void RestoreDefaults()
    {
        RestoreDefaultsRequested?.Invoke(this, EventArgs.Empty);
    }
}