using System;
using System.Linq;
using GTA;
using Nuclei.Helpers.ExtensionMethods;
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
        ReflectionUtilities.CopyBindableProperties(newState, this);
    }

    /// <summary>
    ///     Gets the hash from a display name string.
    /// </summary>
    /// <param name="displayName">The display name.</param>
    /// <returns>The hash associated with the display name.</returns>
    public TEnum GetHashFromDisplayName<TEnum>(string displayName) where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList().Find(@enum =>
            displayName == Game.GetLocalizedString(@enum.ToString()) || displayName == @enum.ToPrettyString());
    }

    /// <summary>
    ///     Get the localized display name from the hash if one exists,
    ///     else get the .ToPrettyString()
    /// </summary>
    /// <param name="hash">The hash to get the localized string from.</param>
    /// <returns>The hash as a localized string.</returns>
    public string GetLocalizedDisplayNameFromHash<TEnum>(TEnum hash) where TEnum : Enum
    {
        var localizedString = Game.GetLocalizedString(hash.ToString());

        if (string.IsNullOrEmpty(localizedString)) return hash.ToPrettyString();

        return localizedString;
    }
}