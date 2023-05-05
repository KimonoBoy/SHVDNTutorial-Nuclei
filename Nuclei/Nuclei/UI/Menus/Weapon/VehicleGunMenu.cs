using System;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Weapon;

public class VehicleGunMenu : GenericMenu<WeaponsService>
{
    public VehicleGunMenu(Enum @enum) : base(@enum)
    {
    }
}