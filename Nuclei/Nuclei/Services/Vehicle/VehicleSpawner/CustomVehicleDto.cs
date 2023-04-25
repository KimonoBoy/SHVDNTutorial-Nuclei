using System.Collections.ObjectModel;
using System.Drawing;
using GTA;
using Nuclei.Enums.Vehicle;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class CustomVehicleDto
{
    public string Title { get; set; }

    public VehicleHash VehicleHash { get; set; }

    public ObservableCollection<CustomVehicleModDto> VehicleMods { get; set; } = new();

    public string LicensePlate { get; set; }

    public LicensePlateStyle LicensePlateStyle { get; set; }

    public VehicleWheelType WheelType { get; set; }

    public VehicleColor RimColor { get; set; }

    public bool CustomTires { get; set; }

    public Color TireSmokeColor { get; set; } = Color.Transparent;

    public VehicleWindowTint WindowTint { get; set; }

    public bool XenonHeadLights { get; set; }

    public NeonLightsLayout NeonLightsLayout { get; set; }
}

public class CustomVehicleModDto
{
    public CustomVehicleModDto(VehicleModType vehicleModType, int modIndex)
    {
        VehicleModType = vehicleModType;
        ModIndex = modIndex;
    }

    public VehicleModType VehicleModType { get; set; }

    public int ModIndex { get; set; }
}