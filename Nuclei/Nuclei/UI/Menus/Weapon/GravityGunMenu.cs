using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class GravityGunMenu : GenericMenu<WeaponsService>
{
    public GravityGunMenu(Enum @enum) : base(@enum)
    {
        GravityGun();
        AdjustThrowVelocity();
    }

    private void AdjustThrowVelocity()
    {
        var sliderItemThrowVelocity = AddSliderItem(WeaponItemTitle.ThrowVelocity, () => Service.ThrowVelocity, Service,
            value => { Service.ThrowVelocity = value; }, 10, 20);
    }

    private void GravityGun()
    {
        var checkBoxGravityGun = AddCheckbox(WeaponItemTitle.GravityGun, () => Service.GravityGun, Service,
            @checked => { Service.GravityGun = @checked; });
    }
}