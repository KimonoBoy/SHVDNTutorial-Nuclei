using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using LemonUI.Elements;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerClassMenu : VehicleSpawnerMenuBase
{
    private readonly VehicleClass _vehicleClass;

    public VehicleSpawnerClassMenu(Enum @enum) : base(@enum)
    {
        _vehicleClass = (VehicleClass)@enum;
        AddVehicles();
    }

    protected override void UpdateMenuItems(IEnumerable<VehicleHash> newItems)
    {
        AddVehicles();
    }

    protected override void OnVehicleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems != null:
                e.NewItems.Cast<VehicleHash>().ToList().ForEach(vHash =>
                {
                    var displayName = vHash.GetLocalizedDisplayNameFromHash();
                    var item = Items.FirstOrDefault(i => i.Title == displayName);
                    if (item != null) item.RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
                });
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<VehicleHash>().ToList().ForEach(vHash =>
                {
                    var displayName = vHash.GetLocalizedDisplayNameFromHash();
                    var item = Items.FirstOrDefault(i => i.Title == displayName);
                    if (item != null) item.RightBadge = null;
                });
                break;
        }
    }

    private void AddVehicles()
    {
        Clear();
        foreach (var vehicleHash in GTA.Vehicle.GetAllModelsOfClass(_vehicleClass).OrderBy(v => v.ToPrettyString()))
        {
            var vehicleName = vehicleHash.GetLocalizedDisplayNameFromHash();

            var itemSpawnVehicle = AddItem(vehicleName, $"Spawn {vehicleName}",
                () => { Service.SpawnVehicle(vehicleHash); });
            itemSpawnVehicle.Selected += (sender, args) => { UpdateSelectedItem(); };

            if (Service.FavoriteVehicles.Value.Contains(vehicleHash))
                itemSpawnVehicle.RightBadge = new ScaledTexture("commonmenu", "shop_new_star");
            else
                itemSpawnVehicle.RightBadge = null;
        }
    }
}