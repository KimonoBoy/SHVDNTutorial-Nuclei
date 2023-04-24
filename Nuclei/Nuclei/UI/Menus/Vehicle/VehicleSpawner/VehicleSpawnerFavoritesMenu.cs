using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerFavoritesMenu : VehicleSpawnerMenuBase
{
    public VehicleSpawnerFavoritesMenu(Enum @enum) : base(@enum)
    {
    }

    protected override void OnShown(object sender, EventArgs e)
    {
        UpdateMenuItems(Service.FavoriteVehicles);
        Service.FavoriteVehicles.CollectionChanged += OnVehicleCollectionChanged<VehicleHash>;
    }

    protected override void UpdateSelectedItem(string title)
    {
        Service.CurrentVehicleHash = title.GetHashFromDisplayName<VehicleHash>();
    }

    protected override void UpdateMenuItems<T>(IEnumerable<T> newItems)
    {
        Clear();
        foreach (var vehicleHash in (ObservableCollection<VehicleHash>)newItems)
        {
            var vehicleName = vehicleHash.GetLocalizedDisplayNameFromHash();

            var itemVehicle = AddItem(vehicleName, $"Spawn {vehicleName}",
                () => { Service.SpawnVehicle(vehicleHash); });
        }
    }

    protected override void OnVehicleCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<VehicleHash>().ToList().ForEach(vHash =>
                {
                    var displayName = vHash.GetLocalizedDisplayNameFromHash();
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