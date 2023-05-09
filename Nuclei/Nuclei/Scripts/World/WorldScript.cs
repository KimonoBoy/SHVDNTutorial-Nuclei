using System;
using Nuclei.Scripts.Generics;
using Nuclei.Services.World;

namespace Nuclei.Scripts.World;

public class WorldScript : GenericScript<WorldService>
{
    protected override void OnTick(object sender, EventArgs e)
    {
    }

    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }
}