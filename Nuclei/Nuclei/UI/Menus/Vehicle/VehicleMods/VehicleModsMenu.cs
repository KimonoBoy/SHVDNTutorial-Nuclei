using System;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleMods;

public class VehicleModsMenu : GenericMenuBase<VehicleModsService>
{
    public VehicleModsMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
    }

    private void OnShown(object sender, EventArgs e)
    {
        Service.RequestInstallModKit();
    }
}