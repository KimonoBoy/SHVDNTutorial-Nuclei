using Nuclei.Enums;

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

public class VehicleModelNotFoundException : CustomExceptionBase
{
    public VehicleModelNotFoundException() : base(ExceptionType.SpawnVehicle, "Vehicle model not found.")
    {
    }

    public VehicleModelNotFoundException(string message) : base(ExceptionType.SpawnVehicle, message)
    {
    }
}

public class VehicleSpawnFailedException : CustomExceptionBase
{
    public VehicleSpawnFailedException() : base(ExceptionType.SpawnVehicle, "Failed to spawn the vehicle object.")
    {
    }

    public VehicleSpawnFailedException(string message) : base(ExceptionType.SpawnVehicle, message)
    {
    }
}