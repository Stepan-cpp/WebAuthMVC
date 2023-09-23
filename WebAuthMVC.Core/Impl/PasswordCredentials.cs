using WebAuth;

namespace WebAuthMVC.Core.Impl;

public class PasswordCredentials : IUserCredentials
{
   public string Username { get; set;  }
   public string Password { get; set; }
   public string PasswordHash => EncryptionService.GetHash(Password);
   public IPasswordEncryptionService EncryptionService { get; set; }

   public PasswordCredentials(string username, string password, IPasswordEncryptionService encryptionService)
   {
      Username = username;
      Password = password;
      EncryptionService = encryptionService;
   }
}