namespace WebAuth.HashProviders;

public class BcryptPasswordEncryptionService : IPasswordEncryptionService
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