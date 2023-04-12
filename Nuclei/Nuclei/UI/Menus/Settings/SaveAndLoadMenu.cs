﻿using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Settings;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Settings;

public class SaveAndLoadMenu : GenericsMenuBase<StorageService>
{
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
        var itemRestoreDefault = AddItem(SettingsTitles.RestoreDefault, () => { Service.RestoreDefaults(); });
    }

    private void Load()
    {
        var itemLoad = AddItem(SettingsTitles.Load, () => { Service.Load(); });
        itemLoad.AltTitle = "CTRL + SHIFT + L";
    }

    private void Save()
    {
        var itemSave = AddItem(SettingsTitles.Save, () => { Service.Save(); });
        itemSave.AltTitle = "CTRL + SHIFT + S";
    }

    private void AutoLoad()
    {
        var checkBoxAutoLoad = AddCheckbox(SettingsTitles.AutoLoad, Service.AutoLoad,
            @checked => { Service.AutoLoad.Value = @checked; });
    }

    private void AutoSave()
    {
        var checkBoxAutoSave = AddCheckbox(SettingsTitles.AutoSave, Service.AutoSave,
            @checked => { Service.AutoSave.Value = @checked; });
    }
}