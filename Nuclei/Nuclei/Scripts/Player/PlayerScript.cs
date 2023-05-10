using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using Nuclei.Enums.Player;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Player;

public class PlayerScript : PlayerScriptBase
{
    private const int DrownsInWater = 3;

    protected override void SubscribeToEvents()
    {
        KeyDown += OnKeyDown;
        Service.PlayerFixRequested += OnPlayerFixRequested;
        Service.CashInputRequested += OnCashInputRequested;
        Service.AddCashRequested += OnAddCashRequested;
        Service.PropertyChanged += OnPropertyChanged;
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.WantedLevel)) Game.Player.WantedLevel = Service.WantedLevel;

        if (e.PropertyName == nameof(Service.IsInvisible)) Game.Player.IsInvincible = Service.IsInvisible;
    }

    protected override void UnsubscribeOnExit()
    {
        KeyDown -= OnKeyDown;
        Service.PlayerFixRequested -= OnPlayerFixRequested;
        Service.CashInputRequested -= OnCashInputRequested;
        Service.AddCashRequested -= OnAddCashRequested;
    }

    protected override void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;

        ProcessNoiseless();
        ProcessSuperJump();
        ProcessInvincible();
        ProcessInvisible();
        ProcessInfiniteBreath();
        ProcessRideOnCars();
        ProcessInfiniteStamina();
        ProcessInfiniteSpecialAbility();
        ProcessLockedWantedLevel();

        UpdateWantedLevel();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.T && e.Control) CurrentEntity.TeleportToBlip(BlipSprite.Waypoint);
    }

    private void OnPlayerFixRequested(object sender, EventArgs e)
    {
        Character.Health = Character.MaxHealth;
        Character.Armor = Game.Player.MaxArmor;
    }

    private void OnCashInputRequested(object sender, EventArgs e)
    {
        RequestCashInput();
    }

    private void OnAddCashRequested(object sender, CashHash cashHash)
    {
        AddCash(cashHash);
    }


    private void ProcessLockedWantedLevel()
    {
        if (Service.IsWantedLevelLocked)
            Game.Player.WantedLevel = Service.LockedWantedLevel;
    }


    private void ProcessInvincible()
    {
        if (Character.IsInvincible == Service.IsInvincible) return;

        Character.IsInvincible = Service.IsInvincible;
    }

    private void ProcessInvisible()
    {
        Character.IsVisible = !Service.IsInvisible;
        Character.CanBeTargetted = !Service.IsInvisible;
        Game.Player.IgnoredByEveryone = Service.IsInvisible;
    }

    private void ProcessInfiniteBreath()
    {
        if (Character.GetConfigFlag(DrownsInWater) == !Service.HasInfiniteBreath) return;

        /*
         * False: Can't drown in water. (InfiniteBreath)
         * True: Can drown in water. (Not InfiniteBreath)  
         */
        Character.SetConfigFlag(DrownsInWater, !Service.HasInfiniteBreath);
    }

    private void ProcessRideOnCars()
    {
        if (Character.CanRagdoll == !Service.CanRideOnCars) return;

        // False means the player won't fall over.
        Character.CanRagdoll = !Service.CanRideOnCars;
    }

    private void UpdateWantedLevel()
    {
        if (Service.WantedLevel == Game.Player.WantedLevel) return;

        Service.WantedLevel = Game.Player.WantedLevel;
    }

    /// <summary>
    ///     No noise.
    /// </summary>
    private void ProcessNoiseless()
    {
        if (!Service.IsNoiseless) return;

        Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 0.0f);
        Function.Call(Hash.SET_PLAYER_SNEAKING_NOISE_MULTIPLIER, Game.Player, 0.0f);
    }

    /// <summary>
    ///     When sprinting or swimming, if the amount of time you can sprint for
    ///     drops below 5 seconds, RESET STAMINA to FULL.
    /// </summary>
    private void ProcessInfiniteStamina()
    {
        if (!Service.HasInfiniteStamina) return;
        if (!Character.IsRunning && !Character.IsSprinting &&
            !Character.IsSwimming) return;

        if (Game.Player.RemainingSprintTime <= 5.0f)
            Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player);
    }

    /// <summary>
    ///     Restores Special Ability Meter to Full.
    /// </summary>
    private void ProcessInfiniteSpecialAbility()
    {
        if (!Service.HasInfiniteSpecialAbility) return;
        var isAbilityMeterFull = Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_METER_FULL, Game.Player);
        if (isAbilityMeterFull) return;

        Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Game.Player, true);
    }

    /// <summary>
    ///     Allows the player to jump as high as a building.
    /// </summary>
    private void ProcessSuperJump()
    {
        if (!Service.CanSuperJump) return;

        Game.Player.SetSuperJumpThisFrame();
    }

    /// <summary>
    ///     Requests a UserInput Window for setting Player Money.
    /// </summary>
    private void RequestCashInput()
    {
        var maxLength = ulong.MaxValue.ToString().Length;
        var cashInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 12);
        if (cashInput.Length > maxLength)
            cashInput = cashInput.Substring(0, maxLength);

        if (string.IsNullOrEmpty(cashInput)) return;

        var cashInputAsULong = ulong.TryParse(cashInput, out var result);

        if (cashInputAsULong)
            Game.Player.Money = result > int.MaxValue ? int.MaxValue : (int)result;
        else
            Display.Notify("Invalid Cash Input", "Please enter a whole number.", false);
    }

    /// <summary>
    ///     Adds the CashHash value to the player's current money amount.
    /// </summary>
    /// <param name="cashHash">The cashHash.</param>
    private static void AddCash(CashHash cashHash)
    {
        var descriptionToInt = new string(cashHash.GetDescription().Where(char.IsDigit).ToArray());

        var parseSuccess = int.TryParse(descriptionToInt, out var result);

        if (!parseSuccess)
            return;

        var newMoney = (long)Game.Player.Money + result;

        if (newMoney > int.MaxValue)
            Game.Player.Money = int.MaxValue;
        else
            Game.Player.Money = (int)newMoney;
    }
}