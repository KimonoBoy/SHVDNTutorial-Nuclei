using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum MenuTitle
{
    [Description("One menu to rule them all! Access all the features and options in one convenient location.")]
    Main,

    [Description("Customize everything associated with your player and its character!")]
    Player,

    [Description(
        "Change your player's appearance and transform into any character, creature or even bird and take to the skies!")]
    ModelChanger,

    [Description(
        "Get complete control over your player's stats and customize them to be the best version of yourself!")]
    ChangeStats,

    [Description(
        "Get full control over vehicles, from repairing them to making them indestructible, adding parachutes, or even vehicle rockets. You can even spawn and modify your own vehicles!")]
    Vehicle,

    [Description("Summon any vehicle from the game and make your grand entrance in style!")]
    SpawnVehicle,

    [Description("Your favorite vehicles stored in one place!")]
    FavoriteVehicles,

    [Description("All your saved vehicles in one place!")]
    SavedVehicles,

    [Description("Vehicle Weapons - Customize your vehicle's weapons and arm it to the teeth!")]
    VehicleWeapons,

    [Description("Change vehicle mods with this easy-to-use menu!")]
    VehicleMods,

    [Description("Change wheel type, wheels, colors and more.")]
    Wheels,

    [Description("Change Front and Rear Bumpers.")]
    Bumpers,

    [Description("A menu for adjusting vehicle headlights.")]
    Lights,

    [Description("Vehicle resprays!")] Respray,

    [Description("Customize everything about your weapons and arm yourself to the teeth!")]
    Weapons,

    [Description("Change your weapon's stats and make it the best version of itself!")]
    WeaponComponents,

    [Description("Make your weapon a Gravity Gun - pick up and throw anything!")]
    GravityGun,

    [Description("Change how the black hole gun behaves.")]
    BlackHoleGun,

    [Description("Shoot vehicles from your gun!\n\nChoose between different vehicles.")]
    VehicleGun,

    [Description("Save/Load and Change HotKeys")]
    Settings,

    [Description("Save and Load")] Storage,

    [Description("Change HotKeys")] HotKeys,
    Interior,

    [Description("Change anything in the game world, from the weather to the time of day, to the gravity level!")]
    World,

    [Description("Adjust Time settings.")] Time,

    [Description("Adjust weather settings.")]
    Weather,

    [Description("Change your appearance and outfit.")]
    Appearance,
    FavoriteModels
}