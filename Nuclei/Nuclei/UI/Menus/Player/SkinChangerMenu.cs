using System;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Player;

public class SkinChangerMenu : GenericsMenuBase<PlayerService>
{
    public SkinChangerMenu(Enum @enum) : base(@enum)
    {
    }
}