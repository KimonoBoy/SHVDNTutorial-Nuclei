/*
 * GenericStateService.cs
 *
 * A service class to manage the state of an object T. This class supports serialization
 * and deserialization to and from a JSON file. It also provides functionality to save
 * the current state, load a saved state, and set the state manually.
 *
 * Usage:
 * var stateService = GenericStateService<YourObjectType>.Instance;
 * YourObjectType currentState = stateService.GetState();
 *
 */

using System;
using System.IO;
using Newtonsoft.Json;
using Nuclei.Constants;
using Nuclei.Helpers.Utilities;

namespace Nuclei.Services.Generics;

/// <summary>
///     Represents a service class for managing the state of an object T.
/// </summary>
/// <typeparam name="T">The type of object to manage state for.</typeparam>
public class GenericStateService<T> where T : new()
{
    private static GenericStateService<T> _instance;

    private readonly T _state;
    private readonly string _stateFilePath;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GenericStateService{T}" /> class.
    /// </summary>
    protected GenericStateService()
    {
        _stateFilePath = $"{Paths.StatesPath}/{typeof(T).Name}.json";
        EnsureDirectoryAndFileExist(_stateFilePath);
        _state = LoadState() ?? new T();
    }

    /// <summary>
    ///     Gets the singleton instance of the <see cref="GenericStateService{T}" /> class.
    /// </summary>
    public static GenericStateService<T> Instance => _instance ??= new GenericStateService<T>();

    /// <summary>
    ///     Gets the current state of the object T.
    /// </summary>
    /// <returns>The current state of the object T.</returns>
    public T GetState()
    {
        return _state;
    }

    /// <summary>
    ///     Saves the current state of the object T to a JSON file.
    /// </summary>
    public void SaveState()
    {
        var serializedState = JsonConvert.SerializeObject(_state, Formatting.Indented);
        File.WriteAllText(_stateFilePath, serializedState);
    }

    /// <summary>
    ///     Loads the saved state of the object T from a JSON file.
    /// </summary>
    /// <returns>The loaded state of the object T.</returns>
    public T LoadState()
    {
        EnsureDirectoryAndFileExist(_stateFilePath);

        var fileContent = File.ReadAllText(_stateFilePath);
        if (string.IsNullOrWhiteSpace(fileContent)) return default;

        var loadedState = JsonConvert.DeserializeObject<T>(fileContent);
        return loadedState;
    }

    /// <summary>
    ///     Ensures the specified file path and its directory exist.
    /// </summary>
    /// <param name="filePath">The file path to check.</param>
    /// <exception cref="ArgumentException">Thrown when the file path is invalid.</exception>
    private void EnsureDirectoryAndFileExist(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        if (!File.Exists(filePath)) File.Create(filePath).Close();
    }

    /// <summary>
    ///     Sets the state of the object T manually.
    /// </summary>
    /// <param name="newState">The new state to set.</param>
    /// <exception cref="ArgumentNullException">Thrown when the new state is null.</exception>
    public void SetState(T newState)
    {
        if (newState == null) throw new ArgumentNullException(nameof(newState));

        ReflectionUtilities.CopyProperties(newState, _state);
    }
}