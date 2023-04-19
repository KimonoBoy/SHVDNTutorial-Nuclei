using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nuclei.Helpers.Utilities.BindableProperty;

namespace Nuclei.Helpers.Utilities;

/// <summary>
///     A utility class for copying properties between objects using reflection.
/// </summary>
public static class ReflectionUtilities
{
    // A dictionary to store mappings between source and destination property pairs.
    // The key is a tuple of the source type and destination type, while the value is a list
    // of tuples containing source and destination PropertyInfo objects.
    private static readonly Dictionary<(Type source, Type destination),
            List<(PropertyInfo sourceProperty, PropertyInfo destinationProperty)>>
        PropertyMappings = new();

    /// <summary>
    ///     Copies the values of all matching BindableProperty properties from the source object to the destination object.
    /// </summary>
    /// <param name="source">The source object to copy properties from.</param>
    /// <param name="destination">The destination object to copy properties to.</param>
    public static void CopyProperties(object source, object destination)
    {
        // Check if the source or destination object is null, and throw an ArgumentNullException if so.
        if (source == null || destination == null)
            throw new ArgumentNullException(source == null ? nameof(source) : nameof(destination));

        // Get the source and destination types.
        var sourceType = source.GetType();
        var destinationType = destination.GetType();

        // Create a key tuple from the source and destination types.
        var key = (sourceType, destinationType);

        // Check if property mappings already exist for the given key.
        // If not, create the mappings and store them in the PropertyMappings dictionary.
        if (!PropertyMappings.TryGetValue(key, out var mappings))
        {
            // Get BindableProperty properties from both source and destination types.
            var sourceProperties = sourceType.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(BindableProperty<>));
            var destinationProperties = destinationType.GetProperties().Where(p => p.PropertyType.IsGenericType &&
                p.PropertyType.GetGenericTypeDefinition() == typeof(BindableProperty<>));

            // Create mappings between source and destination properties based on matching names and property types.
            mappings = (from sourceProperty in sourceProperties
                let destinationProperty = destinationProperties.FirstOrDefault(p =>
                    p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType)
                where destinationProperty != null
                select (sourceProperty, destinationProperty)).ToList();

            PropertyMappings[key] = mappings;
        }

        // Copy BindableProperty values from the source object to the destination object using the created mappings.
        foreach (var (sourceProperty, destinationProperty) in mappings)
        {
            var sourceBindableProperty = sourceProperty.GetValue(source) as dynamic;
            var destinationBindableProperty = destinationProperty.GetValue(destination) as dynamic;

            if (sourceBindableProperty != null && destinationBindableProperty != null)
                destinationBindableProperty.Value = sourceBindableProperty.Value;
        }
    }
}