using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;
using LemonUI.Elements;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleWeapons;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleWeapons;

public class VehicleWeaponsMenu : GenericMenuBase<VehicleWeaponsService>
{
    private readonly List<uint> _excludeHashes = new();
    private readonly ScaledTexture _starTexture = new("commonmenu", "shop_new_star");

    public VehicleWeaponsMenu(Enum @enum) : base(@enum)
    {
        ExcludeWeapons();
        VehicleWeapons();
        SelectNumAttachments();

        AddHeader("Vehicle Weapons");
        GenerateVehicleWeaponsItems();
        GeneratePlayerWeaponsItems();
    }

    private void ExcludeWeapons()
    {
        AddStandardExcludedHashes();
        AddExcludedWeaponHashesFromGroups();
    }

    private void VehicleWeapons()
    {
        var checkBoxVehicleWeapons = AddCheckbox(VehicleWeaponsItemTitles.VehicleWeapons, Service.HasVehicleWeapons,
            @checked => { Service.HasVehicleWeapons.Value = @checked; });
    }

    private void AddStandardExcludedHashes()
    {
        _excludeHashes.AddRange(new List<uint>
        {
            (uint)VehicleWeaponHash.Invalid,
            (uint)VehicleWeaponHash.Dune,
            (uint)VehicleWeaponHash.Stromberg,
            (uint)VehicleWeaponHash.PlayerBuzzard,
            (uint)VehicleWeaponHash.WaterCannon,
            (uint)VehicleWeaponHash.ValkyrieTurret,
            (uint)VehicleWeaponHash.Rotors,
            (uint)VehicleWeaponHash.Radar,
            (uint)VehicleWeaponHash.SearchLight,
            (uint)WeaponHash.UpNAtomizer,
            (uint)WeaponHash.MetalDetector,
            (uint)WeaponHash.Parachute,
            (uint)WeaponHash.CompactEMPLauncher
        });
    }

    private void AddExcludedWeaponHashesFromGroups()
    {
        foreach (WeaponHash weaponHash in Enum.GetValues(typeof(WeaponHash)))
        {
            var weaponGroupHash = Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponHash);
            if (weaponGroupHash is (uint)WeaponGroup.Melee or (uint)WeaponGroup.Unarmed
                or (uint)WeaponGroup.PetrolCan
                or (uint)WeaponGroup.DigiScanner or (uint)WeaponGroup.NightVision or (uint)WeaponGroup.Stungun
                or (uint)WeaponGroup.FireExtinguisher)
                _excludeHashes.Add((uint)weaponHash);
        }
    }

    private void GeneratePlayerWeaponsItems()
    {
        foreach (var weaponHash in GetFilteredWeaponHashes())
        {
            AddHeaderIfNotExists(weaponHash);
            AddItemForWeaponHash(weaponHash);
        }
    }

    private IEnumerable<WeaponHash> GetFilteredWeaponHashes()
    {
        return Enum.GetValues(typeof(WeaponHash))
            .Cast<WeaponHash>()
            .Where(hash => !_excludeHashes.Contains((uint)hash))
            .OrderBy(h => Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, h));
    }

    private void AddHeaderIfNotExists(WeaponHash weaponHash)
    {
        var groupDisplayName = GetWeaponGroupDisplayName(weaponHash);
        if (!Items.Exists(header => header.Title == groupDisplayName))
            AddHeader(groupDisplayName);
    }

    private string GetWeaponGroupDisplayName(WeaponHash weaponHash)
    {
        return ((WeaponGroup)Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponHash))
            .GetLocalizedDisplayNameFromHash();
    }

    private void AddItemForWeaponHash(WeaponHash weaponHash)
    {
        var weaponDisplayName = weaponHash.GetLocalizedDisplayNameFromHash();
        var itemPlayerWeapon = AddItem(weaponDisplayName,
            $"Attach: {weaponDisplayName}",
            () => { Service.VehicleWeapon.Value = (uint)weaponHash; });

        Service.VehicleWeapon.ValueChanged += (sender, args) =>
        {
            itemPlayerWeapon.RightBadge = args.Value == (uint)weaponHash
                ? _starTexture
                : null;
        };
    }

    private void GenerateVehicleWeaponsItems()
    {
        foreach (var vehicleWeaponHash in GetFilteredVehicleWeaponHashes())
            AddItemForVehicleWeaponHash(vehicleWeaponHash);
    }

    private IEnumerable<VehicleWeaponHash> GetFilteredVehicleWeaponHashes()
    {
        return Enum.GetValues(typeof(VehicleWeaponHash))
            .Cast<VehicleWeaponHash>()
            .Where(hash => !_excludeHashes.Contains((uint)hash));
    }

    private void AddItemForVehicleWeaponHash(VehicleWeaponHash vehicleWeaponHash)
    {
        var itemVehicleWeapon = AddItem(vehicleWeaponHash.GetLocalizedDisplayNameFromHash(),
            $"Attach: {vehicleWeaponHash.GetLocalizedDisplayNameFromHash()}",
            () => { Service.VehicleWeapon.Value = (uint)vehicleWeaponHash; });

        Service.VehicleWeapon.ValueChanged += (sender, args) =>
        {
            itemVehicleWeapon.RightBadge = args.Value == (uint)vehicleWeaponHash
                ? _starTexture
                : null;
        };
    }

    private void SelectNumAttachments()
    {
        var listItemNumAttachments = AddListItem(VehicleWeaponsItemTitles.SelectNumAttachments,
            (selected, index) => { }, null, 1, 2, 3);

        Service.NumAttachments.ValueChanged += (sender, args) => { listItemNumAttachments.SelectedItem = args.Value; };
    }
}