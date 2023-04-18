using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;
using LemonUI.Elements;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
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
        Width = 550;

        ExcludeWeapons();
        VehicleWeapons();
        WeaponAttachmentPoints();
        AdjustFireRate();
        PointAndShoot();

        AddHeader("Vehicle Weapons");
        GenerateVehicleWeaponsItems();
        GeneratePlayerWeaponsItems();
    }

    private void PointAndShoot()
    {
        var checkBoxPointAndShoot = AddCheckbox(VehicleWeaponsItemTitles.PointAndShoot, Service.PointAndShoot,
            @checked => { Service.PointAndShoot.Value = @checked; });
    }

    private void AdjustFireRate()
    {
        var sliderItemFireRate = AddSliderItem(VehicleWeaponsItemTitles.FireRate, Service.FireRate,
            value => { Service.FireRate.Value = value; }, 0, 20);

        sliderItemFireRate.Description =
            $"Time Between Shots:\n\n0: Every frame.\n\nCurrent Value: {Service.FireRate.Value * 25}ms.\n\nMax: {500}ms\n\n~r~Note: Projectiles that stays alive long (e.g. Snowball, Grenades, etc) often work better with a higher timer.";

        Service.FireRate.ValueChanged += (sender, args) =>
        {
            sliderItemFireRate.Description =
                $"Time Between Shots:\n\n0: Every frame.\n\nCurrent Value: {Service.FireRate.Value * 25}ms.\n\nMax: {500}ms\n\n~r~Note: Projectiles that stays alive long (e.g. Snowball and Grenades) often work better with a higher timer.";
        };
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
            AddHeaderIfNone(weaponHash);
            AddItemForWeaponHash(weaponHash);
        }
    }

    private IEnumerable<WeaponHash> GetFilteredWeaponHashes()
    {
        return Enum.GetValues(typeof(WeaponHash))
            .Cast<WeaponHash>()
            .Where(weaponHash => !_excludeHashes.Contains((uint)weaponHash))
            .OrderBy(weaponHash => Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponHash));
    }

    private void AddHeaderIfNone(WeaponHash weaponHash)
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
            $"Set Weapon: {weaponDisplayName}",
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
        var vehicleWeaponDisplayName = vehicleWeaponHash.GetLocalizedDisplayNameFromHash();

        var itemVehicleWeapon = AddItem(vehicleWeaponDisplayName,
            $"Set Weapon: {vehicleWeaponDisplayName}",
            () => { Service.VehicleWeapon.Value = (uint)vehicleWeaponHash; });

        Service.VehicleWeapon.ValueChanged += (sender, args) =>
        {
            itemVehicleWeapon.RightBadge = args.Value == (uint)vehicleWeaponHash
                ? _starTexture
                : null;
        };
    }

    private void WeaponAttachmentPoints()
    {
        var attachmentPointsDisplayNames = Enum.GetValues(typeof(VehicleWeaponAttachmentPoint))
            .Cast<VehicleWeaponAttachmentPoint>()
            .Select(attachmentPoint => attachmentPoint.GetLocalizedDisplayNameFromHash())
            .ToArray();

        var listItemWeaponAttachmentPoints = AddListItem(VehicleWeaponsItemTitles.WeaponAttachmentPoints,
            (selected, index) =>
            {
                Service.VehicleWeaponAttachment.Value = selected.GetHashFromDisplayName<VehicleWeaponAttachmentPoint>();
            }, null,
            attachmentPointsDisplayNames);

        Service.VehicleWeaponAttachment.ValueChanged += (sender, args) =>
        {
            listItemWeaponAttachmentPoints.SelectedItem = args.Value.GetLocalizedDisplayNameFromHash();
        };
    }
}