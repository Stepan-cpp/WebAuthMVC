using WebAuthMVC.BLL.Abstractions;

namespace WebAuthMVC.BLL.Infrastructure;

public class PasswordCredentials : IUserCredentials
{
   public string Username { get; set;  }
   public string Password { get; set; }
   public string PasswordHash => EncryptionService.GetHash(Password);
   public IUserVerificationService EncryptionService { get; set; }

   public PasswordCredentials(string username, string password, IUserVerificationService encryptionService)
   {
      Username = username;
      Password = password;
      EncryptionService = encryptionService;
   }
}