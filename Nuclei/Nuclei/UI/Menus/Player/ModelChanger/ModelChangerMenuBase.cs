using System;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using LemonUI.Elements;
using LemonUI.Menus;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public class ModelChangerMenuBase : GenericMenu<ModelChangerService>
{
    protected ModelChangerMenuBase(string subtitle, string description) : base(subtitle, description)
    {
        Shown += OnShown;
        SelectedIndexChanged += OnSelectedIndexChanged;
    }

    protected ModelChangerMenuBase(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
    }

    protected virtual void OnShown(object sender, EventArgs e)
    {
        Service.FavoriteModels.CollectionChanged += OnFavoriteModelsChanged;
    }

    protected virtual void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        Service.CurrentPedHash = Items[e.Index].Title.GetHashFromDisplayName<PedHash>();
    }

    protected virtual void OnFavoriteModelsChanged(object sender, NotifyCollectionChangedEventArgs e)
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
}