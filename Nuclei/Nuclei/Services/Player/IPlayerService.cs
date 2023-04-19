using System;
using Nuclei.Enums.Player;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Player;

public interface IPlayerService
{
    // Properties
    BindableProperty<CashHash> AddCash { get; }
    BindableProperty<SuperSpeedHash> SuperSpeed { get; }
    BindableProperty<bool> IsOnePunchMan { get; }
    BindableProperty<bool> IsInvisible { get; }
    BindableProperty<bool> CanRideOnCars { get; }
    BindableProperty<bool> IsNoiseless { get; }
    BindableProperty<bool> IsWantedLevelLocked { get; }
    BindableProperty<int> LockedWantedLevel { get; }
    BindableProperty<bool> HasInfiniteSpecialAbility { get; }
    BindableProperty<bool> HasInfiniteStamina { get; }
    BindableProperty<bool> HasInfiniteBreath { get; }
    BindableProperty<bool> IsInvincible { get; }
    BindableProperty<int> WantedLevel { get; }
    BindableProperty<bool> CanSuperJump { get; }

    // Events
    event EventHandler PlayerFixed;
    event EventHandler CashInputRequested;
    event EventHandler<CashHash> AddCashRequested;

    // Methods
    void RequestFixPlayer();
    void RequestCashInput();
    void RequestCashResult(CashHash cashHash);
}