using System.Collections.ObjectModel;
using System.Drawing;
using GTA;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class CustomVehicle
{
    public BindableProperty<string> Title { get; set; } = new();
    public BindableProperty<VehicleHash> VehicleHash { get; set; } = new();

    public BindableProperty<ObservableCollection<CustomVehicleMod>> VehicleMods { get; set; } =
        new(new ObservableCollection<CustomVehicleMod>());

    public BindableProperty<string> LicensePlate { get; set; } = new();

    public BindableProperty<LicensePlateStyle> LicensePlateStyle { get; set; } =
        new();

    public BindableProperty<VehicleWheelType> WheelType { get; set; } = new();

    public BindableProperty<VehicleColor> RimColor { get; set; } = new();
    public BindableProperty<bool> CustomTires { get; set; } = new();
    public BindableProperty<Color> TireSmokeColor { get; set; } = new(Color.Transparent);
}

public class CustomVehicleMod
{
    public CustomVehicleMod(VehicleModType vehicleModType, int modIndex)
    {
        VehicleModType.Value = vehicleModType;
        ModIndex.Value = modIndex;
    }

    public BindableProperty<VehicleModType> VehicleModType { get; set; } = new();
    public BindableProperty<int> ModIndex { get; set; } = new();
}