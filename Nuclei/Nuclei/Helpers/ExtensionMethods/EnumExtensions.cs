using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GTA;

namespace Nuclei.Helpers.ExtensionMethods;

public static class EnumExtensions
{
    /// <summary>
    ///     Takes an enum and convert it to a more suitable text string.
    ///     e.g. "MyEnumValue01Cool" becomes "My Enum Value 01 Cool"
    /// </summary>
    /// <param name="enum">The enum to convert to a pretty string.</param>
    /// <returns>A pretty string.</returns>
    public static string ToPrettyString(this Enum @enum)
    {
        var title = @enum.ToString();

        // Split the title string into readable words.
        var titleToRegex = Regex.Replace(title,
            "(?<=[a-zA-Z])(?=[0-9])|(?<=[0-9])(?=[A-Z])|(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])", " ");

        return titleToRegex;
    }

    /// <summary>
    ///     Gets the description Attribute value associated with the enum.
    ///     If no description attribute is found, the enum.ToString() value is returned.
    /// </summary>
    /// <param name="enum">The enum to get the description attribute from.</param>
    /// <returns>The description of the enum.</returns>
    public static string GetDescription(this Enum @enum)
    {
        var genericEnumType = @enum.GetType();
        var memberInfo = genericEnumType.GetMember(@enum.ToString());
        if (memberInfo.Length > 0)
        {
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any()) return ((DescriptionAttribute)attributes.ElementAt(0)).Description;
        }

        return string.Empty;
    }

    public static TEnum GetHashFromDisplayName<TEnum>(this string displayName) where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList().Find(@enum =>
            @enum.ToPrettyString() == displayName || displayName == Game.GetLocalizedString(@enum.ToString()));
    }

    public static string GetLocalizedDisplayNameFromHash<TEnum>(this TEnum hash) where TEnum : Enum
    {
        var localizedString = Game.GetLocalizedString(hash.ToString());

        if (string.IsNullOrEmpty(localizedString)) return hash.ToPrettyString();

        return localizedString;
    }

    public static string[] ToDisplayNameArray(this Type enumType)
    {
        if (!enumType.IsEnum) throw new ArgumentException("The given type is not an enumeration.");

        var method =
            typeof(EnumExtensions).GetMethod("ToDisplayNameArrayGeneric", BindingFlags.NonPublic | BindingFlags.Static);
        var genericMethod = method.MakeGenericMethod(enumType);
        return (string[])genericMethod.Invoke(null, new object[] { enumType });
    }

    private static string[] ToDisplayNameArrayGeneric<TEnum>(Type enumType) where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(tE => tE.GetLocalizedDisplayNameFromHash()).ToArray();
    }
}