using System;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerFavoritesMenu : ModelChangerMenuBase
{
    public ModelChangerFavoritesMenu(Enum @enum) : base(@enum)
    {
    }

    protected override void OnShown(object sender, EventArgs e)
    {
        base.OnShown(sender, e);
        GenerateItems();
        Service.FavoriteModels.CollectionChanged += OnModelCollectionChanged<PedHash>;
    }

    private void GenerateItems()
    {
        Clear();
        foreach (var pedHash in Service.FavoriteModels)
        {
            var itemFavoriteModel = AddItem(pedHash, () => { Service.RequestChangeModel(pedHash); });
        }
    }

    protected override void OnModelCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<PedHash>().ToList().ForEach(pedHash =>
                {
                    var displayName = pedHash.GetLocalizedDisplayNameFromHash();
                    var item = Items.FirstOrDefault(i => i.Title == displayName);

                    if (item != null)
                    {
                        var itemIndex = Items.IndexOf(item);
                        Remove(item);

                        if (Items.Count > 0) SelectedIndex = Math.Max(0, Math.Min(itemIndex, Items.Count - 1));
                    }
                });
                break;
        }
    }
}