using System;
using System.IO;

namespace Nuclei.Helpers.Utilities;

public class Logger
{
    private readonly string _logFilePath;

    public Logger(string logFilePath)
    {
        // Create the logs folder if it doesn't exist
        var logDirectory = Path.GetDirectoryName(logFilePath);
        if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory);

        _logFilePath = logFilePath;
        InitializeLogFile();
    }

    private void InitializeLogFile()
    {
        if (!File.Exists(_logFilePath))
            using (var file = File.Create(_logFilePath))
            {
            }
    }

    public void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logMessage = $"{timestamp} - {message}";

        using var streamWriter = new StreamWriter(_logFilePath, true);
        streamWriter.WriteLine(logMessage);
    }

    public void LogException(Exception exception)
    {
        Log($"Exception: {exception}");
    }
}