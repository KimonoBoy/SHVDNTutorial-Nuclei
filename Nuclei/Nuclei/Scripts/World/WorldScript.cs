using System;
using Nuclei.Scripts.Generics;
using Nuclei.Services.World;

namespace Nuclei.Scripts.World;

public class WorldScript : GenericScriptBase<WorldService>
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