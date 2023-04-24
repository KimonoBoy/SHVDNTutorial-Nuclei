using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using GTA;
using Newtonsoft.Json;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleMods;

public class VehicleModsService : GenericService<VehicleModsService>
{
    private bool _currentCustomTires;

    private VehicleColor _currentRimColor;

    private Color _currentTireSmokeColor;
    private VehicleWheelType _currentWheelType;
    private string _licensePlate;
    private LicensePlateStyle _licensePlateStyle;

    private ObservableCollection<VehicleModType> _validVehicleModTypes = new();
    private ObservableCollection<VehicleWheelType> _validWheelTypes = new();

    [JsonIgnore]
    public Dictionary<string, Color> TireSmokeColorDictionary { get; set; } = new()
    {
        { "White", Color.FromArgb(255, 255, 255) },
        { "Black", Color.FromArgb(20, 20, 20) },
        { "Blue", Color.FromArgb(0, 174, 239) },
        { "Yellow", Color.FromArgb(252, 238, 0) },
        { "Purple", Color.FromArgb(132, 102, 226) },
        { "Orange", Color.FromArgb(255, 127, 0) },
        { "Green", Color.FromArgb(114, 204, 114) },
        { "Red", Color.FromArgb(226, 6, 6) },
        { "Pink", Color.FromArgb(203, 54, 148) },
        { "Patriot", Color.FromArgb(0, 0, 0) }
    };

    [JsonIgnore]
    public ObservableCollection<VehicleWheelType> ValidWheelTypes
    {
        get => _validWheelTypes;
        set
        {
            if (Equals(value, _validWheelTypes)) return;
            _validWheelTypes = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public ObservableCollection<VehicleModType> ValidVehicleModTypes
    {
        get => _validVehicleModTypes;
        set
        {
            if (Equals(value, _validVehicleModTypes)) return;
            _validVehicleModTypes = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public LicensePlateStyle LicensePlateStyle
    {
        get => _licensePlateStyle;
        set
        {
            if (value == _licensePlateStyle) return;
            _licensePlateStyle = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public string LicensePlate
    {
        get => _licensePlate;
        set
        {
            if (value == _licensePlate) return;
            _licensePlate = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public VehicleWheelType CurrentWheelType
    {
        get => _currentWheelType;
        set
        {
            if (value == _currentWheelType) return;
            _currentWheelType = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public VehicleColor CurrentRimColor
    {
        get => _currentRimColor;
        set
        {
            if (value == _currentRimColor) return;
            _currentRimColor = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public bool CurrentCustomTires
    {
        get => _currentCustomTires;
        set
        {
            if (value == _currentCustomTires) return;
            _currentCustomTires = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public Color CurrentTireSmokeColor
    {
        get => _currentTireSmokeColor;
        set
        {
            if (value.Equals(_currentTireSmokeColor)) return;
            _currentTireSmokeColor = value;
            OnPropertyChanged();
        }
    }

    public event EventHandler LicensePlateInputRequested;

    public void RequestLicensePlateInput()
    {
        LicensePlateInputRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<ObservableCollection<VehicleModType>> RandomizeModsRequested;

    public void RequestRandomizeMods(ObservableCollection<VehicleModType> vehicleModTypes)
    {
        RandomizeModsRequested?.Invoke(this, vehicleModTypes);
    }
}