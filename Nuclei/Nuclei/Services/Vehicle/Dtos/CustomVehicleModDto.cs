using GTA;

namespace Nuclei.Services.Vehicle.Dtos;

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