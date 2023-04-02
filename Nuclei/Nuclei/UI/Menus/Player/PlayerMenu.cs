using System;
using GTA;
using Nuclei.Enums;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class PlayerMenu : MenuBase
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    public PlayerMenu(Enum @enum) : base(@enum)
    {
        AddItem(PlayerTitles.FixPlayer,
            () => { _playerService.FixPlayer(); });


        AddInvincibleCheckbox();

        AddWantedLevelItem();
    }

    private void AddInvincibleCheckbox()
    {
        var checkBoxInvincible = AddCheckbox(PlayerTitles.Invincible, Game.Player.Character.IsInvincible,
            @checked => { _playerService.IsInvincible.Value = @checked; });

        _playerService.IsInvincible.ValueChanged += (sender, e) => { checkBoxInvincible.Checked = e.Value; };
    }

    private void AddWantedLevelItem()
    {
        var listItemWantedLevel = AddListItem(PlayerTitles.WantedLevel,
            (item, index) => { _playerService.WantedLevel.Value = item; }, 0, 1, 2, 3, 4, 5);

        _playerService.WantedLevel.ValueChanged += (sender, e) => { listItemWantedLevel.SelectedItem = e.Value; };
    }
}