using Nuclei.UI.Menus.Abstracts;
using Nuclei.UI.Menus.Player;

namespace Nuclei.UI.Menus;

public class MainMenu : MenuBase
{
    public MainMenu(string subtitle, string description) : base(subtitle, description)
    {
        AddPlayerMenu();
    }

    private void AddPlayerMenu()
    {
        var playerMenu = new PlayerMenu("Player", "Everything associated with the Player and its Character.");
        AddMenu(playerMenu);
    }
}