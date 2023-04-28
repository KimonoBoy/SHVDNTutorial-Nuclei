using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using Newtonsoft.Json;
using Nuclei.Enums.Vehicle;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Vehicle.VehicleMods;

public class VehicleModsService : GenericService<VehicleModsService>
{
    private bool _customTires;
    private string _licensePlate;
    private LicensePlateStyle _licensePlateStyle;
    private NeonLightsColor _neonLightsColor;
    private NeonLightsLayout _neonLightsLayout;
    private VehicleColor _pearlescentColor;
    private VehicleColor _primaryColor;
    private VehicleColor _rimColor;
    private VehicleColor _secondaryColor;
    private TireSmokeColor _tireSmokeColor;

    private List<VehicleMod> _vehicleMods = new();
    private VehicleWheelType _wheelType;
    private VehicleWindowTint _windowTint;
    private bool _xenonHeadLights;

    [JsonIgnore]
    public List<VehicleMod> VehicleMods
    {
        get => _vehicleMods;
        set
        {
            if (Equals(value, _vehicleMods)) return;
            _vehicleMods = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public VehicleWheelType WheelType
    {
        get => _wheelType;
        set
        {
            if (value == _wheelType) return;
            _wheelType = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public VehicleColor RimColor
    {
        get => _rimColor;
        set
        {
            if (value == _rimColor) return;
            _rimColor = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public Dictionary<TireSmokeColor, Color> TireSmokeColorDictionary { get; } = new()
    {
        { TireSmokeColor.White, Color.FromArgb(255, 255, 255) },
        { TireSmokeColor.Black, Color.FromArgb(20, 20, 20) },
        { TireSmokeColor.Blue, Color.FromArgb(0, 174, 239) },
        { TireSmokeColor.Yellow, Color.FromArgb(252, 238, 0) },
        { TireSmokeColor.Purple, Color.FromArgb(132, 102, 226) },
        { TireSmokeColor.Orange, Color.FromArgb(255, 127, 0) },
        { TireSmokeColor.Green, Color.FromArgb(114, 204, 114) },
        { TireSmokeColor.Red, Color.FromArgb(226, 6, 6) },
        { TireSmokeColor.Pink, Color.FromArgb(203, 54, 148) },
        { TireSmokeColor.Patriot, Color.FromArgb(0, 0, 0) }
    };

    [JsonIgnore]
    public Dictionary<NeonLightsColor, Color> NeonLightsColorDictionary { get; } = new()
    {
        { NeonLightsColor.White, Color.FromArgb(222, 222, 255) },
        { NeonLightsColor.Blue, Color.FromArgb(2, 21, 255) },
        { NeonLightsColor.ElectricBlue, Color.FromArgb(3, 83, 255) },
        { NeonLightsColor.MintGreen, Color.FromArgb(0, 255, 140) },
        { NeonLightsColor.LimeGreen, Color.FromArgb(94, 255, 1) },
        { NeonLightsColor.Yellow, Color.FromArgb(255, 255, 0) },
        { NeonLightsColor.GoldenShower, Color.FromArgb(255, 150, 5) },
        { NeonLightsColor.Orange, Color.FromArgb(255, 62, 0) },
        { NeonLightsColor.Red, Color.FromArgb(255, 1, 1) },
        { NeonLightsColor.PonyPink, Color.FromArgb(255, 50, 100) },
        { NeonLightsColor.HotPink, Color.FromArgb(255, 5, 190) },
        { NeonLightsColor.Purple, Color.FromArgb(35, 1, 255) },
        { NeonLightsColor.Blacklight, Color.FromArgb(15, 3, 255) }
    };

    [JsonIgnore]
    public TireSmokeColor TireSmokeColor
    {
        get => _tireSmokeColor;
        set
        {
            if (value == _tireSmokeColor) return;
            _tireSmokeColor = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public bool CustomTires
    {
        get => _customTires;
        set
        {
            if (value == _customTires) return;
            _customTires = value;
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
    public VehicleColor PrimaryColor
    {
        get => _primaryColor;
        set
        {
            if (value == _primaryColor) return;
            _primaryColor = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public VehicleColor SecondaryColor
    {
        get => _secondaryColor;
        set
        {
            if (value == _secondaryColor) return;
            _secondaryColor = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public VehicleWindowTint WindowTint
    {
        get => _windowTint;
        set
        {
            if (value == _windowTint) return;
            _windowTint = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public bool XenonHeadLights
    {
        get => _xenonHeadLights;
        set
        {
            if (value == _xenonHeadLights) return;
            _xenonHeadLights = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public VehicleColor PearlescentColor
    {
        get => _pearlescentColor;
        set
        {
            if (value == _pearlescentColor) return;
            _pearlescentColor = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public NeonLightsLayout NeonLightsLayout
    {
        get => _neonLightsLayout;
        set
        {
            if (value == _neonLightsLayout) return;
            _neonLightsLayout = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public NeonLightsColor NeonLightsColor
    {
        get => _neonLightsColor;
        set
        {
            if (value == _neonLightsColor) return;
            _neonLightsColor = value;
            OnPropertyChanged();
        }
    }

    public event EventHandler LicensePlateInputRequested;

    public void RequestLicensePlateInput()
    {
        LicensePlateInputRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler RandomizeAllModsRequested;

    public void RequestRandomizeMods()
    {
        RandomizeAllModsRequested?.Invoke(this, EventArgs.Empty);
    }
}