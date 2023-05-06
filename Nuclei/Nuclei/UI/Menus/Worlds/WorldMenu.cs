﻿using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Worlds;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Worlds;

public class WorldMenu : GenericMenu<WorldService>
{
    public WorldMenu(Enum @enum) : base(@enum)
    {
        TimeMenu();
        WeatherMenu();
    }

    private void WeatherMenu()
    {
        var weatherMenu = new WeatherMenu(MenuTitle.Weather);
        AddMenu(weatherMenu);
    }

    private void TimeMenu()
    {
        var timeMenu = new TimeMenu(MenuTitle.Time);
        AddMenu(timeMenu);
    }
}