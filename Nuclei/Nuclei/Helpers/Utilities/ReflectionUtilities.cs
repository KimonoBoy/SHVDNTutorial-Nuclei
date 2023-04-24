using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nuclei.Helpers.Utilities;

/// <summary>
///     A utility class for copying properties between objects using reflection.
/// </summary>
public static class ReflectionUtilities
{
    private static readonly Dictionary<(Type source, Type destination),
            List<(PropertyInfo sourceProperty, PropertyInfo destinationProperty)>>
        PropertyMappings = new();

    /// <summary>
    ///     Copies the values of all matching properties from the source object to the destination object.
    /// </summary>
    /// <param name="source">The source object to copy properties from.</param>
    /// <param name="destination">The destination object to copy properties to.</param>
    public static void CopyProperties(object source, object destination)
    {
        // Get the source and destination types.
        var sourceType = source.GetType();
        var destinationType = destination.GetType();

        // Create a key tuple from the source and destination types.
        var key = (sourceType, destinationType);

        // Check if property mappings already exist for the given key.
        // If not, create the mappings and store them in the PropertyMappings dictionary.
        if (!PropertyMappings.TryGetValue(key, out var mappings))
        {
            // Get properties from both source and destination types.
            var sourceProperties = sourceType.GetProperties();
            var destinationProperties = destinationType.GetProperties();

            // Create mappings between source and destination properties based on matching names and property types.
            mappings = (from sourceProperty in sourceProperties
                from destinationProperty in destinationProperties
                where sourceProperty.Name == destinationProperty.Name &&
                      sourceProperty.PropertyType == destinationProperty.PropertyType
                select (sourceProperty, destinationProperty)).ToList();

            PropertyMappings[key] = mappings;
        }

        // Copy property values from the source object to the destination object using the created mappings.
        foreach (var (sourceProperty, destinationProperty) in mappings)
            destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
    }
}