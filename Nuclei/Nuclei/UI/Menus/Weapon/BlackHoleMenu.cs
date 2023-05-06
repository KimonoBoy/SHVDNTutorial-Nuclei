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
            value => { Service.BlackHolePower = value; }, 10, 19);
        sliderItemPower.Description =
            $"Change the Power of the Black Hole.\n\nCurrent: {(sliderItemPower.Value + 1) * 50} - Force Multiplier";
        Service.PropertyChanged += (sender, args) =>
        {
            sliderItemPower.Description =
                $"Change the Power of the Black Hole.\n\nCurrent: {(sliderItemPower.Value + 1) * 50} - Force Multiplier";
        };
    }

    private void Size()
    {
        var sliderItemSize = AddSliderItem(WeaponItemTitle.BlackHoleRadius, () => Service.BlackHoleRadius, Service,
            value => { Service.BlackHoleRadius = value; }, 10, 19);
        sliderItemSize.Description =
            $"Change the Radius of the Black Hole.\n\nCurrent: {(sliderItemSize.Value + 1) * 15.0f} - Units";
        Service.PropertyChanged += (sender, args) =>
        {
            sliderItemSize.Description =
                $"Change the Radius of the Black Hole.\n\nCurrent: {(sliderItemSize.Value + 1) * 15.0f} - Units";
        };
    }

    private void LifeSpan()
    {
        var sliderItemLifeSpan = AddSliderItem(WeaponItemTitle.LifeSpan, () => Service.BlackHoleLifeSpan, Service,
            value => { Service.BlackHoleLifeSpan = value; }, 10, 19);
        sliderItemLifeSpan.Description =
            $"Change the Life Span of the Black Hole.\n\nCurrent: {sliderItemLifeSpan.Value + 1} - Seconds";
        Service.PropertyChanged += (sender, args) =>
        {
            sliderItemLifeSpan.Description =
                $"Change the Life Span of the Black Hole.\n\nCurrent: {sliderItemLifeSpan.Value + 1} - Seconds";
        };
    }

    private void BlackHoleGun()
    {
        var checkBoxBlackHoleGun = AddCheckbox(WeaponItemTitle.BlackHoleGun, () => Service.BlackHoleGun, Service,
            @checked => { Service.BlackHoleGun = @checked; });
    }
}