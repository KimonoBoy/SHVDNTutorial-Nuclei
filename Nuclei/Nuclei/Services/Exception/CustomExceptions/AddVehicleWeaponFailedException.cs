using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class AddVehicleWeaponFailedException : CustomExceptionBase
{
    public AddVehicleWeaponFailedException() : base(ExceptionType.VehicleWeapon, "Failed to add vehicle weapon.")
    {
    }

    public AddVehicleWeaponFailedException(string message) : base(ExceptionType.VehicleWeapon, message)
    {
    }
}