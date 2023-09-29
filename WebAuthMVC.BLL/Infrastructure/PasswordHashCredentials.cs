using WebAuthMVC.BLL.Abstractions;

namespace WebAuthMVC.BLL.Infrastructure;

public class PasswordHashCredentials : IUserCredentials
{
   public string Username { get; set;  }
   public string PasswordHash { get; set; }

   public PasswordHashCredentials(string username, string hash)
   {
      Username = username;
      PasswordHash = hash;
   }
}