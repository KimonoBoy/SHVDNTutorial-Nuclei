namespace Nuclei.Services.Exception.CustomExceptions;

public class PlayerException : System.Exception
{
    public PlayerException()
    {
    }

    public PlayerException(string message) : base(message)
    {
    }

    public PlayerException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}

public class EmptyCashInputException : PlayerException
{
    public EmptyCashInputException() : base("Cash input is empty.")
    {
    }

    public EmptyCashInputException(string message) : base(message)
    {
    }
}

public class InvalidCashInputException : PlayerException
{
    public InvalidCashInputException() : base("Invalid cash input.\n\nPlease enter a valid numeric value.")
    {
    }

    public InvalidCashInputException(string message) : base(message)
    {
    }
}