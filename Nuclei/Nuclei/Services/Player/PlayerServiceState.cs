using Nuclei.Enums.Player;

namespace Nuclei.Services.Player;

public class PlayerServiceState
{
    public CashHash AddCash { get; set; }
    public SuperSpeedHash SuperSpeed { get; set; }
    public bool IsOnePunchMan { get; set; }
    public bool IsInvisible { get; set; }
    public bool CanRideOnCars { get; set; }
    public bool IsNoiseless { get; set; }
    public bool IsWantedLevelLocked { get; set; }
    public int LockedWantedLevel { get; set; }
    public bool HasInfiniteSpecialAbility { get; set; }
    public bool IsInvincible { get; set; }
    public bool CanSuperJump { get; set; }
    public int WantedLevel { get; set; }
    public bool HasInfiniteStamina { get; set; }
    public bool HasInfiniteBreath { get; set; }
}