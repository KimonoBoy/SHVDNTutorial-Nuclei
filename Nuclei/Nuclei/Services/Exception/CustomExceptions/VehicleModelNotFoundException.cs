using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class VehicleModelNotFoundException : CustomExceptionBase
{
    public VehicleModelNotFoundException() : base(ExceptionType.SpawnVehicle, "Vehicle model not found.")
    {
    }

    public VehicleModelNotFoundException(string message) : base(ExceptionType.SpawnVehicle, message)
    {
    }
}