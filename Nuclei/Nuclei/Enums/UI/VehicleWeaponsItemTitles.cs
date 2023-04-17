using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum VehicleWeaponsItemTitles
{
    [Description(
        "Allows you to shoot a bunch of different weapons directly from your vehicle.\n\nSelect a weapon below.")]
    VehicleWeapons,

    [Description("Select weapon attachment point.")]
    WeaponAttachmentPoints,

    [Description("Rate at which vehicle weapons are fired.")]
    FireRate,

    [Description(
        "Rather than firing in the direction of the vehicle, the weapon will fire in the direction of where you aim.")]
    PointAndShoot
}