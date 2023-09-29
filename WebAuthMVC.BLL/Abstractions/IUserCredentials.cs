using WebAuthMVC.BLL.Infrastructure;

namespace WebAuthMVC.BLL.Abstractions;

public interface IUserCredentials
{
   public string Username { get; set;  }
   public string PasswordHash { get; }

   public static IUserCredentials FromPassword(string username, string password, IUserVerificationService encryptionService)
   {
      return new PasswordCredentials(username, password, encryptionService);
   }

   public static IUserCredentials FromHash(string username, string hash)
   {
      return new PasswordHashCredentials(username, hash);
   }
}