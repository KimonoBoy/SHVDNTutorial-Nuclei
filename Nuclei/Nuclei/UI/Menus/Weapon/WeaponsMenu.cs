using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class WeaponsMenu : GenericMenuBase<WeaponsService>
{
    public WeaponsMenu(Enum @enum) : base(@enum)
    {
        WeaponComponentsMenu();
        AddHeader("Basics");
        GiveAllWeapons();
        InfiniteAmmo();
        NoReload();
        FireBullets();
    }

    private void NoReload()
    {
        var checkBoxNoReload = AddCheckbox(WeaponItemTitles.NoReload, () => Service.NoReload,
            @checked => { Service.NoReload = @checked; }, Service);
    }

    private void InfiniteAmmo()
    {
        var checkBoxInfiniteAmmo = AddCheckbox(WeaponItemTitles.InfiniteAmmo, () => Service.InfiniteAmmo,
            @checked => { Service.InfiniteAmmo = @checked; }, Service);
    }

    private void FireBullets()
    {
        var checkBoxFireBullets = AddCheckbox(WeaponItemTitles.FireBullets, () => Service.FireBullets,
            @checked => { Service.FireBullets = @checked; }, Service);
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