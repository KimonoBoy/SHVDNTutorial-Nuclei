using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum WeaponItemTitle
{
    [Description("Give all weapons in game.")]
    GiveAllWeapons,

    [Description("Infinite ammunition.")] InfiniteAmmo,

    [Description(
        "Combine this with infinite ammunition to shoot like a complete maniac.\n\nRequires Infinite Ammo to be checked.")]
    NoReload,

    [Description("Ablaze your enemies with fire!")]
    FireBullets,

    [Description("Who doesn't love explosions?")]
    ExplosiveBullets,

    [Description("Levitate objects, vehicles and people with your gun.")]
    LevitationGun,

    [Description("Aim at an entity and hold down J.")]
    GravityGun,

    [Description("Adjust the throw velocity of the gravity-gun objects.")]
    ThrowVelocity,

    [Description("Teleport to any location by shooting at the location.")]
    TeleportGun,

    [Description(
        "What is even more fun than bullets? Vehicles as bullets.\n\n~b~Note: This version is just for testing purposes only - we'll make it better later.")]
    VehicleGun,

    [Description("Create a black hole that sucks in everything.")]
    BlackHoleGun,

    [Description("How long the black hole will stay active for in seconds.")]
    LifeSpan,

    [Description("The Event Horizon Size determines the radius at which objects are pulled towards it.")]
    BlackHoleRadius,

    [Description("Determines how powerful the black hole is.")]
    BlackHolePower
}