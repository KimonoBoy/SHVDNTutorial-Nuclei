using System;
using System.ComponentModel;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Player;

namespace Nuclei.Scripts.Player;

public class AppearanceScript : GenericScript<AppearanceService>
{
    protected override void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;
    }

    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Character == null) return;
    }
}