using System;
using System.Drawing;
using System.Reflection;

namespace Nuclei.Helpers.ExtensionMethods;

public static class ColorExtension
{
    public static Color[] GetAllKnownColors(this Type colorType)
    {
        // Get all the properties in the System.Drawing.Color struct
        var colorProperties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public);

        // Initialize an array to hold all the named colors
        var colorsArray = new Color[colorProperties.Length];

        var colorIndex = 0;
        // Iterate through the properties and add the colors to the array
        foreach (var colorProperty in colorProperties)
            if (colorProperty.PropertyType == typeof(Color))
            {
                var color = (Color)colorProperty.GetValue(null);
                colorsArray[colorIndex++] = color;
            }

        return colorsArray;
    }
}