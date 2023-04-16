using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using LemonUI.Scaleform;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerFavoritesMenu : VehicleSpawnerMenuBase
{
    public VehicleSpawnerFavoritesMenu(Enum @enum) : base(@enum)
    {
    }

    protected override void UpdateMenuItems(IEnumerable<VehicleHash> newItems)
    {
        Clear();
        foreach (var vehicleHash in newItems)
        {
            var itemVehicle = AddItem(vehicleHash, () => { Service.SpawnVehicle(vehicleHash); });
        }
    }

    protected override void OnVehicleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<VehicleHash>().ToList().ForEach(vHash =>
                {
                    var item = Items.FirstOrDefault(i => i.Title == vHash.ToPrettyString());
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

    protected override void AddButtons()
    {
        var buttonDeleteFavorite = new InstructionalButton("Delete", Control.Jump);
        Buttons.Add(buttonDeleteFavorite);
    }
}