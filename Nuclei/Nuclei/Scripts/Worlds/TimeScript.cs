using System;
using System.ComponentModel;
using GTA;
using Nuclei.Enums.World;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Worlds;

namespace Nuclei.Scripts.Worlds;

public class TimeScript : GenericScript<TimeService>
{
    protected override void OnTick(object sender, EventArgs e)
    {
        UpdateTimeOfDay();
        ProcessLockTimeOfDay();
        ProcessTimeScale();
    }

    private void ProcessTimeScale()
    {
        switch (Service.TimeScale)
        {
            case TimeScaleHash.Normal:
                Game.TimeScale = 1.0f;
                break;
            case TimeScaleHash.Slow:
                Game.TimeScale = 0.80f;
                break;
            case TimeScaleHash.Slower:
                Game.TimeScale = 0.50f;
                break;
            case TimeScaleHash.Slowest:
                Game.TimeScale = 0.25f;
                break;
        }
    }

    private void ProcessLockTimeOfDay()
    {
        if (Service.LockTimeOfDay)
            World.CurrentTimeOfDay = TimeSpan.FromHours(Service.CurrentHourOfDay);
    }

    private void UpdateTimeOfDay()
    {
        if (Service.CurrentHourOfDay == World.CurrentTimeOfDay.Hours) return;
        if (Service.LockTimeOfDay) return;

        Service.CurrentHourOfDay = World.CurrentTimeOfDay.Hours;
    }

    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Service.CurrentHourOfDay))
            World.CurrentTimeOfDay = TimeSpan.FromHours(Service.CurrentHourOfDay);
    }
}