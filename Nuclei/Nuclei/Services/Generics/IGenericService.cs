using GTA;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Services.Generics;

public interface IGenericService<TService> where TService : class, new()
{
    BindableProperty<Ped> Character { get; set; }
    BindableProperty<GTA.Vehicle> CurrentVehicle { get; set; }
    GenericStateService<TService> GetStateService();
    void SetState(TService newState);
}