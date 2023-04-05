using System.Collections.Generic;
using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class CustomExceptionBase : System.Exception
{
    private static readonly Dictionary<ExceptionType, string> ExceptionTypePrefixes = new()
    {
        { ExceptionType.SpawnVehicle, "Failed to Spawn Vehicle" },
        { ExceptionType.CashInput, "Set Cash Input Failed" }
    };

    private readonly string _prefix;

    public CustomExceptionBase(ExceptionType exceptionType)
    {
        _prefix = ExceptionTypePrefixes[exceptionType];
    }

    public CustomExceptionBase(ExceptionType exceptionType, string message) : base(message)
    {
        _prefix = ExceptionTypePrefixes[exceptionType];
    }

    public CustomExceptionBase(ExceptionType exceptionType, string message, System.Exception innerException) : base(
        message, innerException)
    {
        _prefix = ExceptionTypePrefixes[exceptionType];
    }

    public override string ToString()
    {
        return $"~h~{_prefix}~h~:\n\n{Message}";
    }
}