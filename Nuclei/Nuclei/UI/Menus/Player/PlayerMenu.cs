using System;
using System.Linq;
using Nuclei.Enums.Player;
using Nuclei.Enums.UI;
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
    }

    private void FixPlayer()
    {
        AddItem(PlayerItemTitle.FixPlayer, () => { Service.RequestFixPlayer(); });
    }

    private void Invincible()
    {
        var checkBoxInvincible = AddCheckbox(PlayerItemTitle.Invincible,
            () => Service.IsInvincible, Service, @checked => { Service.IsInvincible = @checked; });
    }

    private void AdjustWantedLevel()
    {
        var listItemWantedLevel = AddListItem(PlayerItemTitle.WantedLevel,
            () => Service.WantedLevel,
            Service,
            (item, index) =>
            {
                Service.WantedLevel = index;
                Service.LockedWantedLevel = index;
            }, 0, 1, 2,
            3, 4, 5);
    }

    private void LockWantedLevel()
    {
        var checkBoxLockWantedLevel = AddCheckbox(PlayerItemTitle.LockWantedLevel,
            () => Service.IsWantedLevelLocked, Service, @checked => { Service.IsWantedLevelLocked = @checked; });
    }

    private void AddCash()
    {
        var allCashHash = Enum.GetValues(typeof(CashHash)).Cast<CashHash>().ToList();

        var listItemAddCash = AddListItem(PlayerItemTitle.AddCash,
            () => (int)Service.AddCash, Service, (selected, index) => { Service.AddCash = (CashHash)index; },
            allCashHash.Select(c => c.GetDescription()).ToArray());
        listItemAddCash.Activated += (sender, args) => { Service.RequestCashResult(Service.AddCash); };
    }

    private void CashInput()
    {
        var itemSetCash = AddItem(PlayerItemTitle.SetCash, () => { Service.RequestCashInput(); });
    }

    private void InfiniteStamina()
    {
        var checkBoxInfiniteStamina = AddCheckbox(PlayerItemTitle.InfiniteStamina,
            () => Service.HasInfiniteStamina, Service, @checked => { Service.HasInfiniteStamina = @checked; });
    }

    private void InfiniteBreath()
    {
        var checkBoxInfiniteBreath = AddCheckbox(PlayerItemTitle.InfiniteBreath,
            () => Service.HasInfiniteBreath, Service, @checked => { Service.HasInfiniteBreath = @checked; });
    }

    private void InfiniteSpecialAbility()
    {
        var checkBoxInfiniteSpecialAbility = AddCheckbox(PlayerItemTitle.InfiniteSpecialAbility,
            () => Service.HasInfiniteSpecialAbility, Service,
            @checked => { Service.HasInfiniteSpecialAbility = @checked; });
    }

    private void Noiseless()
    {
        var checkBoxNoiseless = AddCheckbox(PlayerItemTitle.Noiseless,
            () => Service.IsNoiseless, Service, @checked => { Service.IsNoiseless = @checked; });
    }

    private void RideOnCars()
    {
        var checkBoxRideOnCars = AddCheckbox(PlayerItemTitle.RideOnCars,
            () => Service.CanRideOnCars, Service, @checked => { Service.CanRideOnCars = @checked; });
    }

    private void SuperJump()
    {
        var checkBoxSuperJump = AddCheckbox(PlayerItemTitle.SuperJump,
            () => Service.CanSuperJump, Service, @checked => { Service.CanSuperJump = @checked; });
    }

    private void OnePunchMan()
    {
        var checkBoxOnePunchMan = AddCheckbox(PlayerItemTitle.OnePunchMan, () => Service.IsOnePunchMan, Service,
            @checked => { Service.IsOnePunchMan = @checked; });
    }

    private void SuperSpeed()
    {
        var listItemSuperSpeed = AddListItem(PlayerItemTitle.SuperSpeed,
            () => (int)Service.SuperSpeed,
            Service,
            (selected, index) => { Service.SuperSpeed = (SuperSpeedHash)index; },
            typeof(SuperSpeedHash).ToDisplayNameArray());
    }

    private void Invisible()
    {
        var checkBoxInvisible = AddCheckbox(PlayerItemTitle.Invisible,
            () => Service.IsInvisible, Service, @checked => { Service.IsInvisible = @checked; });
    }
}