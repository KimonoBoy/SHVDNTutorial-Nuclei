using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum VehicleWeaponsItemTitles
{
    [Description("Enable Vehicle Weapons.")]
    VehicleWeapons,

    [Description("Select number of weapons to shoot from.")]
    NumWeapons,

    [Description("Rate at which vehicle weapons are fired.")]
    FireRate
}