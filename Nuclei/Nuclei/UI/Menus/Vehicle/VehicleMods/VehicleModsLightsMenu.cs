using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.UI.Menus.Base.ItemFactory;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsLightsMenu : VehicleModsMenuBase
{
    public VehicleModsLightsMenu(Enum @enum) : base(@enum)
    {
    }

    protected override IEnumerable<VehicleMod> GetValidMods()
    {
        return new List<VehicleMod>();
    }

    protected override void PostModTypeMods()
    {
        XenonHeadlights();
        NeonLightsLayout();
        NeonLightsColor();
    }

    private void NeonLightsColor()
    {
        var listItemNeonLightsColor = AddListItem(VehicleModsItemTitle.NeonLightsColor,
            () => (int)Service.NeonLightsColor, Service,
            (value, index) => { Service.NeonLightsColor = (NeonLightsColor)index; },
            Enumerable.Range(0, Enum.GetValues(typeof(NeonLightsColor)).Length).Select(index =>
            {
                var count = Enum.GetValues(typeof(NeonLightsColor)).Length;
                if (index == -1) return $"None {0} / {count - 1}";
                var localizedName = ((NeonLightsColor)index).GetLocalizedDisplayNameFromHash();
                if (index == count)
                    localizedName += $" {0} / {count - 1}";
                else
                    localizedName += $" {index} / {count - 1}";
                return localizedName;
            }).ToArray());
        listItemNeonLightsColor.SetSelectedIndexSafe((int)Service.NeonLightsColor);
    }

    private void NeonLightsLayout()
    {
        var listItemNeonLightsLayout = AddListItem(VehicleModsItemTitle.NeonLightsLayout,
            () => (int)Service.NeonLightsLayout, Service,
            (value, index) => { Service.NeonLightsLayout = (NeonLightsLayout)index; },
            Enumerable.Range(0, Enum.GetValues(typeof(NeonLightsLayout)).Length).Select(
                index =>
                {
                    var count = Enum.GetValues(typeof(NeonLightsLayout)).Length;
                    if (index == -1) return $"None {0} / {count - 1}";
                    var localizedName = ((NeonLightsLayout)index).GetLocalizedDisplayNameFromHash();
                    if (index == count)
                        localizedName += $" {0} / {count - 1}";
                    else
                        localizedName += $" {index} / {count - 1}";
                    return localizedName;
                }).ToArray());
        listItemNeonLightsLayout.SetSelectedIndexSafe((int)Service.NeonLightsLayout);
    }

    private void XenonHeadlights()
    {
        var checkBoxXenonHeadLights = AddCheckbox(VehicleModsItemTitle.XenonHeadLights, () => Service.XenonHeadLights,
            Service,
            @checked => { Service.XenonHeadLights = @checked; });
    }
}