using System;
using System.Windows.Forms;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.UI.Menus.Abstracts;
using MainMenu = Nuclei.UI.Menus.MainMenu;

namespace Nuclei;

public class Main : Script
{
    private readonly MainMenu _mainMenu = new(MenuTitles.Main);

    public Main()
    {
        KeyDown += OnKeyDown;
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        MenuBase.Pool.Process();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5)
        {
            if (MenuBase.LatestMenu != null)
                MenuBase.LatestMenu.Toggle();
            else
                _mainMenu.Toggle();
        }
    }
}