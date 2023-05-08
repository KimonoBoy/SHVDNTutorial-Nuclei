using System;
using System.ComponentModel;
using System.Drawing;
using GTA;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Player;
using Nuclei.UI.Text;

namespace Nuclei.Scripts.Player;

public class AppearanceScript : GenericScript<AppearanceService>
{
    protected override void OnTick(object sender, EventArgs e)
    {
        if (Character == null) return;

        Display.DrawTextElement(Character.Style[PedComponentType.Hair].Index.ToString(), 100.0f, 120.0f,
            Color.LightGreen);
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
        if (e.PropertyName == nameof(Service.Character)) UpdateStyles();
    }

    private void UpdateStyles()
    {
        Service.Style = Character.Style;
    }
}