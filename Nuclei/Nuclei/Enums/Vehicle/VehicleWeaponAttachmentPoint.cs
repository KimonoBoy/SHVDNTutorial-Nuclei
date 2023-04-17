using System.ComponentModel;

namespace Nuclei.Enums.Vehicle;

public enum VehicleWeaponAttachmentPoint
{
    [Description("One point from the middle of the vehicle.")]
    OneMiddle,

    [Description("Two points from each side of the vehicle.")]
    OneOnEachSide,

    [Description("Three points: Each side of the vehicle + one in the middle.")]
    EachSideAndMiddle
}