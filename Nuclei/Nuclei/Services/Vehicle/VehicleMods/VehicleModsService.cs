using System;
using System.Collections.Generic;
using GTA;
using Newtonsoft.Json;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleMods;

public class VehicleModsService : GenericService<VehicleModsService>
{
    [JsonIgnore]
    public BindableProperty<List<VehicleModType>> ValidVehicleModTypes { get; set; } =
        new(new List<VehicleModType>());

    [JsonIgnore] public BindableProperty<LicensePlateStyle> LicensePlateStyle { get; set; } = new();

    [JsonIgnore] public BindableProperty<string> LicensePlate { get; set; } = new("");

    [JsonIgnore]
    public BindableProperty<List<VehicleWheelType>> ValidWheelTypes { get; set; } = new(new List<VehicleWheelType>());

    [JsonIgnore] public BindableProperty<VehicleWheelType> CurrentWheelType { get; set; } = new();


    public event EventHandler LicensePlateInputRequested;

    public void RequestLicensePlateInput()
    {
        LicensePlateInputRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<List<VehicleModType>> RandomizeModsRequested;

    public void RequestRandomizeMods(List<VehicleModType> vehicleModTypes)
    {
        RandomizeModsRequested?.Invoke(this, vehicleModTypes);
    }
}