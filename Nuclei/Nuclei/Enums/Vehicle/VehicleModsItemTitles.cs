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
    TireSmokeColor,

    [Description("Tint the windows of your vehicle.")]
    WindowTint,

    [Description("Toggle Xenon Headlights on/off.")]
    XenonHeadLights,

    [Description("Where under the vehicle should the neon lights be placed?")]
    NeonLightsLayout,

    [Description("Change the color type.")]
    ColorType,

    [Description("Your vehicles primary color.")]
    PrimaryColor,

    [Description("Your vehicles secondary color.")]
    SecondaryColor,

    [Description("Your vehicles pearlescent color.")]
    PearlscentColor
}