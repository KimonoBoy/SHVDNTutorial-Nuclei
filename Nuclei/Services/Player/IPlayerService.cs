namespace Nuclei.Services.Player;

public interface IPlayerService
{
    void FixPlayer();
    void SetInvincible(bool isInvincible);
    void SetWantedLevel(int wantedLevel);
}