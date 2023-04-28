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
using Nuclei.Services.Exception;
using Nuclei.Services.Vehicle.VehicleSpawner;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerSavedVehiclesMenu : VehicleSpawnerMenuBase
{
    public VehicleSpawnerSavedVehiclesMenu(Enum @enum) : base(@enum)
    {
    }

    private string GetModdedVehicleDescription(CustomVehicleDto vehicleDto)
    {
        var spawnTitle = $"Spawn: {vehicleDto.VehicleHash.GetLocalizedDisplayNameFromHash()}";
        return $"{spawnTitle}";
    }

    protected override void UpdateMenuItems<T>(IEnumerable<T> newItems)
    {
        Clear();
        SaveCurrentVehicle();
        foreach (var customVehicle in (ObservableCollection<CustomVehicleDto>)newItems)
        {
            var itemSpawnCustomVehicle =
                AddItem(customVehicle.Title,
                    $"{GetModdedVehicleDescription(customVehicle)}",
                    () => { Service.SpawnVehicle(customVehicle); });
        }
    }

    protected override void OnVehicleCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems != null:
                e.NewItems.Cast<CustomVehicleDto>().ToList().ForEach(customVehicle =>
                {
                    var itemVehicle = AddItem(customVehicle.Title,
                        $"{GetModdedVehicleDescription(customVehicle)}",
                        () => { Service.SpawnVehicle(customVehicle); });
                });
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<CustomVehicleDto>().ToList().ForEach(customVehicle =>
                {
                    var item = Items.FirstOrDefault(i => i.Title == customVehicle.Title);
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
        UpdateMenuItems(Service.CustomVehicles);
        Service.CustomVehicles.CollectionChanged += OnVehicleCollectionChanged<CustomVehicleDto>;
    }

    protected override void UpdateSelectedItem(string title)
    {
        var vehicleHash = Service.CustomVehicles.FirstOrDefault(v => v.Title == title)?.VehicleHash;

        if (vehicleHash != null)
            Service.CurrentVehicleHash = (VehicleHash)vehicleHash;
    }

    private void SaveCurrentVehicle()
    {
        var itemSaveCurrentVehicle = AddItem(VehicleSpawnerItemTitle.SaveCurrentVehicle,
            () =>
            {
                try
                {
                    /*
                 * Due for later. Lets finish the Modicfications Menu first before updating this.
                 */
                    if (Service.CurrentVehicle == null)
                    {
                        Notification.Show("You must enter a vehicle first.");
                        return;
                    }

                    var userInput = Game.GetUserInput(WindowTitle.EnterMessage60, "", 60);

                    if (Service.CustomVehicles.Any(v =>
                            v.Title == userInput))
                    {
                        Notification.Show("Vehicle with that title already exists. Please enter a unique title.");
                        return;
                    }

                    if (string.IsNullOrEmpty(userInput))
                    {
                        Notification.Show("Please enter a title to save the vehicle.");
                        return;
                    }

                    var customVehicle = new CustomVehicleDto
                    {
                        Title = userInput,
                        VehicleHash = (VehicleHash)Service.CurrentVehicle.Model.Hash,
                        LicensePlate = Service.CurrentVehicle.Mods.LicensePlate,
                        LicensePlateStyle = Service.CurrentVehicle.Mods.LicensePlateStyle,
                        WheelType = Service.CurrentVehicle.Mods.WheelType,
                        RimColor = Service.CurrentVehicle.Mods.RimColor,
                        CustomTires = Service.CurrentVehicle.Mods[VehicleModType.FrontWheel].Variation,
                        TireSmokeColor = Service.CurrentVehicle.Mods.TireSmokeColor,
                        WindowTint = Service.CurrentVehicle.Mods.WindowTint,
                        XenonHeadLights = Service.CurrentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled,
                        PrimaryColor = Service.CurrentVehicle.Mods.PrimaryColor,
                        SecondaryColor = Service.CurrentVehicle.Mods.SecondaryColor,
                        Turbo = Service.CurrentVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled
                    };

                    foreach (var vehicleMod in Service.CurrentVehicle.Mods.ToArray())
                    {
                        var customVehicleMod = new CustomVehicleModDto(vehicleMod.Type, vehicleMod.Index);
                        customVehicle.VehicleMods.Add(customVehicleMod);
                    }

                    Service.CustomVehicles.Add(customVehicle);
                }
                catch (Exception e)
                {
                    ExceptionService.Instance.RaiseError(e);
                }
            });
    }

    protected override void AddButtons()
    {
        var buttonDelete = new InstructionalButton("Delete Vehicle", Control.Jump);
        Buttons.Add(buttonDelete);
    }
}