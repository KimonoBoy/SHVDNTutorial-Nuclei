using System.ComponentModel;

namespace Nuclei.Enums.UI;

public enum VehicleSpawnerItemTitles
{
    [Description("Puts the player in the Selected Seat of the spawned vehicle immediately.")]
    WarpInSpawned,

    [Description("Selects the seat to warp the player into when the vehicle is spawned.")]
    SelectSeat,

    [Description("Start the engine of the spawned vehicle immediately.")]
    EnginesRunning,

    [Description("Save the vehicle your character is currently in.")]
    SaveCurrentVehicle
}