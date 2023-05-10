using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Enums.Hotkey;
using Nuclei.Enums.UI;
using Nuclei.Services.Settings;
using Nuclei.UI.Menus.Base;
using Control = GTA.Control;
using MainMenu = Nuclei.UI.Menus.MainMenu;

namespace Nuclei;

public class Main : Script
{
    private readonly HotkeysService _hotkeysService = HotkeysService.Instance;
    private readonly MainMenu _mainMenu = new(MenuTitle.Main);

    public Main()
    {
        KeyDown += OnKeyDown;
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        MenuBase.Pool.Process();
        if (MenuBase.Pool.AreAnyVisible) Game.DisableControlThisFrame(Control.ReplayStartStopRecording);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        var toggleMenuKey = _hotkeysService.GetValue(SectionName.Menu, MenuTitle.ToggleMenu);
        if (!_hotkeysService.IsKeyPressed(toggleMenuKey)) return;
        if (MenuBase.LatestMenu != null)
            MenuBase.LatestMenu.Toggle();
        else
            _mainMenu.Toggle();
    }
}