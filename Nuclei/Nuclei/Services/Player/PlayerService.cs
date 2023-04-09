using System;
using Nuclei.Enums.Player;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Player;

/// <summary>
///     This class represents the PlayerService and serves as a layer between the PlayerMenu and the PlayerScript.
///     The primary responsibility of this class is state management.
/// </summary>
public class PlayerService : IPlayerService
{
    /// <summary>
    ///     A singleton instance of the PlayerService class.
    /// </summary>
    public static readonly PlayerService Instance = new();

    /// <summary>
    ///     A property that can set the cash hash.
    /// </summary>
    public BindableProperty<CashHash> AddCash { get; } = new();

    /// <summary>
    ///     A property that can set the super speed hash.
    /// </summary>
    public BindableProperty<SuperSpeedHash> SuperSpeed { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player is one punch man.
    /// </summary>
    public BindableProperty<bool> IsOnePunchMan { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player is invisible.
    /// </summary>
    public BindableProperty<bool> IsInvisible { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player can ride on cars without falling over.
    /// </summary>
    public BindableProperty<bool> CanRideOnCars { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player is noiseless.
    /// </summary>
    public BindableProperty<bool> IsNoiseless { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the Wanted Level
    ///     will be locked at its current value.
    /// </summary>
    public BindableProperty<bool> IsWantedLevelLocked { get; } = new();

    /// <summary>
    ///     The Wanted Level to default to when the IsWantedLevelLocked is true.
    /// </summary>
    public BindableProperty<int> LockedWantedLevel { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player has infinite special ability.
    /// </summary>
    public BindableProperty<bool> HasInfiniteSpecialAbility { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player has infinite stamina.
    /// </summary>
    public BindableProperty<bool> HasInfiniteStamina { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player has infinite underwater breath.
    /// </summary>
    public BindableProperty<bool> HasInfiniteBreath { get; } = new();

    /// <summary>
    ///     A property that determines whether the player is invincible or not.
    /// </summary>
    public BindableProperty<bool> IsInvincible { get; } = new();

    /// <summary>
    ///     A property that represents the wanted level of the player.
    /// </summary>
    public BindableProperty<int> WantedLevel { get; } = new();

    /// <summary>
    ///     A property that defines whether or not the player can super jump.
    /// </summary>
    public BindableProperty<bool> CanSuperJump { get; } = new();

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
}