using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum WeaponItemTitles
{
    [Description("Give all weapons in game.")]
    GiveAllWeapons,

    [Description("Infinite ammunition.")] InfiniteAmmo,

    [Description("Combine this with infinite ammunition to shoot like a complete maniac.")]
    NoReload,
    FireBullets
}