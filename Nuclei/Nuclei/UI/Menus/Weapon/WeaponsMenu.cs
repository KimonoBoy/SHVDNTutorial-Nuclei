using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class WeaponsMenu : GenericMenu<WeaponsService>
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
        GravityGunMenu();
        BlackHoleMenu();
        VehicleGunMenu();
        LevitationGun();
        TeleportGun();
    }

    private void GravityGunMenu()
    {
        var gravityGunMenu = new GravityGunMenu(MenuTitle.GravityGun);
        AddMenu(gravityGunMenu);
    }

    private void BlackHoleMenu()
    {
        var blackHoleMenu = new BlackHoleMenu(MenuTitle.BlackHoleGun);
        AddMenu(blackHoleMenu);
    }

    private void VehicleGunMenu()
    {
        var vehicleGunMenu = new VehicleGunMenu(MenuTitle.VehicleGun);
        AddMenu(vehicleGunMenu);
    }

    private void TeleportGun()
    {
        var checkBoxTeleportGun = AddCheckbox(WeaponItemTitle.TeleportGun, () => Service.TeleportGun, Service,
            @checked => { Service.TeleportGun = @checked; });
    }

    private void LevitationGun()
    {
        var checkBoxLevitationGun = AddCheckbox(WeaponItemTitle.LevitationGun, () => Service.LevitationGun, Service,
            @checked => { Service.LevitationGun = @checked; });
    }

    private void ExplosiveBullets()
    {
        var checkBoxExplosiveBullets = AddCheckbox(WeaponItemTitle.ExplosiveBullets, () => Service.ExplosiveBullets,
            Service, @checked => { Service.ExplosiveBullets = @checked; });
    }

    private void NoReload()
    {
        var checkBoxNoReload = AddCheckbox(WeaponItemTitle.NoReload, () => Service.NoReload, Service,
            @checked => { Service.NoReload = @checked; });
    }

    private void InfiniteAmmo()
    {
        var checkBoxInfiniteAmmo = AddCheckbox(WeaponItemTitle.InfiniteAmmo, () => Service.InfiniteAmmo, Service,
            @checked => { Service.InfiniteAmmo = @checked; });
    }

    private void FireBullets()
    {
        var checkBoxFireBullets = AddCheckbox(WeaponItemTitle.FireBullets, () => Service.FireBullets, Service,
            @checked => { Service.FireBullets = @checked; });
    }

    private void GiveAllWeapons()
    {
        var itemGiveAllWeapons = AddItem(WeaponItemTitle.GiveAllWeapons, () => { Service.RequestAllWeapons(); });
    }

    private void WeaponComponentsMenu()
    {
        var weaponComponents = new WeaponComponentsMenu(MenuTitle.WeaponComponents);
        AddMenu(weaponComponents);
    }
}