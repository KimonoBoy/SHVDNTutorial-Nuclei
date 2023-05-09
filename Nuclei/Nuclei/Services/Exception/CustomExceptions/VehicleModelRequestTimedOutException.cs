using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class VehicleModelRequestTimedOutException : CustomExceptionBase
{
    public VehicleModelRequestTimedOutException() : base(ExceptionType.SpawnVehicle,
        "Loading of vehicle model timed out.")
    {
    }

    public VehicleModelRequestTimedOutException(string message) : base(ExceptionType.SpawnVehicle, message)
    {
    }
}