using System;
using System.Collections.ObjectModel;
using System.Drawing;
using GTA;
using Newtonsoft.Json;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleMods;

public class VehicleModsService : GenericService<VehicleModsService>, IVehicleModsService
{
    [JsonIgnore]
    public BindableProperty<ObservableCollection<VehicleWheelType>> ValidWheelTypes { get; set; } =
        new(new ObservableCollection<VehicleWheelType>());

    [JsonIgnore]
    public BindableProperty<ObservableCollection<VehicleModType>> ValidVehicleModTypes { get; set; } =
        new(new ObservableCollection<VehicleModType>());

    [JsonIgnore] public BindableProperty<LicensePlateStyle> LicensePlateStyle { get; set; } = new();

    [JsonIgnore] public BindableProperty<string> LicensePlate { get; set; } = new("");

    [JsonIgnore] public BindableProperty<VehicleWheelType> CurrentWheelType { get; set; } = new();
    [JsonIgnore] public BindableProperty<VehicleColor> CurrentRimColor { get; set; } = new();
    [JsonIgnore] public BindableProperty<bool> CurrentCustomTires { get; set; } = new();
    [JsonIgnore] public BindableProperty<Color> CurrentTireSmokeColor { get; set; } = new(Color.Transparent);

    public event EventHandler LicensePlateInputRequested;

    public void RequestLicensePlateInput()
    {
        LicensePlateInputRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<ObservableCollection<VehicleModType>> RandomizeModsRequested;

    public void RequestRandomizeMods(ObservableCollection<VehicleModType> vehicleModTypes)
    {
        RandomizeModsRequested?.Invoke(this, vehicleModTypes);
    }
}