using System;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player;

public class SkinChangerMenu : GenericMenuBase<PlayerService>
{
    public SkinChangerMenu(Enum @enum) : base(@enum)
    {
    }
}