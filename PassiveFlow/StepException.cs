namespace PassiveFlow;

public class StepException : Exception
{
    public StepException()
        :base()
    {
    }

    public StepException(string message)
        : base(message)
    {
    }

    public StepException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}