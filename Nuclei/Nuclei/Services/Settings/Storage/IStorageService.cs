using System;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Settings.Storage;

public interface IStorageService
{
    // Properties
    BindableProperty<bool> AutoSave { get; set; }
    BindableProperty<bool> AutoLoad { get; set; }

    // Events
    event EventHandler SaveRequested;
    event EventHandler LoadRequested;
    event EventHandler RestoreDefaultsRequested;

    // Methods
    void Save();
    void Load();
    void RestoreDefaults();
}