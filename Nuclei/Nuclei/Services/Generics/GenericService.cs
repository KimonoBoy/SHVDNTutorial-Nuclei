using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Generics;

public abstract class GenericService<TService> where TService : class, new()
{
    public static TService Instance = new();

    public GenericStateService<TService> GetStateService()
    {
        return GenericStateService<TService>.Instance;
    }

    public void SetState(TService newState)
    {
        ReflectionUtilities.CopyProperties(newState, this);
    }
}