using System;
using System.Linq;
using GTA;
using Nuclei.Enums;
using Nuclei.Enums.Player;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class PlayerMenu : MenuBase
{
    private readonly IPlayerService _playerService = PlayerService.Instance;

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
            }, ListItemEventType.ItemChanged, 0, 1, 2, 3, 4, 5);

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

        _playerService.IsWantedLevelLocked.ValueChanged += (sender, e) =>
        {
            checkBoxLockWantedLevel.Checked = e.Value;
        };
    }

    private void AddCash()
    {
        var allCashHash = Enum.GetValues(typeof(CashHash)).Cast<CashHash>().ToList();

        var listItemAddCash = AddListItem(PlayerItemTitles.AddCash,
            (selected, index) => { _playerService.RequestCashResult((CashHash)index); }, ListItemEventType.Activated,
            allCashHash.Select(c => c.GetDescription()).ToArray());

        listItemAddCash.ItemChanged += (sender, e) => { _playerService.AddCash.Value = (CashHash)e.Index; };

        _playerService.AddCash.ValueChanged += (sender, e) => { listItemAddCash.SelectedIndex = (int)e.Value; };
    }

    private void SetCash()
    {
        var itemSetCash = AddItem(PlayerItemTitles.SetCash, () => { _playerService.RequestCashInput(); });
    }


    private void SetInfiniteStamina()
    {
        var checkBoxInfiniteStamina = AddCheckbox(PlayerItemTitles.InfiniteStamina, false,
            @checked => { _playerService.HasInfiniteStamina.Value = @checked; });

        _playerService.HasInfiniteStamina.ValueChanged += (sender, e) => { checkBoxInfiniteStamina.Checked = e.Value; };
    }

    private void SetInfiniteBreath()
    {
        var checkBoxInfiniteBreath = AddCheckbox(PlayerItemTitles.InfiniteBreath, false,
            @checked => { _playerService.HasInfiniteBreath.Value = @checked; });

        _playerService.HasInfiniteBreath.ValueChanged += (sender, e) => { checkBoxInfiniteBreath.Checked = e.Value; };
    }

    private void SetInfiniteSpecialAbility()
    {
        var checkBoxInfiniteSpecialAbility = AddCheckbox(PlayerItemTitles.InfiniteSpecialAbility, false,
            @checked => { _playerService.HasInfiniteSpecialAbility.Value = @checked; });

        _playerService.HasInfiniteSpecialAbility.ValueChanged += (sender, e) =>
        {
            checkBoxInfiniteSpecialAbility.Checked = e.Value;
        };
    }

    private void SetNoiseless()
    {
        var checkBoxNoiseless = AddCheckbox(PlayerItemTitles.Noiseless, false,
            @checked => { _playerService.IsNoiseless.Value = @checked; });

        _playerService.IsNoiseless.ValueChanged += (sender, e) => { checkBoxNoiseless.Checked = e.Value; };
    }

    private void SetRideOnCars()
    {
        var checkBoxRideOnCars = AddCheckbox(PlayerItemTitles.RideOnCars, false,
            @checked => { _playerService.CanRideOnCars.Value = @checked; });

        _playerService.CanRideOnCars.ValueChanged += (sender, e) => { checkBoxRideOnCars.Checked = e.Value; };
    }


    private void SuperJump()
    {
        var checkBoxSuperJump = AddCheckbox(PlayerItemTitles.SuperJump, false,
            @checked => { _playerService.CanSuperJump.Value = @checked; });

        _playerService.CanSuperJump.ValueChanged += (sender, e) => { checkBoxSuperJump.Checked = e.Value; };
    }

    private void OnePunchMan()
    {
        var checkBoxOnePunchMan = AddCheckbox(PlayerItemTitles.OnePunchMan, false,
            @checked => { _playerService.IsOnePunchMan.Value = @checked; });

        _playerService.IsOnePunchMan.ValueChanged += (sender, e) => { checkBoxOnePunchMan.Checked = e.Value; };
    }

    private void SuperSpeed()
    {
        var allSuperSpeeds = Enum.GetValues(typeof(SuperSpeedHash)).Cast<SuperSpeedHash>().ToList();

        var listItemSuperSpeed = AddListItem(PlayerItemTitles.SuperSpeed,
            (selected, index) => { _playerService.SuperSpeed.Value = (SuperSpeedHash)index; },
            ListItemEventType.ItemChanged, allSuperSpeeds.Select(s => s.ToPrettyString()).ToArray());

        _playerService.SuperSpeed.ValueChanged += (sender, e) =>
        {
            listItemSuperSpeed.SelectedItem = e.Value.ToPrettyString();
        };
    }

    private void Invisible()
    {
        var checkBoxInvisible = AddCheckbox(PlayerItemTitles.Invisible, false,
            @checked => { _playerService.IsInvisible.Value = @checked; });

        _playerService.IsInvisible.ValueChanged += (sender, e) => { checkBoxInvisible.Checked = e.Value; };
    }
}