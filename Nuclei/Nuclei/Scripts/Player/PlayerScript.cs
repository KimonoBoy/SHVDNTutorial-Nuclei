using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Player;

namespace Nuclei.Scripts.Player;

public class PlayerScript : Script
{
    private readonly PlayerService _playerService = PlayerService.Instance;

    public PlayerScript()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
        _playerService.PlayerFixed += OnPlayerFixed;
        _playerService.IsInvincible.ValueChanged += OnInvincibleChanged;
        _playerService.WantedLevel.ValueChanged += OnWantedLevelChanged;
        _playerService.CashInputRequested += OnCashInputRequested;
        _playerService.HasInfiniteBreath.ValueChanged += OnInfiniteBreathChanged;
        _playerService.CanRideOnCars.ValueChanged += OnCanRideOnCarsChanged;
    }

    private void OnCanRideOnCarsChanged(object sender, ValueEventArgs<bool> e)
    {
        // False means the player won't fall over.
        Game.Player.Character.CanRagdoll = !e.Value;
    }

    private void OnInfiniteBreathChanged(object sender, ValueEventArgs<bool> e)
    {
        /*
         * PED_CONFIG_FLAG 3 is "DrownsInWater".
         *
         * False: Can't drown in water. (InfiniteBreath)
         * True: Can drown in water. (Not InfiniteBreath)  
         */
        Game.Player.Character.SetConfigFlag(3, !e.Value);
    }

    private void OnTick(object sender, EventArgs e)
    {
        UpdateStates();
        ProcessFunctions();
    }

    /// <summary>
    ///     Updates the different states in the service.
    /// </summary>
    private void UpdateStates()
    {
        UpdateInvincible();
        UpdateWantedLevel();
    }

    /// <summary>
    ///     Game changes that need to be processed each frame.
    /// </summary>
    private void ProcessFunctions()
    {
        ProcessInfiniteStamina();
        ProcessInfiniteSpecialAbility();
        ProcessNoiseless();
    }

    /// <summary>
    ///     The noise level increases slowly over time. This prevents that.
    /// </summary>
    private void ProcessNoiseless()
    {
        if (!_playerService.IsNoiseless.Value) return;

        Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 0.0f);
        Function.Call(Hash.SET_PLAYER_SNEAKING_NOISE_MULTIPLIER, Game.Player, 0.0f);
    }

    /// <summary>
    ///     Restores Special Ability Meter to Full.
    /// </summary>
    private void ProcessInfiniteSpecialAbility()
    {
        if (!_playerService.HasInfiniteSpecialAbility.Value) return;
        var isAbilityMeterFull = Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_METER_FULL, Game.Player);

        if (isAbilityMeterFull) return;

        Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Game.Player, true);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.T && e.Control)
            Game.Player.Character.TeleportToBlip(BlipSprite.Waypoint);
    }

    private void OnPlayerFixed(object sender, EventArgs e)
    {
        Game.Player.Character.Health = Game.Player.Character.MaxHealth;
        Game.Player.Character.Armor = Game.Player.MaxArmor;
    }

    private void OnInvincibleChanged(object sender, ValueEventArgs<bool> e)
    {
        Game.Player.Character.IsInvincible = e.Value;
    }

    private void OnWantedLevelChanged(object sender, ValueEventArgs<int> e)
    {
        Game.Player.WantedLevel =
            !_playerService.IsWantedLevelLocked.Value ? e.Value : _playerService.LockedWantedLevel.Value;
    }

    private void OnCashInputRequested(object sender, EventArgs e)
    {
        RequestCashInput();
    }

    private void UpdateInvincible()
    {
        // This needs to be implemented differently. We'll cover it later and you'll see why.
        if (_playerService.IsInvincible.Value != Game.Player.Character.IsInvincible)
            _playerService.IsInvincible.Value = Game.Player.Character.IsInvincible;
    }

    private void UpdateWantedLevel()
    {
        if (_playerService.WantedLevel.Value != Game.Player.WantedLevel)
            _playerService.WantedLevel.Value = Game.Player.WantedLevel;
    }

    /// <summary>
    ///     When sprinting or swimming, if the amount of time you can sprint for
    ///     drops below 5 seconds, RESET STAMINA to FULL.
    /// </summary>
    private void ProcessInfiniteStamina()
    {
        if (!_playerService.HasInfiniteStamina.Value) return;
        if (!Game.Player.Character.IsRunning && !Game.Player.Character.IsSprinting &&
            !Game.Player.Character.IsSwimming) return;

        if (Game.Player.RemainingSprintTime <= 5.0f)
            Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player);
    }

    /// <summary>
    ///     Requests a UserInput Window for setting Player Money.
    /// </summary>
    private void RequestCashInput()
    {
        try
        {
            var maxLength = int.MaxValue.ToString().Length;
            var cashInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 20);
            if (cashInput.Length > maxLength)
                cashInput = cashInput.Substring(0, maxLength);

            if (string.IsNullOrEmpty(cashInput)) return;

            var cashInputAsULong = ulong.TryParse(cashInput, out var result);

            if (cashInputAsULong)
                Game.Player.Money = result > int.MaxValue ? int.MaxValue : (int)result;
            else
                throw new InvalidCashInputException();
        }
        catch (CustomExceptionBase cashInputException)
        {
            ExceptionService.Instance.RaiseError(cashInputException);
        }
        catch (Exception ex)
        {
            ExceptionService.Instance.RaiseError(ex);
        }
    }
}