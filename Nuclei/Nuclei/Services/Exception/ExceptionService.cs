using System;
using Nuclei.Enums.Exception;
using Nuclei.Services.Exception.CustomExceptions;

namespace Nuclei.Services.Exception;

public class ExceptionService
{
    public static readonly ExceptionService Instance = new();

    public event EventHandler<CustomExceptionBase> ErrorOccurred;

    public void RaiseError(CustomExceptionBase exception)
    {
        ErrorOccurred?.Invoke(this, exception);
    }

    public void RaiseError(System.Exception ex)
    {
        // Wrap the generic exception inside your CustomExceptionBase with an appropriate ExceptionType
        var wrappedException = new CustomExceptionBase(ExceptionType.Unknown, ex.Message, ex);
        ErrorOccurred?.Invoke(this, wrappedException);
    }
}