using System;
using System.ComponentModel;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Worlds;

namespace Nuclei.Scripts.Worlds;

public class WeatherScript : GenericScript<WeatherService>
{
    protected override void OnTick(object sender, EventArgs e)
    {
        UpdateWeather();
    }

    private void UpdateWeather()
    {
        if (Service.Weather == World.Weather) return;
        Service.Weather = World.Weather;
    }

    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.Weather))
            World.Weather = Service.Weather;
    }
}