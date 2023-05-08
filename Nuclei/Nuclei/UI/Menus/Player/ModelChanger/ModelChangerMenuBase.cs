using System;
using System.Collections.Specialized;
using GTA;
using LemonUI.Menus;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Player;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Player.ModelChanger;

public abstract class ModelChangerMenuBase : GenericMenu<ModelChangerService>
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
    }

    protected virtual void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        Service.CurrentPedHash = Items[e.Index].Title.GetHashFromDisplayName<PedHash>();
    }

    protected abstract void OnModelCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e);
}