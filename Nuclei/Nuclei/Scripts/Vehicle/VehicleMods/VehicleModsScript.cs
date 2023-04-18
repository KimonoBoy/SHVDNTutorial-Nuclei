using Nuclei.Scripts.Generics;
using Nuclei.Services.Vehicle.VehicleMods;

namespace Nuclei.Scripts.Vehicle.VehicleMods;

public class VehicleModsScript : GenericScriptBase<VehicleModsService>
{
    protected override void SubscribeToEvents()
    {
    }

    private void InstallModKit()
    {
        CurrentVehicle.Mods.InstallModKit();
    }
}