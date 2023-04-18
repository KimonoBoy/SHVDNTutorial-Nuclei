using System;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Weapon;

namespace Nuclei.Scripts.Weapon;

public class WeaponsScript : GenericScriptBase<WeaponsService>
{
    protected override void SubscribeToEvents()
    {
        Tick += OnTick;
        Service.AllWeaponsRequested += OnAllWeaponsRequested;
        GameStateTimer.SubscribeToTimerElapsed(UpdateWeapons);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;

        UpdateFeature(Service.FireBullets.Value, ProcessFireBullets);
    }

    private void UpdateWeapons(object sender, EventArgs e)
    {
        if (Character == null) return;
    }

    private void ProcessFireBullets(bool isFireBullets)
    {
        if (!isFireBullets) return;
        if (!Character.IsShooting) return;

        Game.Player.SetFireAmmoThisFrame();
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