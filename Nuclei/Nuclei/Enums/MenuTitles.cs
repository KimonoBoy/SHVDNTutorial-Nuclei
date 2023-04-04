using System.ComponentModel;

namespace Nuclei.Enums;

public enum MenuTitles
{
    [Description("One menu to rule them all! Access all the features and options in one convenient location.")]
    Main,

    [Description("Customize everything associated with your player and its character!")]
    Player,

    [Description(
        "Change your player's appearance and transform into any character, creature or even bird and take to the skies!")]
    SkinChanger,

    [Description(
        "Get complete control over your player's stats and customize them to be the best version of yourself!")]
    ChangeStats,

    [Description(
        "Get full control over vehicles, from repairing them to making them indestructible, adding parachutes, or even vehicle rockets. You can even spawn and modify your own vehicles!")]
    Vehicle,

    [Description("Summon any vehicle from the game and make your grand entrance in style!")]
    SpawnVehicle,

    [Description("Customize everything about your weapons and arm yourself to the teeth!")]
    Weapon
}