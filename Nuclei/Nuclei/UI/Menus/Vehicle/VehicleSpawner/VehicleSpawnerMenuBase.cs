using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using GTA;
using LemonUI.Menus;
using LemonUI.Scaleform;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.Dtos;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Base;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public abstract class VehicleSpawnerMenuBase : GenericMenu<VehicleSpawnerService>
{
    protected VehicleSpawnerMenuBase(string subtitle, string description) : base(subtitle, description)
    {
        Shown += OnShown;
        Closed += OnClosed;
        SelectedIndexChanged += OnSelectedIndexChanged;
    }

    protected VehicleSpawnerMenuBase(Enum @enum) : this(@enum.ToPrettyString(), @enum.GetDescription())
    {
    }

    protected abstract void UpdateMenuItems<T>(IEnumerable<T> newItems);
    protected abstract void OnVehicleCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e);
    protected abstract void OnShown(object sender, EventArgs e);


    private void OnClosed(object sender, EventArgs e)
    {
        Service.FavoriteVehicles.CollectionChanged -= OnVehicleCollectionChanged<VehicleHash>;
        Service.CustomVehicles.CollectionChanged -= OnVehicleCollectionChanged<CustomVehicleDto>;
    }

    private void OnSelectedIndexChanged(object sender, SelectedEventArgs e)
    {
        UpdateSelectedItem(Items[e.Index].Title);
    }

    protected abstract void UpdateSelectedItem(string title);

    protected override void AddButtons()
    {
        var addVehicleToFavorites = new InstructionalButton("Add/Remove Favorite", Control.Jump);
        Buttons.Add(addVehicleToFavorites);
    }
}