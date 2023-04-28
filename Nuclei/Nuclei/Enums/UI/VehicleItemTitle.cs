using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum VehicleItemTitle
{
    [Description("Repairs the vehicle completely.")]
    RepairVehicle,

    [Description("Makes the vehicle indestructible.")]
    Indestructible,

    [Description("Increases the speed of the vehicle.")]
    SpeedBoost,

    [Description("If the vehicle is upside down, flips it back over.")]
    FlipVehicle,

    [Description("Never fly out the windscreen again!")]
    SeatBelt,

    [Description("Keep driving yo!")] DriveUnderWater,

    [Description("You'll never fall off your bike again!")]
    NeverFallOffBike,

    [Description(
        "Doors are always locked. Cops can't pull you out either.\n\n~r~Note: If you want to customize your vehicle in a garage or customs, this must be turned off.")]
    LockDoors
}