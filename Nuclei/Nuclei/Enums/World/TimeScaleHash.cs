using System.ComponentModel;

namespace Nuclei.Enums.World;

public enum TimeScaleHash
{
    [Description("Normal is the normal Time Scale.")]
    Normal,

    [Description("Slow is 80% of the normal Time Scale.")]
    Slow,

    [Description("Slower is 50% of the normal Time Scale.")]
    Slower,

    [Description("Slowest is 25% of the normal Time Scale.")]
    Slowest
}