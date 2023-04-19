using System;
using Nuclei.Enums.Player;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Player;

public class PlayerService : GenericService<PlayerService>, IPlayerService
{
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
    public void RequestFixPlayer()
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