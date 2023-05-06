using System;
using GTA;
using Nuclei.Services.Generics;

namespace Nuclei.Services.Player;

public class ModelChangerService : GenericService<ModelChangerService>
{
    public event EventHandler<PedHash> ModelChangeRequested;

    public void RequestChangeModel(PedHash pedHash)
    {
        ModelChangeRequested?.Invoke(this, pedHash);
    }
}