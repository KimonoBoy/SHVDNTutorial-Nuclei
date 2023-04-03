using System;

namespace Nuclei.Services.Exception;

public class ExceptionService
{
    public static readonly ExceptionService Instance = new();

    public event EventHandler<string> ErrorOccurred;

    public void RaiseError(string errorMessage)
    {
        ErrorOccurred?.Invoke(this, errorMessage);
    }
}