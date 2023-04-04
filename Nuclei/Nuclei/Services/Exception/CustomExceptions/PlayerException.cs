using Nuclei.Enums;

namespace Nuclei.Services.Exception.CustomExceptions;

public class EmptyCashInputException : CustomExceptionBase
{
    public EmptyCashInputException() : base(ExceptionType.CashInput, "Cash input is empty.")
    {
    }

    public EmptyCashInputException(string message) : base(ExceptionType.CashInput, message)
    {
    }
}

public class InvalidCashInputException : CustomExceptionBase
{
    public InvalidCashInputException() : base(ExceptionType.CashInput,
        "Please enter a numeric value.")
    {
    }

    public InvalidCashInputException(string message) : base(ExceptionType.CashInput, message)
    {
    }
}