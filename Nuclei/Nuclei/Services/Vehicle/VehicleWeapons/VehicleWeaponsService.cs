using GTA;
using Nuclei.Enums.Vehicle;
using Nuclei.Helpers.Utilities.BindableProperty;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleWeapons;

public class VehicleWeaponsService : GenericService<VehicleWeaponsService>, IVehicleWeaponService
{
    public BindableProperty<bool> HasVehicleWeapons { get; set; } = new();
    public BindableProperty<VehicleWeaponAttachmentPoint> VehicleWeaponAttachment { get; set; } = new();
    public BindableProperty<uint> VehicleWeapon { get; set; } = new((uint)VehicleWeaponHash.PlayerBullet);
    public BindableProperty<int> FireRate { get; set; } = new();
    public BindableProperty<bool> PointAndShoot { get; set; } = new();
}