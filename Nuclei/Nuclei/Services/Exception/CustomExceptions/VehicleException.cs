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

public class VehicleWeaponRequestTimedOutException : CustomExceptionBase
{
    public VehicleWeaponRequestTimedOutException() : base(ExceptionType.VehicleWeapon,
        "Loading of vehicle weapon timed out.")
    {
    }

    public VehicleWeaponRequestTimedOutException(string message) : base(ExceptionType.VehicleWeapon, message)
    {
    }
}

public class VehicleWeaponNotFoundException : CustomExceptionBase
{
    public VehicleWeaponNotFoundException() : base(ExceptionType.VehicleWeapon, "Vehicle weapon not found.")
    {
    }

    public VehicleWeaponNotFoundException(string message) : base(ExceptionType.VehicleWeapon, message)
    {
    }
}

public class AddVehicleWeaponFailedException : CustomExceptionBase
{
    public AddVehicleWeaponFailedException() : base(ExceptionType.VehicleWeapon, "Failed to add vehicle weapon.")
    {
    }

    public AddVehicleWeaponFailedException(string message) : base(ExceptionType.VehicleWeapon, message)
    {
    }
}