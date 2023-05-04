using System;
using Nuclei.Scripts.Generics;
using Nuclei.Services.Worlds;

namespace Nuclei.Scripts.Worlds;

public class WorldScript : GenericScript<WorldService>
{
    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }

    protected override void ProcessGameStatesTimer(object sender, EventArgs e)
    {
    }

    protected override void UpdateServiceStatesTimer(object sender, EventArgs e)
    {
    }
}