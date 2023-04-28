using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum PlayerItemTitle
{
    [Description("Rejuvenate yourself with this powerful elixir that replenishes your health and armor in a flash!")]
    FixPlayer,

    [Description(
        "Unleash your inner superhero with this incredible power that renders you completely immune to damage!")]
    Invincible,

    [Description("Take control of the law with this nifty tool that allows you to adjust your wanted level at will!")]
    WantedLevel,

    [Description("Put the cops on lockdown with this awesome feature that locks your wanted level in place!")]
    LockWantedLevel,

    [Description("Want to live the high life? Use SetCash to input your desired amount of cash and live like a king!")]
    SetCash,

    [Description(
        "Get rich quick with this amazing feature that lets you add wads of cash to your wallet with a single click!")]
    AddCash,

    [Description(
        "Never tire out again with this incredible ability that lets you run, sprint, and swim without limits!")]
    InfiniteStamina,

    [Description(
        "Take a deep breath and dive into the depths of GTA V with this awesome power that lets you explore underwater without ever running out of air!")]
    InfiniteBreath,

    [Description("Unleash your special powers and keep them going indefinitely with this amazing feature!")]
    InfiniteSpecialAbility,

    [Description("Say goodbye to ragdoll physics and enjoy a smooth ride across town with this awesome feature!")]
    RideOnCars,

    [Description(
        "Become the ultimate stealth assassin with this incredible ability that makes you completely silent and undetectable!")]
    Noiseless,

    [Description(
        "Leap tall buildings in a single bound with this amazing power that lets you jump to incredible heights!")]
    SuperJump,

    [Description(
        "Channel your inner superhero and send pedestrians and vehicles flying with a single punch using this incredible ability!")]
    OnePunchMan,

    [Description(
        "Travel at supersonic speeds and unlock new superpowers with this incredible feature that lets you switch between Normal, Fast, Faster, Sonic, and The Flash modes!")]
    SuperSpeed,

    [Description(
        "Become a master of stealth and disappear from sight with this incredible ability that makes you completely invisible to the naked eye!")]
    Invisible
}