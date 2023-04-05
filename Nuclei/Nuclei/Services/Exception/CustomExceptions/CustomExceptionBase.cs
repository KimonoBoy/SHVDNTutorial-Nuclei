using Nuclei.Enums.Exception;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.Services.Exception.CustomExceptions;

public class CustomExceptionBase : System.Exception
{
    public CustomExceptionBase(ExceptionType exceptionType)
    {
        Prefix = exceptionType.GetDescription();
    }

    public CustomExceptionBase(ExceptionType exceptionType, string message) : base(message)
    {
        Prefix = exceptionType.GetDescription();
    }

    public CustomExceptionBase(ExceptionType exceptionType, string message, System.Exception innerException) : base(
        message, innerException)
    {
        Prefix = exceptionType.GetDescription();
    }

    public string Prefix { get; }
}