using System;

namespace Nuclei.Services.Player;

public interface IPlayerService
{
    bool IsInvincible { get; }
    int WantedLevel { get; }
    event EventHandler PlayerFixed;
    void FixPlayer();
    void SetInvincible(bool isInvincible);
    void SetWantedLevel(int wantedLevel);
}