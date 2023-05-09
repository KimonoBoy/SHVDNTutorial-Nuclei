using Nuclei.Scripts.Generics;
using Nuclei.Services.Player;

namespace Nuclei.Scripts.Player;

public abstract class PlayerScriptBase : GenericScript<PlayerService>
{
    protected override void SubscribeToEvents()
    {
    }

    protected override void UnsubscribeOnExit()
    {
    }
}