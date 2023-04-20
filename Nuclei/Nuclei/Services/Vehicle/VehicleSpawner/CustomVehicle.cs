﻿using System.Collections.ObjectModel;
using GTA;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class CustomVehicle
{
    public BindableProperty<string> Title { get; set; } = new();
    public BindableProperty<VehicleHash> VehicleHash { get; set; } = new();

    public BindableProperty<ObservableCollection<CustomVehicleMod>> VehicleMods { get; set; } =
        new(new ObservableCollection<CustomVehicleMod>());

    public BindableProperty<string> LicensePlate { get; set; } = new();
}

public class CustomVehicleMod
{
    public CustomVehicleMod(VehicleModType vehicleModType, int modIndex)
    {
        VehicleModType.Value = vehicleModType;
        ModIndex.Value = modIndex;
    }

    public BindableProperty<VehicleModType> VehicleModType { get; set; } = new();
    public BindableProperty<int> ModIndex { get; set; } = new();
}