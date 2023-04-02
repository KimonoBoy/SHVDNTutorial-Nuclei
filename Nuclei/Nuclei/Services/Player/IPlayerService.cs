using System;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Player;

public interface IPlayerService
{
    BindableProperty<bool> IsInvincible { get; }
    BindableProperty<int> WantedLevel { get; }

    event EventHandler PlayerFixed;
    void FixPlayer();
}