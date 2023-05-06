using System;
using GTA;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Worlds;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.World;

public class WeatherMenu : GenericMenu<WeatherService>
{
    public WeatherMenu(Enum @enum) : base(@enum)
    {
        ChangeWeather();
    }

    private void ChangeWeather()
    {
        var listItemChangeWeather = AddListItem(WeatherItemTitle.ChangeWeather, () => (int)Service.Weather, Service,
            (value, index) => { Service.Weather = value.GetHashFromDisplayName<Weather>(); },
            typeof(Weather).ToDisplayNameArray());
    }
}