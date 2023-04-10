using System;
using Nuclei.Enums.Player;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Player;

public class PlayerService : IPlayerService
{
    /// <summary>
    ///     A singleton instance of the PlayerService class.
    /// </summary>
    public static readonly PlayerService Instance = new();

    /// <summary>
    ///     A property that can set the cash hash.
    /// </summary>
    public BindableProperty<CashHash> AddCash { get; set; } = new();

    /// <summary>
    ///     A property that can set the super speed hash.
    /// </summary>
    public BindableProperty<SuperSpeedHash> SuperSpeed { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player is one punch man.
    /// </summary>
    public BindableProperty<bool> IsOnePunchMan { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player is invisible.
    /// </summary>
    public BindableProperty<bool> IsInvisible { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player can ride on cars without falling over.
    /// </summary>
    public BindableProperty<bool> CanRideOnCars { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player is noiseless.
    /// </summary>
    public BindableProperty<bool> IsNoiseless { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the Wanted Level
    ///     will be locked at its current value.
    /// </summary>
    public BindableProperty<bool> IsWantedLevelLocked { get; set; } = new();

    /// <summary>
    ///     The Wanted Level to default to when the IsWantedLevelLocked is true.
    /// </summary>
    public BindableProperty<int> LockedWantedLevel { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player has infinite special ability.
    /// </summary>
    public BindableProperty<bool> HasInfiniteSpecialAbility { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player has infinite stamina.
    /// </summary>
    public BindableProperty<bool> HasInfiniteStamina { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player has infinite underwater breath.
    /// </summary>
    public BindableProperty<bool> HasInfiniteBreath { get; set; } = new();

    /// <summary>
    ///     A property that determines whether the player is invincible or not.
    /// </summary>
    public BindableProperty<bool> IsInvincible { get; set; } = new();

    /// <summary>
    ///     A property that represents the wanted level of the player.
    /// </summary>
    public BindableProperty<int> WantedLevel { get; set; } = new();

    /// <summary>
    ///     A property that defines whether or not the player can super jump.
    /// </summary>
    public BindableProperty<bool> CanSuperJump { get; set; } = new();

    /// <summary>
    ///     An event that is invoked when the player is fixed.
    /// </summary>
    public event EventHandler PlayerFixed;

    /// <summary>
    ///     A method that invokes the `PlayerFixed` event.
    /// </summary>
    public void FixPlayer()
    {
        PlayerFixed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     An event that is invoked when the RequestCashInput is called.
    /// </summary>
    public event EventHandler CashInputRequested;

    /// <summary>
    ///     A method that invokes the `CashInputRequested` event.
    /// </summary>
    public void RequestCashInput()
    {
        CashInputRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     An event that is invoked when the RequestCashResult is called.
    /// </summary>
    public event EventHandler<CashHash> AddCashRequested;

    /// <summary>
    ///     A method that invokes the `ÀddCashRequested` event.
    /// </summary>
    /// <param name="cashHash">The hash to get the Cash Amount from.</param>
    public void RequestCashResult(CashHash cashHash)
    {
        AddCashRequested?.Invoke(this, cashHash);
    }

    public PlayerService GetCurrentState()
    {
        // Create a new instance of PlayerService and set its properties with the current values
        var currentState = new PlayerService();

        currentState.AddCash.Value = AddCash.Value;
        currentState.SuperSpeed.Value = SuperSpeed.Value;
        currentState.IsOnePunchMan.Value = IsOnePunchMan.Value;
        currentState.IsInvisible.Value = IsInvisible.Value;
        currentState.CanRideOnCars.Value = CanRideOnCars.Value;
        currentState.IsNoiseless.Value = IsNoiseless.Value;
        currentState.IsWantedLevelLocked.Value = IsWantedLevelLocked.Value;
        currentState.LockedWantedLevel.Value = LockedWantedLevel.Value;
        currentState.HasInfiniteSpecialAbility.Value = HasInfiniteSpecialAbility.Value;
        currentState.HasInfiniteStamina.Value = HasInfiniteStamina.Value;
        currentState.HasInfiniteBreath.Value = HasInfiniteBreath.Value;
        currentState.IsInvincible.Value = IsInvincible.Value;
        currentState.WantedLevel.Value = WantedLevel.Value;
        currentState.CanSuperJump.Value = CanSuperJump.Value;

        return currentState;
    }

    public void SetState(PlayerService newState)
    {
        if (newState == null) return;

        AddCash.Value = newState.AddCash.Value;
        SuperSpeed.Value = newState.SuperSpeed.Value;
        IsOnePunchMan.Value = newState.IsOnePunchMan.Value;
        IsInvisible.Value = newState.IsInvisible.Value;
        CanRideOnCars.Value = newState.CanRideOnCars.Value;
        IsNoiseless.Value = newState.IsNoiseless.Value;
        IsWantedLevelLocked.Value = newState.IsWantedLevelLocked.Value;
        LockedWantedLevel.Value = newState.LockedWantedLevel.Value;
        HasInfiniteSpecialAbility.Value = newState.HasInfiniteSpecialAbility.Value;
        HasInfiniteStamina.Value = newState.HasInfiniteStamina.Value;
        HasInfiniteBreath.Value = newState.HasInfiniteBreath.Value;
        IsInvincible.Value = newState.IsInvincible.Value;
        WantedLevel.Value = newState.WantedLevel.Value;
        CanSuperJump.Value = newState.CanSuperJump.Value;
    }
}