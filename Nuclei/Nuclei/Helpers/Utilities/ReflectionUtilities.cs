using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nuclei.Helpers.Utilities;

public static class ReflectionUtilities
{
    private static readonly Dictionary<(Type source, Type destination),
            List<(PropertyInfo sourceProperty, PropertyInfo destinationProperty)>>
        PropertyMappings = new();

    public static void CopyProperties(object source, object destination)
    {
        var sourceType = source.GetType();
        var destinationType = destination.GetType();
        var key = (sourceType, destinationType);

        if (!PropertyMappings.TryGetValue(key, out var mappings))
        {
            var sourceProperties = sourceType.GetProperties();
            var destinationProperties = destinationType.GetProperties();

            mappings = (from sourceProperty in sourceProperties
                from destinationProperty in destinationProperties
                where sourceProperty.Name == destinationProperty.Name &&
                      sourceProperty.PropertyType == destinationProperty.PropertyType
                select (sourceProperty, destinationProperty)).ToList();

            PropertyMappings[key] = mappings;
        }

        foreach (var (sourceProperty, destinationProperty) in mappings)
            destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
    }

    public static void CopyBindableProperties(object source, object destination)
    {
        if (source == null || destination == null)
            throw new ArgumentNullException(source == null ? nameof(source) : nameof(destination));

        var sourceType = source.GetType();
        var destinationType = destination.GetType();
        var key = (sourceType, destinationType);

        if (!PropertyMappings.TryGetValue(key, out var mappings))
        {
            var sourceProperties = sourceType.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(BindableProperty<>));
            var destinationProperties = destinationType.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(BindableProperty<>));

            mappings = (from sourceProperty in sourceProperties
                from destinationProperty in destinationProperties
                where sourceProperty.Name == destinationProperty.Name &&
                      sourceProperty.PropertyType == destinationProperty.PropertyType
                select (sourceProperty, destinationProperty)).ToList();

            PropertyMappings[key] = mappings;
        }

        foreach (var (sourceProperty, destinationProperty) in mappings)
        {
            var sourceBindableProperty = sourceProperty.GetValue(source) as dynamic;
            var destinationBindableProperty = destinationProperty.GetValue(destination) as dynamic;

            if (sourceBindableProperty != null && destinationBindableProperty != null)
                destinationBindableProperty.Value = sourceBindableProperty.Value;
        }
    }
}