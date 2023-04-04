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
        Tick += OnTick;
        KeyDown += OnKeyDown;
        _playerService.PlayerFixed += OnPlayerFixed;
        _playerService.IsInvincible.ValueChanged += OnInvincibleChanged;
        _playerService.WantedLevel.ValueChanged += OnWantedLevelChanged;
        _playerService.CashInputRequested += OnCashInputRequested;
    }

    private void OnCashInputRequested(object sender, EventArgs e)
    {
        RequestCashInput();
    }

    /// <summary>
    ///     Requests a UserInput Window for setting Player Money.
    /// </summary>
    private static void RequestCashInput()
    {
        try
        {
            var maxLength = int.MaxValue.ToString().Length;
            var cashInput = Game.GetUserInput(WindowTitle.EnterMessage20, "", 20);
            if (cashInput.Length > maxLength)
                cashInput = cashInput.Substring(0, maxLength);

            if (string.IsNullOrEmpty(cashInput))
                throw new EmptyCashInputException();

            var cashInputAsULong = ulong.TryParse(cashInput, out var result);

            if (cashInputAsULong)
                Game.Player.Money = result > int.MaxValue ? int.MaxValue : (int)result;
            else
                throw new InvalidCashInputException();
        }
        catch (CustomExceptionBase cashInputException)
        {
            ExceptionService.Instance.RaiseError($"{cashInputException}");
        }
        catch (Exception ex)
        {
            ExceptionService.Instance.RaiseError(ex.Message);
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        /*
         * Updates the different states in the PlayerService, when
         * changes in the game happens.
         */
        UpdateInvincible();
        UpdateWantedLevel();

        /*
         * Utilities
         */
        ProcessInfiniteStamina();
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
        Game.Player.WantedLevel = e.Value;
    }

    private void UpdateWantedLevel()
    {
        if (_playerService.WantedLevel.Value != Game.Player.WantedLevel)
            _playerService.WantedLevel.Value = Game.Player.WantedLevel;
    }

    private void UpdateInvincible()
    {
        // This needs to be implemented differently. We'll cover it later and you'll see why.
        if (_playerService.IsInvincible.Value != Game.Player.IsInvincible)
            _playerService.IsInvincible.Value = Game.Player.Character.IsInvincible;
    }
}