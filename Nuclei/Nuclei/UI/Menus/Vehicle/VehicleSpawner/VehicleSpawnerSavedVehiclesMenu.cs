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
            var itemSpawnCustomVehicle = AddItem(customVehicle.Title.Value, "", () =>
            {
                // var vehicleModel = new Model(customVehicle.VehicleHash.Value);
                // vehicleModel.Request(1000);
                //
                // var vehicle = World.CreateVehicle(vehicleModel,
                //     Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f,
                //     Game.Player.Character.Heading);
                // vehicle.Mods.InstallModKit();
                // foreach (var customVehicleMod in customVehicle.VehicleMods.Value)
                //     vehicle.Mods[customVehicleMod.VehicleModType.Value].Index = customVehicleMod.ModIndex.Value;

                Service.SpawnVehicle(customVehicle);
            });
        }
    }

    private void SaveCurrentVehicle()
    {
        var itemSaveCurrentVehicle = AddItem(VehicleSpawnerItemTitles.SaveCurrentVehicle,
            () =>
            {
                var userInput = Game.GetUserInput(WindowTitle.EnterMessage60, "", 60);
                if (Service.CustomVehicles.Value.Any(v => v.Title.Value == userInput))
                {
                    Notification.Show("Vehicle with that title already exists. Please enter a unique title.");
                    return;
                }

                var customVehicle = new CustomVehicle();
                customVehicle.Title.Value = userInput;
                customVehicle.VehicleHash.Value = (VehicleHash)Service.CurrentVehicle.Value.Model.Hash;

                foreach (var vehicleMod in Service.CurrentVehicle.Value.Mods.ToArray())
                {
                    var customVehicleMod = new CustomVehicleMod(vehicleMod.Type, vehicleMod.Index);
                    customVehicle.VehicleMods.Value.Add(customVehicleMod);
                }

                Service.CustomVehicles.Value.Add(customVehicle);

                GenerateMenu();
            });
    }
}