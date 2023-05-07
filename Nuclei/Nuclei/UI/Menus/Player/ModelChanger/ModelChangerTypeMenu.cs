using System;
using System.Collections.Generic;
using GTA;
using LemonUI.Elements;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerTypeMenu : ModelChangerMenuBase
{
    private readonly List<PedHash> _pedHashes;

    public ModelChangerTypeMenu(string subtitle, string description, List<PedHash> pedHashes) : base(subtitle,
        description)
    {
        _pedHashes = pedHashes;
    }

    protected override void OnShown(object sender, EventArgs e)
    {
        base.OnShown(sender, e);
        GenerateItems();
    }

    private void GenerateItems()
    {
        Clear();
        foreach (var pedHash in _pedHashes)
        {
            var itemModel = AddItem(pedHash, () => { Service.RequestChangeModel(pedHash); });

            if (Service.FavoriteModels.Contains(pedHash))
                itemModel.RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
            else
                itemModel.RightBadge = null;
        }
    }
}