using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GTA;
using GTA.UI;
using LemonUI.Scaleform;
using Nuclei.Enums.UI;
using Nuclei.Helpers.ExtensionMethods;
using Nuclei.Services.Vehicle.VehicleSpawner;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerSavedVehiclesMenu : VehicleSpawnerMenuBase
{
    public VehicleSpawnerSavedVehiclesMenu(Enum @enum) : base(@enum)
    {
    }

    private string GetModdedVehicleDescription(CustomVehicle vehicle)
    {
        var spawnTitle = $"Spawn: {vehicle.VehicleHash.Value.GetLocalizedDisplayNameFromHash()}";
        return $"{spawnTitle}";
    }

    protected override void UpdateMenuItems<T>(IEnumerable<T> newItems)
    {
        Clear();
        SaveCurrentVehicle();
        foreach (var customVehicle in (ObservableCollection<CustomVehicle>)newItems)
        {
            var itemSpawnCustomVehicle =
                AddItem(customVehicle.Title.Value,
                    $"{GetModdedVehicleDescription(customVehicle)}",
                    () => { Service.SpawnVehicle(customVehicle); });
        }
    }

    protected override void OnVehicleCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems != null:
                e.NewItems.Cast<CustomVehicle>().ToList().ForEach(customVehicle =>
                {
                    var itemVehicle = AddItem(customVehicle.Title.Value,
                        $"{GetModdedVehicleDescription(customVehicle)}",
                        () => { Service.SpawnVehicle(customVehicle); });
                });
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<CustomVehicle>().ToList().ForEach(customVehicle =>
                {
                    var item = Items.FirstOrDefault(i => i.Title == customVehicle.Title.Value);
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

    protected override void OnShown(object sender, EventArgs e)
    {
        UpdateMenuItems(Service.CustomVehicles.Value);
        Service.CustomVehicles.Value.CollectionChanged += OnVehicleCollectionChanged<CustomVehicle>;
    }

    protected override void UpdateSelectedItem(string title)
    {
        var vehicleHash = Service.CustomVehicles.Value.FirstOrDefault(v => v.Title.Value == title)?.VehicleHash.Value;

        if (vehicleHash != null)
            Service.CurrentVehicleHash.Value = (VehicleHash)vehicleHash;
    }

    private void SaveCurrentVehicle()
    {
        var itemSaveCurrentVehicle = AddItem(VehicleSpawnerItemTitles.SaveCurrentVehicle,
            () =>
            {
                if (Service.CurrentVehicle.Value == null)
                {
                    Notification.Show("You must enter a vehicle first.");
                    return;
                }

                var userInput = Game.GetUserInput(WindowTitle.EnterMessage60, "", 60);

                if (Service.CustomVehicles.Value.Any(v =>
                        v.Title.Value == userInput))
                {
                    Notification.Show("Vehicle with that title already exists. Please enter a unique title.");
                    return;
                }

                if (string.IsNullOrEmpty(userInput))
                {
                    Notification.Show("Please enter a title to save the vehicle.");
                    return;
                }

                var customVehicle = new CustomVehicle
                {
                    Title =
                    {
                        Value = userInput
                    },
                    VehicleHash =
                    {
                        Value = (VehicleHash)Service.CurrentVehicle.Value.Model.Hash
                    },
                    LicensePlate =
                    {
                        Value = Service.CurrentVehicle.Value.Mods.LicensePlate
                    },
                    LicensePlateStyle =
                    {
                        Value = Service.CurrentVehicle.Value.Mods.LicensePlateStyle
                    },
                    WheelType =
                    {
                        Value = Service.CurrentVehicle.Value.Mods.WheelType
                    },
                    RimColor =
                    {
                        Value = Service.CurrentVehicle.Value.Mods.RimColor
                    },
                    CustomTires =
                    {
                        Value = Service.CurrentVehicle.Value.Mods[VehicleModType.FrontWheel].Variation &&
                                Service.CurrentVehicle.Value.Mods[VehicleModType.RearWheel].Variation
                    }
                };

                foreach (var vehicleMod in Service.CurrentVehicle.Value.Mods.ToArray())
                {
                    var customVehicleMod = new CustomVehicleMod(vehicleMod.Type, vehicleMod.Index);
                    customVehicle.VehicleMods.Value.Add(customVehicleMod);
                }


                Service.CustomVehicles.Value.Add(customVehicle);
            });
    }

    protected override void AddButtons()
    {
        var buttonDelete = new InstructionalButton("Delete Vehicle", Control.Jump);
        Buttons.Add(buttonDelete);
    }
}