using System;
using System.Collections.Generic;
using System.Linq;
using GTA;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsWheelsMenu : VehicleModsMenuBase
{
    public VehicleModsWheelsMenu(Enum @enum) : base(@enum)
    {
    }

    protected override void UpdateMenuItems()
    {
        Clear();
        base.UpdateMenuItems();
    }

    protected override IEnumerable<VehicleModType> GetValidModTypes()
    {
        return Service.ValidVehicleModTypes.Value.Where(modType =>
            modType is VehicleModType.FrontWheel or VehicleModType.RearWheel);
    }
}