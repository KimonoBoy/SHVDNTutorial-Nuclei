using System;
using Nuclei.Enums;
using Nuclei.UI.Menus.Abstracts;
using Nuclei.UI.Menus.Player;

namespace Nuclei.UI.Menus;

public class MainMenu : MenuBase
{
    public MainMenu(Enum @enum) : base(@enum)
    {
        AddPlayerMenu();
    }

    private void AddPlayerMenu()
    {
        var playerMenu = new PlayerMenu(MenuTitles.Player);
        AddMenu(playerMenu);
    }
}