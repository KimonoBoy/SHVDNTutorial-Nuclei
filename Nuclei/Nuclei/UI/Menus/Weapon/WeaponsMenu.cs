using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Weapon;

public class WeaponsMenu : GenericMenuBase<WeaponsService>
{
    public WeaponsMenu(Enum @enum) : base(@enum)
    {
        WeaponComponentsMenu();
        GiveAllWeapons();
    }

    private void GiveAllWeapons()
    {
        var itemGiveAllWeapons = AddItem(WeaponItemTitles.GiveAllWeapons, () => { Service.RequestAllWeapons(); });
    }

    private void WeaponComponentsMenu()
    {
        var weaponComponents = new WeaponComponentsMenu(MenuTitles.WeaponComponents);
        AddMenu(weaponComponents);
    }
}