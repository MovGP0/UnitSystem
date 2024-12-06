namespace Qs.Runtime;

public class QsVariableNotFoundException : QsException
{
   public QsVariableNotFoundException()
   {
      // Add any type-specific logic, and supply the default message.
   }

   public QsVariableNotFoundException(string message): base(message)
   {
      // Add any type-specific logic.
   }
   public QsVariableNotFoundException(string message, Exception innerException):
      base (message, innerException)
   {
      // Add any type-specific logic for inner exceptions.
   }




   public string Namespace { get; set; }
   public string Variable { get; set; }
}