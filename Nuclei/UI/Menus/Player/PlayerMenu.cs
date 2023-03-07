using Nuclei.Services.Player;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class PlayerMenu : MenuBase
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    public PlayerMenu(string subtitle, string description) : base(subtitle, description)
    {
        AddItem("Fix Player", "Restores Player's Health and Armor back to full.",
            () => { _playerService.FixPlayer(); });

        AddCheckbox("Invincible", "Set the Player Invincible.", false,
            @checked => { _playerService.SetInvincible(@checked); });

        AddListItem("Wanted Level", "Adjust Player's Wanted Level.",
            (item, index) => { _playerService.SetWantedLevel(item); }, 0, 1, 2, 3, 4, 5);
    }
}