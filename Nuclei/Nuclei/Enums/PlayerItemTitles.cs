using System.ComponentModel;

namespace Nuclei.Enums;

public enum PlayerItemTitles
{
    [Description("Restores Player's Health and Armor.")]
    FixPlayer,

    [Description("You no longer take ANY damage.")]
    Invincible,

    [Description("Adjust your Wanted Level.")]
    WantedLevel,

    [Description("Locks the Wanted Level.")]
    LockWantedLevel,
    SuperJump,
    SuperPunch,
    SetCash,
    AddCash
}