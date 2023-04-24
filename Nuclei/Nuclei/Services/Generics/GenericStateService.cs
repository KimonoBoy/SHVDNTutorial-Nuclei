using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Nuclei.Constants;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Generics;

public class GenericStateService<TService> where TService : new()
{
    private static GenericStateService<TService> _instance;

    private readonly TService _state;
    private readonly string _stateFilePath;

    public GenericStateService()
    {
        _stateFilePath = $"{Paths.StoragePath}/{typeof(TService).Name.Replace("Service", "")}.json";
        EnsureDirectoryAndFileExist();
        _state = LoadState() ?? new TService();
    }

    public GenericStateService(string stateFilePath)
    {
        _stateFilePath = stateFilePath;
        EnsureDirectoryAndFileExist();
        _state = LoadState() ?? new TService();
    }

    public static GenericStateService<TService> Instance => _instance ??= new GenericStateService<TService>();

    public TService GetState()
    {
        return _state;
    }

    public void SaveState()
    {
        if (ShouldCreateConfigFileForType(typeof(TService)))
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            var serializedState = JsonConvert.SerializeObject(_state, settings);
            File.WriteAllText(_stateFilePath, serializedState);
        }
    }

    public TService LoadState()
    {
        if (!File.Exists(_stateFilePath)) return default;

        var fileContent = File.ReadAllText(_stateFilePath);
        if (string.IsNullOrWhiteSpace(fileContent)) return default;

        var loadedState = JsonConvert.DeserializeObject<TService>(fileContent);
        return loadedState;
    }

    private void EnsureDirectoryAndFileExist()
    {
        if (string.IsNullOrWhiteSpace(_stateFilePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(_stateFilePath));

        var directory = Path.GetDirectoryName(_stateFilePath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory ?? throw new InvalidOperationException());

        if (!File.Exists(_stateFilePath) && ShouldCreateConfigFileForType(typeof(TService)))
            File.Create(_stateFilePath).Close();
    }

    private bool ShouldCreateConfigFileForType(Type type)
    {
        var properties = type.GetProperties();
        return properties.Any(p => !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute)));
    }

    public void SetState(TService newState)
    {
        if (newState == null) throw new ArgumentNullException(nameof(newState));

        ReflectionUtilities.CopyProperties(newState, _state);
    }
}