using System.ComponentModel;

namespace Nuclei.Enums;

public enum MenuTitles
{
    [Description("One Menu to Rule them all.")]
    Main,

    [Description("Everything associated with the Player and its Character.")]
    Player,

    [Description("Spawn Vehicles, Make Them Indestructible, Add Modifications and Much More")]
    Vehicle,

    [Description("Everything associated with Weapons.")]
    Weapon
}