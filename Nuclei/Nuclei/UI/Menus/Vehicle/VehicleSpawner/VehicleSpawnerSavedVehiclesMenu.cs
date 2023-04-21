using System;
using System.Linq;
using GTA;
using GTA.UI;
using Nuclei.Enums.UI;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.UI.Menus.Abstracts;

namespace Nuclei.UI.Menus.Vehicle.VehicleSpawner;

public class VehicleSpawnerSavedVehiclesMenu : GenericMenuBase<VehicleSpawnerService>
{
    public VehicleSpawnerSavedVehiclesMenu(Enum @enum) : base(@enum)
    {
        Shown += OnShown;
    }

    private void OnShown(object sender, EventArgs e)
    {
        GenerateMenu();
    }

    private void GenerateMenu()
    {
        Clear();

        SaveCurrentVehicle();
        AddItems();
    }

    private void AddItems()
    {
        foreach (var customVehicle in Service.CustomVehicles.Value)
        {
            var itemSpawnCustomVehicle =
                AddItem(customVehicle.Title.Value, "", () => { Service.SpawnVehicle(customVehicle); });
        }
    }

    private void SaveCurrentVehicle()
    {
        var itemSaveCurrentVehicle = AddItem(VehicleSpawnerItemTitles.SaveCurrentVehicle,
            () =>
            {
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

                var customVehicle = new CustomVehicle();
                customVehicle.Title.Value = userInput;
                customVehicle.VehicleHash.Value = (VehicleHash)Service.CurrentVehicle.Value.Model.Hash;
                customVehicle.WheelType.Value = Service.CurrentVehicle.Value.Mods.WheelType;

                foreach (var vehicleMod in Service.CurrentVehicle.Value.Mods.ToArray())
                {
                    var customVehicleMod = new CustomVehicleMod(vehicleMod.Type, vehicleMod.Index);
                    customVehicle.VehicleMods.Value.Add(customVehicleMod);
                }

                customVehicle.LicensePlate.Value = Service.CurrentVehicle.Value.Mods.LicensePlate;
                customVehicle.LicensePlateStyle.Value = Service.CurrentVehicle.Value.Mods.LicensePlateStyle;

                Service.CustomVehicles.Value.Add(customVehicle);

                GenerateMenu();
            });
    }
}