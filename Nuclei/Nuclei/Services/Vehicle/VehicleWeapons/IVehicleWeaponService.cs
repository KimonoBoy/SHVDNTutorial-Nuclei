using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Vehicle.VehicleWeapons;

public interface IVehicleWeaponService
{
    // Properties
    BindableProperty<bool> HasVehicleWeapons { get; }
    BindableProperty<VehicleWeaponAttachmentPoint> VehicleWeaponAttachment { get; }
    BindableProperty<uint> VehicleWeapon { get; }
    BindableProperty<int> FireRate { get; }
    BindableProperty<bool> PointAndShoot { get; }

    // Events

    // Methods
}