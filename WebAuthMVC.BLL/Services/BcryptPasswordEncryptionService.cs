using WebAuthMVC.BLL.Abstractions;

namespace WebAuthMVC.BLL.Services;

public class BcryptUserVerificationService : IUserVerificationService
{
   public string GetHash(string password)
   {
      return BCrypt.Net.BCrypt.HashPassword(password);
   }

   public bool Verify(string hash, string password)
   {
      return BCrypt.Net.BCrypt.Verify(password, hash);
   }
}