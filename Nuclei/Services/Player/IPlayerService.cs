using System;

namespace Nuclei.Services.Player;

public interface IPlayerService
{
    public bool IsInvincible { get; }
    public int WantedLevel { get; }
    event EventHandler PlayerFixed;
    void FixPlayer();
    void SetInvincible(bool isInvincible);
    void SetWantedLevel(int wantedLevel);
}