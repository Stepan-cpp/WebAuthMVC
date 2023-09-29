namespace WebAuthMVC.BLL.BusinessModels;

// ReSharper disable once InconsistentNaming
public class UserDTO
{
   public string Username { get; set; }
   public string FirstName { get; set; }
   public string LastName { get; set; }
   public string PasswordHash { get; set; }
   public bool IsAdmin { get; set; }
}