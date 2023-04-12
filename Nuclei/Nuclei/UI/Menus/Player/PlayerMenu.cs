﻿using System;
using System.Linq;
using Nuclei.Enums;
using Nuclei.Enums.Player;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class PlayerMenu : GenericMenuBase<PlayerService>
{
    // private readonly PlayerService _playerService = PlayerService.Instance;

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
            () => { Service.FixPlayer(); });
    }

    private void SetInvincible()
    {
        var checkBoxInvincible = AddCheckbox(PlayerItemTitles.Invincible, Service.IsInvincible,
            @checked => { Service.IsInvincible.Value = @checked; });
    }

    private void AdjustWantedLevel()
    {
        var listItemWantedLevel = AddListItem(PlayerItemTitles.WantedLevel,
            (item, index) =>
            {
                Service.WantedLevel.Value = item;
                Service.LockedWantedLevel.Value = item;
            }, null, 0, 1, 2, 3, 4, 5);

        Service.WantedLevel.ValueChanged += (sender, e) =>
        {
            listItemWantedLevel.SelectedItem = !Service.IsWantedLevelLocked.Value
                ? e.Value
                : Service.LockedWantedLevel.Value;
        };
    }

    private void LockWantedLevel()
    {
        var checkBoxLockWantedLevel = AddCheckbox(PlayerItemTitles.LockWantedLevel, Service.IsWantedLevelLocked,
            @checked => { Service.IsWantedLevelLocked.Value = @checked; });
    }

    private void AddCash()
    {
        var allCashHash = Enum.GetValues(typeof(CashHash)).Cast<CashHash>().ToList();

        var listItemAddCash = AddListItem(PlayerItemTitles.AddCash,
            (selected, index) => { Service.AddCash.Value = (CashHash)index; },
            (selected, index) => { Service.RequestCashResult((CashHash)index); },
            allCashHash.Select(c => c.GetDescription()).ToArray());

        Service.AddCash.ValueChanged += (sender, e) => { listItemAddCash.SelectedIndex = (int)e.Value; };
    }

    private void SetCash()
    {
        var itemSetCash = AddItem(PlayerItemTitles.SetCash, () => { Service.RequestCashInput(); });
    }


    private void SetInfiniteStamina()
    {
        var checkBoxInfiniteStamina = AddCheckbox(PlayerItemTitles.InfiniteStamina, Service.HasInfiniteStamina,
            @checked => { Service.HasInfiniteStamina.Value = @checked; });
    }

    private void SetInfiniteBreath()
    {
        var checkBoxInfiniteBreath = AddCheckbox(PlayerItemTitles.InfiniteBreath, Service.HasInfiniteBreath,
            @checked => { Service.HasInfiniteBreath.Value = @checked; });
    }

    private void SetInfiniteSpecialAbility()
    {
        var checkBoxInfiniteSpecialAbility = AddCheckbox(PlayerItemTitles.InfiniteSpecialAbility,
            Service.HasInfiniteSpecialAbility,
            @checked => { Service.HasInfiniteSpecialAbility.Value = @checked; });
    }

    private void SetNoiseless()
    {
        var checkBoxNoiseless = AddCheckbox(PlayerItemTitles.Noiseless, Service.IsNoiseless,
            @checked => { Service.IsNoiseless.Value = @checked; });
    }

    private void SetRideOnCars()
    {
        var checkBoxRideOnCars = AddCheckbox(PlayerItemTitles.RideOnCars, Service.CanRideOnCars,
            @checked => { Service.CanRideOnCars.Value = @checked; });
    }


    private void SuperJump()
    {
        var checkBoxSuperJump = AddCheckbox(PlayerItemTitles.SuperJump, Service.CanSuperJump,
            @checked => { Service.CanSuperJump.Value = @checked; });
    }

    private void OnePunchMan()
    {
        var checkBoxOnePunchMan = AddCheckbox(PlayerItemTitles.OnePunchMan, Service.IsOnePunchMan,
            @checked => { Service.IsOnePunchMan.Value = @checked; });
    }

    private void SuperSpeed()
    {
        var allSuperSpeeds = Enum.GetValues(typeof(SuperSpeedHash)).Cast<SuperSpeedHash>().ToList();

        var listItemSuperSpeed = AddListItem(
            PlayerItemTitles.SuperSpeed,
            (selected, index) => { Service.SuperSpeed.Value = (SuperSpeedHash)index; },
            null,
            allSuperSpeeds.Select(superSpeedHash => superSpeedHash.ToPrettyString()).ToArray());

        Service.SuperSpeed.ValueChanged += (sender, args) => { listItemSuperSpeed.SelectedIndex = (int)args.Value; };
    }

    private void Invisible()
    {
        var checkBoxInvisible = AddCheckbox(PlayerItemTitles.Invisible, Service.IsInvisible,
            @checked => { Service.IsInvisible.Value = @checked; });
    }
}