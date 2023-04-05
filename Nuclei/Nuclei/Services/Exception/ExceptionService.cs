using System;
using Nuclei.Enums.Exception;
using Nuclei.Helpers.Utilities;
using Nuclei.Services.Exception.CustomExceptions;

namespace Nuclei.Services.Exception;

public class ExceptionService
{
    private static readonly Lazy<ExceptionService> _instance = new(() => new ExceptionService());
    private readonly Logger _logger;

    private ExceptionService()
    {
        _logger = new Logger("scripts/Nuclei/logs/logs.log");
    }

    public static ExceptionService Instance => _instance.Value;

    public event EventHandler<CustomExceptionBase> ErrorOccurred;

    public void RaiseError(CustomExceptionBase exception)
    {
        _logger.LogException(exception);
        ErrorOccurred?.Invoke(this, exception);
    }

    public void RaiseError(System.Exception exception)
    {
        _logger.LogException(exception);
        ErrorOccurred?.Invoke(this, new CustomExceptionBase(ExceptionType.Unknown, exception.Message));
    }
}