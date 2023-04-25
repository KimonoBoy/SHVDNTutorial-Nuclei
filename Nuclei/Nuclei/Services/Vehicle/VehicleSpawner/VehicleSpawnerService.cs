using System;
using System.Collections.ObjectModel;
using GTA;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class VehicleSpawnerService : GenericService<VehicleSpawnerService>
{
    private VehicleHash _currentVehicleHash;

    private ObservableCollection<CustomVehicleDto> _customVehicles = new();

    private bool _enginesRunning;

    private ObservableCollection<VehicleHash> _favoriteVehicles = new();

    private VehicleSeat _vehicleSeat = VehicleSeat.Driver;

    private bool _warpInSpawned;

    public VehicleHash CurrentVehicleHash
    {
        get => _currentVehicleHash;
        set
        {
            if (_currentVehicleHash == value) return;
            _currentVehicleHash = value;
            OnPropertyChanged(nameof(_currentVehicleHash));
        }
    }

    public bool EnginesRunning
    {
        get => _enginesRunning;
        set
        {
            if (_enginesRunning == value) return;
            _enginesRunning = value;
            OnPropertyChanged(nameof(_enginesRunning));
        }
    }

    public bool WarpInSpawned
    {
        get => _warpInSpawned;
        set
        {
            if (_warpInSpawned == value) return;
            _warpInSpawned = value;
            OnPropertyChanged(nameof(_warpInSpawned));
        }
    }

    public VehicleSeat VehicleSeat
    {
        get => _vehicleSeat;
        set
        {
            if (_vehicleSeat == value) return;
            _vehicleSeat = value;
            OnPropertyChanged(nameof(_vehicleSeat));
        }
    }

    public ObservableCollection<VehicleHash> FavoriteVehicles
    {
        get => _favoriteVehicles;
        set
        {
            if (_favoriteVehicles == value) return;
            _favoriteVehicles = value;
            OnPropertyChanged(nameof(_favoriteVehicles));
        }
    }

    public ObservableCollection<CustomVehicleDto> CustomVehicles
    {
        get => _customVehicles;
        set
        {
            if (_customVehicles == value) return;
            _customVehicles = value;
            OnPropertyChanged(nameof(_customVehicles));
        }
    }

    public event EventHandler<VehicleHash> VehicleSpawned;

    public void SpawnVehicle(VehicleHash vehicleHash)
    {
        VehicleSpawned?.Invoke(this, vehicleHash);
    }

    public event EventHandler<CustomVehicleDto> CustomVehicleSpawned;

    public void SpawnVehicle(CustomVehicleDto customVehicleDto)
    {
        CustomVehicleSpawned?.Invoke(this, customVehicleDto);
    }
}