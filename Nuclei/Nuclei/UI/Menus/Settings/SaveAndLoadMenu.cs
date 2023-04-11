using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Settings;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Settings;

public class SaveAndLoadMenu : MenuBase
{
    /*
     * Settings Service and Script needs to be implemented.
     *
     * This allows us to save and load more efficiently.
     */

    private readonly StorageService _storageService = StorageService.Instance;

    public SaveAndLoadMenu(Enum @enum) : base(@enum)
    {
        Save();
        Load();
        AutoSave();
        AutoLoad();
        RestoreDefaults();
    }

    private void RestoreDefaults()
    {
        var itemRestoreDefault = AddItem(SettingsTitles.RestoreDefault, () => { _storageService.RestoreDefaults(); });
    }

    private void Load()
    {
        var itemLoad = AddItem(SettingsTitles.Load, () => { _storageService.Load(); });
        itemLoad.AltTitle = "CTRL + SHIFT + L";
    }

    private void Save()
    {
        var itemSave = AddItem(SettingsTitles.Save, () => { _storageService.Save(); });
        itemSave.AltTitle = "CTRL + SHIFT + S";
    }

    private void AutoLoad()
    {
        var checkBoxAutoLoad = AddCheckbox(SettingsTitles.AutoLoad, _storageService.AutoLoad,
            @checked => { _storageService.AutoLoad.Value = @checked; });
    }

    private void AutoSave()
    {
        var checkBoxAutoSave = AddCheckbox(SettingsTitles.AutoSave, _storageService.AutoSave,
            @checked => { _storageService.AutoSave.Value = @checked; });
    }
}