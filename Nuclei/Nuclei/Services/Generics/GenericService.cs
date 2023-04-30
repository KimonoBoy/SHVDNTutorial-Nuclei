using GTA;
using Newtonsoft.Json;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Observable;

namespace Nuclei.Services.Generics;

/// <summary>
///     Represents the base class for services with common properties and functionality.
///     This class does not include state management.
/// </summary>
/// <typeparam name="TService">The type of the service being implemented.</typeparam>
public abstract class GenericService<TService> : ObservableService where TService : class, new()
{
    public static TService Instance = new();

    private Ped _character;
    private GTA.Vehicle _currentVehicle;

    [JsonIgnore]
    public Ped Character
    {
        get => _character;
        set
        {
            if (_character == value) return;
            _character = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public GTA.Vehicle CurrentVehicle
    {
        get => _currentVehicle;
        set
        {
            if (_currentVehicle == value) return;
            _currentVehicle = value;
            OnPropertyChanged();
        }
    }

    public GenericStateService<TService> GetStorage()
    {
        return GenericStateService<TService>.Instance;
    }

    public void SetState(TService newState)
    {
        ReflectionUtilities.CopyProperties(newState, this);
    }
}