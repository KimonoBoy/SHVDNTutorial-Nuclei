using System.ComponentModel;

namespace Nuclei.Enums.Vehicle;

public enum VehicleModsItemTitles
{
    [Description("Change your license plate.")]
    LicensePlate,

    [Description("Change the style of your license plate.")]
    LicensePlateStyle,

    [Description("Change the type of your license plate.")]
    LicensePlateType,

    [Description("Randomize all valid vehicle mods.")]
    RandomizeMods,

    [Description("Change the wheel type.")]
    WheelType,

    [Description("Change the rim color of your wheels.")]
    RimColor,
    CustomTires,

    [Description("Change the colors of your tires smoke.")]
    TireSmokeColor
}