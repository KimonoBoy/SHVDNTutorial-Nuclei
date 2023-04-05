using Nuclei.Enums.Exception;

namespace Nuclei.Services.Exception.CustomExceptions;

public class InvalidCashInputException : CustomExceptionBase
{
    public InvalidCashInputException() : base(ExceptionType.CashInput,
        "Please enter a whole number.")
    {
    }

    public InvalidCashInputException(string message) : base(ExceptionType.CashInput, message)
    {
    }
}