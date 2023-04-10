using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum SettingsTitles
{
    [Description("Automatically saves changes made.")]
    AutoSave,

    [Description("Automatically loads the last saved settings.")]
    AutoLoad,

    [Description("Saves the current settings.")]
    Save,

    [Description("Loads the last saved settings.")]
    Load
}