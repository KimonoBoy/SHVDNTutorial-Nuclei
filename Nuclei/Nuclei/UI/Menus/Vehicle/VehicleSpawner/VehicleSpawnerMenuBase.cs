using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using GTA;
using LemonUI.Menus;
using LemonUI.Scaleform;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public abstract class VehicleSpawnerMenuBase : GenericMenuBase<VehicleSpawnerService>
{
    protected VehicleSpawnerMenuBase(string subtitle, string description) : base(subtitle, description)
    {
        Shown += OnShown;
        Closed += OnClosed;
        SelectedIndexChanged += OnSelectedIndexChanged;
    }

    protected VehicleSpawnerMenuBase(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
        Shown += OnShown;
        Closed += OnClosed;
        SelectedIndexChanged += OnSelectedIndexChanged;
    }

    protected abstract void UpdateMenuItems(IEnumerable<VehicleHash> newItems);

    private void OnShown(object sender, EventArgs e)
    {
        UpdateMenuItems(Service.FavoriteVehicles.Value);
        Service.FavoriteVehicles.Value.CollectionChanged += OnVehicleCollectionChanged;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Service.FavoriteVehicles.Value.CollectionChanged -= OnVehicleCollectionChanged;
    }

    private void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        UpdateSelectedItem();
    }

    protected abstract void OnVehicleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

    protected void UpdateSelectedItem()
    {
        Service.CurrentVehicleHash.Value = Items[SelectedIndex].Title.GetHashFromDisplayName<VehicleHash>();
    }

    protected override void AddButtons()
    {
        var addVehicleToFavorites = new InstructionalButton("Add/Remove Favorite", Control.Jump);
        Buttons.Add(addVehicleToFavorites);
    }
}