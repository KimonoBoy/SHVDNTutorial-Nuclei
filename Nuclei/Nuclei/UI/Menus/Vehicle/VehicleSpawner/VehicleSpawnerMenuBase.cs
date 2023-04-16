using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using LemonUI.Menus;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public abstract class VehicleSpawnerMenuBase : GenericMenuBase<VehicleSpawnerService>
{
    protected VehicleSpawnerMenuBase(Enum @enum) : base(@enum)
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
        Service.CurrentVehicleHash.Value =
            Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().FirstOrDefault(vHash =>
                vHash.ToPrettyString() == Items[SelectedIndex].Title);
    }
}