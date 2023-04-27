using System;
using System.Collections.Generic;
using System.Linq;
using GTA;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsBumpersMenu : VehicleModsMenuBase
{
    public VehicleModsBumpersMenu(Enum @enum) : base(@enum)
    {
    }

    protected override IEnumerable<VehicleMod> GetValidMods()
    {
        return Service.VehicleMods.Where(x =>
            x.Type is VehicleModType.FrontBumper or VehicleModType.RearBumper);
    }

    protected override void PreModTypeMods()
    {
    }

    protected override void PostModTypeMods()
    {
    }
}