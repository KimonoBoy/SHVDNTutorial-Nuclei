using GTA;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Worlds;

public class WeatherService : GenericService<WeatherService>
{
    private bool _lockWeather;
    private Weather _weather;

    public bool LockWeather
    {
        get => _lockWeather;
        set
        {
            if (value == _lockWeather) return;
            _lockWeather = value;
            OnPropertyChanged();
        }
    }

    public Weather Weather
    {
        get => _weather;
        set
        {
            if (value.Equals(_weather)) return;
            _weather = value;
            OnPropertyChanged();
        }
    }
}