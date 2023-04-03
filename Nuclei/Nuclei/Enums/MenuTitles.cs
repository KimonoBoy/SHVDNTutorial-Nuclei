using System.ComponentModel;

namespace Nuclei.Enums;

public enum MenuTitles
{
    [Description("One Menu to Rule them all.")]
    Main,

    [Description("Everything associated with the Player and its Character.")]
    Player,

    [Description("Change the Player's model. Be anyone you'd like. Even become a bird and fly.")]
    SkinChanger,

    [Description(
        "This menu allows you to see the Player's current stats and adjust them to your liking. Be the best version of you!")]
    ChangeStats,

    [Description(
        "Repair vehicles, make them indestructible, apply parachutes, how about vehicle rockets, or maybe you just want to spawn your own and modify them?")]
    Vehicle,

    [Description("Spawn any Vehicle in the game.")]
    SpawnVehicle,

    [Description("Everything associated with Weapons.")]
    Weapon
}