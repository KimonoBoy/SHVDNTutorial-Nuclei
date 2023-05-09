using System;
using System.ComponentModel;
using System.Linq;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.World;

namespace Nuclei.Scripts.World;

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
        if (Service.TimeScaleDictionary.FirstOrDefault(timeScale => timeScale.Key == Service.TimeScale).Value
            .Equals(Game.TimeScale)) return;
        Game.TimeScale = Service.TimeScaleDictionary.FirstOrDefault(timeScale => timeScale.Key == Service.TimeScale)
            .Value;
    }

    private void ProcessLockTimeOfDay()
    {
        if (Service.LockTimeOfDay)
            GTA.World.CurrentTimeOfDay = TimeSpan.FromHours(Service.CurrentHourOfDay);
    }

    private void UpdateTimeOfDay()
    {
        if (Service.CurrentHourOfDay == GTA.World.CurrentTimeOfDay.Hours) return;
        if (Service.LockTimeOfDay) return;

        Service.CurrentHourOfDay = GTA.World.CurrentTimeOfDay.Hours;
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
            GTA.World.CurrentTimeOfDay = TimeSpan.FromHours(Service.CurrentHourOfDay);
    }
}