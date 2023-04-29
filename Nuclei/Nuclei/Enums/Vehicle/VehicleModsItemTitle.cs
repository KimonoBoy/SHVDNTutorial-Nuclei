using System.ComponentModel;

namespace Nuclei.Enums.Vehicle;

public enum VehicleModsItemTitle
{
    [Description("Change your license plate.")]
    LicensePlate,

    [Description("Change the style of your license plate.")]
    LicensePlateStyle,

    [Description("Randomize all valid vehicle mods.")]
    RandomizeMods,

    [Description("Change the wheel type.")]
    WheelType,

    [Description("Change the rim color of your wheels.")]
    RimColor,

    [Description("Are you using Custom Tires?")]
    CustomTires,

    [Description("Change the colors of your tires smoke.")]
    TireSmokeColor,

    [Description("Tint the windows of your vehicle.")]
    WindowTint,

    [Description("Toggle Xenon Lights on/off.")]
    XenonHeadLights,

    [Description("Where under the vehicle should the neon lights be placed?")]
    NeonLightsLayout,

    [Description("Your vehicles primary color.")]
    PrimaryColor,

    [Description("Your vehicles secondary color.")]
    SecondaryColor,

    [Description("Your vehicles pearlescent color.")]
    PearlescentColor,

    [Description("The color of your neon lights.")]
    NeonLightsColor,

    [Description("Activate Turbo.")] Turbo,

    [Description("Every 50ms the color of your vehicle changes.")]
    RainbowMode
}