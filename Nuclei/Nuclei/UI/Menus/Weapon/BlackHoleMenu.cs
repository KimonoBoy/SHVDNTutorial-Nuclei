using System;
using Nuclei.Enums.UI;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class BlackHoleMenu : GenericMenu<WeaponsService>
{
    public BlackHoleMenu(Enum @enum) : base(@enum)
    {
        BlackHoleGun();
        Size();
        LifeSpan();
        Power();
    }

    private void Power()
    {
        var sliderItemPower = AddSliderItem(WeaponItemTitle.BlackHolePower, () => Service.BlackHolePower, Service,
            value => { Service.BlackHolePower = value; }, 10, 20);
    }

    private void Size()
    {
        var sliderItemSize = AddSliderItem(WeaponItemTitle.BlackHoleRadius, () => Service.BlackHoleRadius, Service,
            value => { Service.BlackHoleRadius = value; }, 10, 20);
    }

    private void LifeSpan()
    {
        var sliderItemLifeSpan = AddSliderItem(WeaponItemTitle.LifeSpan, () => Service.BlackHoleLifeSpan, Service,
            value => { Service.BlackHoleLifeSpan = value; }, 10, 20);
    }

    private void BlackHoleGun()
    {
        var checkBoxBlackHoleGun = AddCheckbox(WeaponItemTitle.BlackHoleGun, () => Service.BlackHoleGun, Service,
            @checked => { Service.BlackHoleGun = @checked; });
    }
}