using GTA;

namespace Nuclei;

public class MainMenu : MenuBase
{
    public MainMenu(string subtitle, string description) : base(subtitle, description)
    {
        AddItem("Fix Player", "Restores Player's Health and Armor back to full.", () =>
        {
            Game.Player.Character.Health = Game.Player.Character.MaxHealth;
            Game.Player.Character.Armor = Game.Player.MaxArmor;
        });
    }
}