using System;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Generics;

public abstract class GenericService<T> where T : class, new()
{
    private static readonly Lazy<T> _instance = new(() => new T());

    public static T Instance => _instance.Value;

    /// <summary>
    ///     Sets the state of the T service to the state of the new state parameter.
    /// </summary>
    /// <param name="newState">The new state to apply to the service.</param>
    public void SetState(T newState)
    {
        ReflectionUtilities.CopyBindableProperties(newState, this);
    }
}