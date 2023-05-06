using System.Collections.Generic;
using Newtonsoft.Json;
using Nuclei.Enums.World;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Worlds;

public class TimeService : GenericService<TimeService>
{
    [JsonIgnore] public readonly Dictionary<TimeScaleHash, float> TimeScaleDictionary = new()
    {
        { TimeScaleHash.Normal, 1.0f },
        { TimeScaleHash.Slow, 0.8f },
        { TimeScaleHash.Slower, 0.5f },
        { TimeScaleHash.Slowest, 0.25f }
    };

    private int _currentHourOfDay;
    private bool _lockTimeOfDay;
    private TimeScaleHash _timeScale = TimeScaleHash.Normal;

    public int CurrentHourOfDay
    {
        get => _currentHourOfDay;
        set
        {
            if (value.Equals(_currentHourOfDay)) return;
            _currentHourOfDay = value;
            OnPropertyChanged();
        }
    }

    public bool LockTimeOfDay
    {
        get => _lockTimeOfDay;
        set
        {
            if (value == _lockTimeOfDay) return;
            _lockTimeOfDay = value;
            OnPropertyChanged();
        }
    }

    public TimeScaleHash TimeScale
    {
        get => _timeScale;
        set
        {
            if (value.Equals(_timeScale)) return;
            _timeScale = value;
            OnPropertyChanged();
        }
    }
}