using System;
using GTA;
using Nuclei.Enums;
using Nuclei.Enums.UI;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class PlayerMenu : MenuBase
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    public PlayerMenu(Enum @enum) : base(@enum)
    {
        SkinChangerMenu();
        PlayerStatsMenu();

        AddHeader("Basics");
        FixPlayer();
        SetInvincible();
        AdjustWantedLevel();
        LockWantedLevel();
        AddCash();
        SetCash();

        AddHeader("Utilities");
        SetInfiniteStamina();
        SetInfiniteBreath();
        SetInfiniteSpecialAbility();
        SetNoiseless();
        SetRideOnCars();

        AddHeader("Super Powers");
        SuperJump();
        OnePunchMan();
        SuperSpeed();
        Invisible();
    }

    private void SkinChangerMenu()
    {
        var skinChangerMenu = new SkinChangerMenu(MenuTitles.SkinChanger);
        var skinChangerMenuItem = AddMenu(skinChangerMenu);
    }

    private void PlayerStatsMenu()
    {
        var playerStatsMenu = new PlayerStatsMenu(MenuTitles.ChangeStats);
        var playerStatsMenuItem = AddMenu(playerStatsMenu);
    }


    private void FixPlayer()
    {
        AddItem(PlayerItemTitles.FixPlayer,
            () => { _playerService.FixPlayer(); });
    }

    private void SetInvincible()
    {
        var checkBoxInvincible = AddCheckbox(PlayerItemTitles.Invincible, Game.Player.Character.IsInvincible,
            @checked => { _playerService.IsInvincible.Value = @checked; });

        _playerService.IsInvincible.ValueChanged += (sender, e) => { checkBoxInvincible.Checked = e.Value; };
    }

    private void AdjustWantedLevel()
    {
        var listItemWantedLevel = AddListItem(PlayerItemTitles.WantedLevel,
            (item, index) =>
            {
                _playerService.WantedLevel.Value = item;
                _playerService.LockedWantedLevel.Value = item;
            }, 0, 1, 2, 3, 4, 5);

        _playerService.WantedLevel.ValueChanged += (sender, e) =>
        {
            if (!_playerService.IsWantedLevelLocked.Value)
                listItemWantedLevel.SelectedItem = e.Value;
        };
    }

    private void LockWantedLevel()
    {
        var checkBoxLockWantedLevel = AddCheckbox(PlayerItemTitles.LockWantedLevel, false,
            @checked => { _playerService.IsWantedLevelLocked.Value = @checked; });
    }

    private void AddCash()
    {
        // We'll update this later, with better values than strings.
        var listItemAddCash = AddListItem(PlayerItemTitles.AddCash, (selected, index) => { }, "$10.000", "$100.000",
            "$1.000.000", "$100.000.000");
    }

    private void SetCash()
    {
        var itemSetCash = AddItem(PlayerItemTitles.SetCash, () => { _playerService.RequestCashInput(); });
    }


    private void SetInfiniteStamina()
    {
        var checkBoxInfiniteStamina = AddCheckbox(PlayerItemTitles.InfiniteStamina, false,
            @checked => { _playerService.HasInfiniteStamina.Value = @checked; });
    }

    private void SetInfiniteBreath()
    {
        var checkBoxInfiniteBreath = AddCheckbox(PlayerItemTitles.InfiniteBreath, false,
            @checked => { _playerService.HasInfiniteBreath.Value = @checked; });
    }

    private void SetInfiniteSpecialAbility()
    {
        var checkBoxInfiniteSpecialAbility = AddCheckbox(PlayerItemTitles.InfiniteSpecialAbility, false,
            @checked => { _playerService.HasInfiniteSpecialAbility.Value = @checked; });
    }

    private void SetNoiseless()
    {
        var checkBoxNoiseless = AddCheckbox(PlayerItemTitles.Noiseless, false,
            @checked => { _playerService.IsNoiseless.Value = @checked; });
    }

    private void SetRideOnCars()
    {
        var checkBoxRideOnCars = AddCheckbox(PlayerItemTitles.RideOnCars, false,
            @checked => { _playerService.CanRideOnCars.Value = @checked; });
    }


    private void SuperJump()
    {
        var checkBoxSuperJump = AddCheckbox(PlayerItemTitles.SuperJump, false, @checked => { });
    }

    private void OnePunchMan()
    {
        var checkBoxOnePunchMan = AddCheckbox(PlayerItemTitles.OnePunchMan, false, @checked => { });
    }

    private void SuperSpeed()
    {
        // Will be updated later to use enums instead.
        var listItemSuperSpeed = AddListItem(PlayerItemTitles.SuperSpeed, (selected, index) => { }, "Normal", "Fast",
            "Faster", "Sonic", "The Flash");

        // Depending on the selected item, if "Sonic" or "The Flash" a new Sub Menu will be available
        // Needs implementation.
    }

    private void Invisible()
    {
        var checkBoxInvisible = AddCheckbox(PlayerItemTitles.Invisible, false, @checked => { });
    }
}