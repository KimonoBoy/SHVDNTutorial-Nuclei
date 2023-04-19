using GTA;
using Newtonsoft.Json;
using Nuclei.Helpers.Utilities;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Generics;

public abstract class GenericService<TService> where TService : class, new()
{
    public static TService Instance = new();
    // private static Ped _character;
    // private static GTA.Vehicle _currentVehicle;

    // [JsonIgnore]
    // public Ped Character
    // {
    //     get => _character;
    //     set
    //     {
    //         _character = value;
    //         CharacterChanged?.Invoke(this, value);
    //     }
    // }

    // [JsonIgnore]
    // public GTA.Vehicle CurrentVehicle
    // {
    //     get => _currentVehicle;
    //     set
    //     {
    //         _currentVehicle = value;
    //         CurrentVehicleChanged?.Invoke(this, value);
    //     }
    // }
    //
    // public event EventHandler<Ped> CharacterChanged;
    // public event EventHandler<GTA.Vehicle> CurrentVehicleChanged;

    [JsonIgnore] public BindableProperty<Ped> Character { get; set; } = new();

    [JsonIgnore] public BindableProperty<GTA.Vehicle> CurrentVehicle { get; set; } = new();

    public GenericStateService<TService> GetStateService()
    {
        return GenericStateService<TService>.Instance;
    }

    public void SetState(TService newState)
    {
        ReflectionUtilities.CopyProperties(newState, this);
    }
}