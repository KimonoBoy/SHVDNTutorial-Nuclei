using System;
using System.Collections.Generic;
using System.Linq;
using GTA;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsInteriorMenu : VehicleModsMenuBase
{
    public VehicleModsInteriorMenu(Enum @enum) : base(@enum)
    {
    }

    protected override IEnumerable<VehicleMod> GetValidMods()
    {
        return Service.VehicleMods.Where(vehicleMod =>
            vehicleMod.Type is VehicleModType.Dashboard or VehicleModType.DialDesign or VehicleModType.DoorSpeakers
                or VehicleModType.Ornaments or VehicleModType.Seats or VehicleModType.Speakers
                or VehicleModType.SteeringWheels);
    }
}