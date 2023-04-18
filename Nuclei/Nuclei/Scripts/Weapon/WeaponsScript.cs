using System;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;

namespace Nuclei.Scripts.Weapon;

public class WeaponsScript : GenericScriptBase<WeaponsService>
{
    protected override void SubscribeToEvents()
    {
        Service.AllWeaponsRequested += OnAllWeaponsRequested;
    }

    private void OnAllWeaponsRequested(object sender, EventArgs e)
    {
        GiveAllWeapons();
    }

    private void GiveAllWeapons()
    {
        foreach (WeaponHash weaponHash in Enum.GetValues(typeof(WeaponHash))) GiveWeapon(weaponHash);
    }

    private void GiveWeapon(WeaponHash weaponHash)
    {
        var weapon = Character.Weapons.Give(weaponHash, 0, false, true);
        Character.Weapons[weaponHash].Ammo = weapon.MaxAmmo;
    }
}