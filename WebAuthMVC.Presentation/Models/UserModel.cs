namespace WebAuthMVC.Models;

public class UserModel
{
   public string FirstName { get; set; }
   public string LastName { get; set; }
   public string Username { get; set; }
   public string PasswordHash { get; set; }
   public bool IsAdmin { get; set; }
}