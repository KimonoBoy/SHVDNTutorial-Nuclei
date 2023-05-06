using System.Collections.Generic;
using GTA;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerTypeMenu : GenericMenu<ModelChangerService>
{
    private readonly List<PedHash> _pedHashes;

    public ModelChangerTypeMenu(string subtitle, string description, List<PedHash> pedHashes) : base(subtitle,
        description)
    {
        _pedHashes = pedHashes;
        GenerateItems();
    }

    private void GenerateItems()
    {
        foreach (var pedHash in _pedHashes)
        {
            var itemModel = AddItem(pedHash, () => { Service.RequestChangeModel(pedHash); });
        }
    }
}