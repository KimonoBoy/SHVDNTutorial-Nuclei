using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using LemonUI.Elements;
using Nuclei.Helpers.ExtensionMethods;

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

    protected override void OnModelCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems != null:
                e.NewItems.Cast<PedHash>().ToList().ForEach(pedHash =>
                {
                    var displayName = pedHash.GetLocalizedDisplayNameFromHash();
                    var item = Items.FirstOrDefault(i => i.Title == displayName);
                    if (item != null) item.RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
                });
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<PedHash>().ToList().ForEach(pedHash =>
                {
                    var displayName = pedHash.GetLocalizedDisplayNameFromHash();
                    var item = Items.FirstOrDefault(i => i.Title == displayName);
                    if (item != null) item.RightBadge = null;
                });
                break;
        }
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