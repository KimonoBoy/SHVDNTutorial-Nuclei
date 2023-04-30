using System.Collections.Generic;
using GTA;
using Nuclei.Enums.Vehicle;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class CustomVehicleDto
{
    public string Title { get; set; }

    public VehicleHash VehicleHash { get; set; }
    public List<CustomVehicleModDto> VehicleMods { get; set; } = new();

    public VehicleWheelType WheelType { get; set; }

    public VehicleColor RimColor { get; set; }

    public TireSmokeColor TireSmokeColor { get; set; }

    public bool CustomTires { get; set; }

    public string LicensePlate { get; set; }

    public LicensePlateStyle LicensePlateStyle { get; set; }

    public VehicleColor PrimaryColor { get; set; }

    public VehicleColor SecondaryColor { get; set; }

    public VehicleColor PearlescentColor { get; set; }

    public VehicleWindowTint WindowTint { get; set; }

    public bool XenonHeadLights { get; set; }

    public NeonLightsLayout NeonLightsLayout { get; set; }

    public NeonLightsColor NeonLightsColor { get; set; }

    public bool Turbo { get; set; }

    public bool RainbowMode { get; set; }
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