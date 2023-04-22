namespace Nuclei.Services.Generics;

public interface IGenericStateService<TService> where TService : new()
{
    TService GetState();
    void SaveState();
    TService LoadState();
    void SetState(TService newState);
}