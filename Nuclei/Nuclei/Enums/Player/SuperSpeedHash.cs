using System.ComponentModel;

namespace Nuclei.Enums.Player;

public enum SuperSpeedHash
{
    [Description("Move at a normal pace.")]
    Normal,

    [Description("Move at a slightly faster pace.")]
    Fast,

    [Description("Move at a much faster pace.")]
    Faster,

    [Description("Move as fast as Sonic the Hedgehog.")]
    Sonic,

    [Description("Move as fast as The Flash himself.")]
    TheFlash
}