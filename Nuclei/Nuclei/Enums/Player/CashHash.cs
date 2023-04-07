using System.ComponentModel;

namespace Nuclei.Enums.Player;

public enum CashHash
{
    [Description("$10.000")] TenThousand,
    [Description("$100.000")] HundredThousand,
    [Description("$1.000.000")] OneMillion,
    [Description("$10.000.000")] TenMillion,
    [Description("$100.000.000")] OneHundredMillion,
    [Description("$1.000.000.000")] OneBillion
}