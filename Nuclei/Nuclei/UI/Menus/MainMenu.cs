using System;
using Nuclei.Enums.UI;
using Nuclei.UI.Menus.Base;
using Nuclei.UI.Menus.Player;
using Nuclei.UI.Menus.Settings;
using Nuclei.UI.Menus.Vehicle;
using Nuclei.UI.Menus.Weapon;
using Nuclei.UI.Menus.World;

namespace Nuclei.UI.Menus;

public class MainMenu : MenuBase
{
    public MainMenu(Enum @enum) : base(@enum)
    {
        AddPlayerMenu();
        AddVehicleMenu();
        AddWeaponsMenu();

        AddWorldMenu();

        AddStorageMenu();
    }

    private void AddWorldMenu()
    {
        var worldMenu = new WorldMenu(MenuTitle.World);
        AddMenu(worldMenu);
    }

    private void AddVehicleMenu()
    {
        var vehicleMenu = new VehicleMenu(MenuTitle.Vehicle);
        AddMenu(vehicleMenu);
    }

    private void AddWeaponsMenu()
    {
        var weaponsMenu = new WeaponsMenu(MenuTitle.Weapons);
        AddMenu(weaponsMenu);
    }

    private void AddPlayerMenu()
    {
        var playerMenu = new PlayerMenu(MenuTitle.Player);
        AddMenu(playerMenu);
    }

    private void AddStorageMenu()
    {
        var storageMenu = new StorageMenu(MenuTitle.Storage);
        AddMenu(storageMenu);
    }
}