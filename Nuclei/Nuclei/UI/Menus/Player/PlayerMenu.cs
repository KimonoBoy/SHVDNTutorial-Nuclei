using System;
using System.Linq;
using Nuclei.Enums;
using Nuclei.Enums.Player;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
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
        var checkBoxInvincible = AddCheckbox(PlayerItemTitles.Invincible, _playerService.IsInvincible,
            @checked => { _playerService.IsInvincible.Value = @checked; });
    }

    private void AdjustWantedLevel()
    {
        var listItemWantedLevel = AddListItem(PlayerItemTitles.WantedLevel,
            (item, index) =>
            {
                _playerService.WantedLevel.Value = item;
                _playerService.LockedWantedLevel.Value = item;
            }, null, 0, 1, 2, 3, 4, 5);

        _playerService.WantedLevel.ValueChanged += (sender, e) =>
        {
            listItemWantedLevel.SelectedItem = !_playerService.IsWantedLevelLocked.Value
                ? e.Value
                : _playerService.LockedWantedLevel.Value;
        };
    }

    private void LockWantedLevel()
    {
        var checkBoxLockWantedLevel = AddCheckbox(PlayerItemTitles.LockWantedLevel, _playerService.IsWantedLevelLocked,
            @checked => { _playerService.IsWantedLevelLocked.Value = @checked; });
    }

    private void AddCash()
    {
        var allCashHash = Enum.GetValues(typeof(CashHash)).Cast<CashHash>().ToList();

        var listItemAddCash = AddListItem(PlayerItemTitles.AddCash,
            (selected, index) => { _playerService.AddCash.Value = (CashHash)index; },
            (selected, index) => { _playerService.RequestCashResult((CashHash)index); },
            allCashHash.Select(c => c.GetDescription()).ToArray());

        _playerService.AddCash.ValueChanged += (sender, e) => { listItemAddCash.SelectedIndex = (int)e.Value; };
    }

    private void SetCash()
    {
        var itemSetCash = AddItem(PlayerItemTitles.SetCash, () => { _playerService.RequestCashInput(); });
    }


    private void SetInfiniteStamina()
    {
        var checkBoxInfiniteStamina = AddCheckbox(PlayerItemTitles.InfiniteStamina, _playerService.HasInfiniteStamina,
            @checked => { _playerService.HasInfiniteStamina.Value = @checked; });
    }

    private void SetInfiniteBreath()
    {
        var checkBoxInfiniteBreath = AddCheckbox(PlayerItemTitles.InfiniteBreath, _playerService.HasInfiniteBreath,
            @checked => { _playerService.HasInfiniteBreath.Value = @checked; });
    }

    private void SetInfiniteSpecialAbility()
    {
        var checkBoxInfiniteSpecialAbility = AddCheckbox(PlayerItemTitles.InfiniteSpecialAbility,
            _playerService.HasInfiniteSpecialAbility,
            @checked => { _playerService.HasInfiniteSpecialAbility.Value = @checked; });
    }

    private void SetNoiseless()
    {
        var checkBoxNoiseless = AddCheckbox(PlayerItemTitles.Noiseless, _playerService.IsNoiseless,
            @checked => { _playerService.IsNoiseless.Value = @checked; });
    }

    private void SetRideOnCars()
    {
        var checkBoxRideOnCars = AddCheckbox(PlayerItemTitles.RideOnCars, _playerService.CanRideOnCars,
            @checked => { _playerService.CanRideOnCars.Value = @checked; });
    }


    private void SuperJump()
    {
        var checkBoxSuperJump = AddCheckbox(PlayerItemTitles.SuperJump, _playerService.CanSuperJump,
            @checked => { _playerService.CanSuperJump.Value = @checked; });
    }

    private void OnePunchMan()
    {
        var checkBoxOnePunchMan = AddCheckbox(PlayerItemTitles.OnePunchMan, _playerService.IsOnePunchMan,
            @checked => { _playerService.IsOnePunchMan.Value = @checked; });
    }

    private void SuperSpeed()
    {
        var allSuperSpeeds = Enum.GetValues(typeof(SuperSpeedHash)).Cast<SuperSpeedHash>().ToList();

        var listItemSuperSpeed = AddListItem(
            PlayerItemTitles.SuperSpeed,
            (selected, index) => { _playerService.SuperSpeed.Value = (SuperSpeedHash)index; },
            null,
            allSuperSpeeds.Select(superSpeedHash => superSpeedHash.ToPrettyString()).ToArray());

        _playerService.SuperSpeed.ValueChanged += (sender, args) =>
        {
            listItemSuperSpeed.SelectedIndex = (int)args.Value;
        };
    }

    private void Invisible()
    {
        var checkBoxInvisible = AddCheckbox(PlayerItemTitles.Invisible, _playerService.IsInvisible,
            @checked => { _playerService.IsInvisible.Value = @checked; });
    }
}