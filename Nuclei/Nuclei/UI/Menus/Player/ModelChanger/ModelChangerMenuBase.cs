using System;
using System.Collections.Specialized;
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

    protected abstract void OnShown(object sender, EventArgs e);

    private void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        UpdateSelectedItem(Items[e.Index].Title);
    }

    protected abstract void UpdateSelectedItem(string title);

    protected abstract void OnModelCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e);
}