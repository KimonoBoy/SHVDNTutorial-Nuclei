using System;
using GTA.UI;
using Nuclei.Enums.UI;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class WeaponComponentsMenu : GenericMenuBase<WeaponComponentsService>
{
    public WeaponComponentsMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Notification.Show($"{Service.CurrentWeapon == null}");
        if (Service.CurrentWeapon == null)
        {
            NavigateToMenu(MenuTitle.Weapons);
            return;
        }

        GenerateValidWeaponComponents();
    }

    private void GenerateValidWeaponComponents()
    {
        Clear();
        /*
         * Getting ready for weapon components.
         */
    }
}