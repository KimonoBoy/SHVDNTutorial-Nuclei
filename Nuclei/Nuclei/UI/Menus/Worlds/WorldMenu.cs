using System;
using GTA;
using Nuclei.Services.Worlds;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Worlds;

public class WorldMenu : GenericMenu<WorldService>
{
    public WorldMenu(Enum @enum) : base(@enum)
    {
        TimeOfDay();
    }

    private void TimeOfDay()
    {
        // Just used for myself....
        var itemSetDayTime = AddItem("Day Time", "", () => { World.CurrentTimeOfDay = TimeSpan.FromHours(12); });
    }
}