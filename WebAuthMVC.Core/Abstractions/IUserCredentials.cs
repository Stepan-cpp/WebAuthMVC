using WebAuthMVC.Core.Impl;

namespace WebAuth;

public interface IUserCredentials
{
   public string Username { get; set;  }
   public string PasswordHash { get; }

   public static IUserCredentials FromPassword(string username, string password, IPasswordEncryptionService encryptionService)
   {
      return new PasswordCredentials(username, password, encryptionService);
   }

   public static IUserCredentials FromHash(string username, string hash)
   {
      return new PasswordHashCredentials(username, hash);
   }

   public static IUserCredentials FromUser(AbstractUser user)
   {
      return new PasswordHashCredentials(user.Username, user.PasswordHash);
   }
}