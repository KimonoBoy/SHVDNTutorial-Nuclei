﻿using System;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using Nuclei.Enums.Player;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception;
using Nuclei.Services.Exception.CustomExceptions;
using Nuclei.Services.Player;
using Control = GTA.Control;

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
        _playerService.AddCashRequested += OnAddCashRequested;
        _playerService.SuperSpeed.ValueChanged += OnSuperSpeedChanged;
    }

    private void OnSuperSpeedChanged(object sender, ValueEventArgs<SuperSpeedHash> superSpeedValueEventArgs)
    {
        switch (superSpeedValueEventArgs.Value)
        {
            case SuperSpeedHash.Normal:
                Game.Player.SetRunSpeedMultThisFrame(1.0f); // 1.0f is the default speed.
                break;
            case SuperSpeedHash.Fast:
                Game.Player.SetRunSpeedMultThisFrame(1.49f); // 1.49f is the maximum multiplier value.
                break;

            case SuperSpeedHash.Faster:
                Game.Player.SetRunSpeedMultThisFrame(1.49f);
                Tick += OnTickFaster;
                break;

            case SuperSpeedHash.Sonic:
                Game.Player.SetRunSpeedMultThisFrame(1.49f);
                Tick += OnTickSonic;
                break;

            case SuperSpeedHash.TheFlash:
                Game.Player.SetRunSpeedMultThisFrame(1.49f);
                Tick += OnTickTheFlash;
                break;
        }
    }

    private void OnAddCashRequested(object sender, CashHash cashHash)
    {
        AddCash(cashHash);
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
        ProcessSuperJump();
    }

    /// <summary>
    ///     Allows the player to jump as high as a building.
    /// </summary>
    private void ProcessSuperJump()
    {
        if (!_playerService.CanSuperJump.Value) return;

        Game.Player.SetSuperJumpThisFrame();
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

    private void ProcessSuperSpeedTicks(int maxSpeed, SuperSpeedHash speedHash)
    {
        if (!_playerService.SuperSpeed.Value.Equals(speedHash)) return;

        if (!Game.IsControlPressed(Control.Sprint) ||
            (!Game.Player.Character.IsRunning && !Game.Player.Character.IsSprinting)) return;

        Game.Player.Character.MaxSpeed = maxSpeed;
        Game.Player.Character.ApplyForce(Game.Player.Character.ForwardVector * maxSpeed);
    }

    private void OnTickTheFlash(object sender, EventArgs e)
    {
        ProcessSuperSpeedTicks(350, SuperSpeedHash.TheFlash);
    }

    private void OnTickSonic(object sender, EventArgs e)
    {
        ProcessSuperSpeedTicks(120, SuperSpeedHash.Sonic);
    }

    private void OnTickFaster(object sender, EventArgs e)
    {
        ProcessSuperSpeedTicks(50, SuperSpeedHash.Faster);
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