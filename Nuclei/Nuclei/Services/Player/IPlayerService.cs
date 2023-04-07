using System;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Player;

public interface IPlayerService
{
    BindableProperty<bool> IsInvincible { get; }
    BindableProperty<bool> HasInfiniteSpecialAbility { get; }
    BindableProperty<bool> HasInfiniteStamina { get; }
    BindableProperty<bool> HasInfiniteBreath { get; }
    BindableProperty<int> WantedLevel { get; }
    BindableProperty<int> LockedWantedLevel { get; }
    BindableProperty<bool> IsWantedLevelLocked { get; }
    BindableProperty<bool> IsNoiseless { get; }
    BindableProperty<bool> CanSuperJump { get; }
    BindableProperty<bool> CanRideOnCars { get; }

    event EventHandler PlayerFixed;
    void FixPlayer();

    event EventHandler CashInputRequested;
    void RequestCashInput();
}