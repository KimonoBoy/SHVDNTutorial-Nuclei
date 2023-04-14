using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

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
    ///     Converts a pretty string back to the corresponding enum value.
    ///     e.g. "My Enum Value 01 Cool" becomes MyEnumValue01Cool
    /// </summary>
    /// <typeparam name="T">Enum type.</typeparam>
    /// <param name="prettyString">The pretty string to convert back to an enum value.</param>
    /// <returns>The corresponding enum value.</returns>
    public static T FromPrettyString<T>(this string prettyString) where T : struct, Enum
    {
        // Remove spaces and ensure the first letter of each word is uppercase, the rest are lowercase.
        var enumString = string.Concat(prettyString.Split(' ')
            .Select(s => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s)));

        // Parse the string to the corresponding enum value.
        if (Enum.TryParse<T>(enumString, out var enumValue))
            return enumValue;
        throw new ArgumentException($"Cannot convert '{prettyString}' to enum value of type '{typeof(T)}'.");
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
}