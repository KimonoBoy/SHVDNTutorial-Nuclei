using System;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleMods;

public class VehicleModsService : GenericService<VehicleModsService>
{
    public event EventHandler InstallModKitRequested;

    public void RequestInstallModKit()
    {
        InstallModKitRequested?.Invoke(this, EventArgs.Empty);
    }
}