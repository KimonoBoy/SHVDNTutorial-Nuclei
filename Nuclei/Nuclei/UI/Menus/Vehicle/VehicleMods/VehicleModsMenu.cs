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
        ResprayMenu();
        InteriorMenu();
        TurboCharged();
        WindowTint();
    }

    private void InteriorMenu()
    {
        if (Service.VehicleMods.Count(vehicleMod => vehicleMod.Type is VehicleModType.Dashboard
                or VehicleModType.DialDesign or VehicleModType.DoorSpeakers
                or VehicleModType.Ornaments or VehicleModType.Seats or VehicleModType.Speakers
                or VehicleModType.SteeringWheels) <= 0) return;
        var interiorMenu = new VehicleModsInteriorMenu(MenuTitle.Interior);
        AddMenu(interiorMenu);
    }

    private void TurboCharged()
    {
        var checkBoxTurbo = AddCheckbox(VehicleModsItemTitle.Turbo, () => Service.Turbo, Service,
            @checked => Service.Turbo = @checked);
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
        return Service.VehicleMods.Where(vehicleMod => vehicleMod.Type is not
            (VehicleModType.FrontWheel or VehicleModType.RearWheel or VehicleModType.FrontBumper or
            VehicleModType.RearBumper or VehicleModType.Dashboard or VehicleModType.DialDesign or
            VehicleModType.DoorSpeakers or VehicleModType.Speakers or VehicleModType.Ornaments or
            VehicleModType.Seats or VehicleModType.SteeringWheels));
    }

    private void BumpersMenu()
    {
        if (Service.VehicleMods.Count(vehicleMod => vehicleMod.Type is VehicleModType.FrontBumper
                or VehicleModType.RearBumper) <= 0) return;
        var bumpersMenu = new VehicleModsBumpersMenu(MenuTitle.Bumpers);
        AddMenu(bumpersMenu);
    }

    private void WheelsMenu()
    {
        var wheelsMenu = new VehicleModsWheelsMenu(MenuTitle.Wheels);
        AddMenu(wheelsMenu);
    }
}