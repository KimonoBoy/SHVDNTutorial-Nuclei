using System;
using System.ComponentModel;
using System.Linq;
using LemonUI.Menus;
using Nuclei.Enums;
using Nuclei.Enums.Player;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player;

public class PlayerMenu : GenericMenuBase<PlayerService>
{
    public PlayerMenu(Enum @enum) : base(@enum)
    {
        AddHeader("Basics");
        FixPlayer();
        Invincible();
        AdjustWantedLevel();
        LockWantedLevel();
        AddCash();
        CashInput();

        AddHeader("Utilities");
        InfiniteStamina();
        InfiniteBreath();
        InfiniteSpecialAbility();
        Noiseless();
        RideOnCars();

        AddHeader("Super Powers");
        SuperJump();
        OnePunchMan();
        SuperSpeed();
        Invisible();
        Service.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.WantedLevel))
        {
            var listItemWantedLevel = GetItem<NativeListItem<int>>(PlayerItemTitles.WantedLevel);
            listItemWantedLevel.SelectedItem = Service.WantedLevel;
        }

        if (e.PropertyName == nameof(Service.SuperSpeed))
        {
            var listItemSuperSpeed = GetItem<NativeListItem<string>>(PlayerItemTitles.SuperSpeed);
            listItemSuperSpeed.SelectedItem = Service.SuperSpeed.GetLocalizedDisplayNameFromHash();
        }
    }

    private void FixPlayer()
    {
        AddItem(PlayerItemTitles.FixPlayer, () => { Service.RequestFixPlayer(); });
    }

    private void Invincible()
    {
        var checkBoxInvincible = AddCheckbox(PlayerItemTitles.Invincible,
            () => Service.IsInvincible,
            @checked => { Service.IsInvincible = @checked; }, Service);
    }

    private void AdjustWantedLevel()
    {
        var listItemWantedLevel = AddListItem(PlayerItemTitles.WantedLevel,
            (item, index) =>
            {
                Service.WantedLevel = item;
                Service.LockedWantedLevel = item;
            },
            null,
            value => Service.WantedLevel,
            Service,
            0, 1, 2, 3, 4, 5);
    }

    private void LockWantedLevel()
    {
        var checkBoxLockWantedLevel = AddCheckbox(PlayerItemTitles.LockWantedLevel,
            () => Service.IsWantedLevelLocked,
            @checked => { Service.IsWantedLevelLocked = @checked; }, Service);
    }

    private void AddCash()
    {
        var allCashHash = Enum.GetValues(typeof(CashHash)).Cast<CashHash>().ToList();

        var listItemAddCash = AddListItem(PlayerItemTitles.AddCash,
            (selected, index) => { Service.AddCash = (CashHash)index; },
            (selected, index) => { Service.RequestCashResult((CashHash)index); },
            value => Service.AddCash.GetLocalizedDisplayNameFromHash(),
            Service,
            allCashHash.Select(c => c.GetDescription()).ToArray());
    }

    private void CashInput()
    {
        var itemSetCash = AddItem(PlayerItemTitles.SetCash, () => { Service.RequestCashInput(); });
    }

    private void InfiniteStamina()
    {
        var checkBoxInfiniteStamina = AddCheckbox(PlayerItemTitles.InfiniteStamina,
            () => Service.HasInfiniteStamina,
            @checked => { Service.HasInfiniteStamina = @checked; }, Service);
    }

    private void InfiniteBreath()
    {
        var checkBoxInfiniteBreath = AddCheckbox(PlayerItemTitles.InfiniteBreath,
            () => Service.HasInfiniteBreath,
            @checked => { Service.HasInfiniteBreath = @checked; }, Service);
    }

    private void InfiniteSpecialAbility()
    {
        var checkBoxInfiniteSpecialAbility = AddCheckbox(PlayerItemTitles.InfiniteSpecialAbility,
            () => Service.HasInfiniteSpecialAbility,
            @checked => { Service.HasInfiniteSpecialAbility = @checked; }, Service);
    }

    private void Noiseless()
    {
        var checkBoxNoiseless = AddCheckbox(PlayerItemTitles.Noiseless,
            () => Service.IsNoiseless,
            @checked => { Service.IsNoiseless = @checked; }, Service);
    }

    private void RideOnCars()
    {
        var checkBoxRideOnCars = AddCheckbox(PlayerItemTitles.RideOnCars,
            () => Service.CanRideOnCars,
            @checked => { Service.CanRideOnCars = @checked; }, Service);
    }

    private void SuperJump()
    {
        var checkBoxSuperJump = AddCheckbox(PlayerItemTitles.SuperJump,
            () => Service.CanSuperJump,
            @checked => { Service.CanSuperJump = @checked; }, Service);
    }

    private void OnePunchMan()
    {
        var checkBoxOnePunchMan = AddCheckbox(PlayerItemTitles.OnePunchMan, () => Service.IsOnePunchMan,
            @checked => { Service.IsOnePunchMan = @checked; }, Service);
    }

    private void SuperSpeed()
    {
        var listItemSuperSpeed = AddListItem(PlayerItemTitles.SuperSpeed,
            (selected, index) => { Service.SuperSpeed = (SuperSpeedHash)index; },
            null,
            value => Service.SuperSpeed.GetLocalizedDisplayNameFromHash(),
            Service,
            typeof(SuperSpeedHash).ToDisplayNameArray());
    }

    private void Invisible()
    {
        var checkBoxInvisible = AddCheckbox(PlayerItemTitles.Invisible,
            () => Service.IsInvisible,
            @checked => { Service.IsInvisible = @checked; }, Service);
    }
}