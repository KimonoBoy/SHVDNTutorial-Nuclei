using System;
using System.Collections.Generic;
using System.Linq;

namespace Nuclei.Helpers.Utilities;

public static class ReflectionUtilities
{
    private static readonly Dictionary<Type, List<(string sourceName, string destinationName)>>
        PropertyMappings = new();

    public static void CopyProperties(object source, object destination)
    {
        var sourceProperties = source.GetType().GetProperties();
        var destinationProperties = destination.GetType().GetProperties();

        foreach (var sourceProperty in sourceProperties)
        foreach (var destinationProperty in destinationProperties)
            if (sourceProperty.Name == destinationProperty.Name &&
                sourceProperty.PropertyType == destinationProperty.PropertyType)
            {
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                break;
            }
    }

    public static void CopyBindableProperties(object source, object destination)
    {
        if (source == null || destination == null)
            throw new ArgumentNullException(source == null ? nameof(source) : nameof(destination));

        var sourceType = source.GetType();
        var destinationType = destination.GetType();
        List<(string sourceName, string destinationName)> mappings;

        if (!PropertyMappings.TryGetValue(sourceType, out mappings))
        {
            mappings = new List<(string sourceName, string destinationName)>();

            var sourceProperties = sourceType.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(BindableProperty<>));
            var destinationProperties = destinationType.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(BindableProperty<>));

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties
                    .FirstOrDefault(p => p.Name == sourceProperty.Name &&
                                         p.PropertyType == sourceProperty.PropertyType);
                if (destinationProperty != null) mappings.Add((sourceProperty.Name, destinationProperty.Name));
            }

            PropertyMappings[sourceType] = mappings;
        }

        foreach (var (sourceName, destinationName) in mappings)
        {
            var sourceProperty = sourceType.GetProperty(sourceName);
            var destinationProperty = destinationType.GetProperty(destinationName);

            var sourceBindableProperty = sourceProperty.GetValue(source) as dynamic;
            var destinationBindableProperty = destinationProperty.GetValue(destination) as dynamic;

            if (sourceBindableProperty != null && destinationBindableProperty != null)
                destinationBindableProperty.Value = sourceBindableProperty.Value;
        }
    }
}