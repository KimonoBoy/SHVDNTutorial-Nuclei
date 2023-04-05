using System.ComponentModel;

namespace Nuclei.Enums.Exception;

public enum ExceptionType
{
    [Description("Something went wrong")] Unknown,

    [Description("Failed to spawn vehicle")]
    SpawnVehicle,

    [Description("Set cash input failed")] CashInput
}