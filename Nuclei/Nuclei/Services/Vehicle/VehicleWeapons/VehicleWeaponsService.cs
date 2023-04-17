﻿using GTA;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleWeapons;

public class VehicleWeaponsService : GenericService<VehicleWeaponsService>
{
    public BindableProperty<bool> HasVehicleWeapons { get; set; } = new();
    public BindableProperty<int> NumWeapons { get; set; } = new();
    public BindableProperty<uint> VehicleWeapon { get; set; } = new((uint)VehicleWeaponHash.PlayerBullet);
    public BindableProperty<int> FireRate { get; set; } = new();
}