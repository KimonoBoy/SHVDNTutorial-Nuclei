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

        AddHeader("Utilities");
        FireBullets();
        ExplosiveBullets();

        AddHeader("Gutties");
        LevitationGun();
        GravityGun();
        TeleportGun();
    }

    private void TeleportGun()
    {
        var checkBoxTeleportGun = AddCheckbox(WeaponItemTitles.TeleportGun, () => Service.TeleportGun, Service,
            @checked => { Service.TeleportGun = @checked; });
    }

    private void GravityGun()
    {
        var checkBoxGravityGun = AddCheckbox(WeaponItemTitles.GravityGun, () => Service.GravityGun, Service,
            @checked => { Service.GravityGun = @checked; });
    }

    private void LevitationGun()
    {
        var checkBoxLevitationGun = AddCheckbox(WeaponItemTitles.LevitationGun, () => Service.LevitationGun, Service,
            @checked => { Service.LevitationGun = @checked; });
    }

    private void ExplosiveBullets()
    {
        var checkBoxExplosiveBullets = AddCheckbox(WeaponItemTitles.ExplosiveBullets, () => Service.ExplosiveBullets,
            Service, @checked => { Service.ExplosiveBullets = @checked; });
    }

    private void NoReload()
    {
        var checkBoxNoReload = AddCheckbox(WeaponItemTitles.NoReload, () => Service.NoReload, Service,
            @checked => { Service.NoReload = @checked; });
    }

    private void InfiniteAmmo()
    {
        var checkBoxInfiniteAmmo = AddCheckbox(WeaponItemTitles.InfiniteAmmo, () => Service.InfiniteAmmo, Service,
            @checked => { Service.InfiniteAmmo = @checked; });
    }

    private void FireBullets()
    {
        var checkBoxFireBullets = AddCheckbox(WeaponItemTitles.FireBullets, () => Service.FireBullets, Service,
            @checked => { Service.FireBullets = @checked; });
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