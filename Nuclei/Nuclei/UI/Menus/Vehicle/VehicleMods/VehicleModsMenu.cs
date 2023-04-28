using System;
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
        PrimaryColor();
        SecondaryColor();
        WindowTint();
    }

    private void HeadLightsMenu()
    {
        var headLightsMenu = new VehicleModsHeadLightsMenu(MenuTitles.Headlights);
        AddMenu(headLightsMenu);
    }

    private void WindowTint()
    {
        var listItemTintColor = AddListItem(VehicleModsItemTitles.WindowTint, () => (int)Service.WindowTint, Service,
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

    private void SecondaryColor()
    {
        var listItemSecondaryColor = AddListItem(VehicleModsItemTitles.SecondaryColor,
            () => (int)Service.SecondaryColor, Service,
            (value, index) => { Service.SecondaryColor = (VehicleColor)index; },
            typeof(VehicleColor).ToDisplayNameArray());
        listItemSecondaryColor.SetSelectedIndexSafe((int)Service.SecondaryColor);
    }

    private void PrimaryColor()
    {
        var listItemPrimaryColor = AddListItem(VehicleModsItemTitles.PrimaryColor, () => (int)Service.PrimaryColor,
            Service,
            (value, index) => { Service.PrimaryColor = (VehicleColor)index; },
            typeof(VehicleColor).ToDisplayNameArray());
        listItemPrimaryColor.SetSelectedIndexSafe((int)Service.PrimaryColor);
    }

    protected virtual void RandomizeAllMods()
    {
        var itemRandomizeMods = AddItem(VehicleModsItemTitles.RandomizeMods, () =>
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
        var listItemLicensePlateStyle = AddListItem(VehicleModsItemTitles.LicensePlateStyle,
            () => (int)Service.LicensePlateStyle, Service,
            (value, index) => { Service.LicensePlateStyle = (LicensePlateStyle)index; },
            typeof(LicensePlateStyle).ToDisplayNameArray());
        listItemLicensePlateStyle.SetSelectedIndexSafe((int)Service.LicensePlateStyle);
    }

    private void LicensePlate()
    {
        var itemLicensePlate =
            AddItem(VehicleModsItemTitles.LicensePlate, () => { Service.RequestLicensePlateInput(); },
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
        var bumpersMenu = new VehicleModsBumpersMenu(MenuTitles.Bumpers);
        AddMenu(bumpersMenu);
    }

    private void WheelsMenu()
    {
        var wheelsMenu = new VehicleModsWheelsMenu(MenuTitles.Wheels);
        AddMenu(wheelsMenu);
    }
}