﻿using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.UI.Menus.Base.ItemFactory;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsMenu : VehicleModsMenuBase
{
    public VehicleModsMenu(Enum @enum) : base(@enum)
    {
    }

    protected override void PreModTypeMods()
    {
        RandomizeAllMods();
        WheelsMenu();
        BumpersMenu();
        HeadLightsMenu();
        ResprayMenu();
        WindowTint();
    }

    private void ResprayMenu()
    {
        var resprayMenu = new VehicleModsResprayMenu(MenuTitle.Respray);
        AddMenu(resprayMenu);
    }

    private void HeadLightsMenu()
    {
        var lightsMenu = new VehicleModsLightsMenu(MenuTitle.Lights);
        AddMenu(lightsMenu);
    }

    private void WindowTint()
    {
        var listItemTintColor = AddListItem(VehicleModsItemTitle.WindowTint, () => (int)Service.WindowTint, Service,
            (value, index) => { Service.WindowTint = (VehicleWindowTint)index; },
            Enumerable.Range(0, Enum.GetValues(typeof(VehicleWindowTint)).Length - 1).Select(index =>
            {
                var count = Enum.GetValues(typeof(VehicleWindowTint)).Length - 1;
                if (index == -1) return $"None {0} / {count - 1}";
                var localizedName = ((VehicleWindowTint)index).GetLocalizedDisplayNameFromHash();
                if (index == count)
                    localizedName += $" {0} / {count - 1}";
                else
                    localizedName += $" {index} / {count - 1}";
                return localizedName;
            }).ToArray());
        listItemTintColor.SetSelectedIndexSafe((int)Service.WindowTint);
    }

    protected virtual void RandomizeAllMods()
    {
        var itemRandomizeMods = AddItem(VehicleModsItemTitle.RandomizeMods, () =>
        {
            Service.RequestRandomizeMods();
            GenerateMenu();
        });
    }

    protected override void PostModTypeMods()
    {
        LicensePlate();
        LicensePlateStyle();
    }

    private void LicensePlateStyle()
    {
        var listItemLicensePlateStyle = AddListItem(VehicleModsItemTitle.LicensePlateStyle,
            () => (int)Service.LicensePlateStyle, Service,
            (value, index) => { Service.LicensePlateStyle = (LicensePlateStyle)index; },
            Enumerable.Range(0, Enum.GetValues(typeof(LicensePlateStyle)).Length).Select(index =>
            {
                var count = Enum.GetValues(typeof(LicensePlateStyle)).Length;
                if (index == -1) return $"None {0} / {count - 1}";
                var localizedName = ((LicensePlateStyle)index).GetLocalizedDisplayNameFromHash();
                if (index == count)
                    localizedName += $" {0} / {count - 1}";
                else
                    localizedName += $" {index} / {count - 1}";
                return localizedName;
            }).ToArray());
        listItemLicensePlateStyle.SetSelectedIndexSafe((int)Service.LicensePlateStyle);
    }

    private void LicensePlate()
    {
        var itemLicensePlate =
            AddItem(VehicleModsItemTitle.LicensePlate, () => { Service.RequestLicensePlateInput(); },
                Service.LicensePlate);
        Service.LicensePlateInputRequested += (_, _) => { itemLicensePlate.AltTitle = Service.LicensePlate; };
    }

    protected override IEnumerable<VehicleMod> GetValidMods()
    {
        return Service.VehicleMods.Where(vehicleMod =>
            vehicleMod.Type != VehicleModType.FrontWheel && vehicleMod.Type != VehicleModType.RearWheel);
    }

    private void BumpersMenu()
    {
        if (Service.VehicleMods.All(vehicleMod => vehicleMod.Type != VehicleModType.FrontBumper) &&
            Service.VehicleMods.All(vehicleMod => vehicleMod.Type != VehicleModType.RearBumper)) return;
        var bumpersMenu = new VehicleModsBumpersMenu(MenuTitle.Bumpers);
        AddMenu(bumpersMenu);
    }

    private void WheelsMenu()
    {
        var wheelsMenu = new VehicleModsWheelsMenu(MenuTitle.Wheels);
        AddMenu(wheelsMenu);
    }
}