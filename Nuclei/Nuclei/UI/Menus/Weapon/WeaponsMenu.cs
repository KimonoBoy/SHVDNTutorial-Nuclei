using System;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
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
        VehicleGun();
    }

    private void VehicleGun()
    {
        var listItemVehicleGun = AddListItem(WeaponItemTitle.VehicleGun, () => (int)Service.CurrentVehicleGun, Service,
            (value, index) => { Service.CurrentVehicleGun = (VehicleHash)index; },
            typeof(VehicleHash).ToDisplayNameArray());
    }

    private void TeleportGun()
    {
        var checkBoxTeleportGun = AddCheckbox(WeaponItemTitle.TeleportGun, () => Service.TeleportGun, Service,
            @checked => { Service.TeleportGun = @checked; });
    }

    private void GravityGun()
    {
        var checkBoxGravityGun = AddCheckbox(WeaponItemTitle.GravityGun, () => Service.GravityGun, Service,
            @checked => { Service.GravityGun = @checked; });
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