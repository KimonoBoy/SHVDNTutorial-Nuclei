using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Settings;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Settings;

public class SettingsMenu : GenericMenuBase<SettingsService>
{
    public SettingsMenu(Enum @enum) : base(@enum)
    {
        AddSaveAndLoadMenu();
        AddHotKeysMenu();
    }

    private void AddSaveAndLoadMenu()
    {
        var saveAndLoadMenu = new SaveAndLoadMenu(MenuTitles.SaveAndLoad);
        var saveAndLoadMenuItem = AddMenu(saveAndLoadMenu);
    }

    private void AddHotKeysMenu()
    {
        var hotKeysMenu = new HotKeysMenu(MenuTitles.HotKeys);
        var hotKeysMenuItem = AddMenu(hotKeysMenu);
    }
}