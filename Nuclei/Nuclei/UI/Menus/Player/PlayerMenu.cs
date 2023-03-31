using System;
using Nuclei.Enums;
using Nuclei.Services.Player;
using Nuclei.UI.Items;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class PlayerMenu : MenuBase
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    public PlayerMenu(Enum @enum) : base(@enum)
    {
        AddItem(PlayerTitles.FixPlayer,
            () => { _playerService.FixPlayer(); });

        AddCheckbox(PlayerTitles.Invincible, false,
            @checked => { _playerService.SetInvincible(@checked); });

        AddListItem(PlayerTitles.WantedLevel,
            (item, index) => { _playerService.SetWantedLevel(item); }, 0, 1, 2, 3, 4, 5);
    }
}