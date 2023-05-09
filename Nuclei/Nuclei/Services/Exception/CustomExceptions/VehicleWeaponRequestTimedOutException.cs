using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

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