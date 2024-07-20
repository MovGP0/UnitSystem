namespace Qs.Runtime
{
    public class QsParameterNotFoundException : QsException
    {
        public QsParameterNotFoundException()
        {
            // Add any type-specific logic, and supply the default message.
        }

        public QsParameterNotFoundException(string message)
            : base(message)
        {
            // Add any type-specific logic.
        }
        public QsParameterNotFoundException(string message, Exception innerException) :
            base(message, innerException)
        {
            // Add any type-specific logic for inner exceptions.
        }

    }
}
