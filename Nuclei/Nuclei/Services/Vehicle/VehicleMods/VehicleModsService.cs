using System.Collections.Generic;
using GTA;
using Newtonsoft.Json;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleMods;

public class VehicleModsService : GenericService<VehicleModsService>
{
    public BindableProperty<bool> IsModKitInstalled { get; set; } = new();

    [JsonIgnore]
    public BindableProperty<List<VehicleModType>> ValidVehicleModTypes { get; set; } =
        new(new List<VehicleModType>());
}