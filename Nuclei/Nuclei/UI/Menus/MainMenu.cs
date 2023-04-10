﻿using System;
using Nuclei.Enums.UI;
using Nuclei.UI.Menus.Abstracts;
using Nuclei.UI.Menus.Player;
using Nuclei.UI.Menus.Settings;
using Nuclei.UI.Menus.Vehicle;

namespace Nuclei.UI.Menus;

public class MainMenu : MenuBase
{
    public MainMenu(Enum @enum) : base(@enum)
    {
        AddPlayerMenu();
        AddVehicleMenu();

        AddSettingsMenu();
    }

    private void AddVehicleMenu()
    {
        var vehicleMenu = new VehicleMenu(MenuTitles.Vehicle);
        AddMenu(vehicleMenu);
    }

    private void AddPlayerMenu()
    {
        var playerMenu = new PlayerMenu(MenuTitles.Player);
        AddMenu(playerMenu);
    }

    private void AddSettingsMenu()
    {
        var settingsMenu = new SettingsMenu(MenuTitles.Settings);
        AddMenu(settingsMenu);
    }
}