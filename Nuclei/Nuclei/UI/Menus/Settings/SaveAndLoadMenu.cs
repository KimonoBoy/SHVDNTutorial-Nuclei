using System;
using Nuclei.Enums.UI;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Settings;

public class SaveAndLoadMenu : MenuBase
{
    public SaveAndLoadMenu(Enum @enum) : base(@enum)
    {
        AutoSave();
        AutoLoad();
        Save();
        Load();
    }

    private void Load()
    {
        var itemLoad = AddItem(SettingsTitles.Load, () => { });
        itemLoad.AltTitle = "CTRL + SHIFT + L";
    }

    private void Save()
    {
        var itemSave = AddItem(SettingsTitles.Save, () => { });
        itemSave.AltTitle = "CTRL + SHIFT + S";
    }

    private void AutoLoad()
    {
        var checkBoxAutoLoad = AddCheckbox(SettingsTitles.AutoLoad, action: @checked => { });
    }

    private void AutoSave()
    {
        var checkBoxAutoSave = AddCheckbox(SettingsTitles.AutoSave, action: @checked => { });
    }
}