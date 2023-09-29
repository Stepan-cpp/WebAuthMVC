namespace WebAuthMVC.BLL.Infrastructure;

public class ValidationException : Exception
{
   public string Property { get; }
   public ValidationException(string message, string property) : base(message)
   {
      Property = property;
   }
}