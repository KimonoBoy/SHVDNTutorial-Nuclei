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
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.Services.Vehicle.VehicleSpawner;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerSavedVehiclesMenu : VehicleSpawnerMenuBase
{
    public VehicleSpawnerSavedVehiclesMenu(Enum @enum) : base(@enum)
    {
    }

    private string GetModdedVehicleDescription(CustomVehicleDto vehicleDto)
    {
        if (vehicleDto == null) throw new ArgumentNullException(nameof(vehicleDto));

        var spawnTitle = $"Spawn: {vehicleDto.VehicleHash.GetLocalizedDisplayNameFromHash()}";
        return $"{spawnTitle}";
    }

    protected override void UpdateMenuItems<T>(IEnumerable<T> newItems)
    {
        if (newItems == null) throw new ArgumentNullException(nameof(newItems));

        Clear();
        SaveCurrentVehicle();

        if (newItems is ObservableCollection<CustomVehicleDto> customVehicles)
            foreach (var customVehicle in customVehicles)
                AddCustomVehicleItem(customVehicle);
        else
            throw new ArgumentException("Invalid collection type.", nameof(newItems));
    }

    protected override void OnVehicleCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e == null) throw new ArgumentNullException(nameof(e));

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems != null:
                e.NewItems.Cast<CustomVehicleDto>().ToList().ForEach(AddCustomVehicleItem);
                break;
            case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                e.OldItems.Cast<CustomVehicleDto>().ToList().ForEach(RemoveCustomVehicleItem);
                break;
        }
    }

    private void AddCustomVehicleItem(CustomVehicleDto customVehicle)
    {
        if (customVehicle == null) throw new ArgumentNullException(nameof(customVehicle));

        AddItem(customVehicle.Title, $"{GetModdedVehicleDescription(customVehicle)}",
            () => Service.SpawnVehicle(customVehicle));
    }

    private void RemoveCustomVehicleItem(CustomVehicleDto customVehicle)
    {
        if (customVehicle == null) throw new ArgumentNullException(nameof(customVehicle));

        var item = Items.FirstOrDefault(i => i.Title == customVehicle.Title);
        if (item != null)
        {
            var itemIndex = Items.IndexOf(item);
            Remove(item);
            if (Items.Count > 0) SelectedIndex = Math.Max(0, Math.Min(itemIndex, Items.Count - 1));
        }
    }

    protected override void OnShown(object sender, EventArgs e)
    {
        UpdateMenuItems(Service.CustomVehicles);
        Service.CustomVehicles.CollectionChanged += OnVehicleCollectionChanged<CustomVehicleDto>;
    }

    protected override void UpdateSelectedItem(string title)
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));

        var vehicleHash = Service.CustomVehicles.FirstOrDefault(v => v.Title == title)?.VehicleHash;

        if (vehicleHash != null) Service.CurrentVehicleHash = (VehicleHash)vehicleHash;
    }

    private void SaveCurrentVehicle()
    {
        AddItem(VehicleSpawnerItemTitle.SaveCurrentVehicle, () =>
        {
            if (Service.CurrentVehicle == null)
            {
                Notification.Show("You must enter a vehicle first.");
                return;
            }

            var userInput = Game.GetUserInput(WindowTitle.EnterMessage60, "", 60);

            if (Service.CustomVehicles.Any(v => v.Title == userInput))
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

                LicensePlate = VehicleModsService.Instance.LicensePlate,
                LicensePlateStyle = VehicleModsService.Instance.LicensePlateStyle,

                WheelType = VehicleModsService.Instance.WheelType,
                CustomTires = VehicleModsService.Instance.CustomTires,
                RimColor = VehicleModsService.Instance.RimColor,

                PrimaryColor = VehicleModsService.Instance.PrimaryColor,
                SecondaryColor = VehicleModsService.Instance.SecondaryColor,
                PearlescentColor = VehicleModsService.Instance.PearlescentColor,

                NeonLightsLayout = VehicleModsService.Instance.NeonLightsLayout,
                NeonLightsColor = VehicleModsService.Instance.NeonLightsColor,

                WindowTint = VehicleModsService.Instance.WindowTint,

                XenonHeadLights = VehicleModsService.Instance.XenonHeadLights,
                Turbo = VehicleModsService.Instance.Turbo,

                TireSmokeColor = VehicleModsService.Instance.TireSmokeColor,
                RainbowMode = VehicleModsService.Instance.RainbowMode
            };

            foreach (var vehicleMod in Service.CurrentVehicle.Mods.ToArray())
            {
                var customVehicleMod = new CustomVehicleModDto(vehicleMod.Type, vehicleMod.Index);
                customVehicle.VehicleMods.Add(customVehicleMod);
            }

            Service.CustomVehicles.Add(customVehicle);
            Service.GetStateService().SetState(Service);
            Service.GetStateService().SaveState();
        });
    }

    protected override void AddButtons()
    {
        var buttonDelete = new InstructionalButton("Delete Vehicle", Control.Jump);
        Buttons.Add(buttonDelete);
    }
}