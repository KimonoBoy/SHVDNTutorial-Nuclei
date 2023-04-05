using System.Collections.Generic;
using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class CustomExceptionBase : System.Exception
{
    private static readonly Dictionary<ExceptionType, string> ExceptionTypePrefixes = new()
    {
        { ExceptionType.Unknown, "Unknown Error" },
        { ExceptionType.SpawnVehicle, "Failed to Spawn Vehicle" },
        { ExceptionType.CashInput, "Set Cash Input Failed" }
    };

    public CustomExceptionBase(ExceptionType exceptionType)
    {
        Prefix = ExceptionTypePrefixes[exceptionType];
    }

    public CustomExceptionBase(ExceptionType exceptionType, string message) : base(message)
    {
        Prefix = ExceptionTypePrefixes[exceptionType];
    }

    public CustomExceptionBase(ExceptionType exceptionType, string message, System.Exception innerException) : base(
        message, innerException)
    {
        Prefix = ExceptionTypePrefixes[exceptionType];
    }

    public string Prefix { get; }

    public override string ToString()
    {
        return $"{Prefix}{Message}";
    }
}