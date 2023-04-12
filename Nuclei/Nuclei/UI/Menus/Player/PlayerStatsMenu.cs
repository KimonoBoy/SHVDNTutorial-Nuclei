using System;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class PlayerStatsMenu : GenericsMenuBase<PlayerService>
{
    public PlayerStatsMenu(Enum @enum) : base(@enum)
    {
    }
}