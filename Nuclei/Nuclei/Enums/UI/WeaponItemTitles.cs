using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum WeaponItemTitles
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

    [Description("Implemented later...")] GravityGun,

    [Description("Teleport to any location by shooting at the location.")]
    TeleportGun
}