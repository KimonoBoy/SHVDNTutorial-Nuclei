using System;

namespace Nuclei.Services.Player;

public interface IPlayerService
{
    event EventHandler PlayerFixed;
    void FixPlayer();
    void SetInvincible(bool isInvincible);
    void SetWantedLevel(int wantedLevel);
}