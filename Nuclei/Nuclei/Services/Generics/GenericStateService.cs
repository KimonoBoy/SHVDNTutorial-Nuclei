using System;
using System.IO;
using Newtonsoft.Json;
using Nuclei.Constants;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Generics;

public class GenericStateService<T> where T : new()
{
    private static GenericStateService<T> _instance;

    private readonly T _state;
    private readonly string _stateFilePath;

    private GenericStateService()
    {
        _stateFilePath = $"{Paths.BasePath}/states/{typeof(T).Name}.json";
        EnsureDirectoryAndFileExist(_stateFilePath);
        _state = LoadState() ?? new T();
    }

    public static GenericStateService<T> Instance => _instance ??= new GenericStateService<T>();

    public T GetState()
    {
        return _state;
    }

    public void SaveState()
    {
        var serializedState = JsonConvert.SerializeObject(_state, Formatting.Indented);
        File.WriteAllText(_stateFilePath, serializedState);
    }

    public T LoadState()
    {
        EnsureDirectoryAndFileExist(_stateFilePath);

        var fileContent = File.ReadAllText(_stateFilePath);
        if (string.IsNullOrWhiteSpace(fileContent)) return default;

        var loadedState = JsonConvert.DeserializeObject<T>(fileContent);
        return loadedState;
    }

    private void EnsureDirectoryAndFileExist(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        if (!File.Exists(filePath)) File.Create(filePath).Close();
    }

    public void SetState(T newState)
    {
        if (newState == null) throw new ArgumentNullException(nameof(newState));

        // foreach (var property in typeof(T).GetProperties()) property.SetValue(_state, property.GetValue(newState));

        ReflectionUtilities.CopyProperties(newState, _state);
    }
}