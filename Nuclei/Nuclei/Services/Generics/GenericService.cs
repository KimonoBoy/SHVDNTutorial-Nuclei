using GTA;
using Newtonsoft.Json;
using Nuclei.Helpers.Utilities;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Generics;

public abstract class GenericService<TService> where TService : class, new()
{
    public static TService Instance = new();
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