using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class VehicleWeaponNotFoundException : CustomExceptionBase
{
    public VehicleWeaponNotFoundException() : base(ExceptionType.VehicleWeapon, "Vehicle weapon not found.")
    {
    }

    public VehicleWeaponNotFoundException(string message) : base(ExceptionType.VehicleWeapon, message)
    {
    }
}