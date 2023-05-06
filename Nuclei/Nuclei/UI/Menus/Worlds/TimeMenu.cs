using System;
using System.Linq;
using Nuclei.Enums.UI;
using Nuclei.Enums.World;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Worlds;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Worlds;

public class TimeMenu : GenericMenu<TimeService>
{
    public TimeMenu(Enum @enum) : base(@enum)
    {
        LockTimeOfDay();
        TimeOfDay();
        TimeScale();
    }

    private void TimeScale()
    {
        var listItemTimeScale = AddListItem(TimeItemTitle.TimeScale, () => (int)Service.TimeScale, Service,
            (value, index) => { Service.TimeScale = value.GetHashFromDisplayName<TimeScaleHash>(); },
            typeof(TimeScaleHash).ToDisplayNameArray());
    }

    private void LockTimeOfDay()
    {
        var checkBoxLockTimeOfDay = AddCheckbox(TimeItemTitle.LockTimeOfDay, () => Service.LockTimeOfDay, Service,
            @checked => { Service.LockTimeOfDay = @checked; });
    }

    private void TimeOfDay()
    {
        var hoursOfDay = Enumerable.Range(0, 24)
            .Select(h =>
            {
                var hourString = h < 10 ? $"0{h}" : $"{h}";
                return $"{hourString}:00";
            })
            .ToArray();

        var listItemTimeOfDay = AddListItem(TimeItemTitle.TimeOfDay, () => Service.CurrentHourOfDay, Service,
            (value, index) =>
            {
                var success = int.TryParse(value, out var result);
                if (success)
                    Service.CurrentHourOfDay = result;
                else
                    Service.CurrentHourOfDay = TimeSpan.FromHours(index).Hours;
            },
            hoursOfDay);
    }
}