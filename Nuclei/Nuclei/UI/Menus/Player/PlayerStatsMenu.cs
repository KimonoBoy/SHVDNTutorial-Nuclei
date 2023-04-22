using System;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player;

public class PlayerStatsMenu : GenericMenuBase<PlayerService>
{
    public PlayerStatsMenu(Enum @enum) : base(@enum)
    {
    }
}