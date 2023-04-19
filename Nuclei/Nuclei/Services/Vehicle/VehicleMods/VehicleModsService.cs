using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleMods;

public class VehicleModsService : GenericService<VehicleModsService>
{
    public BindableProperty<bool> IsModKitInstalled { get; set; } = new();
}