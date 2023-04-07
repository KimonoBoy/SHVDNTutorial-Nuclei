using System;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Player;

public interface IPlayerService
{
    BindableProperty<bool> IsInvincible { get; }
    BindableProperty<bool> HasInfiniteSpecialAbility { get; }
    BindableProperty<bool> HasInfiniteStamina { get; }
    BindableProperty<int> WantedLevel { get; }

    event EventHandler PlayerFixed;
    void FixPlayer();

    event EventHandler CashInputRequested;
    void RequestCashInput();
}