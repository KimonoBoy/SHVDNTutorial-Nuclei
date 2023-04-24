using System.Collections.ObjectModel;
using System.Drawing;
using GTA;
using Nuclei.Services.Observable;

namespace Nuclei.Services.Vehicle.VehicleSpawner;

public class CustomVehicleDto : ObservableService
{
    private bool _customTires;

    private string _licensePlate;

    private LicensePlateStyle _licensePlateStyle;

    private VehicleColor _rimColor;

    private Color _tireSmokeColor = Color.Transparent;
    private string _title;

    private VehicleHash _vehicleHash;

    private ObservableCollection<CustomVehicleModDto> _vehicleMods = new();

    private VehicleWheelType _wheelType;

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value) return;
            _title = value;
            OnPropertyChanged();
        }
    }

    public VehicleHash VehicleHash
    {
        get => _vehicleHash;
        set
        {
            if (_vehicleHash == value) return;
            _vehicleHash = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<CustomVehicleModDto> VehicleMods
    {
        get => _vehicleMods;
        set
        {
            if (_vehicleMods == value) return;
            _vehicleMods = value;
            OnPropertyChanged();
        }
    }

    public string LicensePlate
    {
        get => _licensePlate;
        set
        {
            if (_licensePlate == value) return;
            _licensePlate = value;
            OnPropertyChanged();
        }
    }

    public LicensePlateStyle LicensePlateStyle
    {
        get => _licensePlateStyle;
        set
        {
            if (_licensePlateStyle == value) return;
            _licensePlateStyle = value;
            OnPropertyChanged();
        }
    }

    public VehicleWheelType WheelType
    {
        get => _wheelType;
        set
        {
            if (_wheelType == value) return;
            _wheelType = value;
            OnPropertyChanged();
        }
    }

    public VehicleColor RimColor
    {
        get => _rimColor;
        set
        {
            if (_rimColor == value) return;
            _rimColor = value;
            OnPropertyChanged();
        }
    }

    public bool CustomTires
    {
        get => _customTires;
        set
        {
            if (_customTires == value) return;
            _customTires = value;
            OnPropertyChanged();
        }
    }

    public Color TireSmokeColor
    {
        get => _tireSmokeColor;
        set
        {
            if (_tireSmokeColor == value) return;
            _tireSmokeColor = value;
            OnPropertyChanged();
        }
    }
}

public class CustomVehicleModDto : ObservableService
{
    private int _modIndex;
    private VehicleModType _vehicleModType;

    public CustomVehicleModDto(VehicleModType vehicleModType, int modIndex)
    {
        VehicleModType = vehicleModType;
        ModIndex = modIndex;
    }

    public VehicleModType VehicleModType
    {
        get => _vehicleModType;
        set
        {
            if (_vehicleModType == value) return;
            _vehicleModType = value;
            OnPropertyChanged();
        }
    }

    public int ModIndex
    {
        get => _modIndex;
        set
        {
            if (_modIndex == value) return;
            _modIndex = value;
            OnPropertyChanged();
        }
    }
}