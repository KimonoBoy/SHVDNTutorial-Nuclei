using System;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class WeaponComponentsMenu : GenericMenu<WeaponComponentsService>
{
    public WeaponComponentsMenu(Enum @enum) : base(@enum)
    {
    }
}