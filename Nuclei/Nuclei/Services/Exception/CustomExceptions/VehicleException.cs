namespace Nuclei.Services.Exception.CustomExceptions;

public class VehicleException : System.Exception
{
    public VehicleException()
    {
    }

    public VehicleException(string message) : base(message)
    {
    }

    public VehicleException(string message, System.Exception innerException) : base(message, innerException)
    {
    }

    public override string ToString()
    {
        return $"~h~Failed to Spawn Vehicle:~h~\n\n{Message}";
    }
}

public class VehicleModelRequestTimedOutException : VehicleException
{
    public VehicleModelRequestTimedOutException() : base("Loading of vehicle model timed out.")
    {
    }

    public VehicleModelRequestTimedOutException(string message) : base(message)
    {
    }
}

public class VehicleModelNotFoundException : VehicleException
{
    public VehicleModelNotFoundException() : base("Vehicle model not found.")
    {
    }

    public VehicleModelNotFoundException(string message) : base(message)
    {
    }
}

public class VehicleSpawnFailedException : VehicleException
{
    public VehicleSpawnFailedException() : base("Failed to spawn the vehicle object.")
    {
    }

    public VehicleSpawnFailedException(string message) : base(message)
    {
    }
}