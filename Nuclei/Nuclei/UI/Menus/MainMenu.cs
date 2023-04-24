using System;
using Nuclei.Enums.UI;
using Nuclei.UI.Menus.Base;
using Nuclei.UI.Menus.Player;
using Nuclei.UI.Menus.Settings;
using Nuclei.UI.Menus.Vehicle;
using Nuclei.UI.Menus.Weapon;

namespace Nuclei.UI.Menus;

public class MainMenu : MenuBase
{
    public MainMenu(Enum @enum) : base(@enum)
    {
        AddPlayerMenu();
        AddVehicleMenu();
        AddWeaponsMenu();

        AddStorageMenu();
    }

    private void AddVehicleMenu()
    {
        var vehicleMenu = new VehicleMenu(MenuTitles.Vehicle);
        AddMenu(vehicleMenu);
    }

    private void AddWeaponsMenu()
    {
        var weaponsMenu = new WeaponsMenu(MenuTitles.Weapons);
        AddMenu(weaponsMenu);
    }

    private void AddPlayerMenu()
    {
        var playerMenu = new PlayerMenu(MenuTitles.Player);
        AddMenu(playerMenu);
    }

    private void AddStorageMenu()
    {
        var storageMenu = new StorageMenu(MenuTitles.Storage);
        AddMenu(storageMenu);
    }
}