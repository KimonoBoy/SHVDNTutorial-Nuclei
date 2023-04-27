using GTA;
using Nuclei.Enums.Vehicle;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleWeapons;

public class VehicleWeaponsService : GenericService<VehicleWeaponsService>
{
    private int _fireRate;
    private bool _hasVehicleWeapons;
    private bool _pointAndShoot;
    private uint _vehicleWeapon = (uint)VehicleWeaponHash.PlayerBullet;
    private VehicleWeaponAttachmentPoint _vehicleWeaponAttachment;

    public bool HasVehicleWeapons
    {
        get => _hasVehicleWeapons;
        set
        {
            if (_hasVehicleWeapons == value) return;
            _hasVehicleWeapons = value;
            OnPropertyChanged();
        }
    }

    public VehicleWeaponAttachmentPoint VehicleWeaponAttachment
    {
        get => _vehicleWeaponAttachment;
        set
        {
            if (_vehicleWeaponAttachment == value) return;
            _vehicleWeaponAttachment = value;
            OnPropertyChanged();
        }
    }

    public uint VehicleWeapon
    {
        get => _vehicleWeapon;
        set
        {
            if (_vehicleWeapon == value) return;
            _vehicleWeapon = value;
            OnPropertyChanged();
        }
    }

    public int FireRate
    {
        get => _fireRate;
        set
        {
            if (_fireRate == value) return;
            _fireRate = value;
            OnPropertyChanged();
        }
    }

    public bool PointAndShoot
    {
        get => _pointAndShoot;
        set
        {
            if (_pointAndShoot == value) return;
            _pointAndShoot = value;
            OnPropertyChanged();
        }
    }
}