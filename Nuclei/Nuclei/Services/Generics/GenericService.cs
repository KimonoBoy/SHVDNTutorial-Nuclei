using System;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Settings;

namespace Nuclei.Services.Generics;

public abstract class GenericService<T> where T : class, new()
{
    private static readonly Lazy<T> _instance = new(() => new T());

    public StorageService Storage = StorageService.Instance;

    public static T Instance => _instance.Value;

    public void SetState(T newState)
    {
        ReflectionUtilities.CopyBindableProperties(newState, this);
    }
}