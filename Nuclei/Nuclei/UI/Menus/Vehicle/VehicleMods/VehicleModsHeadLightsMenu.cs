using System;
using System.Collections.Generic;
using GTA;
using Nuclei.Enums.Vehicle;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsHeadLightsMenu : VehicleModsMenuBase
{
    public VehicleModsHeadLightsMenu(Enum @enum) : base(@enum)
    {
    }

    protected override IEnumerable<VehicleMod> GetValidMods()
    {
        return new List<VehicleMod>();
    }

    protected override void PostModTypeMods()
    {
        XenonHeadlights();
    }

    private void XenonHeadlights()
    {
        var checkBoxXenonHeadLights = AddCheckbox(VehicleModsItemTitles.XenonHeadLights, () => Service.XenonHeadLights,
            Service,
            @checked => { Service.XenonHeadLights = @checked; });
    }
}