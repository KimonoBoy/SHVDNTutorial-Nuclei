using Microsoft.Extensions.DependencyInjection;
using Nuclei.Enums.UI;
using Nuclei.Services.Player;
using Nuclei.Services.Settings;
using Nuclei.Services.Settings.Storage;
using Nuclei.Services.Vehicle;
using Nuclei.Services.Vehicle.VehicleMods;
using Nuclei.Services.Vehicle.VehicleSpawner;
using Nuclei.Services.Vehicle.VehicleWeapons;
using Nuclei.Services.Weapon;
using Nuclei.UI.Menus;

namespace Nuclei.Helpers.Utilities;

public static class DependencyInjection
{
    public static ServiceProvider Configure()
    {
        var services = new ServiceCollection();

        // Register your services here
        services.AddSingleton<PlayerService>();
        services.AddSingleton<WeaponsService>();
        services.AddSingleton<WeaponComponentsService>();
        services.AddSingleton<VehicleService>();
        services.AddSingleton<VehicleSpawnerService>();
        services.AddSingleton<VehicleWeaponsService>();
        services.AddSingleton<VehicleModsService>();
        services.AddSingleton<SettingsService>();
        services.AddSingleton<StorageService>();

        // Register your menus here
        services.AddSingleton(provider => new MainMenu(
            MenuTitles.Main
        ));

        // Register your main class
        services.AddSingleton<Main>();

        return services.BuildServiceProvider();
    }
}