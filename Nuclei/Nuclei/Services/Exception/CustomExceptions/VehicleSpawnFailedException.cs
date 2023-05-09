using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class VehicleSpawnFailedException : CustomExceptionBase
{
    public VehicleSpawnFailedException() : base(ExceptionType.SpawnVehicle, "Failed to spawn the vehicle object.")
    {
    }

    public VehicleSpawnFailedException(string message) : base(ExceptionType.SpawnVehicle, message)
    {
    }
}