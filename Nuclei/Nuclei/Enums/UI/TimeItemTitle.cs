using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum TimeItemTitle
{
    [Description("Change the current time of day.")]
    TimeOfDay,

    [Description("Lock the current time of day.")]
    LockTimeOfDay,

    [Description("Slow down the passage of time.")]
    TimeScale
}