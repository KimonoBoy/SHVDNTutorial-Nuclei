using System;
using Nuclei.Services.World;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.World;

public class WorldMenu : GenericMenuBase<WorldService>
{
    public WorldMenu(Enum @enum) : base(@enum)
    {
        TimeOfDay();
    }

    private void TimeOfDay()
    {
        var itemSetDayTime = AddItem("Day Time", "", () => { GTA.World.CurrentTimeOfDay = TimeSpan.FromHours(12); });
    }
}